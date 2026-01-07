using Godot;

public partial class HurtBox : Area2D
{
    [Export] public HealthComponent HealthComponent { get; set; }

    public void ApplyDamage(int amount)
    {
        HealthComponent?.AdjustHealth(-amount);
    }
}

