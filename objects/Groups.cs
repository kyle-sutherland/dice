using System;
using Godot;

public class Groups
{
    [Export]
    public static StringName PlayerGroup { get; private set; } = "Player";

    [Export]
    public static StringName DiceParentGroup { get; private set; } = "DiceParent";

    [Export]
    public static StringName D4ParentGroup { get; private set; } = "D4Spawn";

    [Export]
    public static StringName D6ParentGroup { get; private set; } = "D6Spawn";

    [Export]
    public static StringName D8ParentGroup { get; private set; } = "D8Spawn";

    [Export]
    public static StringName D10ParentGroup { get; private set; } = "D10Spawn";

    [Export]
    public static StringName D12ParentGroup { get; private set; } = "D12Spawn";

    [Export]
    public static StringName D20ParentGroup { get; private set; } = "D20Spawn";

    [Export]
    public static StringName MarqueeGroup { get; private set; } = "Marquee";
}
