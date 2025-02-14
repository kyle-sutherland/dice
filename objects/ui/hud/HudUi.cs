using System;
using Godot;

public partial class HudUi : Control
{
    private MenuButton MenuButton { get; set; }
    private Menu Menu { get; set; }
    private MenuButton ThrowButton { get; set; }

    private StringName DiceManagerGroup { get; set; } = "DiceManager";

    private StringName DiceThrowerGroup { get; set; } = "DiceThrower";

    private StringName MarqueeGroup { get; set; } = "Marquee";

    private StringName PlayerGroup { get; set; } = "Player";

    private DiceThrower DiceThrower { get; set; }

    private DiceManager DiceManager { get; set; }

    private Label3D marquee;

    public override void _Ready()
    {
        string diceManagerNodePath = GetTree().GetFirstNodeInGroup(DiceManagerGroup).GetPath();
        DiceManager = GetNodeOrNull<DiceManager>(diceManagerNodePath);
        if (DiceManager == null)
        {
            GD.PushWarning("No dice manager found in group " + DiceManagerGroup);
        }
        Menu = (Menu)GetNode("Menu");
        MenuButton = GetNode<MenuButton>("MenuToggle");
        ThrowButton = GetNode<MenuButton>("ThrowButton");
        MenuButton.Pressed += MenuButtonPressed;
        ThrowButton.Pressed += ThrowButtonPressed;
        string marqueeNodePath = GetTree().GetFirstNodeInGroup(MarqueeGroup).GetPath();
        marquee = GetNodeOrNull<Label3D>(marqueeNodePath);
        string DiceThrowerNodePath = GetTree().GetFirstNodeInGroup(DiceThrowerGroup).GetPath();
        DiceThrower = GetNodeOrNull<DiceThrower>(DiceThrowerNodePath);
        string DiceManagerNodePath = GetTree().GetFirstNodeInGroup(DiceManagerGroup).GetPath();
        DiceManager = GetNodeOrNull<DiceManager>(DiceManagerNodePath);
    }

    public override void _Process(double delta)
    {
        string message = CreateMessage();
        marquee.Text = message;
        GetNodeOrNull<Label>("./DiceLabel").Text = message;
    }

    private void MenuButtonPressed()
    {
        Menu.Visible = !Menu.Visible;
        GD.Print("MenuButtonPressed");
    }

    private void ThrowButtonPressed()
    {
        DiceManager.ClearDice();

        DiceThrower.RollDice();
        GD.Print("ThrowButtonPressed");
    }

    public string CreateMessage()
    {
        string message = "";
        if (DiceManager.D4Parent.GetChildCount() != 0)
        {
            message += $"{DiceManager.D4Parent.GetChildCount()}d4: {DiceManager.D4Total}\n";
        }
        if (DiceManager.D6Parent.GetChildCount() != 0)
        {
            message += $"{DiceManager.D6Parent.GetChildCount()}d6: {DiceManager.D6Total}\n";
        }
        if (DiceManager.D8Parent.GetChildCount() != 0)
        {
            message += $"{DiceManager.D8Parent.GetChildCount()}d8: {DiceManager.D8Total}\n";
        }
        if (DiceManager.D10Parent.GetChildCount() != 0)
        {
            message += $"{DiceManager.D10Parent.GetChildCount()}d10: {DiceManager.D10Total}\n";
        }
        if (DiceManager.D12Parent.GetChildCount() != 0)
        {
            message += $"{DiceManager.D12Parent.GetChildCount()}d12: {DiceManager.D12Total}\n";
        }
        if (DiceManager.D20Parent.GetChildCount() != 0)
        {
            message += $"{DiceManager.D20Parent.GetChildCount()}d20: {DiceManager.D20Total}\n";
        }
        message += $"all: {DiceManager.AllTotal}\n";
        if (DiceManager.D4Check != 0)
        {
            message += $"d4 Pass: {DiceManager.ND4Pass}\n";
        }
        if (DiceManager.D6Check != 0)
        {
            message += $"d6 Pass: {DiceManager.ND6Pass}\n";
        }
        if (DiceManager.D8Check != 0)
        {
            message += $"d8 Pass: {DiceManager.ND8Pass}\n";
        }
        if (DiceManager.D10Check != 0)
        {
            message += $"d10 Pass: {DiceManager.ND10Pass}\n";
        }
        if (DiceManager.D12Check != 0)
        {
            message += $"d12 Pass: {DiceManager.ND12Pass}\n";
        }
        if (DiceManager.D20Check != 0)
        {
            message += $"d20 Pass: {DiceManager.ND20Pass}\n";
        }
        if (DiceManager.AllCheck != 0)
        {
            string pass = DiceManager.AllPass ? "pass" : "fail";
            message += $"check: {DiceManager.AllCheck}, {pass}\n";
        }
        return message;
    }
}
