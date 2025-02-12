using System;
using Godot;

public partial class DiceManager : Node
{
    [Export]
    public PackedScene[] DieScenes { get; private set; } = new PackedScene[6];

    public PackedScene[] ReadyDieScenes { get; private set; } = new PackedScene[100];

    [Export]
    public int[] NDice { get; set; } = new int[6];

    public Nullable<int> SelectedDieIndex { get; private set; }

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

    protected PackedScene SelectedDieScene { get; private set; }

    public Node DiceParent { get; set; }

    public Node D4Parent { get; set; }

    public Node D6Parent { get; set; }

    public Node D8Parent { get; set; }

    public Node D10Parent { get; set; }

    public Node D12Parent { get; set; }

    public Node D20Parent { get; set; }

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

    [Export]
    public StringName LineEditsGroup { get; private set; } = "LineEdits";

    [Export]
    public StringName AllCheckInputGroup { get; private set; } = "AllCheckInput";

    public SpinBox AllCheckInput { get; private set; }

    public Godot.Collections.Array<Node> LineEditArray { get; private set; }

    public LineEdit D4LineEdit { get; private set; }

    public LineEdit D6LineEdit { get; private set; }

    public LineEdit D8LineEdit { get; private set; }

    public LineEdit D10LineEdit { get; private set; }

    public LineEdit D12LineEdit { get; private set; }

    public LineEdit D20LineEdit { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SelectedDieIndex = null;
        DiceParent = GetTree().GetFirstNodeInGroup(DiceParentGroup);
        D4Parent = GetTree().GetFirstNodeInGroup(D4ParentGroup);
        D6Parent = GetTree().GetFirstNodeInGroup(D6ParentGroup);
        D8Parent = GetTree().GetFirstNodeInGroup(D8ParentGroup);
        D10Parent = GetTree().GetFirstNodeInGroup(D10ParentGroup);
        D12Parent = GetTree().GetFirstNodeInGroup(D12ParentGroup);
        D20Parent = GetTree().GetFirstNodeInGroup(D20ParentGroup);

        SelectedDieIndex = 1;

        LineEditArray = GetTree().GetNodesInGroup(LineEditsGroup);
        D4LineEdit = (LineEdit)LineEditArray[0];
        D6LineEdit = (LineEdit)LineEditArray[1];
        D8LineEdit = (LineEdit)LineEditArray[2];
        D10LineEdit = (LineEdit)LineEditArray[3];
        D12LineEdit = (LineEdit)LineEditArray[4];
        D20LineEdit = (LineEdit)LineEditArray[5];

        AllCheckInput = (SpinBox)GetTree().GetFirstNodeInGroup(AllCheckInputGroup);

        if (DiceParent == null)
        {
            GD.PushWarning("No dice parent found in group " + DiceParentGroup);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (SelectedDieIndex != null)
        {
            SelectedDieScene = DieScenes[(int)SelectedDieIndex];
        }
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
    }

    public void ReadyChecks()
    {
        double AllCheckInputValue = AllCheckInput.Value;
        AllCheck = (int)AllCheckInputValue;
    }

    //
    //Loads Die objects into ReadyDieScenes array based on values stored in NDice array
    public void ReadyDice()
    {
        NDice[0] = D4LineEdit.Text.ToInt();
        NDice[1] = D6LineEdit.Text.ToInt();
        NDice[2] = D8LineEdit.Text.ToInt();
        NDice[3] = D10LineEdit.Text.ToInt();
        NDice[4] = D12LineEdit.Text.ToInt();
        NDice[5] = D20LineEdit.Text.ToInt();
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
        // GD.Print(ReadyDieScenes);
    }

    //
    //Returns the total value of the NDice array, which corresponds to the total number of dice
    //to be instanced.
    public int GetNAllDice()
    {
        int n = 0;
        foreach (int i in NDice)
        {
            n += i;
        }
        return n;
    }

    //
    //Iterates over all Dice Nodes from their respective parent Nodes and despawns them by calling
    //the Node.QueueFree method on each Die object
    public void ClearDice()
    {
        Godot.Collections.Array<Node> dice =
            D4Parent.GetChildren()
            + D6Parent.GetChildren()
            + D8Parent.GetChildren()
            + D10Parent.GetChildren()
            + D12Parent.GetChildren()
            + D20Parent.GetChildren();
        foreach (Node die in dice)
        {
            die.QueueFree();
        }
    }

    // public static int CountDice(Die[] dice)
    // {
    //     int total = 0;
    //     foreach (Die die in dice)
    //     {
    //         total += die.Value;
    //     }
    //     return total;
    // }

    public int CountDice(string dtype)
    {
        Node DParent = null;
        if (dtype == "d4")
        {
            DParent = D4Parent;
        }
        else if (dtype == "d6")
        {
            DParent = D6Parent;
        }
        else if (dtype == "d8")
        {
            DParent = D8Parent;
        }
        else if (dtype == "d10")
        {
            DParent = D10Parent;
        }
        else if (dtype == "d12")
        {
            DParent = D12Parent;
        }
        else if (dtype == "d20")
        {
            DParent = D20Parent;
        }
        int v;
        int total = 0;
        for (int i = 0; i < DParent.GetChildCount(); i++)
        {
            NodePath dieNodePath = DParent.GetChild(i).GetPath();
            Die die = GetNodeOrNull<Die>(dieNodePath);
            v = die.Value;
            total += v;
        }
        return total;
    }

    public int CheckDice(int check, string dtype)
    {
        Node DParent = null;
        if (dtype == "d4")
        {
            DParent = D4Parent;
        }
        else if (dtype == "d6")
        {
            DParent = D6Parent;
        }
        else if (dtype == "d8")
        {
            DParent = D8Parent;
        }
        else if (dtype == "d10")
        {
            DParent = D12Parent;
        }
        else if (dtype == "d12")
        {
            DParent = D12Parent;
        }
        else if (dtype == "d20")
        {
            DParent = D20Parent;
        }
        int v;
        int n = 0;
        for (int i = 0; i < DParent.GetChildCount(); i++)
        {
            NodePath dieNodePath = DParent.GetChild(i).GetPath();
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

    public string CreateMessage()
    {
        string message = "";
        if (D4Parent.GetChildCount() != 0)
        {
            message += $"{D4Parent.GetChildCount()}d4: {D4Total}\n";
        }
        if (D6Parent.GetChildCount() != 0)
        {
            message += $"{D6Parent.GetChildCount()}d6: {D6Total}\n";
        }
        if (D8Parent.GetChildCount() != 0)
        {
            message += $"{D8Parent.GetChildCount()}d8: {D8Total}\n";
        }
        if (D10Parent.GetChildCount() != 0)
        {
            message += $"{D10Parent.GetChildCount()}d10: {D10Total}\n";
        }
        if (D12Parent.GetChildCount() != 0)
        {
            message += $"{D12Parent.GetChildCount()}d12: {D12Total}\n";
        }
        if (D20Parent.GetChildCount() != 0)
        {
            message += $"{D20Parent.GetChildCount()}d20: {D20Total}\n";
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
}
