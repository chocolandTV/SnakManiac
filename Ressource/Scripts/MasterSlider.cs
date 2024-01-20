using Godot;
using System;

public partial class MasterSlider : HSlider
{
      private int busIndex = 0;
      public void _on_value_changed(float value)
      {
            AudioServer.SetBusVolumeDb(busIndex, Mathf.LinearToDb(value));
      }
}
