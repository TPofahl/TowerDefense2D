using Godot;
using System;

public class Level1 : Node2D
{
	[Export]
	float spawnTime = 2;
	float timer = 0;
	
	//PackedScene enemy = ResourceLoader.Load<PackedScene>("res://Scenes/Enemy.tscn").Instance();
	PackedScene enemy = GD.Load<PackedScene>("res://Scenes/EnemyPathing.tscn");
		
  	// Called every frame. 'delta' is the elapsed time since the previous frame.
  	public override void _Process(float delta) {
	timer = timer + delta;
	if (timer > spawnTime) {
		var newEnemy = enemy.Instance();
		AddChild(newEnemy);
		timer = 0;
		GD.Print("timer reset");
	}
	}
}
