using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Soso.Utils.Helpers
{
    public struct ExtendedArrayBuilder<T>
    {
        public static int Capacity = 32;
        public static int MaxBufferCapacity = Capacity * Capacity;
        private T[] _buffer;
        private ArrayBuilder<T[]> _extendedBuffers;
        private int _length = 0;
        private int _currentIndex = 0;
        
        public ExtendedArrayBuilder()
        {
            _buffer = new T[Capacity];
        }

        public void Append(T item)
        {
            if (_currentIndex >= _buffer.Length)
            {
                if (_currentIndex >= MaxBufferCapacity)
                {
                    if (_length == MaxBufferCapacity)
                    {
                        _extendedBuffers = new ArrayBuilder<T[]>();
                    }

                    Debug.Assert(_buffer != null, "_buffer was null?");
                    _extendedBuffers.Append(_buffer);
                    _buffer = new T[Capacity];
                    _currentIndex = 0;
                }
                else
                {
                    Array.Resize(ref _buffer, _buffer.Length * 2);
                }
            }
            
            _buffer[_currentIndex] = item;
            _currentIndex++;
            _length++;
        }

        public void AppendRange(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                Append(item);
            }
        }

        public T[] ToArray()
        {
            T[] result = new T[_length];
            if (_length > MaxBufferCapacity)
            {
                int index = 0;
                foreach (T[][] buffer in _extendedBuffers.GetBuffers())
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        T[] item = buffer[i];
                        if (item == null)
                        {
                            break;
                        }
                        item.CopyTo(result, index);
                        index += item.Length;
                    }
                }

                Array.Copy(_buffer, 0, result, index, _currentIndex);
            }
            else
            {
                Array.Copy(_buffer, 0, result, 0, _length);
            }

            return result;
        }
    }
}