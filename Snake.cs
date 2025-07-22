using Godot;
using System;
using System.Collections.Generic;

public partial class Snake : Node2D
{
	
	private Vector2 InitialPosition = new Vector2(68, 76);
	private List<String> directions = [
		"right", "left", "up", "down"
	];

	
	List<Vector2> LastPositions = new List<Vector2>();
	public Vector2 lastPosition;

	private int directionIndex;
	public Main mainNode;

	public override void _Ready()
	{
		mainNode = GetNode<Main>("/root/Main");
		Position = InitialPosition;
	}

	public override void _Process(double delta)
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Play(directions[directionIndex]);

		if (Position.X <= 7 || Position.X >= 132 || Position.Y <= 12 || Position.Y >= 140)
		{
			GameOver();
		}
	}

	private void OnTimerTimeout()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		var spriteSize = animatedSprite2D.SpriteFrames.GetFrameTexture("right", 0).GetSize();

		switch (directionIndex)
		{
			case 0:
				Position = new Vector2(Position.X + spriteSize.X, Position.Y);
				break;
			case 1:
				Position = new Vector2(Position.X - spriteSize.X, Position.Y);
				break;
			case 2:
				Position = new Vector2(Position.X, Position.Y - spriteSize.X);
				break;
			case 3:
				Position = new Vector2(Position.X, Position.Y + spriteSize.X);
				break;

		}

		if (mainNode._score > 0)
		{
			LastPositions.Insert(0, Position);
		
			while (LastPositions.Count > mainNode._score)
			{
				LastPositions.RemoveAt(LastPositions.Count - 1);
			}
			
			GD.Print("Positions: " + string.Join(", ", LastPositions));
		} 
	}


	public override void _Input(InputEvent @event)
	{

		if (@event.IsActionPressed("move_right"))
		{
			directionIndex = 0;

		}
		else if (@event.IsActionPressed("move_left"))
		{
			directionIndex = 1;

		}
		else if (@event.IsActionPressed("move_up"))
		{
			directionIndex = 2;

		}
		else if (@event.IsActionPressed("move_down"))
		{
			directionIndex = 3;

		}
	}

	private void GameOver()
	{
		var mainNode = GetNode<Main>("/root/Main");
		mainNode._score = 0;	
		Position = InitialPosition;
	}
}
