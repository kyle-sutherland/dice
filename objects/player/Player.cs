using System;
using Godot;

public partial class Player : CharacterBody3D
{
    [Export]
    public StringName ShowCursorAction { get; private set; } = "ShowCursor";

    [Export]
    public float Speed { get; set; } = 5.0f;

    [Export]
    public float Sensitivity { get; set; } = 0.01f;

    public bool LockCamera { get; set; }

    public Camera3D Camera { get; private set; }
    public Node3D Head { get; private set; }

    public override void _Ready()
    {
        Head = GetNode<Node3D>("Head");
        Camera = GetNode<Camera3D>("Head/Camera3D");
        LockCamera = false;

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
        float inputeDirZ = 0f;
        if (Input.IsActionPressed("up"))
        {
            inputeDirZ = 1f;
        }
        if (Input.IsActionPressed("down"))
        {
            inputeDirZ = -1f;
        }
        Vector3 direction = (
            Transform.Basis * new Vector3(inputDir.X, inputeDirZ, inputDir.Y)
        ).Normalized();
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Y = direction.Y * Speed;
            velocity.Z = direction.Z * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion && !LockCamera)
        {
            InputEventMouseMotion mouseMotion = @event as InputEventMouseMotion;
            RotateY(-mouseMotion.Relative.X * Sensitivity);
            Camera.RotateX(-mouseMotion.Relative.Y * Sensitivity);
        }
        else if (@event.IsActionPressed(ShowCursorAction))
        {
            ShowCursor();
        }
    }

    private void ShowCursor()
    {
        if (Input.MouseMode == Input.MouseModeEnum.Hidden)
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
            LockCamera = true;
        }
        else
        {
            Input.MouseMode = Input.MouseModeEnum.Hidden;
            LockCamera = false;
        }
    }
}
