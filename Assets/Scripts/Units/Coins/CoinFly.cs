using Units.PlayerBehaviors;

namespace Units.Coins
{
    public class CoinFly : BaseCoin
    {
        public override void OnTriggerEnter()
        {
            if (PlayerController.PlayerBehavior is PlayerFlyBehavior)
            {
                return;
            }

            base.OnTriggerEnter();
            PlayerController.SetBehavior(new PlayerFlyBehavior(PlayerController));
        }
    }
}
