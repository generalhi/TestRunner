using UnityEngine;

namespace Modules
{
    public class SinMovement
    {
        public Vector3 Position;

        private const float Speed = 1f;
        private const float Amplitude = 1f;

        private readonly SinValue _sin;
        private Vector3 _vAmplitude;

        public SinMovement()
        {
            _sin = new SinValue(Speed);
            InitAmplitudeVector();
        }

        public void InitAmplitudeVector()
        {
            _vAmplitude = Random.rotation * Vector3.up * Amplitude;
            _sin.ResetAngle();
        }

        public void Update()
        {
            Position = _vAmplitude * _sin.Value;
            _sin.Update();
        }
    }
}
