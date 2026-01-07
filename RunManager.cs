using Godot;

namespace MobileAutoshooter;

public partial class RunManager : Node
{
    private int _currentSouls = 0;

    public override void _Ready()
    {
        GameEvents.Instance.PickupCollected += OnPickupCollected;
    }

    public override void _ExitTree()
    {
        GameEvents.Instance.PickupCollected -= OnPickupCollected;
    }

    private void OnPickupCollected()
    {
        _currentSouls++;
        GameEvents.Instance.RaiseSoulsUpdated(_currentSouls);
    }
}