using Godot;
using System;

public partial class H_Menu : Control
{
    [Export]
    MarginContainer optionMenu;
    [Export]
    MarginContainer HighscoreMenu;
    // HIGHSCORE SCRIPT
    [Export]
    HighscoreManager highscoreManager;

    [Export]
    MarginContainer GameOverMenu;
    // SLIDER EFFECT OFF
    [Export]
    HSlider VolumeSlider, SFXslider, MusicSlider;
    [Export]
    Label GameOverHighscoreValue;

    // BUTTONS 
    [Export]
    Button button_start, button_Resume;
    [Export]
    ColorRect PauseColorRect;
    // EXPORT GAMEBOARD 
    [Export]
    GameBoard gameBoard;
    private bool showOptions = false;
    private bool showHighscore = false;
    private GameManager gameManager;
    public void EnableMenu()
    {
        GameOverMenu.Hide();
        VolumeSlider.Editable = true;
        MusicSlider.Editable = true;
        SFXslider.Editable = true;
    }
    public void EnableGameOverMenu(int[] score)
    {

        GameOverMenu.Show();
        optionMenu.Hide();
        HighscoreMenu.Show();
        highscoreManager.SaveHighscore(score);
        highscoreManager.LoadHighscore();
        GameOverHighscoreValue.Text = score[0].ToString();
    }
    public void EnablePauseMode(bool isActive)
    {


        PauseColorRect.Visible = isActive;
        button_Resume.Visible = isActive;
        button_start.Visible = !isActive;

        //RESUME BUTTON
    }
    public override void _Ready()
    {
        gameManager = GetNode<GameManager>("../");

    }
    public void _on_start_pressed()
    {

        VolumeSlider.Editable = false;
        MusicSlider.Editable = false;
        SFXslider.Editable = false;
        gameManager.ChangeGameState(GameManager.GAMESTATE.Game_Running);
    }
    public void _on_Resume_pressed()
    {
        VolumeSlider.Editable = false;
        MusicSlider.Editable = false;
        SFXslider.Editable = false;
        gameManager.ChangeGameState(GameManager.GAMESTATE.Game_Running);
    }
    public void _on_Restart_pressed()
    {
        GameOverMenu.Hide();
        VolumeSlider.Editable = false;
        MusicSlider.Editable = false;
        SFXslider.Editable = false;
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
        if (showHighscore)
        {
            HighscoreMenu.Show();
            highscoreManager.LoadHighscore();
        }
        else
            HighscoreMenu.Hide();
    }
    public void _on_quit_pressed()
    {
        GetTree().Quit();
    }
    public void _on_optionMenu_Select_Snake(int id)
    {
        gameBoard.ChangeSnakeSkin(id);
    }
    public void _on_optionMenu_Select_Board(int id)
    {
        gameBoard.ChangeBoardSkin(id);
    }
}
