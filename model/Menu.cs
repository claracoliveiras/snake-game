using Godot;
using System;

public partial class Menu : CanvasLayer
{
    [Signal]
    public delegate void StartGameEventHandler();
    public Main Main;

    private void OnStartButtonPressed()
    {
        Main = GetNode<Main>("/root/Main");
        GetNode<Button>("StartButton").Hide();
        GetNode<Sprite2D>("StartMenu").Hide();
        EmitSignal(SignalName.StartGame);
        Main.NewGame();
    }
}
