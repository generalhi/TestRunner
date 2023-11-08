using UnityEngine;

namespace Modules
{
    public class SinValue
    {
        public float Value => _value;

        private readonly float _speed;
        private float _angle;
        private float _value;

        public SinValue(float speed)
        {
            _speed = speed;
        }

        public void ResetAngle(float angle = 0f)
        {
            _angle = angle;
        }

        public void Update()
        {
            _value = Mathf.Sin(_angle);

            _angle += _speed * Time.deltaTime;
            if (_angle > 180f)
            {
                _angle -= 180f;
            }
        }
    }
}
