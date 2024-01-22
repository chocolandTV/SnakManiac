using Godot;
using System;

public partial class FPS_Counter : Label
{
    public override void _Process(double delta)
    {
        Text = "FPS: " + Engine.GetFramesPerSecond();
    }
}
