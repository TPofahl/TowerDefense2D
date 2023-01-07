using Godot;
using System;

public class Enemy : Area2D
{
    [Signal]
    delegate void EnemyDestroyed();
    [Export]
    public int health = 100;
    [Export]
    public Vector2 healthBarOffset = new Vector2(-25,35);
    TextureProgress healthTexture;
    Node2D healthBar;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        healthTexture = GetNode<TextureProgress>("HealthBar/TextureProgress");
        healthBar = GetNode<Node2D>("HealthBar");
        healthTexture.Value = health;

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
    healthBar.GlobalPosition = GlobalPosition + healthBarOffset;
    healthBar.GlobalRotation = 0;

  }

    private void OnEnemyAreaEntered(Node body)
	{
        if (body.IsInGroup("BulletGroup"))
        {
            health -= 10;
            healthTexture.Value = health;
            if (health <= 0) EmitSignal(nameof(EnemyDestroyed));
        }
	}
}
