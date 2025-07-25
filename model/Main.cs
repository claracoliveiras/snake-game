using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class Main : Node2D
{
	public bool IsGameStarted = false;
	public int Score = 0;
	public bool CoinExists = false;
	public List<Vector2> CellCenters = [];

	private Vector2 InitialPosition = new Vector2(68, 76);
	public int DirectionIndex;

	public SnakeHead SnakeHead;
	public int BodyInstanceAmount = 0;
	public List<SnakeBody> SnakebodyList = new List<SnakeBody>();
	public List<Vector2> SnakePositions = new List<Vector2>();

	public int minXPosition = 8;
	public int maxXPosition = 127;
	public int minYPosition = 16;
	public int maxYPosition = 135;

	private void NewGame()
	{
		IsGameStarted = true;
	}

	public override void _Ready()
	{
		GameboardAlgorithm();
		HeadSpawn();
	}

	private void HeadSpawn()
	{
		var scene = GD.Load<PackedScene>("res://model/snake/snake_head.tscn");
		var instance = scene.Instantiate<SnakeHead>();
		AddChild(instance);
		SnakeHead = instance;
		SnakeHead.Position = InitialPosition;
	}

	private void GameboardAlgorithm()
	{
		int xInitialPosition = 8;
		int yInitialPosition = 16;

		for (int j = 0; j < 15; j++)
		{
			for (int i = 0; i < 15; i++)
			{
				CellCenters.Add(new Vector2(x: xInitialPosition + 4, y: yInitialPosition + 4));
				xInitialPosition += 8;
			}
			xInitialPosition = 8;
			yInitialPosition += 8;
		}
	}

	public override async void _Process(double delta)
	{
		await GameOver();

		var scoreNode = GetNode<Label>("ScoreDisplay");
		scoreNode.Text = $"{Score}";

		if (!CoinExists)
		{
			CoinSpawn();
		}

	}

	private async Task GameOver()
	{
		

		if (SnakeHead.Position.X < minXPosition || SnakeHead.Position.X > maxXPosition || SnakeHead.Position.Y < minYPosition || SnakeHead.Position.Y > maxYPosition || SnakePositions.GetRange(1, SnakePositions.Count - 1).Contains(SnakeHead.Position))
		{
			DirectionIndex = 0;
			Score = 0;
			SnakeHead.Position = InitialPosition;
			BodyInstanceAmount = 0;
			HandleDeleteBody();
			IsGameStarted = false;
			await ToSignal(GetTree().CreateTimer(3.0), SceneTreeTimer.SignalName.Timeout);
			IsGameStarted = true;
		}
	}

	private void CoinSpawn()
	{
		var scene = GD.Load<PackedScene>("res://model/coin/coin.tscn");
		var instance = scene.Instantiate();
		AddChild(instance);
		CoinExists = true;
	}

	private void OnTimerTimeout()
	{
		if (IsGameStarted)
		{
			var animatedSprite2D = SnakeHead.AnimatedSprite2D;
			var spriteSize = animatedSprite2D.SpriteFrames.GetFrameTexture("right", 0).GetSize();

			switch (DirectionIndex)
			{
				case 0:
					SnakeHead.Position = new Vector2(SnakeHead.Position.X + spriteSize.X, SnakeHead.Position.Y);
					break;
				case 1:
					SnakeHead.Position = new Vector2(SnakeHead.Position.X - spriteSize.X, SnakeHead.Position.Y);
					break;
				case 2:
					SnakeHead.Position = new Vector2(SnakeHead.Position.X, SnakeHead.Position.Y - spriteSize.X);
					break;
				case 3:
					SnakeHead.Position = new Vector2(SnakeHead.Position.X, SnakeHead.Position.Y + spriteSize.X);
					break;

			}

		UpdateSnakePositionList();
		HandleBodySpawn();
		}
		
	}

	public void UpdateSnakePositionList()
	{
		SnakePositions.Insert(0, SnakeHead.Position);
		while (SnakePositions.Count > Score + 1)
		{
			SnakePositions.RemoveAt(SnakePositions.Count - 1);
		}
	}

	private void HandleBodySpawn()
	{
		var counter = 0;

		while (BodyInstanceAmount != 0)
		{
			var scene = GD.Load<PackedScene>("res://model/snake/snake_body.tscn");
			var instance = scene.Instantiate<SnakeBody>();
			instance.Name = "body_" + counter;
			SnakebodyList.Insert(0, instance);
			AddChild(instance);
			BodyInstanceAmount -= 1;
			counter += 1;
		}

		HandleBodyMovement();
	}

	private void HandleBodyMovement()
	{
		var j = 1;
		for (int i = 0; i < SnakebodyList.Count(); i++)
		{
			SnakebodyList[i].Position = SnakePositions[j];
			j += 1;
		}
	}

	private void HandleDeleteBody()
	{
		foreach (SnakeBody snakeBody in SnakebodyList)
		{
			snakeBody.QueueFree();
		}
	}
}
