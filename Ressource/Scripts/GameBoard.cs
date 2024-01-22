using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameBoard : TileMap
{
	[Export]
	Node2D particle;
	[Export]
	public int BoardSize;
	[Export]
	public AudioStreamPlayer2D audioPlayer;
	[Export]
	public score scoreManager;
	[Export]
	public Texture2D[] groundTexture;
	[Export]
	public TextureRect GroundSkinObject;
	const int CellSize = 40;
	int[] SnakeType;
	private int currentSnakeType = 0;
	private int currentGroundType = 0;
	const int Apple = 1;
	const int Cherry = 2;
	const int Coin = 3;
	const int Unicorn = 4;
	const int Diamond = 5;
	private List<Vector2I> Snake_body = new List<Vector2I>();
	RandomNumberGenerator randi = new RandomNumberGenerator();
	Vector2I fruit_Pos = new Vector2I(0, 0);
	private int fruit_ID = 0;
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

	private GameManager gameManager;
	public override void _Ready()
	{
		SnakeType = new int[5] { 0, 6, 7, 8, 9 };
		gameManager = GetNode<GameManager>("../");
		// HEAD 
		_snake_head.Add(new Vector2I(0, 1), new Vector2I(3, 0));
		_snake_head.Add(new Vector2I(0, 0), new Vector2I(3, 0));
		_snake_head.Add(new Vector2I(0, -1), new Vector2I(2, 1));
		_snake_head.Add(new Vector2I(1, 0), new Vector2I(2, 0));
		_snake_head.Add(new Vector2I(-1, 0), new Vector2I(3, 1));
		//TAIL
		_snake_tail.Add(new Vector2I(0, 1), new Vector2I(0, 1));
		_snake_tail.Add(new Vector2I(0, 0), new Vector2I(0, 1));
		_snake_tail.Add(new Vector2I(0, -1), new Vector2I(1, 1));
		_snake_tail.Add(new Vector2I(1, 0), new Vector2I(0, 0));
		_snake_tail.Add(new Vector2I(-1, 0), new Vector2I(1, 0));
		//corner left
		_snake_corner_Left.Add(new Vector2I(0, 1), new Vector2I(5, 0));
		_snake_corner_Left.Add(new Vector2I(0, 0), new Vector2I(5, 0));
		_snake_corner_Left.Add(new Vector2I(0, -1), new Vector2I(6, 1));
		_snake_corner_Left.Add(new Vector2I(1, 0), new Vector2I(5, 1));
		_snake_corner_Left.Add(new Vector2I(-1, 0), new Vector2I(6, 0));
		//corner Right
		_snake_corner_Right.Add(new Vector2I(0, 1), new Vector2I(6, 0));
		_snake_corner_Right.Add(new Vector2I(0, 0), new Vector2I(6, 0));
		_snake_corner_Right.Add(new Vector2I(0, -1), new Vector2I(5, 1));
		_snake_corner_Right.Add(new Vector2I(1, 0), new Vector2I(5, 0));
		_snake_corner_Right.Add(new Vector2I(-1, 0), new Vector2I(6, 1));

		// MID 
		_snake_mid.Add(new Vector2I(0, 0), new Vector2I(4, 1));
		_snake_mid.Add(new Vector2I(0, 1), new Vector2I(4, 1));
		_snake_mid.Add(new Vector2I(0, -1), new Vector2I(4, 1));
		_snake_mid.Add(new Vector2I(1, 0), new Vector2I(4, 0));
		_snake_mid.Add(new Vector2I(-1, 0), new Vector2I(4, 0));


		Reset_Snake();
		isRunning = false;
	}
	public void RestartBoard()
	{
		scoreManager.Clear_Score();

		Clear_Board();
		Reset_Snake();
		Spawn_Fruit();
		isRunning = true;
	}
	public void ChangeSnakeSkin(int id)
	{
		currentSnakeType = SnakeType[id];
		Draw_Snake();

	}
	public void ChangeBoardSkin(int id)
	{
		currentGroundType = id;
		GroundSkinObject.Texture = groundTexture[currentGroundType];
	}
	private void Reset_Snake()
	{
		Snake_body.Clear();
		Snake_body.Add(new Vector2I(3, 15));
		Snake_body.Add(new Vector2I(2, 15));
		Snake_body.Add(new Vector2I(1, 15));
		snake_old_Direction = new Vector2I(1, 0);
		SetCell(0, Snake_body[0], currentSnakeType, new Vector2I(2, 0));
		SetCell(0, Snake_body[1], currentSnakeType, new Vector2I(4, 0));
		SetCell(0, Snake_body[2], currentSnakeType, new Vector2I(0, 0));
	}
	private void Clear_Board()
	{
		for (int x = 0; x <= BoardSize; x++)
		{
			for (int y = 0; y <= BoardSize; y++)
			{
				SetCell(0, new Vector2I(x, y), currentSnakeType, new Vector2I(7, 1));
			}
		}
	}
	private int GetRandomFruit()
	{

		int x = randi.RandiRange(0, 100);
		if (x < 11 + scoreManager.Score[1])
		{// CHERRY CHANCE 10% over 25
			int y = randi.RandiRange(0, 100);
			GD.Print("Coin - Rolled: " + y + "Chance:" + (11 + scoreManager.Score[2]));
			if (y < 11 + scoreManager.Score[2])
			{// COIN CHANCE 1% over 50 
				int z = randi.RandiRange(0, 100);
				GD.Print("Unicorn - Rolled: " + z + "Chance:" + (11 + scoreManager.Score[3]));
				if (Snake_body.Count > 50 && z < 11 + scoreManager.Score[3])
				{// UNICORN CHANCE 0,1% over 100
					int v = randi.RandiRange(0, 100);
					GD.Print("Diamond - Rolled: " + v + "Chance:" + (11 + scoreManager.Score[3]));
					if (Snake_body.Count > 100 && v < 11 + scoreManager.Score[4])
					{//DIAMOND CHANCE 0,01% over 200
						return Diamond;
					}
					return Unicorn;
				}
				return Coin;
			}
			return Cherry;
		}
		return Apple;
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
	private void Spawn_Fruit()
	{
		fruit_Pos = get_New_Apple_Pos();
		fruit_ID = GetRandomFruit();
		SetCell(0, fruit_Pos, fruit_ID, new Vector2I(0, 0));
		Vector2 particle_position = fruit_Pos * CellSize;

		particle.Position = particle_position;
	}
	private void Draw_Snake()
	{
		// draw head
		SetCell(0, Snake_body.First(), currentSnakeType, _snake_head[snake_old_Direction]);// HEAD add + direction
															     // for each snakebody
		for (int i = 1; i < Snake_body.Count; i++) // RETURN RAUS AUS DER FUNKTION
									 // BREAK AUS DER FOR SCHLEIFE
		{                                               // CONTINUE WEITER IN DER SCHLEIFE
			Vector2I current_Direction = Snake_body[i - 1] - Snake_body[i];
			if (current_Direction.X == BoardSize)
			{
				current_Direction = new Vector2I(current_Direction.X - (BoardSize + 1), current_Direction.Y);
			}
			if (current_Direction.Y == BoardSize)
			{
				current_Direction = new Vector2I(current_Direction.X, current_Direction.Y - (BoardSize + 1));
			}
			if (current_Direction.X == -BoardSize)
			{
				current_Direction = new Vector2I(current_Direction.X + (BoardSize + 1), current_Direction.Y);
			}
			if (current_Direction.Y == -BoardSize)
			{
				current_Direction = new Vector2I(current_Direction.X, current_Direction.Y + (BoardSize + 1));
			}

			//if last index of snake_body
			if (i == Snake_body.Count - 1)
			{
				SetCell(0, Snake_body[i], currentSnakeType, _snake_tail[current_Direction]);
			}

			else
			{
				Vector2I next_Direction = Snake_body[i] - Snake_body[i + 1];
				if (next_Direction.X == BoardSize)
				{
					next_Direction = new Vector2I(next_Direction.X - (BoardSize + 1), next_Direction.Y);
				}
				if (next_Direction.Y == BoardSize)
				{
					next_Direction = new Vector2I(next_Direction.X, next_Direction.Y - (BoardSize + 1));
				}
				if (next_Direction.X == -BoardSize)
				{
					next_Direction = new Vector2I(next_Direction.X + (BoardSize + 1), next_Direction.Y);
				}
				if (next_Direction.Y == -BoardSize)
				{
					next_Direction = new Vector2I(next_Direction.X, next_Direction.Y + (BoardSize + 1));
				}
				Vector2I direction_Change = current_Direction - next_Direction;
				// if Mid
				if (direction_Change.X == 0 || direction_Change.Y == 0)
				{
					SetCell(0, Snake_body[i], currentSnakeType, _snake_mid[current_Direction]);
				}

				// if corner right
				else if (new Vector3(current_Direction.X, current_Direction.Y, 0).Cross(new Vector3(next_Direction.X, next_Direction.Y, 0)).Z < 0)
				{
					SetCell(0, Snake_body[i], currentSnakeType, _snake_corner_Right[current_Direction]);
				}
				// if corner Left
				else
				{
					SetCell(0, Snake_body[i], currentSnakeType, _snake_corner_Left[current_Direction]);

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
			Input_Direction = snake_old_Direction;
		}
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
		SetCell(0, lastPos, currentSnakeType, new Vector2I(7, 1));
	}

	private void Check_Fruit_Eaten()
	{
		if (fruit_Pos == Snake_body[0])
		{
			// APPLE SCORE + fruit_ID Changing
			scoreManager.Add_Score(fruit_ID);
			Snake_body.Add(Snake_body.Last());
			audioPlayer.Play();
			Spawn_Fruit();

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
	private void Check_Border()
	{

		// Snake Leaves the screen -> turn right
		if (Snake_body[0].X > BoardSize)
		{
			SetCell(0, Snake_body[0], currentSnakeType, new Vector2I(7, 1));
			Snake_body[0] = new Vector2I(0, Snake_body[0].Y);
			Check_Fruit_Eaten();

		}
		if (Snake_body[0].X < 0)
		{
			SetCell(0, Snake_body[0], currentSnakeType, new Vector2I(7, 1));
			Snake_body[0] = new Vector2I(19, Snake_body[0].Y);
			Check_Fruit_Eaten();
		}
		if (Snake_body[0].Y > BoardSize)
		{
			SetCell(0, Snake_body[0], currentSnakeType, new Vector2I(7, 1));
			Snake_body[0] = new Vector2I(Snake_body[0].X, 0);
			Check_Fruit_Eaten();
		}
		if (Snake_body[0].Y < 0)
		{
			SetCell(0, Snake_body[0], currentSnakeType, new Vector2I(7, 1));
			Snake_body[0] = new Vector2I(Snake_body[0].X, 19);
			Check_Fruit_Eaten();
		}
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
			Move();
			Draw_Snake();
			Check_Fruit_Eaten();
		}

	}
	public override void _Process(double delta)
	{
		if (isRunning)
		{
			Check_Border();
			Snake_Direction = Input_Direction;
		}
	}
}
