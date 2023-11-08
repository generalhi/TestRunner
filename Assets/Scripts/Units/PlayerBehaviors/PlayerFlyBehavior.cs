using Modules;
using UnityEngine;

namespace Units.PlayerBehaviors
{
    public class PlayerFlyBehavior : IPlayerBehavior
    {
        private readonly Player _player;
        private Vector2 _targetPosition;

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
            _targetPosition = position;
        }

        public void OnUp()
        {
            if (_player == null)
            {
                return;
            }

            _targetPosition.y += 0.64f;
        }

        public void OnDown()
        {
            if (_player == null)
            {
                return;
            }

            _targetPosition.y -= 0.64f;
        }

        public void Update()
        {
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
        }
    }
}
