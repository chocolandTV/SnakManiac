using Godot;
using System;

public partial class highscore_test : Node2D
{
      [Export]
      public score scoreManager;

      private FileManager fileManager;

      public override void _Ready()
      {
            fileManager = GetNode<FileManager>("/root/FileManager");
      }


      public void _on_button_add_Fruit(int Fruit_ID)
      {
            GD.Print("Button Clicked - Add Fruit :" + Fruit_ID);
            scoreManager.Add_Score(Fruit_ID);
      }
      public void _on_button_SaveData()
      {
            fileManager.SaveScoreData(scoreManager.Score);
      }
      public void _on_button_LoadData()
      {
            scoreManager.Score = fileManager.GetScoreData();
            
            scoreManager.LoadHighscore();
      }
}
