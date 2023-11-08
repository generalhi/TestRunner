namespace Units.PlayerBehaviors
{
    public interface IPlayerBehavior
    {
        void Start();
        void OnUp();
        void OnDown();
        void Update();
    }
}
