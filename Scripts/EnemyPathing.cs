using Godot;
using System;

public class EnemyPathing : Node2D 
{
	[Export]
	public int EnemySpeed = 100;
	public PathFollow2D EnemyPath;


	public override void _Ready()
	{
		EnemyPath = GetNode<PathFollow2D>("EnemyLine/EnemyPath");
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
}
