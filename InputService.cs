using Godot;
using System;

public partial class InputService : Node
{
    public static InputService Instance { get; private set; }

    private Vector2 _joystickOutput;

    public override void _EnterTree() => Instance = this;

    public Vector2 GetJoystickOutput()
    {
        return _joystickOutput;
    }

    public void SetJoystickOutput(Vector2 joystickOutput)
    {
        _joystickOutput = joystickOutput;
    }
}
