using System;
using IL2CPU.API.Attribs;
using Cosmos.HAL;

namespace Cosmos.System_Plugs.System
{
    [Plug(Target = typeof(Random))]
    public static class RandomImpl
    {
        public static void Ctor(Random aThis)
        {
            //empty
        }

        public static void Ctor(Random aThis, int seed)
        {
            //empty ATM
        }

        // TODO: improve this
        public static int GenerateGlobalSeed() => RTC.Second;

        public static int Next(Random aThis, int maxValue)
        {
            return (int)(GetUniform() * maxValue);
        }

        public static int Next(Random aThis)
        {
            return (int)(GetUniform() * int.MaxValue);
        }

        public static int Next(Random aThis, int minValue, int maxValue)
        {
            int diff = maxValue - minValue;
            if (diff <= 1)
                return minValue;
            return Next(aThis) % diff + minValue;
        }

        public static void NextBytes(Random aThis, byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (byte)(GetUniform() * (byte.MaxValue + 1));
            }
        }

        public static double NextDouble(Random aThis)
        {
            return GetUniform();
        }

        static uint counter = 0;
        static uint lastSeed = 0;
        static double GetUniform()
        {
            uint seed = (uint)RTC.Second;
            if(lastSeed == seed)
            {
                counter++; //If either of these overflow, no problem since we just have to ensure that two random numbers quickly generated after each other are not the same
                seed += counter * 1000;
            }
            lastSeed = seed;
            uint m_w = (uint)(seed >> 16);
            uint m_z = (uint)(seed % 4294967296);
            m_z = 36969 * (m_z & 65535) + (m_z >> 16);
            m_w = 18000 * (m_w & 65535) + (m_w >> 16);
            uint u = (m_z << 16) + m_w;
            double uniform = (u + 1.0) * 2.328306435454494e-10;
            return uniform;
        }
    }
}
