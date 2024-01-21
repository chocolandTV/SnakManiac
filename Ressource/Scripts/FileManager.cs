using Godot;
using System;

public partial class FileManager : Node
{
      private int[]score;
      const string SAVEFILE = "user://savefile.save";
      public int[] GetScoreData()
      {
            GD.Print("Load Data");
            using var file = FileAccess.Open(SAVEFILE, FileAccess.ModeFlags.Read);
            if(FileAccess.FileExists(SAVEFILE))
            {
                  score = (int[]) file.GetVar();
                  GD.Print("Data found :" + score);
                  file.Close();
                  return score;

            }else{
                  GD.Print("No Data found, Create File and Default Highscore");
                  int[] temp= new int[]{0,0,0,0,0,0};
                  // CREATE DATA
                  SaveScoreData(temp);
                  return temp;
            }
      }

      public void SaveScoreData(int[] data)
      {
            GD.Print("Saving Data :" + data);
            using var file = FileAccess.Open(SAVEFILE, FileAccess.ModeFlags.Write);
            file.StoreVar(data);
            
            file.Close();
            GD.Print("Saving Data success");
      }
}
