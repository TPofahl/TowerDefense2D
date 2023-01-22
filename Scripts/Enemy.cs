using Godot;
using System;

public class Enemy : Area2D
{
    [Signal]
    public delegate void EnemyDestroyed();
    [Export]
    public int health = 5000;
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
        healthTexture.Visible = false;
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
            health -= Convert.ToInt32(body.EditorDescription);
            healthTexture.Visible = true;
            healthTexture.Value = health;
            if (IsInstanceValid(healthBar))
            {
                if (health <= 0) EmitSignal(nameof(EnemyDestroyed));
            }
        }
	}
}
