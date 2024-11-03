using System;
using Godot;

public partial class Floor2 : StaticBody3D
{
    CollisionShape3D CollisionShape;
    MeshInstance3D MeshInstance;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CollisionShape = GetNodeOrNull<CollisionShape3D>("./CollisionShape3D");
        MeshInstance = GetNodeOrNull<MeshInstance3D>("./CollisionShape3D/MeshInstance3D");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
