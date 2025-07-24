using Godot;
using System;

public partial class SnakeBody2 : Area2D
{
    public AnimatedSprite2D AnimatedSprite2D;
	public override void _Ready()
	{
        AnimatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

	public override void _Process(double delta)
	{
		AnimatedSprite2D.Play();
	}
}
