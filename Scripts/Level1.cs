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
	Button UITurretSprite1;
	Button UITurretSprite2;
	Button UITurretSprite3;
	Button UITurretSprite4;



		// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		backGround = GetNode<TileMap>("Background");
		foreGround = GetNode<TileMap>("Background/Foreground");
		moneyUI = GetNode<LineEdit>("UI/UIContainer/StatsContainer/MoneyUI/LineEdit");
		healthUI = GetNode<LineEdit>("UI/UIContainer/StatsContainer/HealthUI/Label");
		moneyUI.Text = money.ToString();
		UITurretSprite1 = GetNode<Button>("UI/UIContainer/TurretsContainer/TurretContainer1/Turret1");
		UITurretSprite2 = GetNode<Button>("UI/UIContainer/TurretsContainer/TurretContainer2/Turret2");
		UITurretSprite3 = GetNode<Button>("UI/UIContainer/TurretsContainer/TurretContainer3/Turret3");
		UITurretSprite4 = GetNode<Button>("UI/UIContainer/TurretsContainer/TurretContainer4/Turret4");
		SetUIButtonStatus();
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
				SetUIButtonStatus();
			}
		}
	}

	private void SetUIButtonStatus()
	{
		var currentMoney = Convert.ToInt32(moneyUI.Text);
		if (currentMoney >= 50) UITurretSprite1.Disabled = false;
		else UITurretSprite1.Disabled = true;

		if (currentMoney >= 100) UITurretSprite2.Disabled = false;
		else UITurretSprite2.Disabled = true;

		if (currentMoney >= 150) UITurretSprite3.Disabled = false;
		else UITurretSprite3.Disabled = true;

		if (currentMoney >= 200) UITurretSprite4.Disabled = false;
		else UITurretSprite4.Disabled = true;
	}
}
