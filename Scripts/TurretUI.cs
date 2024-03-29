using Godot;
using System;

public class TurretUI : Node2D
{
    [Signal]
    public delegate void TurretUIExited();
    [Signal]
    public delegate void UpgradeButtonPressed(int currentTurretLevel, int turretCost);
    private Area2D parentNode;
    private Area2D uiArea;
    private RichTextLabel uiTurretLevel;
    private RichTextLabel uiCurrentDamage;
    private RichTextLabel uiNewDamage;
    private Sprite uiDamageArrow;
    private RichTextLabel uiCurrentRange;
    private RichTextLabel uiNewRange;
    private Sprite uiRangeArrow;
    private Button upgradeButton;
    public int turretLevel = 1;
    public int upgradeCost = 100;
    private Node2D levelNode;
    private int currentPlayerMoney = 500;
    private bool isUpgradeEnabled = true;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
       levelNode = GetTree().Root.GetNode<Node2D>("LevelContainer/Level1");
       uiArea = GetNode<Area2D>("Area2D"); 
       uiTurretLevel = GetNode<RichTextLabel>("Background/VBoxContainer/LevelContainer/TurretLevel");
       uiCurrentDamage = GetNode<RichTextLabel>("Background/VBoxContainer/DamageContainer/DamageUpgrade/CurrentDamage");
       uiNewDamage = GetNode<RichTextLabel>("Background/VBoxContainer/DamageContainer/DamageUpgrade/NewDamage");
       uiDamageArrow = GetNode<Sprite>("Background/VBoxContainer/DamageContainer/DamageUpgrade/Arrow");
       uiCurrentRange = GetNode<RichTextLabel>("Background/VBoxContainer/RangeContainer/RangeUpgrade/CurrentRange");
       uiNewRange = GetNode<RichTextLabel>("Background/VBoxContainer/RangeContainer/RangeUpgrade/NewRange");
       uiRangeArrow = GetNode<Sprite>("Background/VBoxContainer/RangeContainer/RangeUpgrade/Arrow");
       upgradeButton = GetNode<Button>("Background/VBoxContainer/Button");
       parentNode = this.GetParent<Area2D>();
       parentNode.Connect("TurretUpgraded", this, "OnTurretUpgraded");
       levelNode.Connect("PlayerMoneyChanged", this, "OnPlayerMoneyChanged");
       upgradeButton.Text = upgradeCost.ToString();
    }

    private void OnUIAreaExited(Area2D body)
    {
        if (body.IsInGroup("MouseGroup2"))
        {
            uiArea.SetDeferred("monitorable", false);
            uiArea.SetDeferred("monitoring", false);
            EmitSignal(nameof(TurretUIExited)); 
        }
    }

    private void OnUpgradeButtonPressed()
    {
        if (turretLevel < 5 && isUpgradeEnabled == true)
        {
            turretLevel++;
            upgradeCost *= 2;
            upgradeButton.Text = upgradeCost.ToString();
            EmitSignal(nameof(UpgradeButtonPressed), turretLevel, upgradeCost);
        }
    }

    private void OnTurretUpgraded(int newRangeText, int newDamageText, int turretUpgradedCost = 0)
    {
        uiTurretLevel.BbcodeText = $"[center]Level {turretLevel}[/center]";
        if (turretLevel < 5)
        {
            int nextRangeUpgrade = (int)(newRangeText * 0.2 + newRangeText);
            int nextDamageUpgrade = (int)(newDamageText + 500);
            uiCurrentRange.BbcodeText = $"[center]{newRangeText}[/center]";
            uiNewRange.BbcodeText = $"[center]{nextRangeUpgrade}[/center]";
            uiCurrentDamage.BbcodeText = $"[center]{newDamageText}[/center]";
            uiNewDamage.BbcodeText = $"[center]{nextDamageUpgrade}[/center]";
        }
        else
        {
            uiCurrentRange.BbcodeText = $"[center]MAX[/center]";
            uiCurrentDamage.BbcodeText = $"[center]MAX[/center]";
            uiNewRange.BbcodeText = $"[center]{newRangeText}[/center]";
            uiNewDamage.BbcodeText = $"[center]{newDamageText}[/center]";
            upgradeButton.Text = "MAX";
            uiDamageArrow.Visible = false;
            uiRangeArrow.Visible = false;
            upgradeButton.Modulate = Color.Color8(130,130,130,200);
            upgradeButton.Disabled = true;
        }
    }

    private void OnPlayerMoneyChanged(string newPlayerMoney)
    {
       UpdateButtonStatus(newPlayerMoney);
    }

    private void UpdateButtonStatus(string newPlayerMoney)
    {
        int playerMoney = Convert.ToInt32(newPlayerMoney);
        currentPlayerMoney = playerMoney;
        if (playerMoney < upgradeCost)
        {
            upgradeButton.Modulate = Color.Color8(255,50,50,255); //Red.
            isUpgradeEnabled = false;
        }
        else
        {
            upgradeButton.Modulate = Color.Color8(134,255,50,255); //Green.
            isUpgradeEnabled = true;
        }
    }
}