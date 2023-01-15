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
	PackedScene turret = GD.Load<PackedScene>("res://Scenes/RocketTurret.tscn");
	Area2D mouse;
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
		mouse = GetNode<Area2D>("MouseArea");
		UITurretSprite1 = GetNode<Button>("UI/UIContainer/TurretsContainer/TurretContainer1/Turret1");
		UITurretSprite2 = GetNode<Button>("UI/UIContainer/TurretsContainer/TurretContainer2/Turret2");
		UITurretSprite3 = GetNode<Button>("UI/UIContainer/TurretsContainer/TurretContainer3/Turret3");
		UITurretSprite4 = GetNode<Button>("UI/UIContainer/TurretsContainer/TurretContainer4/Turret4");
		moneyUI.Text = money.ToString();
		SetUIButtonStatus();
	}

  	// Called every frame. 'delta' is the elapsed time since the previous frame.
  	public override void _Process(float delta) 
	{
		SpawnEnemy(delta);
		if (Input.IsActionJustPressed("click"))
		{
			mouse.GlobalPosition = GetGlobalMousePosition();
		}
		if (Input.IsActionJustReleased("click"))
		{
			var targets = mouse.GetOverlappingAreas();
			if (targets.Count != 0)
			{
				GD.Print("True");
				Console.WriteLine("True");
			} else {
				// Place turret on the center of the selected tilemap area. Each tile is 64px
				Area2D newTurret = (Area2D)turret.Instance();
				var mousePosition = GetGlobalMousePosition();
				var xPos = Math.Floor(mousePosition.x / 64);
				var yPos = Math.Floor(mousePosition.y / 64);
				xPos = xPos * 64;
				yPos = yPos * 64;
				newTurret.GlobalPosition = new Vector2((float)xPos + 32, (float)yPos + 32);
				AddChild(newTurret); 
			}
		}
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
			// 4923 is the total distance the enemies travel along their set path. This can be found under the EnemyPathing.tscn > EnemyPath > 
			// and in the inspector, Unit Offset.
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
