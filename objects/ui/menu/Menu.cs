using System;
using Godot;

public partial class Menu : Control
{
    public SpinBox D4SpinBox { get; private set; }

    public SpinBox D6SpinBox { get; private set; }

    public SpinBox D8SpinBox { get; private set; }

    public SpinBox D10SpinBox { get; private set; }

    public SpinBox D12SpinBox { get; private set; }

    public SpinBox D20SpinBox { get; private set; }

    public override void _Ready()
    {
        D4SpinBox = (SpinBox)GetNode("d4");
        D6SpinBox = (SpinBox)GetNode("d6");
        D8SpinBox = (SpinBox)GetNode("d8");
        D10SpinBox = (SpinBox)GetNode("d10");
        D12SpinBox = (SpinBox)GetNode("d12");
        D20SpinBox = (SpinBox)GetNode("d20");

        D4SpinBox.Value = 0;
        D6SpinBox.Value = 0;
        D8SpinBox.Value = 0;
        D10SpinBox.Value = 0;
        D12SpinBox.Value = 0;
        D20SpinBox.Value = 0;

        D4SpinBox.ValueChanged += ValidateDiceLimit;
    }

    private void ValidateDiceLimit(double entry)
    {
        GD.Print(entry);
    }

    public override void _Process(double delta) { }
}
