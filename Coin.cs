using Godot;
using System;

public partial class Coin : Area2D
{
	public Main mainNode;
	public override void _Ready()
	{
		mainNode = GetNode<Main>("/root/Main");
		Random randomNumber = new Random();
		Position = mainNode.CellCenters[randomNumber.Next(0, 225)];
		AreaEntered += OnAreaEntered;
	}

	public void OnAreaEntered(Node2D node)
	{
		mainNode._score += 1;
		mainNode.CoinExists = false;
		QueueFree();
	}

	public override void _Process(double delta)
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Play();
	}
}
