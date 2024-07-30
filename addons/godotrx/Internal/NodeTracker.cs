using Godot;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace GodotRx.Internal {
  internal partial class NodeTracker : Node {
    public static readonly string DefaultName = "__NodeTracker__";
    private Subject<InputEvent>? _onInput;
    private Subject<double>? _onPhysicsProcess;
    private Subject<double>? _onProcess;
    private Subject<InputEvent>? _onUnhandledInput;
    private Subject<InputEvent>? _onUnhandledKeyInput;

    public IObservable<double> OnProcess {
      get {
        if (_onProcess == null) {
          _onProcess = new Subject<double>();
          SetProcess(true);
        }

        return _onProcess.AsObservable();
      }
    }

    public IObservable<double> OnPhysicsProcess {
      get {
        if (_onPhysicsProcess == null) {
          _onPhysicsProcess = new Subject<double>();
          SetPhysicsProcess(true);
        }

        return _onPhysicsProcess.AsObservable();
      }
    }

    public IObservable<InputEvent> OnInput {
      get {
        if (_onInput == null) {
          _onInput = new Subject<InputEvent>();
          SetProcessInput(true);
        }

        return _onInput.AsObservable();
      }
    }

    public IObservable<InputEvent> OnUnhandledInput {
      get {
        if (_onUnhandledInput == null) {
          _onUnhandledInput = new Subject<InputEvent>();
          SetProcessUnhandledInput(true);
        }

        return _onUnhandledInput.AsObservable();
      }
    }

    public IObservable<InputEvent> OnUnhandledKeyInput {
      get {
        if (_onUnhandledKeyInput == null) {
          _onUnhandledKeyInput = new Subject<InputEvent>();
          SetProcessUnhandledKeyInput(true);
        }

        return _onUnhandledKeyInput.AsObservable();
      }
    }

    public override void _Ready() {
      SetProcess(false);
      SetPhysicsProcess(false);
      SetProcessInput(false);
      SetProcessUnhandledInput(false);
      SetProcessUnhandledKeyInput(false);
    }

    public override void _Process(double delta) => _onProcess?.OnNext(delta);
    public override void _PhysicsProcess(double delta) => _onPhysicsProcess?.OnNext(delta);
    public override void _Input(InputEvent ev) => _onInput?.OnNext(ev);
    public override void _UnhandledInput(InputEvent ev) => _onUnhandledInput?.OnNext(ev);
    public override void _UnhandledKeyInput(InputEvent ev) => _onUnhandledKeyInput?.OnNext(ev);

    protected override void Dispose(bool disposing) {
      _onProcess?.CompleteAndDispose();
      _onPhysicsProcess?.CompleteAndDispose();
      _onInput?.CompleteAndDispose();
      _onUnhandledInput?.CompleteAndDispose();
      _onUnhandledKeyInput?.CompleteAndDispose();

      base.Dispose(disposing);
    }
  }
}
