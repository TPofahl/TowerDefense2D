using Godot;
using System;

public class Turret : Node2D
{
	public Node2D target;
	public Area2D turretRange;
	public Sprite turretSprite;
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
		if (target != null)
		{
			var angleToTarget = GlobalPosition.DirectionTo(target.GlobalPosition).Angle();
			turretSprite.Rotation = angleToTarget;
			if (reloadTimer.IsStopped()) Shoot();
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
	}

	private void Shoot()
	{
		Area2D bullet = (Area2D)bulletScene.Instance();
		bullet.GlobalPosition = GlobalPosition;
		bullet.GlobalRotation = turretSprite.Rotation;
		GetTree().CurrentScene.AddChild(bullet);
		reloadTimer.Start();
	}
}
