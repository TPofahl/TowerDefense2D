using Godot;
using System;

public class EnemyPathing : Node2D 
{
	[Export]
	public int EnemySpeed = 80;
	public PathFollow2D EnemyPath;
	public Area2D Enemy;

	public override void _Ready()
	{
		EnemyPath = GetNode<PathFollow2D>("EnemyLine/EnemyPath");
		Enemy = GetNode<Area2D>("EnemyLine/EnemyPath/Enemy");
		Enemy.Connect("EnemyDestroyed", this, "OnEnemyDestroyed");
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
  	public override void _Process(float delta) 
	{
		EnemyPath.SetOffset(EnemyPath.GetOffset() + EnemySpeed * delta);
		if (EnemyPath.GetUnitOffset() == 1) 
		{
			QueueFree();
		}
  	}
	
	public void OnEnemyDestroyed()
	{
		Enemy.Disconnect("EnemyDestroyed", this, "OnEnemyDestroyed");
		QueueFree();
	}
}
