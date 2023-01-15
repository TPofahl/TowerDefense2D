using Godot;
using System;

public class RocketTurret : Area2D
{
	public Node2D target;
	public Area2D turretRange;
	public Sprite turretSprite;
	public int turretSpriteRadius = 50;
    public bool switchCannon = false;
	public Timer reloadTimer;
	PackedScene bulletScene = GD.Load<PackedScene>("res://Scenes/Bullet.tscn");

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		turretRange = GetNode<Area2D>("Area2D");
		turretSprite = GetNode<Sprite>("Sprite");
		reloadTimer = GetNode<Timer>("Area2D/TurretDetectionArea/ReloadTimer");
		reloadTimer.Start();
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
		Area2D bullet = (Area2D)bulletScene.Instance();
		Vector2 endOfTurret = new Vector2((float)Math.Cos(turretSprite.Rotation), (float)Math.Sin(turretSprite.Rotation));
        if (switchCannon) bullet.GlobalPosition = GlobalPosition + new Vector2(8,0) + endOfTurret * turretSpriteRadius;
		else bullet.GlobalPosition = GlobalPosition + new Vector2(-8,0) + endOfTurret * turretSpriteRadius;
		bullet.GlobalRotation = turretSprite.Rotation;
		GetTree().CurrentScene.AddChild(bullet);
		reloadTimer.Start();
        switchCannon = !switchCannon;
	}

	public void OnEnemyDestroyed()
	{
		if (IsInstanceValid(target)) target.Disconnect("EnemyDestroyed", this, "OnEnemyDestroyed");
		target = null;
	}
}
