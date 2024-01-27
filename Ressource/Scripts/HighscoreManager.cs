using Godot;
using System;

public partial class HighscoreManager : MarginContainer
{
      private int[] highscore;

      private FileManager fileManager;
      [Export]
      public Label v_Score_All, v_Score_Apple, v_Score_Cherry, v_Score_Coin, v_Score_Unicorn, v_Score_Diamond;
      [Export]
      public Separator[] separators;
      [Export]
      public Panel[] panels;

      [Export]
      public Panel updatedScore;
      public bool[] vPanel = new bool[6] { false, false, false, false, false, false };

      public override void _Ready()
      {
            fileManager = GetNode<FileManager>("/root/FileManager");
      }

      public void LoadHighscore()
      {
            
            highscore = fileManager.GetScoreData();

            GD.Print("Load Highscore");
            highscore[0] = highscore[1] + (highscore[2] * 10) + (highscore[3] * 100) + (highscore[4] * 1000) + (highscore[5] * 10000);
            v_Score_All.Text = highscore[0].ToString();
            v_Score_Apple.Text = highscore[1].ToString();
            if (highscore[2] > 0)
            {
                  v_Score_Cherry.Text = highscore[2].ToString();
                  ShowNewFruitUI(2);
            }
            if (highscore[3] > 0)
            {
                  v_Score_Coin.Text = highscore[3].ToString();
                  ShowNewFruitUI(3);
            }
            if (highscore[4] > 0)
            {
                  ShowNewFruitUI(4);
                  v_Score_Unicorn.Text = highscore[4].ToString();
            }
            if (highscore[5] > 0)
            {
                  ShowNewFruitUI(5);
                  v_Score_Diamond.Text = highscore[5].ToString();
            }
            GD.Print("Highscore loaded");
      }
      private void ShowNewFruitUI(int type)
      {

            panels[type].Show();
            separators[type].Show();
            vPanel[type] = true;

      }
      public void SaveHighscore(int[] score)
      {
            if(fileManager.GetScoreData()[0]< score[0])
            {
                  fileManager.SaveScoreData(score);
                  GD.Print("New Highscore - Saved.");
                  updatedScore.Show();
            }
            else{
                  GD.Print("Highscore Not Saved.");
                  updatedScore.Hide();
            }
      }

}
