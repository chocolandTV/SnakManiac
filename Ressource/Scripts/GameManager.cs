using Godot;
using System;

public partial class GameManager : Node
{
      // VISUALS HMENU 
      [Export]
      Node2D ScorePanel;
      [Export]
      score scoreManager;
      [Export]
      Control MenuPanel;
      [Export]
      H_Menu h_Menu;
      [Export]
      GameBoard gameBoard;
      private bool isPaused = false;
      //SCORE

      public enum GAMESTATE
      {
            Menu,
            Game_Running,
            Game_Over,
            Pause
      }
      private GAMESTATE CurrentState;

      //START GAME 
      private void StartGame()
      {
            gameBoard.isRunning = true;
            gameBoard.RestartBoard();
            CurrentState = GAMESTATE.Game_Running;
            MenuPanel.Hide();
            ScorePanel.Show();
      }
      // RESTART
      private void RestartGame()
      {
            gameBoard.isRunning = false;
            CurrentState = GAMESTATE.Game_Over;
            StartGame();
      }
      // CHANGE PAUSE
      public void OnChangePauseButton()
      {
            isPaused = !isPaused;
            if (isPaused)
            {
                  ChangeGameState(GAMESTATE.Pause);
            }
            else
            {
                  ChangeGameState(GAMESTATE.Game_Running);
            }
      }
      // CHANGE GAMESTATE 
      public void ChangeGameState(GAMESTATE newState)
      {     // ISSUE SECURE

            if (CurrentState == GAMESTATE.Menu && newState == GAMESTATE.Game_Over)
                  return;
            if (CurrentState == GAMESTATE.Game_Running && newState == GAMESTATE.Game_Running)
                  return;

            // IF GAME STARTS FIRST TIME
            if (CurrentState == GAMESTATE.Menu && newState == GAMESTATE.Game_Running)
            {
                  // MENUPANEL RESTART BUTTON SHOW 
                  MenuPanel.Hide();
                  ScorePanel.Show();
                  StartGame();
            }
            // IF GAME RESTARTS 
            if (CurrentState == GAMESTATE.Game_Over && newState == GAMESTATE.Game_Running)
            {
                  MenuPanel.Hide();
                  ScorePanel.Show();
                  RestartGame();
            }
            // IF GAME IS PAUSED
            if (CurrentState == GAMESTATE.Game_Running && newState == GAMESTATE.Pause)
            {
                  // RESUME BUTTON SHOW
                  MenuPanel.Show();
                  h_Menu.EnableMenu();
                  h_Menu.EnablePauseMode(true);
                  ScorePanel.Hide();
                  gameBoard.isRunning = false;
            }
            if (CurrentState == GAMESTATE.Pause && newState == GAMESTATE.Game_Running)
            {
                  // RESUME BUTTON HIDE
                  MenuPanel.Hide();
                  h_Menu.EnablePauseMode(false);
                  ScorePanel.Show();
                  isPaused = false;
                  gameBoard.isRunning = true;
            }
            // if GAME OVER -> SHOW HIGHSCORE
            if (CurrentState == GAMESTATE.Game_Running && newState == GAMESTATE.Game_Over)
            {
                  // RESTART BUTTON SHOW
                  MenuPanel.Show();
                  h_Menu.EnableGameOverMenu(scoreManager.Score);
                  ScorePanel.Show();
                  // HIGHSCORE SAVE
            }
            CurrentState = newState;

      }



}
