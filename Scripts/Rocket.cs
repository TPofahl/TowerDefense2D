using Godot;
using System;

public class Rocket : Area2D
{
    [Export]
    int bulletSpeed = 550;
    Vector2 rotation = Vector2.Right;
    Sprite rocketSprite;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        rocketSprite = GetNode<Sprite>("Sprite");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
        var movement = rotation.Rotated(GlobalRotation) * bulletSpeed * delta;
	    GlobalPosition += movement;
        //rocketSprite.RotationDegrees -= 10;
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
