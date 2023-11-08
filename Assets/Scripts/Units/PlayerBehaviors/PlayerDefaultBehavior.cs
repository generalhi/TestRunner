using Modules;
using UnityEngine;

namespace Units.PlayerBehaviors
{
    public class PlayerDefaultBehavior : IPlayerBehavior
    {
        private readonly PlayerController _controller;
        private readonly LevelBuilder _levelBuilder;

        public PlayerDefaultBehavior(
            PlayerController controller,
            LevelBuilder levelBuilder)
        {
            _controller = controller;
            _levelBuilder = levelBuilder;
        }

        public void Start()
        {
            if (_controller.Player == null)
            {
                return;
            }

            _controller.Player.RB.gravityScale = 2f;
            _levelBuilder.ResetSpeed();
        }

        public void OnUp()
        {
            var player = _controller.Player;
            if (player == null)
            {
                return;
            }

            if (!player.IsGround)
            {
                return;
            }

            player.RB.AddForce(Vector2.up * 7f, ForceMode2D.Impulse);
            player.SetIsGround(false);
        }

        public void OnDown()
        {
        }

        public void Update()
        {
        }
    }
}
