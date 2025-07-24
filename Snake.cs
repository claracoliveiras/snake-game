using Godot;
using System;
using System.Collections.Generic;

public partial class Snake : Node2D
{
	private List<String> directions = [
		"right", "left", "up", "down"
	];

	public AnimatedSprite2D AnimatedSprite2D;
	
	public Main MainNode;

	public override void _Ready()
	{
		AnimatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		MainNode = GetNode<Main>("/root/Main");
	}

	public override void _Process(double delta)
	{
		AnimatedSprite2D.Play(directions[MainNode.DirectionIndex]);
	}

	public override void _Input(InputEvent @event)
	{

		if (@event.IsActionPressed("move_right"))
		{
			MainNode.DirectionIndex = 0;

		}
		else if (@event.IsActionPressed("move_left"))
		{
			MainNode.DirectionIndex = 1;

		}
		else if (@event.IsActionPressed("move_up"))
		{
			MainNode.DirectionIndex = 2;

		}
		else if (@event.IsActionPressed("move_down"))
		{
			MainNode.DirectionIndex = 3;

		}
	}

	
}
