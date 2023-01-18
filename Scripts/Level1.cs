using Godot;
using System;

public class Level1 : Node2D
{
	[Export]
	public float spawnTime = 2;
	[Export]
	public int money = 50000;
	float timer = 0;
	private bool buttonToggled = false;
	string lastButtonPressed = "";
	PackedScene enemy = GD.Load<PackedScene>("res://Scenes/EnemyPathing.tscn");
	PackedScene turret;
	PackedScene machineTurret = GD.Load<PackedScene>("res://Scenes/MachineTurret.tscn");
	PackedScene singleCannon = GD.Load<PackedScene>("res://Scenes/SingleCannon.tscn");
	PackedScene doubleCannon = GD.Load<PackedScene>("res://Scenes/DoubleCannon.tscn");
	PackedScene rocketTurret = GD.Load<PackedScene>("res://Scenes/RocketTurret.tscn");
	Area2D mouse;
	TileMap backGround;
	TileMap foreGround;
	Control UI;
	LineEdit moneyUI;
	LineEdit healthUI;
	public Area2D Enemy;
	Button UIButton1;
	Button UIButton2;
	Button UIButton3;
	Button UIButton4;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		backGround = GetNode<TileMap>("Background");
		foreGround = GetNode<TileMap>("Background/Foreground");
		UI = GetNode<Control>("UI");
		moneyUI = GetNode<LineEdit>("UI/UIContainer/StatsContainer/MoneyUI/LineEdit");
		healthUI = GetNode<LineEdit>("UI/UIContainer/StatsContainer/HealthUI/Label");
		mouse = GetNode<Area2D>("MouseArea");
		UIButton1 = GetNode<Button>("UI/UIContainer/TurretsContainer/TurretContainer1/Turret1");
		UIButton2 = GetNode<Button>("UI/UIContainer/TurretsContainer/TurretContainer2/Turret2");
		UIButton3 = GetNode<Button>("UI/UIContainer/TurretsContainer/TurretContainer3/Turret3");
		UIButton4 = GetNode<Button>("UI/UIContainer/TurretsContainer/TurretContainer4/Turret4");
		moneyUI.Text = money.ToString();
		SetUIButtonStatus();
		UI.Connect("TurretUIButtonToggled", this, "OnTurretUIButtonToggled");
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
			if (targets.Count == 0 && turret != null)
			{
				PlaceTurret(mouse.GlobalPosition);
				UpdateMoneySpent();
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
			float endOfPath = ePath.Offset;
			// 4923 is the total distance the enemies travel along their set path. This can be found under the EnemyPathing.tscn > EnemyPath > 
			// and in the inspector, Unit Offset.
 			if (endOfPath >= 4923)
			{
				var currentHealth = healthUI.Text;
				var newHealth = Convert.ToInt32(currentHealth);
				healthUI.Text = (newHealth -= 5 ).ToString();
			}
			else 
			{
				var currentMoney = moneyUI.Text;
				var newMoney = Convert.ToInt32(currentMoney);
				moneyUI.Text = ( newMoney += 50 ).ToString();
				SetUIButtonStatus();
			}
		}
	}

	private void SetUIButtonStatus()
	{
		var currentMoney = Convert.ToInt32(moneyUI.Text);
		if (currentMoney >= 50) UIButton1.Disabled = false;
		else UIButton1.Disabled = true;

		if (currentMoney >= 100) UIButton2.Disabled = false;
		else UIButton2.Disabled = true;

		if (currentMoney >= 150) UIButton3.Disabled = false;
		else UIButton3.Disabled = true;

		if (currentMoney >= 200) UIButton4.Disabled = false;
		else UIButton4.Disabled = true;
	}

	private void OnTurretUIButtonToggled(string buttonType)
	{
		UIButton1.Pressed = false;
		UIButton2.Pressed = false;
		UIButton3.Pressed = false;
		UIButton4.Pressed = false;
		switch (buttonType) 
		{
			case "MachineTurret":
				if (lastButtonPressed == buttonType)
				{
					UIButton1.Pressed = false;
					lastButtonPressed = "";
					turret = null;
					return;
				} else
				{
					UIButton1.Pressed = true;
					turret = machineTurret;
					turret.ResourceName = lastButtonPressed = buttonType;
				}
				break;
			case "SingleCannon":
				if (lastButtonPressed == buttonType)
				{
					UIButton2.Pressed = false;
					lastButtonPressed = "";
					turret = null;
					return;
				} else
				{
					UIButton2.Pressed = true;
					turret = singleCannon;
					turret.ResourceName = lastButtonPressed = buttonType;
				}
				break;
			case "DoubleCannon":
				if (lastButtonPressed == buttonType)
				{
					UIButton3.Pressed = false;
					lastButtonPressed = "";
					turret = null;
					return;
				} else
				{
					UIButton3.Pressed = true;
					turret = doubleCannon;
					turret.ResourceName = lastButtonPressed = buttonType;
				}
				break;
			case "RocketTurret":
				if (lastButtonPressed == buttonType)
				{
					UIButton4.Pressed = false;
					lastButtonPressed = "";
					turret = null;
					return;
				} else
				{
					UIButton4.Pressed = true;
					turret = rocketTurret;
					turret.ResourceName = lastButtonPressed = buttonType;
				}
				break;
			default:
				turret = null;
				break;
		}
	}

	private void PlaceTurret(Vector2 mousePosition)
	{
		// Place turret on the center of the selected tilemap area. Each tile is 64px
		if (turret == null) return;
		Area2D newTurret = (Area2D)turret.Instance();
		var xPos = Math.Floor(mousePosition.x / 64);
		var yPos = Math.Floor(mousePosition.y / 64);
		xPos = xPos * 64;
		yPos = yPos * 64;
		newTurret.GlobalPosition = new Vector2((float)xPos + 32, (float)yPos + 32);
		AddChild(newTurret); 
	}
	private void UpdateMoneySpent()
	{
		if (turret == null) return;
		string turretType = turret.ResourceName;
		var currentMoney = Convert.ToInt32(moneyUI.Text);
		switch (turretType)
		{
			case "MachineTurret":
				if (UIButton1.Disabled) turret = null;
				else 
				{ 
					currentMoney -= 50;
					moneyUI.Text = ( currentMoney).ToString();
					if (currentMoney < 50)
					{
						turret = null;
						UIButton1.Pressed = false;
					}
				}
				break;
			case "SingleCannon":	
				if (UIButton2.Disabled) turret = null;
				else 
				{ 
					currentMoney -= 100;
					moneyUI.Text = ( currentMoney).ToString();
					if (currentMoney < 100)
					{
						turret = null;
						UIButton2.Pressed = false;
					}
				}
				break;
			case "DoubleCannon":
				if (UIButton3.Disabled) turret = null;
				else 
				{ 
					currentMoney -= 150;
					moneyUI.Text = ( currentMoney).ToString();
					if (currentMoney < 150)
					{
						turret = null;
						UIButton3.Pressed = false;
					}
				}
				break;
			case "RocketTurret":
				if (UIButton4.Disabled) turret = null;
				else 
				{ 
					currentMoney -= 200;
					moneyUI.Text = ( currentMoney).ToString();
					if (currentMoney < 200)
					{
						turret = null;
						UIButton4.Pressed = false;
					}
				}
				break;
			default:
				GD.Print("ERROR: Invalid turretType in UpdateMoneySpentSpent");
				break;
		}
		SetUIButtonStatus();
	}
}
