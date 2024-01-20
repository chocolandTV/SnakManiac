using Godot;
using System;

public partial class GameManager : Node
{
      // VISUALS HMENU 
      [Export]
      Node2D ScorePanel;
      [Export]
      Control MenuPanel;

      [Export]
      GameBoard gameBoard;
      //SCORE
      private H_Menu h_Menu;
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
            
            ChangeGameState( CurrentState = GAMESTATE.Game_Running);
      }
      // RESTART
      private void RestartGame()
      {
            gameBoard.isRunning =false;
            CurrentState = GAMESTATE.Game_Over;
            StartGame();
      }

      // CHANGE GAMESTATE 
      public void ChangeGameState (GAMESTATE newState)
      {     // ISSUE SECURE

            if(CurrentState == GAMESTATE.Menu && newState == GAMESTATE.Game_Over)
                  return;
            if(CurrentState == GAMESTATE.Game_Running && newState == GAMESTATE.Game_Running)
                  return;
            
            // IF GAME STARTS FIRST TIME
            if(CurrentState == GAMESTATE.Menu && newState == GAMESTATE.Game_Running)
            {
                  // MENUPANEL RESTART BUTTON SHOW 
                  MenuPanel.Hide();
                  ScorePanel.Show();
                  StartGame();
            }
            // IF GAME RESTARTS 
            if(CurrentState == GAMESTATE.Game_Over && newState == GAMESTATE.Game_Running)
            {
                  MenuPanel.Hide();
                  ScorePanel.Show();
                  RestartGame();
            }
            // IF GAME IS PAUSED
            if(CurrentState == GAMESTATE.Game_Running && newState == GAMESTATE.Pause)
            {
                  // RESUME BUTTON SHOW
                  MenuPanel.Show();
                  ScorePanel.Hide();
                  gameBoard.isRunning = false;
            }
            if(CurrentState == GAMESTATE.Pause && newState == GAMESTATE.Game_Running)
            {
                  // RESUME BUTTON HIDE
                  MenuPanel.Hide();
                  ScorePanel.Show();
                  h_Menu = MenuPanel.GetNode<H_Menu>("../");
                  h_Menu.EnableMenu();
                  gameBoard.isRunning = true;
            }
            // if GAME OVER -> SHOW HIGHSCORE
            if(CurrentState == GAMESTATE.Game_Running && newState == GAMESTATE.Game_Over)
            {
                  // RESTART BUTTON SHOW
                  MenuPanel.Show();
                  ScorePanel.Hide();
                  gameBoard.isRunning = false;
                  // HIGHSCORE 
            }
            CurrentState = newState;
            
      }
      


}
