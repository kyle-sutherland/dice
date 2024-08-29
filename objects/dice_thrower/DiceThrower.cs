using System;
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

    public int dSixTotal { get; private set; }

    public int total { get; private set; }

    public DiceRack diceRack { get; private set; } = new DiceRack();

    public class DiceRack
    {
        public DSix[] dsix { get; set; }
        public DFour[] dfour { get; set; }
        public DEight[] deight { get; set; }
        public DTen[] dten { get; set; }
        public DTwelve[] dtwelve { get; set; }
        public DTwenty[] dtwenty { get; set; }

        public void addDie(Die die)
        {
            string type = die.GetType().ToString();
            if (type == "DSix")
            {
                dsix.SetValue(die, dsix.GetLength(0));
            }
            else if (type == "DFour")
            {
                dfour.SetValue(die, dfour.GetLength(0));
            }
            else if (type == "DEight")
            {
                deight.SetValue(die, deight.GetLength(0));
            }
            else if (type == "DTen")
            {
                dten.SetValue(die, dten.GetLength(0));
            }
            else if (type == "DTwelve")
            {
                dtwelve.SetValue(die, dtwelve.GetLength(0));
            }
            else if (type == "DTwenty")
            {
                dtwenty.SetValue(die, dtwenty.GetLength(0));
            }
        }

        public int CountDSix()
        {
            int total = 0;
            foreach (Die die in dsix)
            {
                total += die.Value;
            }
            return total;
        }

        public int CountDFour()
        {
            int total = 0;
            foreach (Die die in dfour)
            {
                total += die.Value;
            }
            return total;
        }

        public int CountDEight()
        {
            int total = 0;
            foreach (Die die in deight)
            {
                total += die.Value;
            }
            return total;
        }

        public int CountDTen()
        {
            int total = 0;
            foreach (Die die in dten)
            {
                total += die.Value;
            }
            return total;
        }

        public int CountDTwelve()
        {
            int total = 0;
            foreach (Die die in dtwelve)
            {
                total += die.Value;
            }
            return total;
        }

        public int CountDTwenty()
        {
            int total = 0;
            foreach (Die die in dtwenty)
            {
                total += die.Value;
            }
            return total;
        }

        public int CountAll()
        {
            int total = 0;
            total += CountDEight();
            total += CountDSix();
            total += CountDFour();
            total += CountDTen();
            total += CountDTwelve();
            total += CountDTwenty();
            return total;
        }

        public int Check(Array dice, int check)
        {
            int n = 0;
            foreach (Die die in dice)
            {
                if (die.Value >= check)
                {
                    n++;
                }
            }
            return n;
        }

        public int checkDSix(int check)
        {
            return Check(dsix, check);
        }

        public int checkDFour(int check)
        {
            return Check(dfour, check);
        }

        public int checkDEight(int check)
        {
            return Check(deight, check);
        }

        public int checkDTen(int check)
        {
            return Check(dten, check);
        }

        public int checkDTwelve(int check)
        {
            return Check(dtwelve, check);
        }

        public int checkDTwenty(int check)
        {
            return Check(dtwenty, check);
        }
    }

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
        marquee.Text = $"{diceRack.dtwenty.GetLength(0)}d20 total: {diceRack.CountDTwenty()}";
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
        diceRack.addDie(dieInstance);
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
