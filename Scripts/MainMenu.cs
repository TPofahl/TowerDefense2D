using Godot;
using System;

public class MainMenu : Node2D
{

    [Signal]
    public delegate void StartButtonPressed();
    [Signal]
    public delegate void ExitButtonPressed();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    private void OnMainMenuStartButtonPressed()
    {
        EmitSignal(nameof(StartButtonPressed));
    }

        private void OnMainMenuExitButtonPressed()
    {
        EmitSignal(nameof(ExitButtonPressed));
    }
}
