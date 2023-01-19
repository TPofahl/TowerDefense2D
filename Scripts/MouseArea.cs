using Godot;
using System;

public class MouseArea : Area2D
{
    private Area2D target;
    private MeshInstance2D targetMesh;
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("LeftMouse"))
        {
            if (target != null) targetMesh.Visible = !targetMesh.Visible;
        }
    }

    private void OnMouseAreaEntered(Area2D body)
    {
        if (body.IsInGroup("TurretGroup"))
        {
            if (target != null && body.GetInstanceId() != target.GetInstanceId())
            {
                targetMesh.Visible = false;
                target = null;   
            }
            target = body;
            targetMesh = body.GetNode<MeshInstance2D>("Area2D/TurretDetectionArea/TurretDetectionMesh");
        }
    }
    private void OnMouseAreaExited(Area2D body)
    {
        if (body == target)
        {
            targetMesh.Visible = false;
            target = null;
        }
    }
}
