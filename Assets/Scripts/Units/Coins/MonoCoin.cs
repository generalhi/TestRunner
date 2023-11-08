using System;
using Enums;
using UnityEngine;

namespace Units.Coins
{
    public class MonoCoin : MonoBehaviour
    {
        public Transform Transform => _transform;
        public BoxCollider2D Box => _box;
        public UnitType Type;
        public Action OnTriggerEnter;

        public float Scale
        {
            set => _transform.localScale = Vector3.one * value;
        }

        private Transform _transform;
        private BoxCollider2D _box;

        private void Awake()
        {
            _transform = transform;
            _box = GetComponent<BoxCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerEnter!.Invoke();
        }
    }
}
