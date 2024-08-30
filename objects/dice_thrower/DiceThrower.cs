using System;
using Godot;

public partial class DiceThrower : Node3D
{
    [Export]
    public Node3D Aimer { get; private set; }

    [Export]
    public StringName ThrowAction { get; private set; } = "Throw";

    [Export]
    public StringName ClearAction { get; private set; } = "Clear";

    [Export]
    public StringName CountAction { get; private set; } = "Count";

    [Export]
    public StringName NextDieAction { get; private set; } = "NextDie";

    [Export]
    public StringName PrevDieAction { get; private set; } = "PrevDie";

    [Export]
    public StringName Selectd4Action { get; private set; } = "Selectd4";

    [Export]
    public StringName Selectd6Action { get; private set; } = "Selectd6";

    [Export]
    public StringName Selectd8Action { get; private set; } = "Selectd8";

    [Export]
    public StringName Selectd10Action { get; private set; } = "Selectd10";

    [Export]
    public StringName Selectd12Action { get; private set; } = "Selectd12";

    [Export]
    public StringName Selectd20Action { get; private set; } = "Selectd20";

    [Export]
    public StringName DiceParentGroup { get; private set; } = "DiceParent";

    [Export]
    public StringName D4ParentGroup { get; private set; } = "D4Spawn";

    [Export]
    public StringName D6ParentGroup { get; private set; } = "D6Spawn";

    [Export]
    public StringName D8ParentGroup { get; private set; } = "D8Spawn";

    [Export]
    public StringName D10ParentGroup { get; private set; } = "D10Spawn";

    [Export]
    public StringName D12ParentGroup { get; private set; } = "D12Spawn";

    [Export]
    public StringName D20ParentGroup { get; private set; } = "D20Spawn";

    [Export]
    public StringName MarqueeGroup { get; private set; } = "Marquee";

    [Export]
    public float LaunchForce { get; private set; } = 1000f;

    [Export]
    public PackedScene[] DieScenes { get; private set; }

    public int SelectedDieIndex { get; private set; } = 0;

    [Export]
    public int D4Check { get; private set; }

    [Export]
    public int D6Check { get; private set; }

    [Export]
    public int D8Check { get; private set; }

    [Export]
    public int D10Check { get; private set; }

    [Export]
    public int D12Check { get; private set; }

    [Export]
    public int D20Check { get; private set; }

    protected PackedScene SelectedDieScene { get; private set; }

    private Node diceParent;

    private Node d4Parent;

    private Node d6Parent;

    private Node d8Parent;

    private Node d10Parent;

    private Node d12Parent;

    private Node d20Parent;

    private Vector3 aimDirection;

    private Label3D marquee;

    public int d4Total { get; private set; }

    public int d6Total { get; private set; }

    public int d8Total { get; private set; }

    public int d10Total { get; private set; }

    public int d12Total { get; private set; }

    public int d20Total { get; private set; }

    public int total { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        diceParent = GetTree().GetFirstNodeInGroup(DiceParentGroup);
        d4Parent = GetTree().GetFirstNodeInGroup(D4ParentGroup);
        d6Parent = GetTree().GetFirstNodeInGroup(D6ParentGroup);
        d8Parent = GetTree().GetFirstNodeInGroup(D8ParentGroup);
        d10Parent = GetTree().GetFirstNodeInGroup(D10ParentGroup);
        d12Parent = GetTree().GetFirstNodeInGroup(D12ParentGroup);
        d20Parent = GetTree().GetFirstNodeInGroup(D20ParentGroup);

        string marqueeNodePath = GetTree().GetFirstNodeInGroup(MarqueeGroup).GetPath();
        marquee = GetNodeOrNull<Label3D>(marqueeNodePath);
        SelectedDieIndex = 1;

        if (diceParent == null)
        {
            GD.PushWarning("No dice parent found in group " + DiceParentGroup);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        d4Total = CountD4();
        d6Total = CountD6();
        d8Total = CountD8();
        d10Total = CountD10();
        d12Total = CountD12();
        d20Total = CountD20();
        marquee.Text = CreateMessage();
        SelectedDieScene = DieScenes[SelectedDieIndex];
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(ThrowAction))
        {
            ThrowDice(SelectedDieScene);
        }
        else if (@event.IsActionPressed(ClearAction))
        {
            ClearDice();
        }
        else if (@event.IsActionPressed(CountAction)) { }
        else if (@event.IsActionPressed(NextDieAction))
        {
            if (SelectedDieIndex == DieScenes.GetLength(0))
            {
                SelectedDieIndex = 0;
                SelectedDieScene = DieScenes[SelectedDieIndex];
            }
            else
            {
                SelectedDieIndex++;
                SelectedDieScene = DieScenes[SelectedDieIndex];
            }
        }
        else if (@event.IsActionPressed(PrevDieAction))
        {
            if (SelectedDieIndex == 0)
            {
                SelectedDieIndex = DieScenes.GetLength(0);
                SelectedDieScene = DieScenes[SelectedDieIndex];
            }
            SelectedDieIndex--;
            SelectedDieScene = DieScenes[SelectedDieIndex];
        }
        else if (@event.IsActionPressed(Selectd4Action))
        {
            SelectedDieIndex = 0;
        }
        else if (@event.IsActionPressed(Selectd6Action))
        {
            SelectedDieIndex = 1;
        }
        else if (@event.IsActionPressed(Selectd8Action))
        {
            SelectedDieIndex = 2;
        }
        else if (@event.IsActionPressed(Selectd10Action))
        {
            SelectedDieIndex = 3;
        }
        else if (@event.IsActionPressed(Selectd12Action))
        {
            SelectedDieIndex = 4;
        }
        else if (@event.IsActionPressed(Selectd20Action))
        {
            SelectedDieIndex = 5;
        }
    }

    public void ThrowDice(PackedScene diceToThrow)
    {
        Die dieInstance = diceToThrow.Instantiate<Die>();
        string dieInstanceTypeName = dieInstance.GetType().ToString();
        var random = new RandomNumberGenerator();
        float aimX = random.RandfRange(-0.2f, -0.7f);
        float aimY = random.RandfRange(-0.2f, 0.2f);
        float aimZ = random.RandfRange(-0.2f, 0.2f);
        float torqueX;
        float torqueY;
        float torqueZ;
        if (dieInstanceTypeName == "DFour")
        {
            torqueX = 0f;
            torqueY = 0f;
            torqueZ = 0f;
        }
        else
        {
            torqueX = random.RandfRange(-140f, 140f);
            torqueY = random.RandfRange(-140f, 140f);
            torqueZ = random.RandfRange(-140f, 140f);
        }
        aimDirection = new Vector3(aimX, aimY, aimZ);
        LaunchForce = random.RandfRange(1200f, 1800f);
        if (dieInstanceTypeName == "DFour")
        {
            d4Parent.AddChild(dieInstance);
        }
        else if (dieInstanceTypeName == "DSix")
        {
            d6Parent.AddChild(dieInstance);
        }
        else if (dieInstanceTypeName == "DEight")
        {
            d8Parent.AddChild(dieInstance);
        }
        else if (dieInstanceTypeName == "DTen")
        {
            d10Parent.AddChild(dieInstance);
        }
        else if (dieInstanceTypeName == "DTwelve")
        {
            d12Parent.AddChild(dieInstance);
        }
        else if (dieInstanceTypeName == "DTwenty")
        {
            d20Parent.AddChild(dieInstance);
        }
        // else
        // {
        //     diceParent.AddChild(dieInstance);
        // }
        dieInstance.GlobalPosition = GlobalPosition;
        Vector3 launchVector = aimDirection * LaunchForce;
        Vector3 launchTorque = new Vector3(torqueX, torqueY, torqueZ);
        dieInstance.ApplyForce(launchVector);
        dieInstance.ApplyTorque(launchTorque);
    }

    public string CreateMessage()
    {
        string message = "";
        if (d4Parent.GetChildCount() != 0)
        {
            message += $"{d4Parent.GetChildCount()}d4: {d4Total}\n";
        }
        if (d6Parent.GetChildCount() != 0)
        {
            message += $"{d6Parent.GetChildCount()}d6: {d6Total}\n";
        }
        if (d8Parent.GetChildCount() != 0)
        {
            message += $"{d8Parent.GetChildCount()}d8: {d8Total}\n";
        }
        if (d10Parent.GetChildCount() != 0)
        {
            message += $"{d10Parent.GetChildCount()}d10: {d10Total}\n";
        }
        if (d12Parent.GetChildCount() != 0)
        {
            message += $"{d12Parent.GetChildCount()}d12: {d12Total}\n";
        }
        if (d20Parent.GetChildCount() != 0)
        {
            message += $"{d20Parent.GetChildCount()}d20: {d20Total}\n";
        }
        return message;
    }

    public void ClearDice()
    {
        var dice =
            d4Parent.GetChildren()
            + d6Parent.GetChildren()
            + d8Parent.GetChildren()
            + d10Parent.GetChildren()
            + d12Parent.GetChildren()
            + d20Parent.GetChildren();
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

    public int CountAllDice()
    {
        total = 0;
        total += CountD4();
        total += CountD6();
        total += CountD8();
        total += CountD10();
        total += CountD12();
        total += CountD20();

        return total;
    }

    public int CountD4()
    {
        int v = 0;
        int total = 0;
        for (int i = 0; i < d4Parent.GetChildCount(); i++)
        {
            NodePath dieNodePath = d4Parent.GetChild(i).GetPath();
            Die die = GetNodeOrNull<Die>(dieNodePath);
            v = die.Value;
            total += v;
        }
        return total;
    }

    public int CountD6()
    {
        int v = 0;
        int total = 0;
        for (int i = 0; i < d6Parent.GetChildCount(); i++)
        {
            NodePath dieNodePath = d6Parent.GetChild(i).GetPath();
            Die die = GetNodeOrNull<Die>(dieNodePath);
            v = die.Value;
            total += v;
        }
        return total;
    }

    public int CountD8()
    {
        int v = 0;
        int total = 0;
        for (int i = 0; i < d8Parent.GetChildCount(); i++)
        {
            NodePath dieNodePath = d8Parent.GetChild(i).GetPath();
            Die die = GetNodeOrNull<Die>(dieNodePath);
            v = die.Value;
            total += v;
        }
        return total;
    }

    public int CountD10()
    {
        int v = 0;
        int total = 0;
        for (int i = 0; i < d10Parent.GetChildCount(); i++)
        {
            NodePath dieNodePath = d10Parent.GetChild(i).GetPath();
            Die die = GetNodeOrNull<Die>(dieNodePath);
            v = die.Value;
            total += v;
        }
        return total;
    }

    public int CountD12()
    {
        int v = 0;
        int total = 0;
        for (int i = 0; i < d12Parent.GetChildCount(); i++)
        {
            NodePath dieNodePath = d12Parent.GetChild(i).GetPath();
            Die die = GetNodeOrNull<Die>(dieNodePath);
            v = die.Value;
            total += v;
        }
        return total;
    }

    public int CountD20()
    {
        int v = 0;
        int total = 0;
        for (int i = 0; i < d20Parent.GetChildCount(); i++)
        {
            NodePath dieNodePath = d20Parent.GetChild(i).GetPath();
            Die die = GetNodeOrNull<Die>(dieNodePath);
            v = die.Value;
            total += v;
        }
        return total;
    }
}
