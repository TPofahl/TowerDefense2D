using Godot;
using System;

public class TimerUI : Control
{
    private Label timerText;
    public Timer timer;
    private int seconds = 0;
    private int minutes = 0;
    private int hours = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        timerText = GetNode<Label>("Background/TimeText");
        timer = GetNode<Timer>("Timer");
        timer.Start();
    }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
    if (timer.IsStopped())
    {
        UpdateTime();
    }
  }

  private void UpdateTime()
  {
    seconds++;
    if (seconds >= 60)
    {
        minutes++;
        seconds = 0;
    }
    if (minutes >= 60)
    {
        hours++;
        minutes = 0;
    }
    if (hours > 0)
    {
        timerText.Text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    } else
    {
        timerText.Text = $"{minutes:D2}:{seconds:D2}";
    }
    timer.Start();
  }
}
