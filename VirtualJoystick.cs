using Godot;
using System;

using Godot;

public partial class VirtualJoystick : Control
{
    [Export] public float MaxRadius = 120f;
    [Export] public float DeadZone = 12f;

    public Vector2 Output { get; private set; } = Vector2.Zero;

    private bool _active = false;
    private int _fingerId = -1;
    private Vector2 _center;

    private Control _base;
    private Control _knob;

    public override void _Ready()
    {
        _base = GetNodeOrNull<Control>("Base");
        _knob = GetNodeOrNull<Control>("Knob");

        SetVisualVisible(false);
    }

    public override void _UnhandledInput(InputEvent e)
    {
        // Touch down
        if (e is InputEventScreenTouch touch)
        {
            if (touch.Pressed && !_active)
            {
                _active = true;
                _fingerId = touch.Index;

                _center = touch.Position; // local to this Control (Full Rect)
                SetOutput(Vector2.Zero);

                SetVisualVisible(true);
                PlaceVisuals(_center, _center);
            }
            else if (!touch.Pressed && _active && touch.Index == _fingerId)
            {
                ResetStick();
            }
        }

        // Drag
        if (e is InputEventScreenDrag drag)
        {
            if (_active && drag.Index == _fingerId)
            {
                UpdateStick(drag.Position);
            }
        }
    }

    private void UpdateStick(Vector2 fingerPos)
    {
        Vector2 delta = fingerPos - _center;
        float dist = delta.Length();

        if (dist < DeadZone)
        {
            Output = Vector2.Zero;
            PlaceVisuals(_center, _center);
            return;
        }

        Vector2 clamped = delta;
        if (dist > MaxRadius)
            clamped = delta / dist * MaxRadius;

        SetOutput(delta.Normalized());

        PlaceVisuals(_center, _center + clamped);
    }

    private void SetOutput(Vector2 output)
    {
        Output = output;
        InputService.Instance.SetJoystickOutput(output);
    }

    private void ResetStick()
    {
        _active = false;
        _fingerId = -1;
        SetOutput(Vector2.Zero);
        SetVisualVisible(false);
    }

    private void SetVisualVisible(bool visible)
    {
        if (_base != null) _base.Visible = visible;
        if (_knob != null) _knob.Visible = visible;
    }

    private void PlaceVisuals(Vector2 basePos, Vector2 knobPos)
    {
        // These nodes should be centered visuals; easiest is to set their pivot/anchors appropriately.
        if (_base != null) _base.Position = basePos - _base.Size * 0.5f;
        if (_knob != null) _knob.Position = knobPos - _knob.Size * 0.5f;
    }
}

