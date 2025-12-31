using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    [Export] private float MoveSpeed { get; set; } = 100f;
    
    private Node2D _currentTarget;

    public override void _Ready()
    {
        _currentTarget = GetTree().GetFirstNodeInGroup("Player") as Node2D;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 dir = (_currentTarget.GlobalPosition - GlobalPosition).Normalized();
        Velocity = dir * MoveSpeed;
        MoveAndSlide();
    }
}
