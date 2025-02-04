using Godot;

public partial class DTen : Die
{
    private DTen()
    {
        N = 10;
    }

    //
    //CheckValue override requires different logic for checking value of ten sided die
    public override void CheckValue()
    {
        for (int i = 0; i < ShapeCasts.Length; i++)
        {
            ShapeCast3D shapecast = ShapeCasts[i];
            if (shapecast.IsColliding())
            {
                Value = 0;
                GodotObject collider = shapecast.GetCollider(0);
                if (collider.HasMeta("Name"))
                {
                    var colliderName = collider.GetMeta("Name");
                    if (colliderName.ToString() == "floor")
                    {
                        if (i == 9)
                        {
                            Value = 9;
                        }
                        else
                        {
                            Value = 8 - i;
                        }
                    }
                }
            }
        }
    }
}
