using System;
using Godot;

public partial class HudUi : Control
{
    [Export]
    public Label diceLabel { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        diceLabel = GetNode<Label>("DiceLabel");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
