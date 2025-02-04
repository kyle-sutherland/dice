using System;
using Godot;

public partial class HudUi : Control
{
    private MenuButton MenuButton { get; set; }
    private Menu Menu { get; set; }

    public override void _Ready()
    {
        Menu = (Menu)GetNode("Menu");
        MenuButton = (MenuButton)GetNode("MenuToggle");
        MenuButton.Pressed += MenuButtonPressed;
    }

    private void MenuButtonPressed()
    {
        Menu.Visible = !Menu.Visible;
        GD.Print("MenuButtonPressed");
    }
}
