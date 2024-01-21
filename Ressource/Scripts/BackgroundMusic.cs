using Godot;
using System;

public partial class BackgroundMusic : AudioStreamPlayer2D
{
      public void _on_background_music_finished()
      {
            Play();
      }
}
