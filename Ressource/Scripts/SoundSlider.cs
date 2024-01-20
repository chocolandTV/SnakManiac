using Godot;
using System;

public partial class SoundSlider : HSlider
{
      private int busIndex = 1;
      public void _on_value_changed(float value)
      {
            AudioServer.SetBusVolumeDb(busIndex, Mathf.LinearToDb(value));
      }
}
