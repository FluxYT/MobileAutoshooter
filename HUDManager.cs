using Godot;

namespace MobileAutoshooter;

public partial class HUDManager : CanvasLayer
{
    [Export] private Label SoulsCounter { get; set; }

    public override void _Ready()
    {
        GameEvents.Instance.SoulsUpdated += (newSouls) =>
        {
            SoulsCounter.Text = $"Souls: {newSouls}";
        };
    }
}