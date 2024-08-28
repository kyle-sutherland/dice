using System;
using Godot;

public partial class DFour : Die
{
    public static int n = 4;
    public ShapeCast3D[] shapecasts = new ShapeCast3D[n];

    public override void CheckValue()
    {
        for (int i = 0; i < shapecasts.Length; i++)
        {
            ShapeCast3D shapecast = shapecasts[i];
            if (shapecast.IsColliding())
            {
                GodotObject collider = shapecast.GetCollider(0);
                var colliderName = collider.GetMeta("Name");
                if (colliderName.ToString() == "floor")
                {
                    Value = n - i;
                }
                else
                {
                    Value = 0;
                }
            }
        }
    }

    public override void _Ready()
    {
        for (int i = 0; i < n; i++)
        {
            ShapeCast3D shapecast = GetNode<ShapeCast3D>($"face{i + 1}");
            shapecasts.SetValue(shapecast, i);
        }
    }
}
