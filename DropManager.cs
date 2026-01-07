using Godot;
using System;

public partial class DropManager : Node
{
    [Export] private PackedScene DropPrefab { get; set; }

    public static DropManager Instance;

    public override void _EnterTree() => Instance = this;

    public void DropItem(Vector2 position)
    {
        var drop = DropPrefab.Instantiate<PickUp>();
        drop.Position = position;
        GetTree().CurrentScene.AddChild(drop);
    }
}
