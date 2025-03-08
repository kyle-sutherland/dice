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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public override void _PhysicsProcess(double delta)
    {
        DevGyroscope = Input.GetGyroscope();
        DevAcceleration = Input.GetAccelerometer();
        DevDeltaRotation = ComputeDeltaRotation(delta, DevGyroscope);
        DevDeltaPosition = ComputeDeltaPosition(delta, DevAcceleration - DevGravity);
        RotateObjectLocal(new(0, 0, 1), DevDeltaRotation.X * -1);
        RotateObjectLocal(new(1, 0, 0), DevDeltaRotation.Y * -1);
        RotateObjectLocal(new(0, 1, 0), DevDeltaRotation.Z);
        // TranslateObjectLocal(DevDeltaPosition);
    }

    private static Vector3 ComputeDeltaRotation(double delta, Vector3 gyro)
    {
        Vector3 DeltaRotation;
        float d = (float)delta;
        DeltaRotation = gyro * d;
        return DeltaRotation;
    }

    private static Vector3 ComputeDeltaPosition(double delta, Vector3 accel)
    {
        Vector3 DeltaPosition;
        float d = (float)delta;
        DeltaPosition = accel * d;
        return DeltaPosition;
    }
}
