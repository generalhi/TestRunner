using Enums;
using Modules;
using UnityEngine;

namespace Units
{
    public class Brick : MonoBehaviour, IUnit
    {
        public UnitState State => _state;
        public float PlaceX => _placePosition.x;

        public float Scale
        {
            set => _transform.localScale = Vector3.one * value;
        }

        private LevelBuilder _levelBuilder;
        private BrickSpawner _brickSpawner;
        private PlayerController _playerController;

        private Transform _transform;
        private BoxCollider2D _boxCollider2D;
        private UnitState _state;
        private Vector3 _placePosition;
        private Vector3 _spawnPosition;
        private const float _beginSpeed = 3f;
        private const float _endSpeed = 0.5f;
        private SinMovement _sin;
        private float _progress;

        private void Awake()
        {
            _transform = transform;
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _state = UnitState.Spawn;
            _spawnPosition = _transform.position;
            _sin = new SinMovement();
        }

        public void Init(
            LevelBuilder levelBuilder,
            BrickSpawner brickSpawner,
            PlayerController playerController)
        {
            _levelBuilder = levelBuilder;
            _brickSpawner = brickSpawner;
            _playerController = playerController;
        }

        public void SetBegin(Vector3 position)
        {
            _state = UnitState.Begin;
            _placePosition = position;
            _spawnPosition = _transform.position;
            _progress = 0f;
        }

        private void SetPlace()
        {
            _state = UnitState.Place;
        }

        private void SetEnd()
        {
            _state = UnitState.End;
            _spawnPosition = _brickSpawner.Position;
            _progress = 0f;
        }

        private void SetSpawn()
        {
            _state = UnitState.Spawn;
            _sin.InitAmplitudeVector();
            _brickSpawner.Push(this);
        }

        public void CoreUpdate()
        {
            switch (_state)
            {
                case UnitState.Spawn:
                    UpdateSpawn();
                    break;
                case UnitState.Begin:
                    UpdateBegin();
                    break;
                case UnitState.Place:
                    UpdatePlace();
                    break;
                case UnitState.End:
                    UpdateEnd();
                    break;
            }
        }

        private void UpdateSpawn()
        {
            _placePosition = _spawnPosition + _sin.Position;
            _transform.position = _placePosition;
            _sin.Update();
        }

        private void UpdateBegin()
        {
            _transform.position = Vector3.Lerp(_spawnPosition, _placePosition, _progress);
            _placePosition += Time.deltaTime * _levelBuilder.Speed * Vector3.left;
            Scale = 0.5f;

            _progress += Time.deltaTime * _beginSpeed;
            if (_progress >= 1f)
            {
                SetPlace();
            }
        }

        private void UpdatePlace()
        {
            _placePosition += Time.deltaTime * _levelBuilder.Speed * Vector3.left;
            _transform.position = _placePosition;

            if (_placePosition.x <= _levelBuilder.X1)
            {
                SetEnd();
                return;
            }

            var scaleR = _levelBuilder.X2 - PlaceX;
            var scaleL = _levelBuilder.X1 - PlaceX;
            if (scaleR > 0f && scaleR < 2f)
            {
                Scale = Mathf.Clamp(scaleR / 2f, 0f, 1f);
                _boxCollider2D.enabled = false;
            }
            else if (scaleL < 0f && scaleL > -1f)
            {
                Scale = Mathf.Clamp(scaleL / -1f, 0f, 1f);
                _boxCollider2D.enabled = false;
            }
            else
            {
                Scale = 1f;
                _boxCollider2D.enabled = true;
            }
        }

        private void UpdateEnd()
        {
            _transform.position = Vector3.Lerp(_placePosition, _spawnPosition, _progress);

            _progress += Time.deltaTime * _endSpeed;
            if (_progress >= 1f)
            {
                SetSpawn();
            }

            Scale = Mathf.Lerp(0f, 0.5f, _progress);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var player = _playerController.Player;
            var playerPosition = player.transform.position;
            var dx = Mathf.Abs(_transform.position.x - playerPosition.x);
            var dy = Mathf.Abs(_transform.position.y - playerPosition.y);
            if (dx > dy)
            {
                return;
            }

            _playerController.Player.SetIsGround(true);
        }
    }
}
