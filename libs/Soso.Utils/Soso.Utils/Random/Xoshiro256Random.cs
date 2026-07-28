namespace Soso.Utils.Random
{
    /// <summary>
    /// Xoshiro256++
    /// </summary>
    public class Xoshiro256Random
    {
        private ulong[] _state = new ulong[4];

        public Xoshiro256Random(ulong seed)
        {
            _state[0] = SplitMix(seed);
            _state[1] = SplitMix(_state[0]);
            _state[2] = SplitMix(_state[1]);
            _state[3] = SplitMix(_state[2]);
        }

        public ulong Next()
        {
            ulong result = Rol64(_state[0] + _state[3], 23) + _state[0];
            ulong t = _state[1] << 17;

            _state[2] ^= _state[0];
            _state[3] ^= _state[1];
            _state[1] ^= _state[2];
            _state[0] ^= _state[3];

            _state[2] ^= t;
            _state[3] = Rol64(_state[3], 45);

            return result;
        }

        private ulong SplitMix(ulong state)
        {
            ulong result = (state + 0x9E3779B97f4A7C15);
            result = (result ^ (result >> 30)) * 0xBF58476D1CE4E5B9;
            result = (result ^ (result >> 27)) * 0x94D049BB133111EB;
            return result ^ (result >> 31);
        }

        private ulong Rol64(ulong x, int k)
        {
            return (x << k) | (x >> (64 - k));
        }
    }
}