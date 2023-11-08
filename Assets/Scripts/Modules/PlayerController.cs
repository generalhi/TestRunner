using Enums;
using Ui;
using Units;
using Units.PlayerBehaviors;
using Unity.Mathematics;
using UnityEngine;

namespace Modules
{
    public class PlayerController
    {
        public Player Player => _player;
        public IPlayerBehavior PlayerBehavior => _playerBehavior;

        private readonly Core _core;
        private readonly MainForm _mainForm;
        private Player _player;
        private IPlayerBehavior _playerBehavior;
        private readonly IPlayerBehavior _playerDefaultBehavior;
        private bool _isDefaultBehavior = true;
        private float _timer;
        private const float BonusDeltaTime = 5f; // Not 10 seconds. My game my rules :) 

        public PlayerController(
            Core core,
            LevelBuilder levelBuilder,
            MainForm mainForm)
        {
            _core = core;
            _mainForm = mainForm;
            _playerDefaultBehavior = new PlayerDefaultBehavior(this, levelBuilder);
            SetDefaultBehavior();
        }

        public void ResetTimer(float newTime = 0f)
        {
            _timer = newTime == 0f ? BonusDeltaTime : newTime;
        }

        public void SetBehavior(IPlayerBehavior behavior)
        {
            _playerBehavior = behavior;
            _isDefaultBehavior = false;
            _playerBehavior.Start();
        }

        public void Update()
        {
            CreatePlayer();
            UpdateTimer();
            _playerBehavior.Update();

            if (_player != null)
            {
                _player!.CoreUpdate();
            }
        }

        private void CreatePlayer()
        {
            if (_player == null &&
                _core.LevelBuilder.IsStartGroundComplete)
            {
                var position = new Vector3(
                    700f,
                    Screen.height / 2f,
                    0f);
                position = _core.Camera.ScreenToWorldPoint(position);
                position.z = 0f;
                var gameObject = _core.PrefabManager.CreateNew(
                    UnitType.Player, position,
                    quaternion.identity);
                _player = gameObject.GetComponent<Player>();

                InitEvents();
            }
        }

        private void UpdateTimer()
        {
            var isFirst = _timer > 0f;
            _timer -= Time.deltaTime;
            var isSecond = _timer > 0f;

            if (isFirst != isSecond)
            {
                if (_isDefaultBehavior)
                {
                    _playerBehavior.Start();
                }
                else
                {
                    SetDefaultBehavior();
                }
            }
        }

        private void InitEvents()
        {
            _mainForm.OnButtonUp = PlayerBehaviorOnUpHandler;
            _mainForm.OnButtonDown = PlayerBehaviorOnDownHandler;
        }

        private void PlayerBehaviorOnUpHandler()
        {
            _playerBehavior.OnUp();
        }

        private void PlayerBehaviorOnDownHandler()
        {
            _playerBehavior.OnDown();
        }

        private void SetDefaultBehavior()
        {
            _playerBehavior = _playerDefaultBehavior;
            _playerBehavior.Start();
            _isDefaultBehavior = true;
        }
    }
}
