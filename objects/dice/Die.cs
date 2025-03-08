using Godot;

// Base class for all dice.
public abstract partial class Die : RigidBody3D
{
    //Face-showing value of the die after successful CheckValue
    public int Value { get; set; }

    public Vector3 GravityVector { get; set; }
    public Vector3 AccVector { get; private set; }
    public Vector3 RealAccVector { get; private set; }
    public Vector3 DeviceGravity { get; set; }

    private Vector3 DeviceAcceleration;

    //number of sides the die has
    protected int N { get; set; }

    //Array of dies face shapecasts
    protected ShapeCast3D[] ShapeCasts { get; set; }

    //
    //Checks the face-showing value of the die binds it to member variable Value.
    public virtual void CheckValue()
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
                        Value = N - i;
                    }
                }
            }
        }
    }

    public override void _Ready()
    {
        //Load surface shapecasts into an array for easier iteration.
        ShapeCasts = new ShapeCast3D[N];
        for (int i = 0; i < N; i++)
        {
            ShapeCast3D shapecast = GetNode<ShapeCast3D>($"face{i + 1}");
            ShapeCasts[i] = shapecast;
        }
        GravityScale = 0.0f;
    }

    public override void _PhysicsProcess(double delta)
    {
        DeviceGravity = Input.GetGravity();
        DeviceAcceleration = Input.GetAccelerometer();
        GravityVector = new(DeviceGravity.X, DeviceGravity.Z, DeviceGravity.Y * -1);
    }

    public override void _IntegrateForces(PhysicsDirectBodyState3D state)
    {
        ApplyForce(GravityVector);
    }

    public override void _Process(double delta)
    {
        //The value is checked every frame. If I found a way to reliably check if the dice had stopped
        //moving, it would be more elegant and efficient to call this once when that happens.
        CheckValue();
    }
}
