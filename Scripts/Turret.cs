using Godot;
using System;

public class Turret : Area2D
{
	public Node2D target;
	public Area2D turretRange;
	public Sprite turretSprite;
	public int turretSpriteRadius = 50;
    public bool switchCannon = false;
	public Timer reloadTimer;
	public Node2D turretUI;
	public MeshInstance2D turretAreaMesh;
	PackedScene bulletScene = GD.Load<PackedScene>("res://Scenes/Bullet.tscn");
	PackedScene bulletScene2 = GD.Load<PackedScene>("res://Scenes/Bullet2.tscn");
	PackedScene rocketScene = GD.Load<PackedScene>("res://Scenes/SmallRocket.tscn");
	PackedScene shotScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		turretRange = GetNode<Area2D>("Area2D");
		turretSprite = GetNode<Sprite>("Sprite");
		reloadTimer = GetNode<Timer>("Area2D/TurretDetectionArea/ReloadTimer");
		turretAreaMesh = GetNode<MeshInstance2D>("Area2D/TurretDetectionArea/TurretDetectionMesh");
		turretUI = GetNode<Node2D>("TurretUI");
		reloadTimer.Start();
		turretUI.Connect("TurretUIExited", this, "OnTurretUIExited");
		AssignBulletType();
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
