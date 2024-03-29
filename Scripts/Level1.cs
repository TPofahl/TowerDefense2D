using Godot;
using System;

public class Level1 : Node2D
{
	public float enemySpawnTime = 4;
	public float tankSpawnTime = 5;
	public float airplaneSpawnTime = 10;
	public int enemiesSpawned = 1;
	public int tanksSpawned = 1;
	public int airplanesSpawned = 1;
	public int money = 500;
	public float enemyTimer = 0;
	public float tankTimer = 0;
	public float airplaneTimer = 0;
	string lastButtonPressed = "";
	[Signal]
    public delegate void PlayerMoneyChanged(string newPlayerMoney);
	PackedScene enemyPathing = GD.Load<PackedScene>("res://Scenes/EnemyPathing.tscn");
	PackedScene airplanePathing = GD.Load<PackedScene>("res://Scenes/AirplanePathing.tscn");
	PackedScene enemy = GD.Load<PackedScene>("res://Scenes/Enemy.tscn");
	PackedScene enemy2 = GD.Load<PackedScene>("res://Scenes/Enemy2.tscn");
	PackedScene enemy3 = GD.Load<PackedScene>("res://Scenes/Enemy3.tscn");
	PackedScene enemy4 = GD.Load<PackedScene>("res://Scenes/Enemy4.tscn");
	PackedScene enemyTank = GD.Load<PackedScene>("res://Scenes/Tank.tscn");
	PackedScene enemyTank2 = GD.Load<PackedScene>("res://Scenes/Tank2.tscn");
	PackedScene airplane = GD.Load<PackedScene>("res://Scenes/Airplane.tscn");
	PackedScene airplane2 = GD.Load<PackedScene>("res://Scenes/Airplane2.tscn");
	PackedScene enemyHealthBar = GD.Load<PackedScene>("res://Scenes/HealthBar.tscn");
	PackedScene turret;
	PackedScene machineTurret = GD.Load<PackedScene>("res://Scenes/MachineTurret.tscn");
	PackedScene singleCannon = GD.Load<PackedScene>("res://Scenes/SingleCannon.tscn");
	PackedScene doubleCannon = GD.Load<PackedScene>("res://Scenes/DoubleCannon.tscn");
	PackedScene rocketTurret = GD.Load<PackedScene>("res://Scenes/RocketTurret.tscn");
	Texture machineTurretSprite = GD.Load<Texture>("res://Assets/png/towerDefense_tile203.png");
	Texture singleCannonSprite = GD.Load<Texture>("res://Assets/png/towerDefense_tile249.png");
	Texture DoubleCannonSprite = GD.Load<Texture>("res://Assets/png/towerDefense_tile250.png");
	Texture RocketTurretSprite = GD.Load<Texture>("res://Assets/png/towerDefense_tile204.png");
	Area2D mouse;
	Sprite ghostSprite;
	TileMap backGround;
	TileMap foreGround;
	Control UI;
	LineEdit moneyUI;
	LineEdit healthUI;
	Button UIButton1;
	Button UIButton2;
	Button UIButton3;
	Button UIButton4;
	Timer enemySpawnTimer;
	Timer tankSpawnTimer;
	Timer airplaneSpawnTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		backGround = GetNode<TileMap>("Background");
		foreGround = GetNode<TileMap>("Background/Foreground");
		UI = GetNode<Control>("UI");
		moneyUI = GetNode<LineEdit>("UI/UIContainer/StatsContainer/MoneyUI/LineEdit");
		healthUI = GetNode<LineEdit>("UI/UIContainer/StatsContainer/HealthUI/Label");
		mouse = GetNode<Area2D>("MouseArea");
		ghostSprite = GetNode<Sprite>("MouseArea/TurretGhost");
		UIButton1 = GetNode<Button>("UI/UIContainer/TurretsContainer/TurretContainer1/Turret1");
		UIButton2 = GetNode<Button>("UI/UIContainer/TurretsContainer/TurretContainer2/Turret2");
		UIButton3 = GetNode<Button>("UI/UIContainer/TurretsContainer/TurretContainer3/Turret3");
		UIButton4 = GetNode<Button>("UI/UIContainer/TurretsContainer/TurretContainer4/Turret4");
		moneyUI.Text = money.ToString();
		SetUIButtonStatus();
		UI.Connect("TurretUIButtonToggled", this, "OnTurretUIButtonToggled");
		enemySpawnTimer = GetNode<Timer>("SpawnTimers/EnemySpawnTimer");
		tankSpawnTimer = GetNode<Timer>("SpawnTimers/TankSpawnTimer");
		airplaneSpawnTimer = GetNode<Timer>("SpawnTimers/AirplaneSpawnTimer");
		enemySpawnTimer.Stop();
		tankSpawnTimer.Stop();
		airplaneSpawnTimer.Stop();
	}

  	// Called every frame. 'delta' is the elapsed time since the previous frame.
  	public override void _Process(float delta) 
	{
		EnemySpawner(delta);
		mouse.GlobalPosition = GetGlobalMousePosition();
		var targets = mouse.GetOverlappingAreas();
		if (targets.Count == 0 && turret != null)
		{
			ghostSprite.Visible = true;
			var xPos = Math.Floor(mouse.GlobalPosition.x / 64);
			var yPos = Math.Floor(mouse.GlobalPosition.y / 64);
			xPos = xPos * 64;
			yPos = yPos * 64;
			ghostSprite.GlobalPosition = new Vector2((float)xPos + 32, (float)yPos + 32);
		}
		else ghostSprite.Visible = false;
		if (Input.IsActionJustPressed("LeftMouse"))
		{
			if (targets.Count == 0 && turret != null)
			{
				PlaceTurret(mouse.GlobalPosition);
				UpdateMoneySpent();
			}
		}
	}

	private void EnemySpawner(float delta)
	{
		enemyTimer += delta;
		tankTimer += delta;
		airplaneTimer += delta;
		if (enemyTimer > enemySpawnTime) 
		{
			enemySpawnTimer.Start();
			enemyTimer = 0;
		}
		if (tankTimer > tankSpawnTime) 
		{
			tankSpawnTimer.Start();
			tankTimer = 0;
		}
		if (airplaneTimer > airplaneSpawnTime) 
		{
			airplaneSpawnTimer.Start();
			airplaneTimer = 0;
		}
	}

	private void SpawnEnemy(string enemyToSpawn)
	{
		var newEnemyPath = enemyPathing.Instance();
		var newEnemy = SelectEnemySpawnType(enemyToSpawn);
		if (enemyToSpawn == "Airplane" || enemyToSpawn == "Airplane2")
		{
			var placeholderPath = newEnemyPath.GetNode<Path2D>("EnemyLine");
			newEnemyPath.RemoveChild(placeholderPath);
			placeholderPath.QueueFree();
			newEnemyPath.AddChild(airplanePathing.Instance());
		}
		var enemyPath = newEnemyPath.GetNode<PathFollow2D>("EnemyLine/EnemyPath");
		var placeholderEnemy = newEnemyPath.GetNode("EnemyLine/EnemyPath/Enemy");
		newEnemyPath.Connect("OnEnemyFinishingPath", this, "EnemyFinishedPath");
		newEnemyPath.Connect("OnTurretDestroyedEnemy", this, "TurretDestroytedEnemy");
		newEnemy.AddChild(enemyHealthBar.Instance());
		enemyPath.RemoveChild(placeholderEnemy);
		placeholderEnemy.QueueFree();
		enemyPath.AddChild(newEnemy);
		AddChild(newEnemyPath);
	}

	private void OnEnemySpawnTimerTimeout()
	{
		if (enemiesSpawned >= 3)
		{
			enemySpawnTimer.Stop();
			enemiesSpawned = 0;
		}
		{
			SpawnEnemy("Enemy");
			enemiesSpawned++;
		}
	}

	private void OnTankSpawnTimerTimeout()
	{
		if (tanksSpawned >= 1)
		{
			tankSpawnTimer.Stop();
			tanksSpawned = 0;
		}
		{
			SpawnEnemy("Tank");
			tanksSpawned++;
		}
	}

	private void OnAirplaneSpawnTimerTimeout()
	{
		if (airplanesSpawned >= 1)
		{
			airplaneSpawnTimer.Stop();
			airplanesSpawned = 0;
		}
		{
			SpawnEnemy("Airplane");
			airplanesSpawned++;
		}
	}
	public void OnLevel1ChildExitingTree(object body)
	{
		Node target = (Node)body;
		if (target.IsInGroup("EnemyPathingGroup"))
		{
		}
	}

	private void EnemyFinishedPath()
	{
		var currentHealth = healthUI.Text;
		var newHealth = Convert.ToInt32(currentHealth);
		healthUI.Text = (newHealth -= 5 ).ToString();
	}

	private void TurretDestroytedEnemy()
	{
		var currentMoney = moneyUI.Text;
		var newMoney = Convert.ToInt32(currentMoney);
		moneyUI.Text = ( newMoney += 50 ).ToString();
		EmitSignal(nameof(PlayerMoneyChanged), moneyUI.Text);
		SetUIButtonStatus();
	}

	private void SetUIButtonStatus()
	{
		var currentMoney = Convert.ToInt32(moneyUI.Text);
		if (currentMoney >= 50) UIButton1.Disabled = false;
		else
		{
			UIButton1.Disabled = true;
			lastButtonPressed = "";
		}

		if (currentMoney >= 100) UIButton2.Disabled = false;
		else
		{
			UIButton2.Disabled = true;
			lastButtonPressed = "";
		}

		if (currentMoney >= 150) UIButton3.Disabled = false;
		else
		{
			UIButton3.Disabled = true;
			lastButtonPressed = "";
		}

		if (currentMoney >= 200) UIButton4.Disabled = false;
		else
		{
			UIButton4.Disabled = true;
			lastButtonPressed = "";
		}
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
					ghostSprite.Texture = machineTurretSprite;
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
					ghostSprite.Texture = singleCannonSprite;
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
					ghostSprite.Texture = DoubleCannonSprite;
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
					ghostSprite.Texture = RocketTurretSprite;
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
		newTurret.Connect("TurretUpgraded", this, "OnTurretUpgraded");
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
					EmitSignal(nameof(PlayerMoneyChanged), moneyUI.Text);
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
					EmitSignal(nameof(PlayerMoneyChanged), moneyUI.Text);
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
					EmitSignal(nameof(PlayerMoneyChanged), moneyUI.Text);
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
					EmitSignal(nameof(PlayerMoneyChanged), moneyUI.Text);
					if (currentMoney < 200)
					{
						turret = null;
						UIButton4.Pressed = false;
					}
				}
				break;
			default:
				throw new Exception("ERROR: Invalid turretType in UpdateMoneySpentSpent");
		}
		SetUIButtonStatus();
	}

	private void OnTurretUpgraded(int newRangeText, int newDamageText, int turretUpgradedCost)
	{
		moneyUI.Text = (Convert.ToInt32(moneyUI.Text) - turretUpgradedCost).ToString();
		EmitSignal(nameof(PlayerMoneyChanged), moneyUI.Text);
		SetUIButtonStatus();
    }

	private Node SelectEnemySpawnType(string enemySelection)
	{
		Node result;
		switch (enemySelection)
		{
			case "Enemy":
			{
				result = enemy.Instance();
				break;
			}
			case "Enemy2":
			{
				result = enemy2.Instance();
				break;
			}
			case "Enemy3":
			{
				result = enemy3.Instance();
				break;
			}
			case "Enemy4":
			{
				result = enemy4.Instance();
				break;
			} 
			case "Tank":
			{
				result = enemyTank.Instance();
				break;
			}
			case "Tank2":
			{
				result = enemyTank2.Instance();
				break;
			}
			case "Airplane":
			{
				result = airplane.Instance();
				break;
			}
			case "Airplane2":
			{
				result = airplane2.Instance();
				break;
			}
			default:
			throw new Exception("Invalid enemy type selected");
		}
		return result;
	}
}