using Godot;
using System;

public partial class score : Node2D
{
    [Export]
    public Label scoreText;
    public void update_score(int snake_length)
    {
        scoreText.Text = snake_length.ToString();
    }
}
