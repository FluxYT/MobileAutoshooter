using Godot;
using System;

public partial class PickUpCollector : Area2D
{
    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is PickUp pickup)
        {
            pickup.QueueFree();
            GameEvents.Instance.RaisePickupCollected();
        }
    }
}
