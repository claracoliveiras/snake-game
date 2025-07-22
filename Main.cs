using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

	public override void _Ready()
	{
		GameboardAlgorithm();
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
		var scoreNode = GetNode<Label>("Label");
		scoreNode.Text = $"Score: {_score}";

		if (!CoinExists)
		{
			SpawnAlgorithm();
		}

	}

	private void SpawnAlgorithm()
	{
		var scene = GD.Load<PackedScene>("res://coin.tscn");
		var _instance = scene.Instantiate();
		AddChild(_instance);
		CoinExists = true;
	}

}
