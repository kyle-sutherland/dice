using System;
using Godot;

public partial class DiceManager : Node
{
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
    public PackedScene[] DieScenes { get; private set; } = new PackedScene[6];

    public PackedScene[] ReadyDieScenes { get; private set; } = new PackedScene[100];

    [Export]
    public int[] nDice { get; private set; } = new int[6];

    public int SelectedDieIndex { get; set; } = 0;

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
    public PackedScene SelectedDieScene { get; set; }

    public Node diceParent;

    public Node d4Parent;

    public Node d6Parent;

    public Node d8Parent;

    public Node d10Parent;

    public Node d12Parent;

    public Node d20Parent;

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

    public override void _Ready()
    {
        diceParent = GetTree().GetFirstNodeInGroup(DiceParentGroup);
        d4Parent = GetTree().GetFirstNodeInGroup(D4ParentGroup);
        d6Parent = GetTree().GetFirstNodeInGroup(D6ParentGroup);
        d8Parent = GetTree().GetFirstNodeInGroup(D8ParentGroup);
        d10Parent = GetTree().GetFirstNodeInGroup(D10ParentGroup);
        d12Parent = GetTree().GetFirstNodeInGroup(D12ParentGroup);
        d20Parent = GetTree().GetFirstNodeInGroup(D20ParentGroup);

        SelectedDieIndex = 1;
        ReadyDice();
        GD.Print(ReadyDieScenes);

        if (diceParent == null)
        {
            GD.PushWarning("No dice parent found in group " + DiceParentGroup);
        }
    }

    public override void _Process(double delta)
    {
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
    }

    public void ReadyDice()
    {
        for (int i = 0; i < nDice.GetLength(0) - 1; i++)
        {
            int n = nDice[i];
            for (int j = 0; j < n; j++)
            {
                ReadyDieScenes.SetValue(DieScenes[i], j);
            }
        }
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
}
