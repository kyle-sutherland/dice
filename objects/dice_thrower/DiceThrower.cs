using System;
using Godot;

public partial class DiceThrower : Node3D
{
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
    public StringName DiceManagerGroup { get; private set; } = "DiceManager";

    public DiceManager DiceManager { get; private set; }

    [Export]
    public bool Throw { get; private set; } = true;

    [Export]
    public float LaunchForce { get; private set; } = 1000f;

    public PackedScene[] Dice { get; private set; } = new PackedScene[100];

    private Vector3 aimDirection;

    public Timer Delay { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        string diceManagerNodePath = GetTree().GetFirstNodeInGroup(DiceManagerGroup).GetPath();
        DiceManager = GetNodeOrNull<DiceManager>(diceManagerNodePath);
        if (DiceManager == null)
        {
            GD.PushWarning("No dice manager found in group " + DiceManagerGroup);
        }
        GD.Print(diceManagerNodePath);
        Delay = GetNode<Timer>("Delay");
    }

    //
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(ThrowAction))
        {
            RollDice();
        }
        else if (@event.IsActionPressed(ClearAction))
        {
            DiceManager.ClearDice();
        }
        else if (@event.IsActionPressed(CountAction)) { }
        // commenting out for now as these actions may not be needed.
        // else if (@event.IsActionPressed(NextDieAction))
        // {
        //     if (SelectedDieIndex == DieScenes.GetLength(0))
        //     {
        //         SelectedDieIndex = 0;
        //         SelectedDieScene = DieScenes[SelectedDieIndex];
        //     }
        //     else
        //     {
        //         SelectedDieIndex++;
        //         SelectedDieScene = DieScenes[SelectedDieIndex];
        //     }
        // }
        // else if (@event.IsActionPressed(PrevDieAction))
        // {
        //     if (SelectedDieIndex == 0)
        //     {
        //         SelectedDieIndex = DieScenes.GetLength(0);
        //         SelectedDieScene = DieScenes[SelectedDieIndex];
        //     }
        //     SelectedDieIndex--;
        //     SelectedDieScene = DieScenes[SelectedDieIndex];
        // }
    }

    //
    //
    public void RollDice()
    {
        DiceManager.ReadyChecks();
        DiceManager.ReadyDice();
        Dice = DiceManager.ReadyDieScenes;
        if (Throw)
        {
            ThrowDice(Dice);
        }
        else
        {
            DropDice(Dice);
        }
    }

    //
    //Takes one PackedScene array argument and
    public async void ThrowDice(PackedScene[] diceToThrow)
    {
        float positionZOffset = 1.8f;
        float positionYOffset = 1.8f;
        int positionOffsetCount = 0;
        int rowCount = 0;
        int nAllDice = DiceManager.GetNAllDice();

        for (int i = 0; i < nAllDice; i++)
        {
            Delay.Start();
            PackedScene dieToThrow = diceToThrow[i];
            if (dieToThrow != null)
            {
                Vector3 position = GlobalPosition;
                Vector3 rotation = GlobalRotation;
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
                float aimX = random.RandfRange(-0.3f, 0.3f);
                float aimY = random.RandfRange(-0.8f, -0.1f);
                float aimZ = random.RandfRange(-0.3f, 0.3f);
                float torqueX;
                float torqueY;
                float torqueZ;
                aimDirection = new Vector3(aimX, aimY, aimZ) + rotation;
                LaunchForce = random.RandfRange(1200f, 1800f);
                DiceManager.AssignDieToParent(dieInstance, dieInstanceTypeName);
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
        int nAllDice = DiceManager.GetNAllDice();

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
                DiceManager.AssignDieToParent(dieInstance, dieInstanceTypeName);
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
}
