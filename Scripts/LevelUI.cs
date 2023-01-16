using Godot;
using System;

public class LevelUI : Control
{
    [Signal]
    public delegate void TurretUIButtonToggled(string buttonType);

    public void OnTurret1Pressed()
    {
        EmitSignal(nameof(TurretUIButtonToggled), "MachineTurret");
    }
    public void OnTurret2Pressed()
    {
        EmitSignal(nameof(TurretUIButtonToggled), "SingleCannon");
    }
    public void OnTurret3Pressed()
    {
        EmitSignal(nameof(TurretUIButtonToggled), "DoubleCannon");
    }
    public void OnTurret4Pressed()
    {
        EmitSignal(nameof(TurretUIButtonToggled), "RocketTurret");
    }
}
