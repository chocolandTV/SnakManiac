using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameBoard : TileMap
{
	[Export]
	public int BoardSize;
	[Export]
	public AudioStreamPlayer2D audioPlayer;
	const int Snake = 0;
	const int Apple = 1;
	private List<Vector2I> Snake_body = new List<Vector2I>();
	RandomNumberGenerator randi = new RandomNumberGenerator();
	Vector2I apple_Pos = new Vector2I(0, 0);
	public bool isRunning = false;
	// LOOKUP DIRECTION
	Dictionary<Vector2I, Vector2I> _snake_tail = new Dictionary<Vector2I, Vector2I>();
	Dictionary<Vector2I, Vector2I> _snake_corner_Left = new Dictionary<Vector2I, Vector2I>();
	Dictionary<Vector2I, Vector2I> _snake_corner_Right = new Dictionary<Vector2I, Vector2I>();
	Dictionary<Vector2I, Vector2I> _snake_head = new Dictionary<Vector2I, Vector2I>();
	Dictionary<Vector2I, Vector2I> _snake_mid = new Dictionary<Vector2I, Vector2I>();

	// INPUT DIRECTION 
	public Vector2I Input_Direction = Vector2I.Right;
	private Vector2I Snake_Direction = Vector2I.Right;
	private Vector2I snake_old_Direction = new Vector2I(-1, 0);
	public int GetScore => Snake_body.Count;
	private GameManager gameManager;
	public override void _Ready()
	{
		gameManager = GetNode<GameManager>("../");
		// HEAD 
		_snake_head.Add(new Vector2I(0, 1), new Vector2I(3, 0));
		_snake_head.Add(new Vector2I(0, -1), new Vector2I(2, 1));
		_snake_head.Add(new Vector2I(1, 0), new Vector2I(2, 0));
		_snake_head.Add(new Vector2I(-1, 0), new Vector2I(3, 1));
		//TAIL
		_snake_tail.Add(new Vector2I(0, 1), new Vector2I(0, 1));
		_snake_tail.Add(new Vector2I(0, -1), new Vector2I(1, 1));
		_snake_tail.Add(new Vector2I(1, 0), new Vector2I(0, 0));
		_snake_tail.Add(new Vector2I(-1, 0), new Vector2I(1, 0));
		//corner left
		_snake_corner_Left.Add(new Vector2I(0, 1), new Vector2I(5, 0));
		_snake_corner_Left.Add(new Vector2I(0, -1), new Vector2I(6, 1));
		_snake_corner_Left.Add(new Vector2I(1, 0), new Vector2I(5, 1));
		_snake_corner_Left.Add(new Vector2I(-1, 0), new Vector2I(6, 0));
		//corner Right
		_snake_corner_Right.Add(new Vector2I(0, 1), new Vector2I(6, 0));
		_snake_corner_Right.Add(new Vector2I(0, -1), new Vector2I(5, 1));
		_snake_corner_Right.Add(new Vector2I(1, 0), new Vector2I(5, 0));
		_snake_corner_Right.Add(new Vector2I(-1, 0), new Vector2I(6, 1));

		// MID 
		_snake_mid.Add(new Vector2I(0, 1), new Vector2I(4, 1));
		_snake_mid.Add(new Vector2I(0, -1), new Vector2I(4, 1));
		_snake_mid.Add(new Vector2I(1, 0), new Vector2I(4, 0));
		_snake_mid.Add(new Vector2I(-1, 0), new Vector2I(4, 0));

		Spawn_Apple();
		// GD.Print(apple_Pos);
		// Spawn default snake
		Reset_Snake();
		isRunning = false;
	}
	public void RestartBoard()
	{
		Clear_Board();
		Reset_Snake();
		Spawn_Apple();
		isRunning = true;
	}
	private void Reset_Snake()
	{
		Snake_body.Clear();
		Snake_body.Add(new Vector2I(10, 10));
		Snake_body.Add(new Vector2I(9, 10));
		Snake_body.Add(new Vector2I(8, 10));
		snake_old_Direction = new Vector2I(1, 0);
		SetCell(0, Snake_body[0], Snake, new Vector2I(2, 0));
		SetCell(0, Snake_body[1], Snake, new Vector2I(4, 0));
		SetCell(0, Snake_body[2], Snake, new Vector2I(0, 0));
	}
	private void Clear_Board()
	{
		for (int x = 0; x < BoardSize; x++)
		{
			for (int y = 0; y < BoardSize; y++)
			{
				SetCell(0, new Vector2I(x, y), Snake, new Vector2I(7, 1));
			}
		}
	}
	private Vector2I get_New_Apple_Pos()
	{
		int x = randi.RandiRange(0, 19);
		int y = randi.RandiRange(0, 19);
		//if Pos allready SnakeBody
		foreach (Vector2I item in Snake_body)
		{
			Vector2I temp_Pos = new Vector2I(x, y);
			if (temp_Pos == item)
				return get_New_Apple_Pos();
		}
		return new Vector2I(x, y);
	}
	private void Spawn_Apple()
	{
		apple_Pos = get_New_Apple_Pos();
		SetCell(0, apple_Pos, Apple, new Vector2I(0, 0));
	}
	private void Draw_Snake()
	{
		// draw head
		SetCell(0, Snake_body.First(), Snake, _snake_head[snake_old_Direction]);// HEAD add + direction
																				// for each snakebody
		for (int i = 1; i < Snake_body.Count; i++)
		{
			Vector2I current_Direction = Snake_body[i - 1] - Snake_body[i];
			//if last index of snake_body
			if (i == Snake_body.Count - 1)
			{
				SetCell(0, Snake_body[i], Snake, _snake_tail[current_Direction]);
			}
			// else 
			else
			{
				Vector2I next_Direction = Snake_body[i] - Snake_body[i + 1];
				Vector2I direction_Change = current_Direction - next_Direction;
				// if Mid
				if (direction_Change.X == 0 || direction_Change.Y == 0)
				{
					SetCell(0, Snake_body[i], Snake, _snake_mid[current_Direction]);
				}

				// if corner right
				else if (new Vector3(current_Direction.X, current_Direction.Y, 0).Cross(new Vector3(next_Direction.X, next_Direction.Y, 0)).Z < 0)
				{
					SetCell(0, Snake_body[i], Snake, _snake_corner_Right[current_Direction]);
				}
				// if corner Left
				else
				{
					SetCell(0, Snake_body[i], Snake, _snake_corner_Left[current_Direction]);

				}

			}
		}
	}
	private void Move()
	{
		// CHECK IF NEW DIRECTION != 180 
		if (Snake_Direction == OppositeDirection(snake_old_Direction))
		{
			Snake_Direction = snake_old_Direction;
			
		}
		// CHECK COLLISION AND MAP
		Check_Collission();

		Vector2I lastPos = Snake_body.First();
		Snake_body[0] = Snake_body[0] + Snake_Direction;

		for (int i = 1; i < Snake_body.Count; i++)
		{
			Vector2I temp_pos = Snake_body[i];
			Snake_body[i] = lastPos;
			lastPos = temp_pos;
		}
		snake_old_Direction = Snake_Direction;
		// // REMOVE TAIL
		SetCell(0, lastPos, Snake, new Vector2I(7, 1));
	}

	private void Check_apple_eaten()
	{
		if (apple_Pos == Snake_body[0])
		{
			// GD.Print("Apple eaten =D ");
			GetTree().CallGroup("ScoreGroup", "update_score", Snake_body.Count);
			Snake_body.Add(Snake_body.Last());
			audioPlayer.Play();
			Spawn_Apple();

		}

	}

	private static Vector2I OppositeDirection(Vector2I snake_old_Direction)
	{
		switch (snake_old_Direction)
		{
			case Vector2I(0, -1): return Vector2I.Down;
			case Vector2I(-1, 0): return Vector2I.Right;
			case Vector2I(1, 0): return Vector2I.Left;
			case Vector2I(0, 1): return Vector2I.Up;
			default: throw new ArgumentException();
		}
	}
	private void Check_Collission()
	{

		// Snake Leaves the screen -> turn right
		if (Snake_body[0].X >= BoardSize && Snake_Direction == new Vector2I(1, 0))
			Snake_Direction = new Vector2I(0, 1);
		if (Snake_body[0].X <= 0 && Snake_Direction == new Vector2I(-1, 0))
			Snake_Direction = new Vector2I(0, -1);
		if (Snake_body[0].Y >= BoardSize && Snake_Direction == new Vector2I(0, 1))
			Snake_Direction = new Vector2I(-1, 0);
		if (Snake_body[0].Y <= 0 && Snake_Direction == new Vector2I(0, -1))
			Snake_Direction = new Vector2I(1, 0);
		// Snake bites its own body
		for (int i = 1; i < Snake_body.Count; i++)
		{
			if (Snake_body[0] == Snake_body[i])
				Game_Over();
		}


	}
	private void Game_Over()
	{
		GD.Print("Game Over");
		
		isRunning = false;
		gameManager.ChangeGameState(GameManager.GAMESTATE.Game_Over);
		

	}
	public void On_game_snake_tick_timeout()
	{
		if (isRunning)
		{
			Snake_Direction = Input_Direction;
			Move();
			Draw_Snake();
			Check_apple_eaten();
		}

	}
}
