using Enums;
using Units;
using Units.Coins;
using UnityEngine;

namespace Modules
{
    public class LevelBuilder
    {
        public float X1 => _x1;
        public float X2 => _x2;
        public float Speed => _speed;

        public bool IsStartGroundComplete => _isStartGroundComplete;
        public CoinSpawner<CoinSpeedUp> CoinSpeedUpSpawner => _coinSpeedUpSpawner;

        private readonly BrickSpawner _upBrickSpawner;
        private readonly BrickSpawner _downBrickSpawner;
        private readonly CoinSpawner<CoinSpeedUp> _coinSpeedUpSpawner;
        private readonly CoinSpawner<CoinSpeedDown> _coinSpeedDownSpawner;
        private readonly CoinSpawner<CoinSpeedRevers> _coinSpeedRevers;
        private readonly CoinSpawner<CoinFly> _coinFlySpawner;

        private bool _isStartGroundComplete = false;

        private const float BaseSpeed = 3f;
        private const float DeltaSpeed = 1f;
        private float _speed = BaseSpeed;
        private bool _isSpeedBonus;

        private readonly float _x1;
        private readonly float _x2;

        private Brick _lastBrickGround;
        private bool _isNewLine;

        private int _hGround = 1;
        private int _hCeiling = 1;

        private int _lGround;
        private int _lCeiling;
        private int _lCoins;

        public LevelBuilder(Core core)
        {
            _x1 = 400f;
            _x2 = Screen.width - 50f;
            var halfScreenX = _x1 + (_x2 - _x1) / 2f;
            var v1 = Vector3.right * _x1;
            var v2 = Vector3.right * _x2;
            _x1 = core.Camera.ScreenToWorldPoint(v1).x;
            _x2 = core.Camera.ScreenToWorldPoint(v2).x;

            _lastBrickGround = null;

            _upBrickSpawner = new BrickSpawner(
                core,
                core.BrickManager,
                UnitType.Brick,
                InitSpawnerPosition(core, halfScreenX, VerticalState.Up));
            _downBrickSpawner = new BrickSpawner(
                core,
                core.BrickManager,
                UnitType.Brick,
                InitSpawnerPosition(core, halfScreenX, VerticalState.Down))
            {
                OnPush = OnBrickSpawnerPushHandler
            };

            _coinSpeedUpSpawner = new CoinSpawner<CoinSpeedUp>(
                core,
                UnitType.CoinSpeedUp,
                core.CoinManager,
                new Vector3(-2000f, 0f, 0f));
            _coinSpeedDownSpawner = new CoinSpawner<CoinSpeedDown>(
                core,
                UnitType.CountSpeedDown,
                core.CoinManager,
                new Vector3(-2000f, 0f, 0f));
            _coinSpeedRevers = new CoinSpawner<CoinSpeedRevers>(
                core,
                UnitType.CountSpeedRevers,
                core.CoinManager,
                new Vector3(-2000f, 0f, 0f));

            _coinFlySpawner = new CoinSpawner<CoinFly>(
                core,
                UnitType.CoinFly,
                core.CoinManager,
                new Vector3(-2000f, 0f, 0f));
        }

        private void OnBrickSpawnerPushHandler()
        {
            _isStartGroundComplete = true;
        }

        private Vector3 InitSpawnerPosition(Core core, float x, VerticalState verticalState)
        {
            float y = 0f;
            Vector3 v = Vector3.zero;

            switch (verticalState)
            {
                case VerticalState.Up:
                {
                    y = Screen.height - 40f;
                    v = new Vector3(x, y, 0f);
                    v = core.Camera.ScreenToWorldPoint(v);
                    break;
                }
                case VerticalState.Down:
                {
                    y = 40f;
                    v = new Vector3(x, y, 0f);
                    v = core.Camera.ScreenToWorldPoint(v);
                    break;
                }
            }

            v.z = 0f;
            return v;
        }

        public void ResetSpeed()
        {
            _speed = BaseSpeed;
        }

        public void AddSpeed()
        {
            _speed += DeltaSpeed;
        }

        public void SubSpeed()
        {
            _speed -= DeltaSpeed;
        }

        public void ReverseSpeed()
        {
            _speed = -DeltaSpeed;
        }

        public void Update()
        {
            UpdateGround();
            UpdateCeiling();
            UpdateCoins();
        }

        private void UpdateGround()
        {
            if (_lastBrickGround == null)
            {
                _lastBrickGround = _downBrickSpawner.Pop();
                _lastBrickGround.SetBegin(
                    new Vector3(0f, GetGroundY(0), 0f));
                return;
            }

            _isNewLine = false;

            if ((_lastBrickGround.State == UnitState.Begin ||
                 _lastBrickGround.State == UnitState.Place) &&
                _lastBrickGround.PlaceX < X2)
            {
                _isNewLine = true;
                UpdateGroundHeight();

                for (var h = 0; h < _hGround; h++)
                {
                    var brick = _downBrickSpawner.Pop();
                    brick.SetBegin(
                        new Vector3(
                            _lastBrickGround.PlaceX + 0.32f,
                            GetGroundY(h),
                            0f));

                    if (h == 0)
                    {
                        _lastBrickGround = brick;
                    }
                }
            }
        }

        private void UpdateCeiling()
        {
            if (_isStartGroundComplete &&
                _isNewLine)
            {
                UpdateCeilingHeight();

                for (var h = 0; h < _hCeiling; h++)
                {
                    var brick = _upBrickSpawner.Pop();
                    brick.SetBegin(
                        new Vector3(
                            _lastBrickGround.PlaceX + 0.32f,
                            GetCeilingY(h),
                            0f));
                }
            }
        }

        private void UpdateGroundHeight()
        {
            if (!IsStartGroundComplete)
            {
                return;
            }

            _lGround--;

            if (_lGround < 1)
            {
                _lGround = Random.Range(6, 12);

                var hRandom = Random.Range(1, 10);
                if (_hGround <= 5 && hRandom > 5)
                {
                    _hGround++;
                }
                else if (_hGround > 1)
                {
                    _hGround--;
                }
            }
        }

        private void UpdateCeilingHeight()
        {
            if (!IsStartGroundComplete)
            {
                return;
            }

            _lCeiling--;

            if (_lCeiling < 0)
            {
                _lCeiling = Random.Range(1, 2);

                var hRandom = Random.Range(1, 10);
                if (_hCeiling <= 5 && hRandom > 4)
                {
                    _hCeiling += Random.Range(1, 3);
                }
                else if (_hCeiling > 1)
                {
                    _hCeiling -= Random.Range(1, 5);
                }
            }

            _hCeiling = Mathf.Clamp(_hCeiling, 1, 5);
        }

        private float GetGroundY(int h)
        {
            return -2.24f + h * 0.32f;
        }

        private float GetCeilingY(int h)
        {
            return -2.24f + 0.32f * 14f - h * 0.32f;
        }

        private void UpdateCoins()
        {
            if (!_isStartGroundComplete ||
                !_isNewLine)
            {
                return;
            }

            _lCoins--;

            if (_lCoins < 1)
            {
                BaseCoin monoCoin = null;

                _lCoins = Random.Range(3, 20);
                var rType = Random.Range(0, 100);
                if (rType > 90)
                {
                    monoCoin = _coinFlySpawner.Pop();
                }
                else if (rType > 80)
                {
                    monoCoin = _coinSpeedRevers.Pop();
                }
                else
                {
                    monoCoin = _isSpeedBonus ? _coinSpeedUpSpawner.Pop() : _coinSpeedDownSpawner.Pop();
                    _isSpeedBonus = !_isSpeedBonus;
                }

                monoCoin.SetPlace(
                    new Vector3(
                        _lastBrickGround.PlaceX + 0.32f,
                        GetGroundY(_hGround),
                        0f));
            }
        }
    }
}
