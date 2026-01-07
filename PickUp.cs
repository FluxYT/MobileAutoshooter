using Godot;
using System;
using GTweens.Easings;
using GTweens.Enums;
using GTweensGodot.Extensions;

public partial class PickUp : Area2D
{
    [Export] private Sprite2D Sprite { get; set; }
    [Export] public float MaxSpeed = 900f;
    [Export] public float Accel = 6000f;

    private Node2D _target;
    private Vector2 _velocity;

    public void MagnetTo(Node2D target)
    {
        _target = target;
    }

    public override void _Ready()
    {
        var tween = Sprite.TweenPositionY(-2f, 0.75f)
            .SetEasing(Easing.InOutSine)
            .SetLoops(0, ResetMode.PingPong);

        tween.OnComplete(() =>
        {
            tween.Reset(false, ResetMode.PingPong);
            tween.Play();
        });
        
        tween.Play();
    }
    
    public override void _PhysicsProcess(double delta)
    {
        if (!IsInstanceValid(_target)) return;

        var dt = (float)delta;
        var toTarget = _target.GlobalPosition - GlobalPosition;
        var dist = toTarget.Length();

        var desired = toTarget.Normalized() * MaxSpeed;
        _velocity = _velocity.MoveToward(desired, Accel * dt);

        GlobalPosition += _velocity * dt;
    }
}
