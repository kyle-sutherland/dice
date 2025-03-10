using System;
using Godot;

public partial class LevelRoot : StaticBody3D
{
    [Export]
    public Vector3 DevGravity { get; private set; }

    [Export]
    public Vector3 DevAcceleration { get; private set; }

    [Export]
    public Vector3 DevGyroscope { get; private set; }

    [Export]
    public Vector3 DevDeltaRotation { get; private set; }

    public Vector3 DevDeltaPosition { get; private set; }

    public bool IsDeviceLevelFaceUp { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        DevGyroscope = Input.GetGyroscope();
        DevAcceleration = Input.GetAccelerometer();
        DevDeltaRotation = ComputeDeltaRotation(delta, DevGyroscope);
        DevDeltaPosition = ComputeDeltaPosition(delta, DevAcceleration - DevGravity);
        IsDeviceLevelFaceUp = IsDevAccelDown(DevAcceleration);
    }

    public override void _PhysicsProcess(double delta)
    {
        RotateObjectLocal(new(1, 0, 0), DevDeltaRotation.Y * -1);
        RotateObjectLocal(new(0, 1, 0), DevDeltaRotation.Z);
        RotateObjectLocal(new(0, 0, 1), DevDeltaRotation.X * -1);

        TranslateObjectLocal(DevDeltaPosition);
    }

    private static void LevelWorld()
    {
        Vector3 up = Vector3.Up;
    }

    private static Vector3 ComputeDeltaRotation(double delta, Vector3 gyro)
    {
        Vector3 DeltaRotation;
        float d = (float)delta;
        DeltaRotation = gyro * d;
        return DeltaRotation;
    }

    private static bool IsDevAccelDown(Vector3 acc)
    {
        Vector3 down = new(0, -9.8f, 0);
        Vector3 accNormal = acc;
        return down.Y >= acc.Y;
    }

    private static Vector3 ComputeDeltaPosition(double delta, Vector3 accel)
    {
        Vector3 DeltaPosition;
        float d = (float)delta;
        DeltaPosition = accel * d;
        return DeltaPosition;
    }
}
