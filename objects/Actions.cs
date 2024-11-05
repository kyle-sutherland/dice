using System;
using Godot;

public class Actions
{
    [Export]
    public static StringName ThrowAction { get; private set; } = "Throw";

    [Export]
    public static StringName ClearAction { get; private set; } = "Clear";

    [Export]
    public static StringName CountAction { get; private set; } = "Count";

    [Export]
    public static StringName NextDieAction { get; private set; } = "NextDie";

    [Export]
    public static StringName PrevDieAction { get; private set; } = "PrevDie";

    [Export]
    public static StringName Selectd4Action { get; private set; } = "Selectd4";

    [Export]
    public static StringName Selectd6Action { get; private set; } = "Selectd6";

    [Export]
    public static StringName Selectd8Action { get; private set; } = "Selectd8";

    [Export]
    public static StringName Selectd10Action { get; private set; } = "Selectd10";

    [Export]
    public static StringName Selectd12Action { get; private set; } = "Selectd12";

    [Export]
    public static StringName Selectd20Action { get; private set; } = "Selectd20";
}
