namespace Units.Coins
{
    public class CoinSpeedRevers : BaseCoin
    {
        public override void OnTriggerEnter()
        {
            base.OnTriggerEnter();
            LevelBuilder.ReverseSpeed();
            PlayerController.ResetTimer(2f);
        }
    }
}
