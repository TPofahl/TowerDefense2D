using Godot;
using System;

public class TurretUI : Node2D
{
    [Signal]
    public delegate void TurretUIExited();
    private Area2D uiArea;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
       uiArea = GetNode<Area2D>("Area2D"); 
    }

    private void OnUIAreaExited(Area2D body)
    {
        if (body.IsInGroup("MouseGroup2"))
        {
            uiArea.SetDeferred("monitorable", false);
            uiArea.SetDeferred("monitoring", false);
            EmitSignal(nameof(TurretUIExited)); 
        }
    }
}
