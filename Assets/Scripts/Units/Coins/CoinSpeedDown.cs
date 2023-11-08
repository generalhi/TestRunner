namespace Units.Coins
{
    public class CoinSpeedDown : BaseCoin
    {
        public override void OnTriggerEnter()
        {
            base.OnTriggerEnter();
            LevelBuilder.SubSpeed();
        }
    }
}
