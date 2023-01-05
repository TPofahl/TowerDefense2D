using Godot;
using System;

public class Bullet : Area2D
{
    [Export]
    int bulletSpeed = 550;
    Vector2 rotation = Vector2.Right;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
        var movement = rotation.Rotated(GlobalRotation) * bulletSpeed * delta;
	    GlobalPosition += movement;
	}

    private void OnVisibilityNotifier2DScreenExited()
    {
        QueueFree();
    }

    private void OnBulletAreaEntered(Node body)
    {
        if (body.IsInGroup("EnemyGroup"))
        {
        QueueFree();
        }
    }
}
