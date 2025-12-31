using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
    [Export] private float MoveSpeed { get; set; } = 150f;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 moveDirection = InputService.Instance.GetJoystickOutput();
        if (moveDirection == Vector2.Zero)
        {
            moveDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        }
        
        Velocity = moveDirection * MoveSpeed;
        MoveAndSlide();
            
    }
}
