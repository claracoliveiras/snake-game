using Godot;
using System;

public partial class SnakeBody : Node2D
{
	private Snake snakeNode;
	public override void _Ready()
	{
		snakeNode = GetNode<Snake>("/root/Snake");
		Position = snakeNode.lastPosition;
	}
	
	public override void _Process(double delta)
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Play();
	}
}
