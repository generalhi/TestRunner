using Modules;
using UnityEngine;

namespace Units
{
    public class Player : MonoBehaviour
    {
        public Transform Transform => _transform;
        public Rigidbody2D RB => _rb;
        public bool IsGround => _isGround;

        private Transform _transform;
        private Rigidbody2D _rb;
        private SinValue _sin;

        private bool _isGround;

        private void Awake()
        {
            _transform = transform;
            _rb = GetComponent<Rigidbody2D>();
            _sin = new SinValue(10f);
            _sin.ResetAngle();
        }

        public void SetIsGround(bool isGround)
        {
            _isGround = isGround;
        }

        public void CoreUpdate()
        {
            _transform.localScale = new Vector3(
                1f,
                0.75f + 0.25f * _sin.Value,
                1f);
            _sin.Update();
        }
    }
}
