using Godot;
using System;

public class Enemy : Area2D
{
    [Signal]
    delegate void EnemyDestroyed();
    int health = 100;
    TextureProgress healthBar;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        healthBar = GetNode<TextureProgress>("HealthBar/TextureProgress");
        healthBar.Value = health;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    private void OnEnemyAreaEntered(Node body)
	{
        if (body.IsInGroup("BulletGroup"))
        {
            health -= 10;
            healthBar.Value = health;
            if (health <= 0) EmitSignal(nameof(EnemyDestroyed));
        }
	}
}
