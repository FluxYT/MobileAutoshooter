using Godot;

namespace MobileAutoshooter;

public partial class Projectile : Node2D
{
    [Export] public float Speed { get; set; } = 600f;
    
    [Export] private Area2D Collider { get; set; }
    
    private Vector2 _direction = Vector2.Right;

    public override void _Ready()
    {
        Collider.AreaEntered += (area) =>
        {
            if (area is HurtBox hurtbox)
            {
                hurtbox.ApplyDamage(50);
                
                QueueFree();
            }
        };
    }

    public void SetDirection(Vector2 dir)
    {
        _direction = dir.Normalized();
        Rotation = dir.Angle();
    }
    
    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition += _direction * Speed * (float)delta;
    }
}