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
    public StringName PlayerGroup { get; private set; } = "Player";

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
    public bool Throw { get; private set; } = true;

    [Export]
    public float LaunchForce { get; private set; } = 1000f;

    [Export]
    public PackedScene[] DieScenes { get; private set; } = new PackedScene[6];

    public PackedScene[] ReadyDieScenes { get; private set; } = new PackedScene[100];

    [Export]
    public int[] nDice { get; private set; } = new int[6];

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

    [Export]
    public int AllCheck { get; private set; }

    private Node Player;

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

    public int allTotal { get; private set; }

    public int nD4Pass { get; private set; }

    public int nD6Pass { get; private set; }
    public int nD8Pass { get; private set; }
    public int nD10Pass { get; private set; }
    public int nD12Pass { get; private set; }
    public int nD20Pass { get; private set; }
    public bool allPass { get; private set; }

    public Timer Delay { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Player = GetTree().GetFirstNodeInGroup(PlayerGroup);
        diceParent = GetTree().GetFirstNodeInGroup(DiceParentGroup);
        d4Parent = GetTree().GetFirstNodeInGroup(D4ParentGroup);
        d6Parent = GetTree().GetFirstNodeInGroup(D6ParentGroup);
        d8Parent = GetTree().GetFirstNodeInGroup(D8ParentGroup);
        d10Parent = GetTree().GetFirstNodeInGroup(D10ParentGroup);
        d12Parent = GetTree().GetFirstNodeInGroup(D12ParentGroup);
        d20Parent = GetTree().GetFirstNodeInGroup(D20ParentGroup);
        Delay = GetNode<Timer>("Delay");

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
        SelectedDieScene = DieScenes[SelectedDieIndex];
        d4Total = CountDice("d4");
        d6Total = CountDice("d6");
        d8Total = CountDice("d8");
        d10Total = CountDice("d10");
        d12Total = CountDice("d12");
        d20Total = CountDice("d20");
        allTotal = d4Total + d6Total + d8Total + d10Total + d12Total + d20Total;
        if (D4Check != 0)
        {
            nD4Pass = CheckDice(D4Check, "d4");
        }
        if (D6Check != 0)
        {
            nD6Pass = CheckDice(D6Check, "d6");
        }
        if (D8Check != 0)
        {
            nD8Pass = CheckDice(D8Check, "d8");
        }
        if (D10Check != 0)
        {
            nD10Pass = CheckDice(D10Check, "d10");
        }
        if (D12Check != 0)
        {
            nD12Pass = CheckDice(D12Check, "d12");
        }
        if (D20Check != 0)
        {
            nD20Pass = CheckDice(D20Check, "d20");
        }
        if (AllCheck != 0)
        {
            allPass = CheckAllDice(AllCheck);
        }
        string message = CreateMessage();
        marquee.Text = message;
        Player.GetNode<Label>("./Hud/DiceLabel").Text = message;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(ThrowAction))
        {
            ReadyDice();
            RollDice();
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

    public void RollDice()
    {
        if (Throw)
        {
            ThrowDice(ReadyDieScenes);
        }
        else
        {
            DropDice(ReadyDieScenes);
        }
    }

    public void ReadyDice()
    {
        int nSum = 0;
        for (int i = 0; i < nDice.GetLength(0); i++)
        {
            int n = (int)nDice.GetValue(i);
            for (int j = 0; j < n; j++)
            {
                ReadyDieScenes.SetValue(DieScenes.GetValue(i), j + nSum);
            }
            nSum += n;
        }
        GD.Print(ReadyDieScenes);
    }

    public int GetNAllDice()
    {
        int n = 0;
        foreach (int i in nDice)
        {
            n += i;
        }
        return n;
    }

    public async void ThrowDice(PackedScene[] diceToThrow)
    {
        float positionZOffset = 1.8f;
        float positionYOffset = 1.8f;
        int positionOffsetCount = 0;
        int rowCount = 0;
        int nAllDice = GetNAllDice();

        for (int i = 0; i < nAllDice; i++)
        {
            Delay.Start();
            PackedScene dieToThrow = diceToThrow[i];
            if (dieToThrow != null)
            {
                Vector3 position = GlobalPosition;
                position.Z = position.Z + positionZOffset - positionZOffset * positionOffsetCount;
                position.Y = position.Y + positionYOffset - positionYOffset * rowCount;
                if (positionOffsetCount > 1)
                {
                    positionOffsetCount = 0;
                    if (rowCount > 1)
                    {
                        rowCount = 0;
                    }
                    else
                    {
                        rowCount++;
                    }
                }
                else
                {
                    positionOffsetCount++;
                }
                Die dieInstance = dieToThrow.Instantiate<Die>();
                string dieInstanceTypeName = dieInstance.GetType().ToString();
                RandomNumberGenerator random = new();
                float aimX = random.RandfRange(-0.2f, -0.7f);
                float aimY = random.RandfRange(-0.2f, 0.2f);
                float aimZ = random.RandfRange(-0.2f, 0.2f);
                float torqueX;
                float torqueY;
                float torqueZ;
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
                dieInstance.GlobalPosition = position;
                if (dieInstanceTypeName != "DFour")
                {
                    torqueX = random.RandfRange(-140f, 140f);
                    torqueY = random.RandfRange(-140f, 140f);
                    torqueZ = random.RandfRange(-140f, 140f);
                    Vector3 launchTorque = new(torqueX, torqueY, torqueZ);
                    dieInstance.ApplyTorque(launchTorque);
                }
                Vector3 launchVector = aimDirection * LaunchForce;
                dieInstance.ApplyForce(launchVector);
                if ((i + 1) % 9 == 0)
                {
                    await ToSignal(Delay, "timeout");
                }
            }
        }
        Delay.Stop();
    }

    public async void DropDice(PackedScene[] diceToThrow)
    {
        float positionZOffset = 1.8f;
        float positionXOffset = 1.8f;
        int positionOffsetCount = 0;
        int rowCount = 0;
        int nAllDice = GetNAllDice();

        Delay.Start();
        for (int i = 0; i < nAllDice; i++)
        {
            PackedScene dieToThrow = diceToThrow[i];
            if (dieToThrow != null)
            {
                Vector3 position = GlobalPosition;
                position.Z = position.Z + positionZOffset - positionZOffset * positionOffsetCount;
                position.X = position.X + positionXOffset - positionXOffset * rowCount;
                if (positionOffsetCount > 1)
                {
                    positionOffsetCount = 0;
                    if (rowCount > 1)
                    {
                        rowCount = 0;
                    }
                    else
                    {
                        rowCount++;
                    }
                }
                else
                {
                    positionOffsetCount++;
                }
                Die dieInstance = dieToThrow.Instantiate<Die>();
                string dieInstanceTypeName = dieInstance.GetType().ToString();
                RandomNumberGenerator random = new();
                float torqueX;
                float torqueY;
                float torqueZ;
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
                dieInstance.GlobalPosition = position;
                torqueX = random.RandfRange(-70f, 70f);
                torqueY = random.RandfRange(-70f, 70f);
                torqueZ = random.RandfRange(-70f, 70f);
                Vector3 launchTorque = new Vector3(torqueX, torqueY, torqueZ);
                dieInstance.ApplyTorque(launchTorque);
                if ((i + 1) % 9 == 0)
                {
                    await ToSignal(Delay, "timeout");
                }
            }
        }
        Delay.Stop();
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
        message += $"all: {allTotal}\n";
        if (D4Check != 0)
        {
            message += $"d4 Pass: {nD4Pass}\n";
        }
        if (D6Check != 0)
        {
            message += $"d6 Pass: {nD6Pass}\n";
        }
        if (D8Check != 0)
        {
            message += $"d8 Pass: {nD8Pass}\n";
        }
        if (D10Check != 0)
        {
            message += $"d10 Pass: {nD10Pass}\n";
        }
        if (D12Check != 0)
        {
            message += $"d12 Pass: {nD12Pass}\n";
        }
        if (D20Check != 0)
        {
            message += $"d20 Pass: {nD20Pass}\n";
        }
        if (AllCheck != 0)
        {
            string pass;
            if (allPass)
            {
                pass = "pass";
            }
            else
            {
                pass = "fail";
            }

            message += $"check: {AllCheck}, {pass}\n";
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
        int total = 0;
        total = CountDice("d4");
        total = CountDice("d6");
        total = CountDice("d8");
        total = CountDice("d10");
        total = CountDice("d12");
        total = CountDice("d20");

        return total;
    }

    public int CountDice(string dtype)
    {
        Node dParent = null;
        if (dtype == "d4")
        {
            dParent = d4Parent;
        }
        else if (dtype == "d6")
        {
            dParent = d6Parent;
        }
        else if (dtype == "d8")
        {
            dParent = d8Parent;
        }
        else if (dtype == "d10")
        {
            dParent = d10Parent;
        }
        else if (dtype == "d12")
        {
            dParent = d12Parent;
        }
        else if (dtype == "d20")
        {
            dParent = d20Parent;
        }
        int v = 0;
        int total = 0;
        for (int i = 0; i < dParent.GetChildCount(); i++)
        {
            NodePath dieNodePath = dParent.GetChild(i).GetPath();
            Die die = GetNodeOrNull<Die>(dieNodePath);
            v = die.Value;
            total += v;
        }
        return total;
    }

    public int CheckDice(int check, string dtype)
    {
        Node dParent = null;
        if (dtype == "d4")
        {
            dParent = d4Parent;
        }
        else if (dtype == "d6")
        {
            dParent = d6Parent;
        }
        else if (dtype == "d8")
        {
            dParent = d8Parent;
        }
        else if (dtype == "d10")
        {
            dParent = d12Parent;
        }
        else if (dtype == "d12")
        {
            dParent = d12Parent;
        }
        else if (dtype == "d20")
        {
            dParent = d20Parent;
        }
        int v = 0;
        int n = 0;
        for (int i = 0; i < dParent.GetChildCount(); i++)
        {
            NodePath dieNodePath = dParent.GetChild(i).GetPath();
            Die die = GetNodeOrNull<Die>(dieNodePath);
            v = die.Value;
            if (v >= check)
            {
                n++;
            }
        }
        return n;
    }

    public bool CheckAllDice(int check)
    {
        bool pass = false;
        if (allTotal >= check)
        {
            pass = true;
        }
        return pass;
    }
}
