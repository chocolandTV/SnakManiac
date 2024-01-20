using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameBoard : TileMap
{
	[Export]
	public int BoardSize;
	const int Snake = 0;
	const int Apple = 1;
	private List<Vector2I> Snake_body = new List<Vector2I>();
	RandomNumberGenerator randi = new RandomNumberGenerator();
	Vector2I apple_Pos = new Vector2I(0, 0);

	// LOOKUP DIRECTION
	Dictionary<Vector2I, Vector2I> _snake_tail = new Dictionary<Vector2I, Vector2I>();
	Dictionary<Vector2I, Vector2I> _snake_corner_Left = new Dictionary<Vector2I, Vector2I>();
	Dictionary<Vector2I, Vector2I> _snake_corner_Right = new Dictionary<Vector2I, Vector2I>();
	Dictionary<Vector2I, Vector2I> _snake_head = new Dictionary<Vector2I, Vector2I>();
	Dictionary<Vector2I, Vector2I> _snake_mid = new Dictionary<Vector2I, Vector2I>();

	// INPUT DIRECTION 
	public Vector2I Snake_Direction;
	private Vector2I snake_old_Direction = new Vector2I(-1, 0);
	public override void _Ready()
	{
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

		apple_Pos = get_New_Apple_Pos();
		Spawn_Apple();
		// GD.Print(apple_Pos);
		// Spawn default snake
		Reset_Snake();

	}

	private void Reset_Snake()
	{
		Snake_body.Clear();
		Snake_body.Add(new Vector2I(3, 5));
		Snake_body.Add(new Vector2I(4, 5));
		Snake_body.Add(new Vector2I(5, 5));
		snake_old_Direction = new Vector2I(-1, 0);
		SetCell(0, Snake_body[0], Snake, new Vector2I(3, 1));
		SetCell(0, Snake_body[1], Snake, new Vector2I(4, 0));
		SetCell(0, Snake_body[2], Snake, new Vector2I(0, 0));
	}
	private void Clear_Board()
	{

	}
	private Vector2I get_New_Apple_Pos()
	{
		int x = randi.RandiRange(0, BoardSize);
		int y = randi.RandiRange(0, BoardSize);
		
		return new Vector2I(x, y);
	}
	private void Spawn_Apple()
	{
		SetCell(0, get_New_Apple_Pos(), Apple, new Vector2I(0, 0));
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
		Vector2I lastPos = Snake_body.First();
		Snake_body[0] = Snake_body[0] + Snake_Direction;

		for (int i = 1; i < Snake_body.Count; i++)
		{
			Vector2I temp_pos = Snake_body[i];
			Snake_body[i] = lastPos;
			lastPos = temp_pos;
		}
		snake_old_Direction = Snake_Direction;
	}
	
	private void Check_apple_eaten(){
		if (apple_Pos == Snake_body[0])
		{
			GD.Print("Apple eaten =D ");
			GetTree().CallGroup("ScoreGroup", "update_score", Snake_body.Count);
			Snake_body.Add(Snake_body.Last());
			Spawn_Apple();
			
		}else
		{
			// REMOVE TAIL
			SetCell(0, Snake_body.Last(), Snake, new Vector2I(7, 1));
		}
	}
	public void On_game_snake_tick_timeout()
	{
		Move();
		Draw_Snake();
		Check_apple_eaten();

	}
}
