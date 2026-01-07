using Godot;
using System;
using GTweens.Builders;
using GTweens.Easings;
using GTweens.Extensions;
using GTweens.Tweens;
using GTweensGodot.Extensions;

public partial class Enemy : CharacterBody2D
{
    [Export] private float MoveSpeed { get; set; } = 100f;
    [Export] private Sprite2D Sprite { get; set; }
    [Export] private HealthComponent HealthComponent { get; set; }
    [Export] private HurtBox HurtBox { get; set; }
    [Export] private CollisionShape2D Collider { get; set; }
    
    private Node2D _currentTarget;
    private ShaderMaterial _spriteMaterial;
    private GTween _flashTween;
    
    private bool _isDying = false;

    public override void _Ready()
    {
        _currentTarget = GetTree().GetFirstNodeInGroup("Player") as Node2D;
        _spriteMaterial = Sprite.Material as ShaderMaterial;
        HealthComponent.HealthChanged += OnHealthChanged;
        HealthComponent.Destroyed += () =>
        {
            if (_isDying) return;
            _isDying = true;

            CallDeferred(nameof(OnDiedDeferred));
        };
    }

    private void OnDiedDeferred()
    {
        HurtBox.Monitoring = false;
        HurtBox.Monitorable = false;
        Collider.Disabled = true;
        
        SetPhysicsProcess(false);
        SetProcess(false);

        DropManager.Instance.DropItem(GlobalPosition);

        var sequence = GTweenSequenceBuilder.New()
            .Append(Sprite.TweenScale(Vector2.Zero, 0.25f))
            .AppendCallback(QueueFree);
        
        sequence.Build().Play();
    }

    private void OnHealthChanged(int newHealth)
    {
        HitFlash();
        HitJuice();
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 dir = (_currentTarget.GlobalPosition - GlobalPosition).Normalized();
        Velocity = dir * MoveSpeed;
        MoveAndSlide();
    }
    
    public void HitFlash(float duration = 0.2f)
    {
        if (_spriteMaterial == null) return;
        
        // Kill previous flash so rapid hits feel snappy
        _flashTween?.Kill();
        
        _spriteMaterial.SetShaderParameter("flash_amount", 1.0f);

        _flashTween = GTweenExtensions.Tween(
                () => (float)_spriteMaterial.GetShaderParameter("flash_amount"),
                v => _spriteMaterial.SetShaderParameter("flash_amount", v),
                0.0f,
                duration
            )
            .SetEasing(Easing.OutCubic);

        _flashTween.Play();
    }
    
    public void HitJuice(float duration = 0.1f)
    {
        var sequence = GTweenSequenceBuilder.New()
            .Append(Sprite.TweenScale(new Vector2(1.08f, 0.92f), 0.1f))
            .Append(Sprite.TweenScale(Vector2.One, duration));
        
        sequence.Build().Play();
    }
}
