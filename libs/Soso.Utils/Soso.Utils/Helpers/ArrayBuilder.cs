using System;
using System.Collections.Generic;

namespace Soso.Utils.Helpers
{
    public struct ArrayBuilder<T>
    {
        public static uint Capacity = 16;
        public static uint MaxCapacity = Capacity * Capacity;
        public int Length => _totalLength;
        private List<T[]> _buffers;
        private T[] _array;
        private int _totalLength = 0;
        private int _arrayIndex = 0;

        public ArrayBuilder()
        {
            _array = new T[Capacity];
        }

        public void Append(T item)
        {
            if (_arrayIndex >= _array.Length)
            {
                if (_array.Length >= MaxCapacity)
                {
                    if (_buffers == null)
                    {
                        _buffers = new List<T[]>();
                    }

                    _buffers.Add(_array);
                    _array = new T[Capacity];
                    _arrayIndex = 0;
                }
                else
                {
                    Array.Resize(ref _array, _array.Length * 2);
                }
            }
            _array[_arrayIndex] = item;
            _arrayIndex++;
            _totalLength++;
        }

        public void Insert(int index, T item)
        {
            if (index < 0) throw new IndexOutOfRangeException();
            if (_totalLength + 1 >= _array.Length) throw new IndexOutOfRangeException();
            _array[index] = item;
        }

        public IEnumerable<T[]> GetBuffers()
        {
            if (_buffers != null)
            {
                foreach (T[] buffer in _buffers)
                {
                    yield return buffer;
                }
            }
            yield return _array;
        }

        public T[] ToArray()
        {
            T[] result = new T[_totalLength];
            int index = 0;
            if (_buffers != null)
            {
                foreach (T[] buffer in _buffers)
                {
                    buffer.CopyTo(result, index);
                    index += buffer.Length;
                }
            }

            Array.Copy(_array, 0, result, index, _arrayIndex);
            return result;
        }

        public void AppendRange(in IEnumerable<T> source)
        {
            foreach (T item in source)
            {
                Append(item);
            }
        }
    }
}