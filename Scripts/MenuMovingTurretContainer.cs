using Godot;
using System;

public class MenuMovingTurretContainer : Node2D
{
    PackedScene menuTurretPathingNode = GD.Load<PackedScene>("res://Scenes/MainMenuTurretPathing.tscn");
    public float spawnTime = 1.5F;
    float timer = 0;
    int[,] turretSpawnPoints = new int[23,2]
    {
        // 1st half is cannons spawn points on bottom of screen.
        {-96,224}, {-96,480}, {-96,736}, {-96,992}, {-32,1184}, {224, 1184}, {480,1184}, {736,1184}, {992, 1184}, {1248,1184}, {1504,1184}, {1760,1184},
        {352,-96}, {608,-96}, {864,-96}, {1120,-96}, {1376,-96}, {1632,-96}, {1888,-96}, {2016,32}, {2016,288}, {2016,544}, {2016,800}
    }; 

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        timer = timer + delta;
		if (timer > spawnTime) 
		{
            for (int i = 0; i < turretSpawnPoints.Length / 2; i++)
            {
                Node2D newTurret = menuTurretPathingNode.Instance<Node2D>();
                newTurret.GlobalPosition = new Vector2(turretSpawnPoints[i,0], turretSpawnPoints[i,1]);
                if (i > 11)
                {
                    newTurret.RotationDegrees += 180;
                }
                AddChild(newTurret);
            }
			timer = 0;
		}
    }
}
