using System.Collections.Generic;
using Units;

namespace Modules
{
    public class UnitManager<T> where T : IUnit
    {
        private readonly List<T> _items = new List<T>(1000);

        public void Update()
        {
            for (var i = 0; i < _items.Count; i++)
            {
                _items[i].CoreUpdate();
            }
        }

        public void Add(T item)
        {
            _items.Add(item);
        }
    }
}
