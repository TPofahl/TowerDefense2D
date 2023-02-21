using Godot;
using System;

public class EnemyPathing : Node2D 
{
	[Export]
	public int EnemySpeed = 100; //80
	public PathFollow2D EnemyPath;
	public Area2D Enemy;
	[Signal]
	public delegate void OnEnemyFinishingPath();
	[Signal]
	public delegate void OnTurretDestroyedEnemy();

	public override void _Ready()
	{
		EnemyPath = GetNode<PathFollow2D>("EnemyLine/EnemyPath");
		Enemy = EnemyPath.GetChild<Area2D>(0);
		Enemy.Connect("EnemyDestroyed", this, "OnEnemyDestroyed");
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
  	public override void _Process(float delta) 
	{
		EnemyPath.Offset = EnemyPath.Offset + EnemySpeed * delta;
		if (EnemyPath.UnitOffset == 1) 
		{
			EmitSignal(nameof(OnEnemyFinishingPath));
			QueueFree();
			EnemyPath.Offset++;
		}
  	}
	
	public void OnEnemyDestroyed()
	{
		EmitSignal(nameof(OnTurretDestroyedEnemy));
		Enemy.Disconnect("EnemyDestroyed", this, "OnEnemyDestroyed");
		QueueFree();
	}
}
