using Godot;
using System;

public partial class GameEvents : Node
{
    public static GameEvents Instance;

    public override void _EnterTree() => Instance = this;
    public override void _ExitTree() => Instance = null;
    
    [Signal] public delegate void PickupCollectedEventHandler();
    [Signal] public delegate void SoulsUpdatedEventHandler(int newSouls);

    public void RaisePickupCollected() => EmitSignal(SignalName.PickupCollected);
    public void RaiseSoulsUpdated(int newSouls) => EmitSignal(SignalName.SoulsUpdated, newSouls);
}
