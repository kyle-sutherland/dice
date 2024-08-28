using Godot;

public partial class DiceThrower : Node3D
{
    [Export]
    public Node3D Aimer { get; private set; }

    [Export]
    public PackedScene DieScene { get; private set; }

    [Export]
    public StringName ThrowAction { get; private set; } = "Throw";

    [Export]
    public StringName ClearAction { get; private set; } = "Clear";

    [Export]
    public StringName CountAction { get; private set; } = "Count";

    [Export]
    public StringName DiceParentGroup { get; private set; } = "DiceParent";

    [Export]
    public StringName MarqueeGroup { get; private set; } = "Marquee";

    [Export]
    public float LaunchForce { get; private set; } = 1000f;

    private Node diceParent;

    private Vector3 aimDirection;

    private Label3D marquee;

    public Die[] AllDice { get; private set; }

    public int dSixTotal { get; private set; }

    public int total { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        diceParent = GetTree().GetFirstNodeInGroup(DiceParentGroup);
        string marqueeNodePath = GetTree().GetFirstNodeInGroup(MarqueeGroup).GetPath();
        marquee = GetNodeOrNull<Label3D>(marqueeNodePath);

        if (diceParent == null)
        {
            GD.PushWarning("No dice parent found in group " + DiceParentGroup);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        CountAllDice();
        marquee.Text = $"{diceParent.GetChildCount()}d6 total: {total}";
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(ThrowAction))
        {
            ThrowDice(DieScene);
        }
        else if (@event.IsActionPressed(ClearAction))
        {
            ClearDice();
        }
        else if (@event.IsActionPressed(CountAction)) { }
    }

    public void ThrowDice(PackedScene diceToThrow)
    {
        var random = new RandomNumberGenerator();
        float aimX = random.RandfRange(-0.2f, -0.7f);
        float aimY = random.RandfRange(-0.2f, 0.2f);
        float aimZ = random.RandfRange(-0.2f, 0.2f);
        float torqueX = random.RandfRange(-150f, 150f);
        float torqueY = random.RandfRange(-150f, 150f);
        float torqueZ = random.RandfRange(-150f, 150f);
        aimDirection = new Vector3(aimX, aimY, aimZ);
        LaunchForce = random.RandfRange(1200f, 1800f);
        Die dieInstance = diceToThrow.Instantiate<Die>();
        diceParent.AddChild(dieInstance);
        dieInstance.GlobalPosition = GlobalPosition;
        Vector3 launchVector = aimDirection * LaunchForce;
        Vector3 launchTorque = new Vector3(torqueX, torqueY, torqueZ);
        dieInstance.ApplyForce(launchVector);
        dieInstance.ApplyTorque(launchTorque);
        AllDice.SetValue(dieInstance, diceParent.GetChildCount());
    }

    public void ClearDice()
    {
        var dice = diceParent.GetChildren();
        foreach (Node die in dice)
        {
            die.QueueFree();
        }
    }

    public int CountDice(Die[] dice)
    {
        int total = 0;
        foreach (Die die in dice)
        {
            total += die.Value;
        }
        return total;
    }

    public void CountAllDice()
    {
        total = 0;
        dSixTotal = 0;
        int v = 0;
        for (int i = 0; i < diceParent.GetChildCount(); i++)
        {
            NodePath dieNodePath = diceParent.GetChild(i).GetPath();
            Die die = GetNodeOrNull<Die>(dieNodePath);
            v = die.Value;
            total += v;
            if ((string)die.GetMeta("type") == "d6")
            {
                dSixTotal += v;
            }
        }
    }
}
