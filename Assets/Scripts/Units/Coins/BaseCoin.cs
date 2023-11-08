using System;
using Enums;
using Modules;
using UnityEngine;

namespace Units.Coins
{
    public abstract class BaseCoin : IUnit
    {
        public LevelBuilder LevelBuilder => _levelBuilder;
        public PlayerController PlayerController => _playerController;

        private MonoCoin _monoCoin;

        private UnitState _unitState;
        private LevelBuilder _levelBuilder;
        private Action<BaseCoin> _onSpawnerPush;
        private PlayerController _playerController;

        public void Init(
            MonoCoin monoCoin,
            LevelBuilder levelBuilder,
            Action<BaseCoin> onSpawnerPush,
            PlayerController playerController)
        {
            _monoCoin = monoCoin;
            _levelBuilder = levelBuilder;
            _onSpawnerPush = onSpawnerPush;
            _playerController = playerController;

            _monoCoin.OnTriggerEnter = OnTriggerEnter;
        }

        public void CoreUpdate()
        {
            if (_unitState == UnitState.Spawn)
            {
                return;
            }

            _monoCoin.Transform.position += Time.deltaTime * _levelBuilder.Speed * Vector3.left;

            if (_monoCoin.Transform.position.x <= _levelBuilder.X1)
            {
                SetSpawn();
                return;
            }

            var scaleR = Mathf.Clamp(_levelBuilder.X2 + 0.64f - _monoCoin.Transform.position.x, 0f, 2f);
            var scaleL = _levelBuilder.X1 - _monoCoin.Transform.position.x;
            if (scaleR > 0f && scaleR < 2f)
            {
                _monoCoin.Scale = Mathf.Clamp(scaleR / 2f, 0f, 1f);
            }
            else if (scaleL < 0f && scaleL > -1f)
            {
                _monoCoin.Scale = Mathf.Clamp(scaleL / -1f, 0f, 1f);
            }
            else
            {
                _monoCoin.Scale = 1f;
            }
        }

        public void SetPlace(Vector3 position)
        {
            _unitState = UnitState.Place;
            _monoCoin.Box.enabled = true;
            _monoCoin.Transform.position = position;
            _monoCoin.Scale = 0f;
        }

        private void SetSpawn()
        {
            _unitState = UnitState.Spawn;
            _monoCoin.Box.enabled = false;
            _monoCoin.Transform.position = _levelBuilder.CoinSpeedUpSpawner.Position;
            _onSpawnerPush(this);
        }

        public virtual void OnTriggerEnter()
        {
            _playerController.ResetTimer();
            SetSpawn();
        }
    }
}
