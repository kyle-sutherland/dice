using Godot;

public partial class DTen : Die
{
    DTen()
    {
        n = 10;
    }

    public override void CheckValue()
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
                        Value = 8 - i;
                    }
                }
            }
        }
    }
}
