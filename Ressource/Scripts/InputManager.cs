using Godot;
using System;

public partial class InputManager : Node
{
    [Export]
    public GameBoard gameBoard;
    public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("Up"))
            gameBoard.Snake_Direction  = new Vector2I(0, -1);
        if(Input.IsActionJustPressed("Right"))
            gameBoard.Snake_Direction  = new Vector2I(1, 0);
        if(Input.IsActionJustPressed("Left"))
            gameBoard.Snake_Direction  = new Vector2I(-1, 0);
        if(Input.IsActionJustPressed("Down"))
            gameBoard.Snake_Direction  = new Vector2I(0, 1);
        
    }

}
