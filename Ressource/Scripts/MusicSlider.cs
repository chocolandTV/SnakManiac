using Godot;
using System;

public partial class MusicSlider : HSlider
{
      private int busIndex = 2;
      public void _on_value_changed(float value)
      {
            AudioServer.SetBusVolumeDb(busIndex, Mathf.LinearToDb(value));
      }
}
