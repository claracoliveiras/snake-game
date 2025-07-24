using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

public partial class Main : Node2D
{
	// game board constraints
	// left = 8
	// right = 127
	// top = 16
	// bottom = 135

	public int _score = 0;
	public bool CoinExists = false;
	public List<Vector2> CellCenters = [];
	public List<Vector2> SnakePositions = new List<Vector2>();
	private Vector2 InitialPosition = new Vector2(68, 76);
	public int DirectionIndex;

	public Snake Snake;

	public override void _Ready()
	{
		GameboardAlgorithm();
		HeadSpawn();
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

	public override void _Process(double delta)
	{
		GameOver();
		var scoreNode = GetNode<Label>("Label");
		scoreNode.Text = $"Score: {_score}";
		BodySpawn();
		if (!CoinExists)
		{
			CoinSpawn();
		}

	}

	private void CoinSpawn()
	{
		var scene = GD.Load<PackedScene>("res://coin.tscn");
		var _instance = scene.Instantiate();
		AddChild(_instance);
		CoinExists = true;
	}

	private void HeadSpawn()
	{
		var scene = GD.Load<PackedScene>("res://snake.tscn");
		var _instance = scene.Instantiate<Snake>();
		AddChild(_instance);
		Snake = _instance;
		Snake.Position = InitialPosition;
	}

	private void BodySpawn()
	{
		foreach (Vector2 position in SnakePositions)
		{
			if (position != SnakePositions[0])
			{
				var scene = GD.Load<PackedScene>("res://snake_body_2.tscn");
				var _instance = scene.Instantiate<SnakeBody2>();
				AddChild(_instance);
				_instance.Position = position;
			}
		}

		// TO-DO: FIGURE OUT DELETING

	}

	private void GameOver()
	{
		if (Snake.Position.X <= 7 || Snake.Position.X >= 132 || Snake.Position.Y <= 12 || Snake.Position.Y >= 140)
		{
			var specificChildren = GetChildren();
			foreach (Node child in specificChildren)
			{
				if (child is SnakeBody2)
				{
					child.QueueFree();
				}
			}			
			
			DirectionIndex = 0;
			_score = 0;
			Snake.Position = InitialPosition;
		}
	}

	private void OnTimerTimeout()
	{
		var animatedSprite2D = Snake.AnimatedSprite2D;
		var spriteSize = animatedSprite2D.SpriteFrames.GetFrameTexture("right", 0).GetSize();

		switch (DirectionIndex)
		{
			case 0:
				Snake.Position = new Vector2(Snake.Position.X + spriteSize.X, Snake.Position.Y);
				break;
			case 1:
				Snake.Position = new Vector2(Snake.Position.X - spriteSize.X, Snake.Position.Y);
				break;
			case 2:
				Snake.Position = new Vector2(Snake.Position.X, Snake.Position.Y - spriteSize.X);
				break;
			case 3:
				Snake.Position = new Vector2(Snake.Position.X, Snake.Position.Y + spriteSize.X);
				break;

		}

		if (_score > 0)
		{
			SnakePositions.Insert(0, Snake.Position);

			while (SnakePositions.Count > _score)
			{
				SnakePositions.RemoveAt(_score + 1);
			}

			GD.Print($"Position: {Snake.Position}");
			GD.Print("Positions: " + string.Join(", ", SnakePositions));
		}
		
	}

}
