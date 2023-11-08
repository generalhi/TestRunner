using Modules;
using Ui;
using Units;
using Units.Coins;
using UnityEngine;

public class Core : MonoBehaviour
{
    public Camera Camera => _camera;
    public PrefabManager PrefabManager => _prefabManager;
    public PlayerController _PlayerController => _playerController;
    public UnitManager<Brick> BrickManager => _brickManager;
    public UnitManager<BaseCoin> CoinManager => _coinManager;
    public LevelBuilder LevelBuilder => _levelBuilder;

    public MainForm MainForm;

    private Camera _camera;
    private PrefabManager _prefabManager;
    private PlayerController _playerController;
    private UnitManager<Brick> _brickManager;
    private UnitManager<BaseCoin> _coinManager;
    private LevelBuilder _levelBuilder;

    // Main entry point
    private void Awake()
    {
        _camera = Camera.main;
        if (!TryGetComponent(out _prefabManager))
        {
            Debug.LogError("Prefab Manager not found");
        }

        _brickManager = new UnitManager<Brick>();
        _coinManager = new UnitManager<BaseCoin>();
        _levelBuilder = new LevelBuilder(this);
        _playerController = new PlayerController(this, _levelBuilder, MainForm);
    }

    private void Update()
    {
        _playerController.Update();
        _levelBuilder.Update();
        _brickManager.Update();
        _coinManager.Update();
    }
}
