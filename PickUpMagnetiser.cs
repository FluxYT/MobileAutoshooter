using Godot;

namespace MobileAutoshooter;

public partial class PickUpMagnetiser : Area2D
{
    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is PickUp pickup)
        {
            pickup.MagnetTo(GetParent() as Node2D);
        }
    }
}