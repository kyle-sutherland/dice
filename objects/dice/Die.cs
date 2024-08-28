using Godot;

// Base class for all dice
public abstract partial class Die : RigidBody3D
{
    public int Value { get; set; }
    protected int n;
    protected ShapeCast3D[] shapecasts;

    public virtual void CheckValue()
    {
        for (int i = 0; i < shapecasts.Length; i++)
        {
            ShapeCast3D shapecast = shapecasts[i];
            if (shapecast.IsColliding())
            {
                Value = 0;
                GodotObject collider = shapecast.GetCollider(0);
                if (collider.HasMeta("Name"))
                {
                    var colliderName = collider.GetMeta("Name");
                    if (colliderName.ToString() == "floor")
                    {
                        Value = n - i;
                    }
                }
            }
        }
    }

    public override void _Ready()
    {
        shapecasts = new ShapeCast3D[n];
        for (int i = 0; i < n; i++)
        {
            ShapeCast3D shapecast = GetNode<ShapeCast3D>($"face{i + 1}");
            shapecasts[i] = shapecast;
        }
    }

    public override void _Process(double delta)
    {
        CheckValue();
    }
}
