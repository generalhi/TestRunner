using Enums;
using UnityEngine;

namespace Modules
{
    public class PrefabManager : MonoBehaviour
    {
        public GameObject Player;
        public GameObject Brick;
        public GameObject CoinSpeedUp;
        public GameObject CoinSpeedDown;
        public GameObject CoinFly;
        public GameObject CoinRevers;

        public GameObject CreateNew(
            UnitType type,
            Vector3 position,
            Quaternion rotation,
            Transform parent = null)
        {
            GameObject prefab = null;

            switch (type)
            {
                case UnitType.Player:
                {
                    prefab = Player;
                    break;
                }
                case UnitType.Brick:
                {
                    prefab = Brick;
                    break;
                }
                case UnitType.CoinSpeedUp:
                {
                    prefab = CoinSpeedUp;
                    break;
                }
                case UnitType.CountSpeedDown:
                {
                    prefab = CoinSpeedDown;
                    break;
                }
                case UnitType.CoinFly:
                {
                    prefab = CoinFly;
                    break;
                }
                case UnitType.CountSpeedRevers:
                {
                    prefab = CoinRevers;
                    break;
                }
            }

            if (prefab == null)
            {
                Debug.LogError($"{type} prefab is null");
                return null;
            }

            return Instantiate(prefab, position, rotation, parent);
        }
    }
}
