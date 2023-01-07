using Godot;
using System;

public class Level1 : Node2D
{
	[Export]
	public float spawnTime = 2;
	float timer = 0;
	PackedScene enemy = GD.Load<PackedScene>("res://Scenes/EnemyPathing.tscn");
	TileMap backGround;
	TileMap foreGround;



		// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		backGround = GetNode<TileMap>("Background");
		foreGround = GetNode<TileMap>("Background/Foreground");
	}

  	// Called every frame. 'delta' is the elapsed time since the previous frame.
  	public override void _Process(float delta) 
	{
		SpawnEnemy(delta);
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
}
