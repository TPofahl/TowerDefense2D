using Godot;
using System;

public class MainMenuTurretPathing : Node2D
{
    public PathFollow2D menuTurret;
    public Sprite menuTurretBaseSprite;
    public Sprite menuTurretSprite;
    public Texture machineTurretSprite = GD.Load<Texture>("res://Assets/png/towerDefense_tile203.png");
    public Texture singleCannonSprite = GD.Load<Texture>("res://Assets/png/towerDefense_tile249.png");
    public Texture doubleCannonSprite = GD.Load<Texture>("res://Assets/png/towerDefense_tile250.png");
    public Texture rocketTurretSprite = GD.Load<Texture>("res://Assets/png/towerDefense_tile204.png");
    public int turretSpeed = 80;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        menuTurret = GetNode<PathFollow2D>("Path2D/PathFollow2D");
        menuTurretBaseSprite = GetNode<Sprite>("Path2D/PathFollow2D/MainMenuTurret/TurretBase");
        menuTurretSprite = GetNode<Sprite>("Path2D/PathFollow2D/MainMenuTurret/Turret");
        RandomizeTurret();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        menuTurret.Offset = menuTurret.Offset + turretSpeed * delta;
        menuTurretSprite.Rotation += (float)0.007;
		if (menuTurret.UnitOffset == 1) 
		{
			QueueFree();
			menuTurret.Offset++;
		}
    }

    private void RandomizeTurret()
    {
        RandomizeTurretSprite();
        RandomizeTurretBaseColor();
    }

    private void RandomizeTurretBaseColor()
    {
        Random random = new Random();
        int randomNum = random.Next(0,6);
        switch (randomNum)
        {
            case 0:
            {
                menuTurretBaseSprite.Modulate = Color.Color8(247,2,2,255);// Red
                break;
            }
            case 1:
            {
                menuTurretBaseSprite.Modulate = Color.Color8(252,143,3,255); //orange
                break;
            }
            case 2:
            {
                menuTurretBaseSprite.Modulate = Color.Color8(189,186,8,255);// Yellow
                break;
            }
            case 3:
            {
                menuTurretBaseSprite.Modulate = Color.Color8(109,209,2,255); //green
                break;
            }
            case 4:
            {
                menuTurretBaseSprite.Modulate = Color.Color8(29,144,255,255); //blue
                break;
            }
            case 5:
            {
                menuTurretBaseSprite.Modulate = Color.Color8(244,3,252,255); //purple
                break;
            }
            default:
            {
                throw new Exception("Invalid color for MenuTurretBase in MainMenuTurretPathing.cs");
            }
        }
    }

    private void RandomizeTurretSprite()
    {
        Random random = new Random();
        int randomNum = random.Next(0,4);
        switch (randomNum)
        {
            case 0:
            {
                menuTurretSprite.Texture = machineTurretSprite;
                break;
            }
            case 1:
            {
                menuTurretSprite.Texture = singleCannonSprite;
                break;
            }
            case 2:
            {
                menuTurretSprite.Texture = doubleCannonSprite;
                break;
            }
            case 3:
            {
                menuTurretSprite.Texture = rocketTurretSprite;
                break;
            }
            default:
            {
                throw new Exception("Invalid sprite for MenuTurret in MainMenuTurretPathing.cs");
            }
        }
    }
}
