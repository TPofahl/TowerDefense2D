using Godot;
using System;

public class LevelContainer : Node2D
{
    private Node2D level;
    private SceneTree levelTree;
    private Node2D pauseBackground;
    public override void _Ready()
    {
        level = GetNode<Node2D>("Level1");
        pauseBackground = GetNode<Node2D>("PauseBackgroundContainer");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
    {
        levelTree = level.GetTree();
        if (Input.IsActionJustPressed("EscKey"))
        {
            levelTree = level.GetTree();
            levelTree.Paused = !levelTree.Paused;
            pauseBackground.Visible = !pauseBackground.Visible;
        }

        if (Input.IsActionJustPressed("PrintStrayNodes"))
        {
            PrintStrayNodes();
        }
    }
}
