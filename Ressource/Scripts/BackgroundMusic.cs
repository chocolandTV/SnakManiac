using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BackgroundMusic : AudioStreamPlayer
{
      [Export]
      public AudioStream[] DefaultAudioSongList;
      public List<int>Playlist = new List<int>();
      
      private RandomNumberGenerator randi = new RandomNumberGenerator();
      public override void _Ready()
      {
           Playlist.Add(0);
           Playlist.Add(1);
           Playlist.Add(2);
      }
      public void _on_background_music_finished()
      {
                    
            Stream = DefaultAudioSongList[Playlist[randi.RandiRange(0,Playlist.Count-1)]];
           
            Play();
      }
      
}
