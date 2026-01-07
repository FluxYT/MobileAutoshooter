using Godot;

namespace MobileAutoshooter;

public partial class EnemySpawner : Node
{
    [Export] private PackedScene EnemyPrefab { get; set; }
    [Export] private Timer SpawnTimer { get; set; }
    
    private Camera2D _camera;
    private RandomNumberGenerator _rng = new();

    public override void _Ready()
    {
        _camera = GetViewport().GetCamera2D();
        _rng.Randomize();
        SpawnTimer.Timeout += SpawnEnemy;
    }

    private void SpawnEnemy()
    {
        var enemy = EnemyPrefab.Instantiate<Enemy>();
        enemy.GlobalPosition = GetRandomSpawnPosition(_camera);
        GetTree().CurrentScene.AddChild(enemy);
    }


    private static Rect2 GetCameraRect(Camera2D camera)
    {
        var viewport = camera.GetViewport();
        var size = viewport.GetVisibleRect().Size * camera.Zoom;
        var center = camera.GlobalPosition;

        return new Rect2(
            center - size / 2f,
            size
        );
    }
    
    public Vector2 GetRandomSpawnPosition(Camera2D camera, float padding = 64f)
    {
        var rect = GetCameraRect(camera);

        rect.Position -= new Vector2(padding, padding);
        rect.Size += new Vector2(padding * 2, padding * 2);

        int side = (int)_rng.Randi() % 4;

        return side switch
        {
            0 => new Vector2( // top
                _rng.RandfRange(rect.Position.X, rect.End.X),
                rect.Position.Y
            ),
            1 => new Vector2( // bottom
                _rng.RandfRange(rect.Position.X, rect.End.X),
                rect.End.Y
            ),
            2 => new Vector2( // left
                rect.Position.X,
                _rng.RandfRange(rect.Position.Y, rect.End.Y)
            ),
            _ => new Vector2( // right
                rect.End.X,
                _rng.RandfRange(rect.Position.Y, rect.End.Y)
            )
        };
    }

}