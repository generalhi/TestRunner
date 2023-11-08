using System;
using System.Collections.Generic;
using Enums;
using Units;
using Units.Coins;
using Unity.Mathematics;
using UnityEngine;

namespace Modules
{
    public class CoinSpawner<T> where T : BaseCoin, new()
    {
        public Vector3 Position => _position;
        public Action OnPushDelay;

        private readonly Core _core;
        private readonly UnitType _unitType;
        private readonly UnitManager<BaseCoin> _unitManager;
        private readonly Vector3 _position;
        private readonly Stack<T> _units = new Stack<T>(100);
        private readonly Action<BaseCoin> OnPush;

        public CoinSpawner(
            Core core,
            UnitType unitType,
            UnitManager<BaseCoin> unitManager,
            Vector3 position)
        {
            _core = core;
            _unitType = unitType;
            _unitManager = unitManager;
            _position = position;

            OnPush = Push;
        }

        private void Push(BaseCoin coin)
        {
            _units.Push(coin as T);
            OnPushDelay?.Invoke();
        }

        public T Pop()
        {
            T unit;

            if (_units.Count == 0)
            {
                unit = New(_unitType);
            }
            else
            {
                unit = _units.Pop();
            }

            return unit;
        }

        private T New(UnitType unitType)
        {
            var gameObject = _core.PrefabManager.CreateNew(
                unitType,
                _position,
                quaternion.identity,
                _core.transform);

            var monoCoin = gameObject.GetComponent<MonoCoin>();
            T coin = new T();
            coin.Init(
                monoCoin,
                _core.LevelBuilder,
                OnPush,
                _core._PlayerController);
            _unitManager.Add(coin);

            return coin;
        }
    }
}
