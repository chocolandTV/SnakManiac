using Godot;
using System;

public partial class H_Menu : Control
{
    [Export]
    MarginContainer optionMenu;
    [Export]
    MarginContainer HighscoreMenu;
    private bool showOptions = false;
    private bool showHighscore = false;
    private GameManager gameManager;

    public override void _Ready()
    {
        gameManager = GetNode<GameManager>("../");
        GD.Print(gameManager.Name);
    }
    public void _on_start_pressed()
    {
        // GetTree().ChangeSceneToFile("res://main_scene.tscn");
        gameManager.ChangeGameState(GameManager.GAMESTATE.Game_Running);
    }
    public void _on_option_pressed()
    {
        showOptions = !showOptions;
        if (showOptions)
            optionMenu.Show();
        else
            optionMenu.Hide();
    }
    public void _on_highscore_pressed()
    {
       showHighscore = !showHighscore;
       if(showHighscore)
            HighscoreMenu.Show();
        else
            HighscoreMenu.Hide();
    }
    public void _on_quit_pressed()
    {
        GetTree().Quit();
    }
}
