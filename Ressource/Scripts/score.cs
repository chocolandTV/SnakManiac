using Godot;
using System;

public partial class score : Node2D
{
    [Export]
    public Label v_Score_All, v_Score_Apple, v_Score_Cherry, v_Score_Coin, v_Score_Unicorn, v_Score_Diamond;


    [Export]
    public Separator[] separators;
    [Export]
    public Panel[] panels;
    public bool[] vPanel = new bool[6] { false, false, false, false, false, false };
    public int[] Score;
    private FileManager fileManager;

    public override void _Ready()
    {
        fileManager = GetNode<FileManager>("/root/FileManager");
        Score = fileManager.GetScoreData();
        LoadHighscore();
        Clear_Score();
    }
    public void Clear_Score()
    {
       Score  =null;
       Score = new int[6] { 0, 0, 0, 0, 0, 0 };
       UpdateMainScore();
    }
    public void Add_Score(int FruitType)
    {
        Score[FruitType]++;
        if (FruitType > 1 && Score[FruitType] > 0 && !vPanel[FruitType])
            ShowNewFruitUI(FruitType);
        UpdateMainScore();
    }
    private void ShowNewFruitUI(int type)
    {
        GD.Print("Activate new FruitScore : " +  type);
        // ACTIVATE PANEL
        panels[type].Show();
        separators[type].Show();
        vPanel[type] = true;

    }
    private void UpdateMainScore()
    {
        Score[0] = Score[1] + (Score[2] * 10) + (Score[3] * 100) + (Score[4] * 1000) + (Score[5] * 10000);
        v_Score_All.Text = Score[0].ToString();
        v_Score_Apple.Text = Score[1].ToString();
        v_Score_Cherry.Text = Score[2].ToString();
        v_Score_Coin.Text = Score[3].ToString();
        v_Score_Unicorn.Text = Score[4].ToString();
        v_Score_Diamond.Text = Score[5].ToString();

    }
    public void LoadHighscore()
    {
        GD.Print("Load Highscore");
        Score[0] = Score[1] + (Score[2] * 10) + (Score[3] * 100) + (Score[4] * 1000) + (Score[5] * 10000);
        v_Score_All.Text = Score[0].ToString();
        v_Score_Apple.Text = Score[1].ToString();
        if (Score[2] > 0)
        {
            v_Score_Cherry.Text = Score[2].ToString();
            ShowNewFruitUI(2);
        }
        if (Score[3] > 0)
        {
            v_Score_Coin.Text = Score[3].ToString();
            ShowNewFruitUI(3);
        }
        if (Score[4] > 0)
        {
            ShowNewFruitUI(4);
            v_Score_Unicorn.Text = Score[4].ToString();
        }
        if (Score[5] > 0)
        {
            ShowNewFruitUI(5);
            v_Score_Diamond.Text = Score[5].ToString();
        }
    }
}
