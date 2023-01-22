using Godot;
using System;

public class Turret : Area2D
{
	[Signal]
	public delegate void TurretUpgraded(int newRange, int NewDamage);
	public Node2D target;
	public Area2D turretRange;
	public Sprite turretSprite;
	public Sprite turretBase;
	public int turretSpriteRadius = 50;
	public int turretRangeText = 290;
	public int turretDamageText = 50;
    public bool switchCannon = false;
	public Timer reloadTimer;
	public Node2D turretUI;
	public CollisionShape2D turretUICollider;
	public CollisionShape2D turretDetectionArea;
	public MeshInstance2D turretAreaMesh;
	PackedScene bulletScene = GD.Load<PackedScene>("res://Scenes/Bullet.tscn");
	PackedScene bulletScene2 = GD.Load<PackedScene>("res://Scenes/Bullet2.tscn");
	PackedScene rocketScene = GD.Load<PackedScene>("res://Scenes/SmallRocket.tscn");
	PackedScene shotScene;
	private Vector2 screenSize = OS.WindowSize;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		turretRange = GetNode<Area2D>("Area2D");
		turretSprite = GetNode<Sprite>("Sprite");
		turretBase = GetNode<Sprite>("TurretBase");
		reloadTimer = GetNode<Timer>("Area2D/TurretDetectionArea/ReloadTimer");
		turretDetectionArea = GetNode<CollisionShape2D>("Area2D/TurretDetectionArea");
		turretAreaMesh = GetNode<MeshInstance2D>("Area2D/TurretDetectionArea/TurretDetectionMesh");
		turretUI = GetNode<Node2D>("TurretUI");
		turretUICollider = GetNode<CollisionShape2D>("TurretUI/Area2D/CollisionShape2D");
		reloadTimer.Start();
		turretUI.Connect("TurretUIExited", this, "OnTurretUIExited");
		turretUI.Connect("UpgradeButtonPressed", this, "OnUpgradeButtonPressed");
		AssignBulletType();
		// Flip UI if turret is placed on lower-half of screen, to prevent the UI from clipping outside game world.
		if (this.GlobalPosition.y > screenSize.y / 2)
		{
			var xPos = turretUICollider.GlobalPosition.x;
			var yPos = turretUICollider.GlobalPosition.y;
			var uiXPos = turretUI.GlobalPosition.x;
			var uiyPos = turretUI.GlobalPosition.y;
			turretUICollider.GlobalPosition = new Vector2(xPos, yPos += 64);
			turretUI.GlobalPosition = new Vector2(uiXPos, uiyPos -= 232);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
		if (target != null && IsInstanceValid(target))
		{
			var angleToTarget = GlobalPosition.DirectionTo(target.GlobalPosition).Angle();
			turretSprite.Rotation = angleToTarget;
			if (reloadTimer.IsStopped()) Shoot();
		}
		else
		{
			target = null;
			AssignTarget();
		}
	}

	private void OnTurretDetectionAreaEntered(object body) 
	{
		AssignTarget();
	}

	private void OnTurretDetectionAreaExit(object body) 
	{
		var enemyList = turretRange.GetOverlappingAreas();
		if (!enemyList.Contains(target))
		{
			target = null;
			AssignTarget();
		} 
	}

	private void AssignTarget() 
	{
		if (target != null) return;
		var targets = turretRange.GetOverlappingAreas();
		if (targets.Count != 0) target = (Node2D)targets[0];
		if (IsInstanceValid(target) && !target.IsConnected("EnemyDestroyed", this, "OnEnemyDestroyed")) target.Connect("EnemyDestroyed", this, "OnEnemyDestroyed");
	}

	private void Shoot()
	{
		Area2D shot = (Area2D)shotScene.Instance();
		shot.EditorDescription = turretDamageText.ToString();
		Vector2 endOfTurret = new Vector2((float)Math.Cos(turretSprite.Rotation), (float)Math.Sin(turretSprite.Rotation));
		if (this.Filename == "res://Scenes/SingleCannon.tscn") shot.GlobalPosition = GlobalPosition + endOfTurret * turretSpriteRadius;
		else 
		{
			if (switchCannon) shot.GlobalPosition = GlobalPosition + new Vector2(8,0) + endOfTurret * turretSpriteRadius;
			else shot.GlobalPosition = GlobalPosition + new Vector2(-8,0) + endOfTurret * turretSpriteRadius;
		}
		shot.GlobalRotation = turretSprite.Rotation;
		GetTree().CurrentScene.AddChild(shot);
		reloadTimer.Start();
        switchCannon = !switchCannon;
	}

	public void OnEnemyDestroyed()
	{
		if (IsInstanceValid(target)) target.Disconnect("EnemyDestroyed", this, "OnEnemyDestroyed");
		target = null;
	}

	private void OnTurretUIExited()
	{
		turretUI.Visible = false;
		turretAreaMesh.Visible = false;
		target = null;
	}

	private void OnUpgradeButtonPressed(int upgradeLevel)
	{
		switch (upgradeLevel)
		{
			case 1:
				turretBase.Modulate = Color.Color8(255,255,255,255); //grey
				break;
			case 2:
				turretBase.Modulate = Color.Color8(109,209,2,255); //green
				break;
			case 3:
				turretBase.Modulate = Color.Color8(29,144,255,255); //blue
				break;
			case 4:
				turretBase.Modulate = Color.Color8(252,143,3,255); //orange
				break;
			case 5:
				turretBase.Modulate = Color.Color8(244,3,252,255); //purple
				break;
			default:
				GD.Print("ERROR: invalid turret level assignment to Turret.cs");
				break;
		}
		turretDetectionArea.Scale = new Vector2((float)(turretDetectionArea.Scale.x * 0.2 + turretDetectionArea.Scale.x), (float)(turretDetectionArea.Scale.y * 0.2 + turretDetectionArea.Scale.y));
		turretRangeText = (int)(turretRangeText * 0.2 + turretRangeText);
		turretDamageText += 500;
		EmitSignal(nameof(TurretUpgraded), turretRangeText, turretDamageText);
	}

	private void AssignBulletType()
	{
		switch(this.Filename)
		{
			case "res://Scenes/MachineTurret.tscn":
				shotScene = bulletScene2;
			break;
			case "res://Scenes/SingleCannon.tscn":
				shotScene = bulletScene;
			break;
			case "res://Scenes/DoubleCannon.tscn":
				shotScene = bulletScene;
			break;
			case "res://Scenes/RocketTurret.tscn":
				shotScene = rocketScene;
			break;
			default:
			GD.Print("ERROR: invalid turret type selected in Turret script.");
			break;
		}
	}
}
