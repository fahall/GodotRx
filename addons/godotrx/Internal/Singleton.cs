using Godot;
using System;

namespace GodotRx.Internal
{
  internal sealed partial class Singleton : Node
  {
    private static readonly GDScript _gdInstanceScript = (GDScript) GD.Load("res://addons/godotrx/GodotRx.gd");
    private static readonly GodotObject _gdInstance = (GodotObject) _gdInstanceScript.New();

    public static int RegisterInstanceTracker(InstanceTracker tracker, GodotObject target)
    {
      var id = (int) _gdInstance.Call("inject_instance_tracker", target);
      _gdInstance.Connect("instance_tracker_freed", Callable.From(() => tracker.OnTrackerFreed(id)));
      return id;
    }

    #nullable disable
    public static Singleton Instance { get; private set; }
    #nullable enable

    private Singleton()
    {
      if (Instance != null)
      {
        throw new InvalidOperationException();
      }

      Instance = this;
      ProcessMode = ProcessModeEnum.Always;
    }
  }
}
