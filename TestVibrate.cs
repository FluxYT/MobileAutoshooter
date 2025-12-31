using Godot;

namespace MobileAutoshooter;

public partial class TestVibrate : TouchScreenButton
{
    public override void _Ready()
    {
        Pressed += () =>
        {
            GD.Print("Pressed");
            Input.VibrateHandheld(500, 1.0f);
        };
    }
}