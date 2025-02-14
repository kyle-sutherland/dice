using System;
using Godot;

public partial class Player : CharacterBody3D
{
    [Export]
    public StringName ShowCursorAction { get; private set; } = "ShowCursor";

    public Camera3D Camera { get; set; }

    public Node3D Head { get; set; }

    public override void _Ready()
    {
        // Head = GetNode<Node3D>("Head");
        // Camera = GetNode<Camera3D>("Head/Camera3D");

        // Input.MouseMode = Input.MouseModeEnum.Captured;
        // Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    public override void _PhysicsProcess(double delta) { }

    // public override void _Input(InputEvent @event)
    // {
    //     if (@event.IsActionPressed(ShowCursorAction))
    //     {
    //         ShowCursor();
    //     }
    // }

    // private static void ShowCursor()
    // {
    //     Input.MouseMode =
    //         Input.MouseMode != Input.MouseModeEnum.Hidden
    //             ? Input.MouseModeEnum.Hidden
    //             : Input.MouseModeEnum.Visible;
    // }
}
