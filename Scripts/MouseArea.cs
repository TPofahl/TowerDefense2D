using Godot;
using System;

public class MouseArea : Area2D
{
    private Area2D target;
    private MeshInstance2D targetMesh;
    private Node2D targetUI;
    private Area2D targetUIArea;
    private Area2D lastTarget;
    private bool targetChanged = false;

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("LeftMouse"))
        {
            if (target != null && targetChanged)
            {
                targetMesh.Visible = !targetMesh.Visible;
                targetUI.Visible = !targetUI.Visible;
                if (targetUIArea.Monitorable == true)
                {
                    targetUIArea.SetDeferred("monitorable", false);
                    targetUIArea.SetDeferred("monitoring", false);
                }
                else 
                {
                    targetUIArea.SetDeferred("monitorable", true);
                    targetUIArea.SetDeferred("monitoring", true);
                }
            }
        }
    }

    private void OnMouseAreaEntered(Area2D body)
    {
        bool bodyIsTurret = body.IsInGroup("TurretGroup");
        bool bodyIsTurretUI = body.IsInGroup("TurretUIGroup");
        if (bodyIsTurret || bodyIsTurretUI)
        {
            if (body.IsInGroup("TurretUIGroup") && target != null && body.GetInstanceId() != target.GetInstanceId()) 
            {
                targetMesh.Visible = false;
                targetUI.Visible = false;
                target = null; 
            }
            if (bodyIsTurret)
            {
                target = body;
                targetMesh = body.GetNode<MeshInstance2D>("Area2D/TurretDetectionArea/TurretDetectionMesh");
                targetUI = body.GetNode<Node2D>("TurretUI");
                targetUIArea = body.GetNode<Area2D>("TurretUI/Area2D");
                targetChanged = true;
            }
        }
    }

    private void OnMouseAreaExited(Area2D body)
    {
        if (body.IsInGroup("TurretGroup"))
        {
            if (body.GetInstanceId() == target.GetInstanceId()) targetChanged = false;
        }
    }
}
