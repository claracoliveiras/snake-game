using Godot;
using System;

public partial class Coin : Area2D
{
	public Main MainNode;
	public override void _Ready()
	{
		MainNode = GetNode<Main>("/root/Main");
		Random randomNumber = new Random();
		Position = MainNode.CellCenters[randomNumber.Next(0, 225)];
		AreaEntered += OnAreaEntered;
	}

	public void OnAreaEntered(Node2D node)
	{
		MainNode._score += 1;
		MainNode._bodyInstanceAmount += 1;
		MainNode.CoinExists = false;
		QueueFree();
	}

	public override void _Process(double delta)
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Play();
	}
}
