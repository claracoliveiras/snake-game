using Godot;
using System;

public partial class Menu : CanvasLayer
{
    [Signal]
    public delegate void StartGameEventHandler();

    private void OnStartButtonPressed()
    {
        GetNode<Button>("StartButton").Hide();
        GetNode<Sprite2D>("StartMenu").Hide();
        EmitSignal(SignalName.StartGame);
    }
}
