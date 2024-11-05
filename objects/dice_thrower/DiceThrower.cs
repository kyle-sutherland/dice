using System;
using Godot;

public partial class DiceThrower : Node3D
{
    [Export]
    public bool Throw { get; private set; } = true;

    [Export]
    public float LaunchForce { get; private set; } = 1000f;

    [Export]
    public PackedScene[] DieScenes { get; private set; } = new PackedScene[6];

    public PackedScene[] ReadyDieScenes { get; private set; } = new PackedScene[100];

    [Export]
    public int[] NDice { get; private set; } = new int[6];

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

    public int D4Total { get; private set; }

    public int D6Total { get; private set; }

    public int D8Total { get; private set; }

    public int D10Total { get; private set; }

    public int D12Total { get; private set; }

    public int D20Total { get; private set; }

    public int AllTotal { get; private set; }

    public int ND4Pass { get; private set; }

    public int ND6Pass { get; private set; }

    public int ND8Pass { get; private set; }

    public int ND10Pass { get; private set; }

    public int ND12Pass { get; private set; }

    public int ND20Pass { get; private set; }

    public bool AllPass { get; private set; }

    public Timer Delay { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Player = GetTree().GetFirstNodeInGroup(Groups.PlayerGroup);
        diceParent = GetTree().GetFirstNodeInGroup(Groups.DiceParentGroup);
        d4Parent = GetTree().GetFirstNodeInGroup(Groups.D4ParentGroup);
        d6Parent = GetTree().GetFirstNodeInGroup(Groups.D6ParentGroup);
        d8Parent = GetTree().GetFirstNodeInGroup(Groups.D8ParentGroup);
        d10Parent = GetTree().GetFirstNodeInGroup(Groups.D10ParentGroup);
        d12Parent = GetTree().GetFirstNodeInGroup(Groups.D12ParentGroup);
        d20Parent = GetTree().GetFirstNodeInGroup(Groups.D20ParentGroup);
        Delay = GetNode<Timer>("Delay");

        string marqueeNodePath = GetTree().GetFirstNodeInGroup(Groups.MarqueeGroup).GetPath();
        marquee = GetNodeOrNull<Label3D>(marqueeNodePath);
        SelectedDieIndex = 1;

        if (diceParent == null)
        {
            GD.PushWarning("No dice parent found in group " + Groups.DiceParentGroup);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        SelectedDieScene = DieScenes[SelectedDieIndex];
        D4Total = CountDice("d4");
        D6Total = CountDice("d6");
        D8Total = CountDice("d8");
        D10Total = CountDice("d10");
        D12Total = CountDice("d12");
        D20Total = CountDice("d20");
        AllTotal = D4Total + D6Total + D8Total + D10Total + D12Total + D20Total;
        if (D4Check != 0)
        {
            ND4Pass = CheckDice(D4Check, "d4");
        }
        if (D6Check != 0)
        {
            ND6Pass = CheckDice(D6Check, "d6");
        }
        if (D8Check != 0)
        {
            ND8Pass = CheckDice(D8Check, "d8");
        }
        if (D10Check != 0)
        {
            ND10Pass = CheckDice(D10Check, "d10");
        }
        if (D12Check != 0)
        {
            ND12Pass = CheckDice(D12Check, "d12");
        }
        if (D20Check != 0)
        {
            ND20Pass = CheckDice(D20Check, "d20");
        }
        if (AllCheck != 0)
        {
            AllPass = CheckAllDice(AllCheck);
        }
        string message = CreateMessage();
        marquee.Text = message;
        Player.GetNode<Label>("./Hud/DiceLabel").Text = message;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(Actions.ThrowAction))
        {
            ReadyDice();
            RollDice();
        }
        else if (@event.IsActionPressed(Actions.ClearAction))
        {
            ClearDice();
        }
        else if (@event.IsActionPressed(Actions.CountAction)) { }
        else if (@event.IsActionPressed(Actions.NextDieAction))
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
        else if (@event.IsActionPressed(Actions.PrevDieAction))
        {
            if (SelectedDieIndex == 0)
            {
                SelectedDieIndex = DieScenes.GetLength(0);
                SelectedDieScene = DieScenes[SelectedDieIndex];
            }
            SelectedDieIndex--;
            SelectedDieScene = DieScenes[SelectedDieIndex];
        }
        else if (@event.IsActionPressed(Actions.Selectd4Action))
        {
            SelectedDieIndex = 0;
        }
        else if (@event.IsActionPressed(Actions.Selectd6Action))
        {
            SelectedDieIndex = 1;
        }
        else if (@event.IsActionPressed(Actions.Selectd8Action))
        {
            SelectedDieIndex = 2;
        }
        else if (@event.IsActionPressed(Actions.Selectd10Action))
        {
            SelectedDieIndex = 3;
        }
        else if (@event.IsActionPressed(Actions.Selectd12Action))
        {
            SelectedDieIndex = 4;
        }
        else if (@event.IsActionPressed(Actions.Selectd20Action))
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
        for (int i = 0; i < NDice.GetLength(0); i++)
        {
            int n = (int)NDice.GetValue(i);
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
        foreach (int i in NDice)
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
                position.Z = position.Z + positionZOffset - (positionZOffset * positionOffsetCount);
                position.Y = position.Y + positionYOffset - (positionYOffset * rowCount);
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
                position.Z = position.Z + positionZOffset - (positionZOffset * positionOffsetCount);
                position.X = position.X + positionXOffset - (positionXOffset * rowCount);
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
                Vector3 launchTorque = new(torqueX, torqueY, torqueZ);
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
            message += $"{d4Parent.GetChildCount()}d4: {D4Total}\n";
        }
        if (d6Parent.GetChildCount() != 0)
        {
            message += $"{d6Parent.GetChildCount()}d6: {D6Total}\n";
        }
        if (d8Parent.GetChildCount() != 0)
        {
            message += $"{d8Parent.GetChildCount()}d8: {D8Total}\n";
        }
        if (d10Parent.GetChildCount() != 0)
        {
            message += $"{d10Parent.GetChildCount()}d10: {D10Total}\n";
        }
        if (d12Parent.GetChildCount() != 0)
        {
            message += $"{d12Parent.GetChildCount()}d12: {D12Total}\n";
        }
        if (d20Parent.GetChildCount() != 0)
        {
            message += $"{d20Parent.GetChildCount()}d20: {D20Total}\n";
        }
        message += $"all: {AllTotal}\n";
        if (D4Check != 0)
        {
            message += $"d4 Pass: {ND4Pass}\n";
        }
        if (D6Check != 0)
        {
            message += $"d6 Pass: {ND6Pass}\n";
        }
        if (D8Check != 0)
        {
            message += $"d8 Pass: {ND8Pass}\n";
        }
        if (D10Check != 0)
        {
            message += $"d10 Pass: {ND10Pass}\n";
        }
        if (D12Check != 0)
        {
            message += $"d12 Pass: {ND12Pass}\n";
        }
        if (D20Check != 0)
        {
            message += $"d20 Pass: {ND20Pass}\n";
        }
        if (AllCheck != 0)
        {
            string pass = AllPass ? "pass" : "fail";
            message += $"check: {AllCheck}, {pass}\n";
        }
        return message;
    }

    public void ClearDice()
    {
        Godot.Collections.Array<Node> dice =
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

    public static int CountDice(Die[] dice)
    {
        int total = 0;
        foreach (Die die in dice)
        {
            total += die.Value;
        }
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
        int v;
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
        int v;
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
        if (AllTotal >= check)
        {
            pass = true;
        }
        return pass;
    }
}
