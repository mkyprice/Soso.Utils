using System;

namespace Soso.Utils.Random
{
    public class SosoRandom
    {
        public readonly ulong Seed = 0;
        public long State { get; private set; } = 0;
        private readonly Xoshiro256Random _generator = null;
        private static readonly float _floatMax = Convert.ToSingle(UInt64.MaxValue);
        private static readonly double _doubleMax = Convert.ToDouble(UInt64.MaxValue);
        
        public SosoRandom(ulong seed, long state = 0)
        {
            Seed = seed;
            State = state;
            _generator = new Xoshiro256Random(Seed);
            unchecked
            {
                long ff = state;
                while (ff > 0)
                {
                    ff--;
                    _generator.Next();
                }
            }
        }

        public SosoRandom() : this((ulong)Guid.NewGuid().ToString().GetHashCode())
        {
        }

        public SosoRandom(int seed) : this((ulong)seed)
        {
            
        }

        public int Next(int min, int max)
        {
            State++;
            ulong range = (ulong)(max - min);
            int result = (int)(Next() % range) + min;
            if (result >= max) result--;
            if (result < min) result = min;
            return result;
        }

        public float Next(float min, float max)
        {
            State++;
            float randFloat = Convert.ToSingle(Next());
            float range = max - min;
            return (randFloat / _floatMax) * range + min;
        }

        public double Next(double min, double max)
        {
            State++;
            double randDouble = Convert.ToDouble(Next());
            double range = max - min;
            return (randDouble / _doubleMax) * range + min;
        }

        private ulong Next()
        {
            return _generator.Next();
        }
    }
}