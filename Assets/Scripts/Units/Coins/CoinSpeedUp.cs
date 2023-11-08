namespace Units.Coins
{
    public class CoinSpeedUp : BaseCoin
    {
        public override void OnTriggerEnter()
        {
            base.OnTriggerEnter();
            LevelBuilder.AddSpeed();
        }
    }
}
