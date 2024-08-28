using System;
using Godot;

public partial class DSix : Die
{
    public static int n = 6;
    public RayCast3D[] rays = new RayCast3D[n];

    public override void CheckValue()
    {
        for (int i = 0; i < rays.Length; i++)
        {
            RayCast3D ray = rays[i];
            GodotObject collider = ray.GetCollider();
            if (collider != null)
            {
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
            RayCast3D ray = GetNode<RayCast3D>($"face{i + 1}");
            rays.SetValue(ray, i);
        }
    }
}
