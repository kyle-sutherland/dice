using System;
using Godot;

public partial class MenuLineEdit : LineEdit
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() { }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }

	private void ValidateEntry(string entry)
	{
		string word = "";
		RegEx regex = new RegEx();
		regex.Compile("[0-9]");
	}
}
