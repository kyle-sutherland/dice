using System;
using Godot;

public abstract partial class Die : RigidBody3D
{
    public int Value { get; set; }

    public abstract void CheckValue();

    public override void _Process(double delta)
    {
        CheckValue();
    }
}
