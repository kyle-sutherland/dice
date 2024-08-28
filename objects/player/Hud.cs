using System;
using Godot;

public partial class Hud : CanvasLayer
{
    [Export]
    public Label TotalLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        TotalLabel = GetNode<Label>("VBoxContainer/HBoxContainer/Total");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
