using System;
using System.Collections.Generic;
using Enums;
using Units;
using Unity.Mathematics;
using UnityEngine;

namespace Modules
{
    public class BrickSpawner
    {
        public Vector3 Position => _position;
        public Action OnPush;

        private readonly Core _core;
        private readonly UnitType _unitType;
        private readonly UnitManager<Brick> _unitManager;
        private readonly Vector3 _position;
        private readonly Stack<Brick> _units = new Stack<Brick>(100);

        public BrickSpawner(
            Core core,
            UnitManager<Brick> unitManager,
            UnitType unitType,
            Vector3 position)
        {
            _core = core;
            _unitManager = unitManager;
            _unitType = unitType;
            _position = position;
        }

        public void Push(Brick brick)
        {
            _units.Push(brick);
            OnPush?.Invoke();
        }

        public Brick Pop()
        {
            Brick unit;

            if (_units.Count == 0)
            {
                unit = New();
            }
            else
            {
                unit = _units.Pop();
            }

            return unit;
        }

        private Brick New()
        {
            var gameObject = _core.PrefabManager.CreateNew(
                _unitType,
                _position,
                quaternion.identity,
                _core.transform);

            var brick = gameObject.GetComponent<Brick>();
            _unitManager.Add(brick);

            brick.Init(_core.LevelBuilder, this, _core._PlayerController);

            return brick;
        }
    }
}
