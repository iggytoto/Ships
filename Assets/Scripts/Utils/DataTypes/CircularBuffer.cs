using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.DataTypes
{
    public class CircularBuffer<TKey, TValue> 
        where TKey : struct, IConvertible 
        where TValue : class
    {
        private TValue[] _buffer;
        private int _index;
        private Func<TValue, TKey> _keyGetter;

        public CircularBuffer(int bufferSize, Func<TValue, TKey> getKeyFn)
        {
            _buffer = new TValue[bufferSize];
            _index = 0;
            _keyGetter = getKeyFn;
        }
        
        public void Add(TValue value)
        {
            var bufferIndex = _index % _buffer.Length;
            
            _buffer[bufferIndex] = value;

            _index++;
        }

        public TValue FindByKey(TKey key)
        {
            return _buffer.FirstOrDefault(t => t != null &&  _keyGetter(t).Equals(key));
        }

        public void ReplaceByKey(TKey key, TValue value)
        {
            //TODO dummy
            for (var i = 0; i < _buffer.Length; i++)
            {
                if (_buffer[i] != null && _keyGetter(_buffer[i]).Equals(key))
                {
                    _buffer[i] = value;
                    break;
                }
            }
        }

        public IEnumerable<TValue> Where(Func<TValue, bool> predicate)
        {
            return _buffer.Where(predicate);
        }
    }
}