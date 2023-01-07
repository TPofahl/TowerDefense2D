using Godot;
using System;

public class GrassBuyPlot : Area2D
{
    PackedScene turret = GD.Load<PackedScene>("res://Scenes/Turret.tscn");

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void OnGrassBuyPlotInputEvent(Viewport viewport, InputEvent inputEvent, int shapeIdx)
    {
        if (Input.IsActionJustPressed("click") && this.GetOverlappingAreas().Count == 0)
		{
            var newTurret = turret.Instance();
            AddChild(newTurret);
		} 
    }
}
