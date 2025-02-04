using System;
using Godot;

public partial class Menu : Control
{
    public LineEdit D4LineEdit { get; private set; }

    public LineEdit D6LineEdit { get; private set; }

    public LineEdit D8LineEdit { get; private set; }

    public LineEdit D10LineEdit { get; private set; }

    public LineEdit D12LineEdit { get; private set; }

    public LineEdit D20LineEdit { get; private set; }

    public override void _Ready()
    {
        D4LineEdit = (LineEdit)GetNode("d4");
        D6LineEdit = (LineEdit)GetNode("d6");
        D8LineEdit = (LineEdit)GetNode("d8");
        D10LineEdit = (LineEdit)GetNode("d10");
        D12LineEdit = (LineEdit)GetNode("d12");
        D20LineEdit = (LineEdit)GetNode("d20");

        D4LineEdit.Text = "0";
        D6LineEdit.Text = "0";
        D8LineEdit.Text = "0";
        D10LineEdit.Text = "0";
        D12LineEdit.Text = "0";
        D20LineEdit.Text = "0";

        D4LineEdit.TextChanged += ValidateDiceLimit;
    }

    private void ValidateDiceLimit(string entry)
    {
        GD.Print(entry);
    }

    public override void _Process(double delta) { }
}
