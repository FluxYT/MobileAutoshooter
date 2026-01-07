using Godot;
using System;

public partial class HealthComponent : Node
{
    [Export] public int MaxHealth { get; set; } = 100;
    [Signal] public delegate void HealthChangedEventHandler(int newHealth);
    [Signal] public delegate void DestroyedEventHandler();
    
    private int _currentHealth;

    public override void _Ready()
    {
        _currentHealth = MaxHealth;
    }

    public void AdjustHealth(int amount)
    {
        _currentHealth += amount;
        EmitSignal(SignalName.HealthChanged, _currentHealth);

        if (_currentHealth > 0) return;
        
        _currentHealth = 0;
        OnDeath();
    }

    private void OnDeath()
    {
        EmitSignal(SignalName.Destroyed);
    }
}
