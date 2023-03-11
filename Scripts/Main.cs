using Godot;
using System;

public class Main : Node2D
{
    private PackedScene MainMenu = GD.Load<PackedScene>("res://Scenes/MainMenu.tscn");
    private PackedScene LevelContainer = GD.Load<PackedScene>("res://Scenes/LevelContainer.tscn");
    private Node2D MainMenuNode;
    private PathFollow2D BlackBarPath1;
    private PathFollow2D BlackBarPath2;
    private PathFollow2D BlackBarPath3;
    private PathFollow2D BlackBarPath4;
    private PathFollow2D BlackBarPath5;
    private PathFollow2D BlackBarPath6;
    private PathFollow2D BlackBarPath7;
    private PathFollow2D BlackBarPath8;
    private PathFollow2D BlackBarPath9;
    public int BlackBarSpeed = 4000;
    public bool BlackBarsMoving = false;
    public bool BlackBarsHalfway = false;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.AddChild(MainMenu.Instance());
        MainMenuNode = GetNode<Node2D>("MainMenu");
        BlackBarPath1 = GetNode<PathFollow2D>("SceneTransition/BlackBarPath1/PathFollow2D");
        BlackBarPath2 = GetNode<PathFollow2D>("SceneTransition/BlackBarPath2/PathFollow2D");
        BlackBarPath3 = GetNode<PathFollow2D>("SceneTransition/BlackBarPath3/PathFollow2D");
        BlackBarPath4 = GetNode<PathFollow2D>("SceneTransition/BlackBarPath4/PathFollow2D");
        BlackBarPath5 = GetNode<PathFollow2D>("SceneTransition/BlackBarPath5/PathFollow2D");
        BlackBarPath6 = GetNode<PathFollow2D>("SceneTransition/BlackBarPath6/PathFollow2D");
        BlackBarPath7 = GetNode<PathFollow2D>("SceneTransition/BlackBarPath7/PathFollow2D");
        BlackBarPath8 = GetNode<PathFollow2D>("SceneTransition/BlackBarPath8/PathFollow2D");
        BlackBarPath9 = GetNode<PathFollow2D>("SceneTransition/BlackBarPath9/PathFollow2D");
        MainMenuNode.Connect("StartButtonPressed", this, "MainMenuStartButtonPressed");
        MainMenuNode.Connect("ExitButtonPressed", this, "MainMenuExitButtonPressed");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (BlackBarsMoving == true)
        {
            if (BlackBarPath1.UnitOffset >= 0.5 && !BlackBarsHalfway) 
            {
                BlackBarsMoving = false;
                BlackBarsHalfway = true;
            }
            else if (BlackBarPath1.UnitOffset >= 0.99)
            {
                BlackBarsMoving = false;
                BlackBarsHalfway = false;
                ResetSceneTransitionBlackBars();
            }
            else
            {
                MoveSceneTransitionBlackBars(delta);
            }
        }
    }


    private void MoveSceneTransitionBlackBars(float delta)
    {
        BlackBarPath1.Offset = BlackBarPath2.Offset = BlackBarPath3.Offset = BlackBarPath4.Offset = BlackBarPath5.Offset = BlackBarPath6.Offset = BlackBarPath7.Offset = BlackBarPath8.Offset = BlackBarPath9.Offset = BlackBarPath9.Offset + BlackBarSpeed * delta;
    }

    private void ResetSceneTransitionBlackBars()
    {
        BlackBarPath1.Offset = BlackBarPath2.Offset = BlackBarPath3.Offset = BlackBarPath4.Offset = BlackBarPath5.Offset = BlackBarPath6.Offset = BlackBarPath7.Offset = BlackBarPath8.Offset = BlackBarPath9.Offset = 0;
    }
    private void MainMenuStartButtonPressed()
    {
        BlackBarsMoving = true;
    }

    private void MainMenuExitButtonPressed()
    {
        GetTree().Quit();
    }
}
