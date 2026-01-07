using System.Collections.Generic;
using Godot;

namespace MobileAutoshooter;

public partial class AttackController : Node2D
{
    [Export] private Area2D DetectionZone { get; set; }
    [Export] private Timer AttackTimer { get; set; }
    [Export] private PackedScene BulletPrefab { get; set; }
    
    private readonly HashSet<Node2D> _enemiesInRange = [];

    public override void _Ready()
    {
        AttackTimer.Timeout += OnAttackTick;
        DetectionZone.BodyEntered += OnBodyEntered;
        DetectionZone.BodyExited += OnBodyExited;
    }

    private void OnAttackTick()
    {
        _enemiesInRange.RemoveWhere(e => !IsInstanceValid(e));
        
        var target = GetClosestEnemy();
        if (target is null)
            return;
        
        FireBullet(target);
    }

    private void FireBullet(Node2D target)
    {
        if (BulletPrefab is null) return;

        var bullet = BulletPrefab.Instantiate<Projectile>();
        bullet.GlobalPosition = this.GlobalPosition;
        var direction = GlobalPosition.DirectionTo(target.GlobalPosition);
        bullet.SetDirection(direction);
        GetTree().CurrentScene.AddChild(bullet);
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("Enemy"))
        {
            _enemiesInRange.Add(body);
        }
    }
    
    private void OnBodyExited(Node2D body)
    {
        if (body.IsInGroup("Enemy"))
        {
            _enemiesInRange.Remove(body);
        }
    }
    
    private Node2D GetClosestEnemy()
    {
        Node2D best = null;
        float bestDistSq = float.PositiveInfinity;

        // If this script is on the Player, use GetParent<Node2D>() or export a Player reference
        var origin = GetParent<Node2D>().GlobalPosition;

        foreach (var enemy in _enemiesInRange)
        {
            if (!IsInstanceValid(enemy))
                continue;

            float d = origin.DistanceSquaredTo(enemy.GlobalPosition);
            if (d < bestDistSq)
            {
                bestDistSq = d;
                best = enemy;
            }
        }

        return best;
    }
}