using Godot;
using System;

public class Level1 : Node2D
{
	[Export]
	public float spawnTime = 2;
	[Export]
	public int money = 150;
	float timer = 0;
	PackedScene enemy = GD.Load<PackedScene>("res://Scenes/EnemyPathing.tscn");
	TileMap backGround;
	TileMap foreGround;
	LineEdit moneyUI;
	LineEdit healthUI;
	public Area2D Enemy;



		// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		backGround = GetNode<TileMap>("Background");
		foreGround = GetNode<TileMap>("Background/Foreground");
		moneyUI = GetNode<LineEdit>("UI/UIContainer/StatsContainer/MoneyUI/LineEdit");
		healthUI = GetNode<LineEdit>("UI/UIContainer/StatsContainer/HealthUI/Label");
		moneyUI.Text = money.ToString();
	}

  	// Called every frame. 'delta' is the elapsed time since the previous frame.
  	public override void _Process(float delta) 
	{
		SpawnEnemy(delta);
	}

	private void SpawnEnemy (float delta)
	{
		timer = timer + delta;
		if (timer > spawnTime) 
		{
			var newEnemy = enemy.Instance();
			AddChild(newEnemy);
			timer = 0;
		}
	}

	public void OnLevel1ChildExitingTree(object body)
	{
		Node target = (Node)body;
		if (target.IsInGroup("EnemyPathingGroup"))
		{
			PathFollow2D ePath = target.GetNode<PathFollow2D>("EnemyLine/EnemyPath");
			float endOfPath = ePath.GetOffset();
 			if (endOfPath >= 4923)
			{
				var currentHealth = healthUI.GetText();
				var newHealth = Convert.ToInt32(currentHealth);
				healthUI.SetText((newHealth -= 5 ).ToString());
			}
			else 
			{
				var currentMoney = moneyUI.GetText();
				var newMoney = Convert.ToInt32(currentMoney);
				moneyUI.SetText(( newMoney += 50 ).ToString());
			}
		}
	}
}
