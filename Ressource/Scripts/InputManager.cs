using Godot;
using System;

public partial class InputManager : Node
{
    [Export]
    public GameBoard gameBoard;
    [Export]
    public GameManager gameManager;

    public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("Up"))
          gameBoard.Input_Buffer.Enqueue(new Vector2I(0, -1));
        if(Input.IsActionJustPressed("Right"))
            gameBoard.Input_Buffer.Enqueue(new Vector2I(1, 0));
        if(Input.IsActionJustPressed("Left"))
            gameBoard.Input_Buffer.Enqueue(new Vector2I(-1, 0));
        if(Input.IsActionJustPressed("Down"))
            gameBoard.Input_Buffer.Enqueue(new Vector2I(0, 1));
        if(Input.IsActionJustPressed("Pause"))
            gameManager.OnChangePauseButton();
    
    }
    

}
