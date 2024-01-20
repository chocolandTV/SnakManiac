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

    public void _on_start_pressed()
    {
        GetTree().ChangeSceneToFile("res://main_scene.tscn");
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
