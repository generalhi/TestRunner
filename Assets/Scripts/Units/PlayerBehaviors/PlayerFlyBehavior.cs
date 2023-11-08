using Modules;
using UnityEngine;

namespace Units.PlayerBehaviors
{
    public class PlayerFlyBehavior : IPlayerBehavior
    {
        private readonly Player _player;
#if UNITY_EDITOR
        private Vector2 _targetPosition;
#endif

        public PlayerFlyBehavior(PlayerController controller)
        {
            _player = controller.Player;
        }

        public void Start()
        {
            if (_player == null)
            {
                return;
            }

            _player.RB.gravityScale = 0f;
            _player.RB.velocity = Vector2.zero;

            var position = _player.RB.position;
            position.y = 0f;

            _player.RB.MovePosition(position);
#if UNITY_EDITOR
            _targetPosition = position;
#endif            
        }

        public void OnUp()
        {
            if (_player == null)
            {
                return;
            }

#if UNITY_EDITOR            
            _targetPosition.y += 0.64f;
#else            
            _player.RB.MovePosition(
                _player.RB.position + Vector2.up * 10f * Time.deltaTime);
#endif
        }

        public void OnDown()
        {
            if (_player == null)
            {
                return;
            }
            
#if UNITY_EDITOR
            _targetPosition.y -= 0.64f;
#else            
            _player.RB.MovePosition(
                _player.RB.position + Vector2.down * 10f * Time.deltaTime);
#endif
        }

        public void Update()
        {
#if UNITY_EDITOR            
            var position = _player.RB.position;
            _targetPosition.x = position.x;
            var vDir = _targetPosition - position;
            if (vDir.sqrMagnitude < 0.05f)
            {
                _player.RB.MovePosition(_targetPosition);
            }
            else
            {
                var newPosition = position + vDir * (100f * Time.deltaTime);
                _player.RB.MovePosition(newPosition);
            }
#endif            
        }
    }
}
