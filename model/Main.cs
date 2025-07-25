using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;

public partial class Main : Node2D
{
	// game board constraints
	// left = 8
	// right = 127
	// top = 16
	// bottom = 135
	public bool isGameStarted = false;
	public int _score = 0;
	public bool CoinExists = false;
	public List<Vector2> CellCenters = [];

	private Vector2 InitialPosition = new Vector2(68, 76);
	public int DirectionIndex;

	public SnakeHead SnakeHead;
	public int _bodyInstanceAmount = 0;
	public List<SnakeBody> SnakebodyList = new List<SnakeBody>();
	public List<Vector2> SnakePositions = new List<Vector2>();

	private void NewGame()
	{
		isGameStarted = true;
	}

	public override void _Ready()
	{
		GameboardAlgorithm();
		HeadSpawn();
	}

	private void HeadSpawn()
	{
		var scene = GD.Load<PackedScene>("res://model/snake/snake_head.tscn");
		var _instance = scene.Instantiate<SnakeHead>();
		AddChild(_instance);
		SnakeHead = _instance;
		SnakeHead.Position = InitialPosition;
	}

	private void GameboardAlgorithm()
	{
		int x = 8;
		int y = 16;

		for (int j = 0; j < 15; j++)
		{
			for (int i = 0; i < 15; i++)
			{
				CellCenters.Add(new Vector2(x: x + 4, y: y + 4));
				x += 8;
			}
			x = 8;
			y += 8;
		}
	}

	public override async void _Process(double delta)
	{
		await GameOver();

		var scoreNode = GetNode<Label>("ScoreDisplay");
		scoreNode.Text = $"{_score}";

		if (!CoinExists)
		{
			CoinSpawn();
		}

	}

	private async Task GameOver()
	{
		if (SnakeHead.Position.X <= 7 || SnakeHead.Position.X >= 132 || SnakeHead.Position.Y <= 12 || SnakeHead.Position.Y >= 140 || SnakePositions.GetRange(1, SnakePositions.Count - 1).Contains(SnakeHead.Position))
		{
			DirectionIndex = 0;
			_score = 0;
			SnakeHead.Position = InitialPosition;
			_bodyInstanceAmount = 0;
			HandleDeleteBody();
			isGameStarted = false;
			await ToSignal(GetTree().CreateTimer(3.0), SceneTreeTimer.SignalName.Timeout);
			isGameStarted = true;
		}
	}

	private void CoinSpawn()
	{
		var scene = GD.Load<PackedScene>("res://model/coin/coin.tscn");
		var _instance = scene.Instantiate();
		AddChild(_instance);
		CoinExists = true;
	}

	private void OnTimerTimeout()
	{
		if (isGameStarted)
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
		while (SnakePositions.Count > _score + 1)
		{
			SnakePositions.RemoveAt(SnakePositions.Count - 1);
		}
	}

	private void HandleBodySpawn()
	{
		var counter = 0;

		while (_bodyInstanceAmount != 0)
		{
			var scene = GD.Load<PackedScene>("res://model/snake/snake_body.tscn");
			var _instance = scene.Instantiate<SnakeBody>();
			_instance.Name = "body_" + counter;
			SnakebodyList.Insert(0, _instance);
			AddChild(_instance);
			_bodyInstanceAmount -= 1;
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
