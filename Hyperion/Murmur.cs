using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Hyperion
{
    /// <summary>
    /// Murmur3 Hash implementation
    /// </summary>
    internal static class Murmur
    {
        // Magic values used for MurmurHash's 32 bit hash.
        // Don't change these without consulting a hashing expert!
        private const uint VISIBLE_MAGIC = 0x971e137b;
        private const uint HIDDEN_MAGIC_A = 0x95543787;
        private const uint HIDDEN_MAGIC_B = 0x2ad7eb25;
        private const uint VISIBLE_MIXER = 0x52dce729;
        private const uint HIDDEN_MIXER_A = 0x7b7d159c;
        private const uint HIDDEN_MIXER_B = 0x6bce6396;
        private const uint FINAL_MIXER_1 = 0x85ebca6b;
        private const uint FINAL_MIXER_2 = 0xc2b2ae35;

        // Arbitrary values used for hashing certain classes

        private const uint StringSeed = 0x331df49;
        private const uint ArraySeed = 0x3c074a61;

        /** The first 23 magic integers from the first stream are stored here */
        private static readonly uint[] StoredMagicA;

        /** The first 23 magic integers from the second stream are stored here */
        private static readonly uint[] StoredMagicB;

        /// <summary>
        /// The initial magic integer in the first stream.
        /// </summary>
        private const uint StartMagicA = HIDDEN_MAGIC_A;

        /// <summary>
        /// The initial magic integer in the second stream.
        /// </summary>
        private const uint StartMagicB = HIDDEN_MAGIC_B;

        /// <summary>
        /// TBD
        /// </summary>
        static Murmur()
        {
            //compute range of values for StoredMagicA
            var storedMagicA = new List<uint>();
            var nextMagicA = HIDDEN_MAGIC_A;
            foreach (var i in Enumerable.Repeat(0, 23))
            {
                nextMagicA = NextMagicA(nextMagicA);
                storedMagicA.Add(nextMagicA);
            }
            StoredMagicA = storedMagicA.ToArray();

            //compute range of values for StoredMagicB
            var storedMagicB = new List<uint>();
            var nextMagicB = HIDDEN_MAGIC_B;
            foreach (var i in Enumerable.Repeat(0, 23))
            {
                nextMagicB = NextMagicB(nextMagicB);
                storedMagicB.Add(nextMagicB);
            }
            StoredMagicB = storedMagicB.ToArray();
        }

        /// <summary>
        /// Begin a new hash with a seed value.
        /// </summary>
        /// <param name="seed">TBD</param>
        /// <returns>TBD</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint StartHash(uint seed)
        {
            return seed ^ VISIBLE_MAGIC;
        }

        /// <summary>
        /// Given a magic integer from the first stream, compute the next
        /// </summary>
        /// <param name="magicA">TBD</param>
        /// <returns>TBD</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint NextMagicA(uint magicA)
        {
            return magicA * 5 + HIDDEN_MIXER_A;
        }

        /// <summary>
        /// Given a magic integer from the second stream, compute the next
        /// </summary>
        /// <param name="magicB">TBD</param>
        /// <returns>TBD</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint NextMagicB(uint magicB)
        {
            return magicB * 5 + HIDDEN_MIXER_B;
        }

        /// <summary>
        /// Incorporates a new value into an existing hash
        /// </summary>
        /// <param name="hash">The prior hash value</param>
        /// <param name="value">The new value to incorporate</param>
        /// <param name="magicA">A magic integer from the left of the stream</param>
        /// <param name="magicB">A magic integer from a different stream</param>
        /// <returns>The updated hash value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint ExtendHash(uint hash, uint value, uint magicA, uint magicB)
        {
            return (hash ^ RotateLeft32(value * magicA, 11) * magicB) * 3 + VISIBLE_MIXER;
        }

        /// <summary>
        /// Once all hashes have been incorporated, this performs a final mixing.
        /// </summary>
        /// <param name="hash">TBD</param>
        /// <returns>TBD</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint FinalizeHash(uint hash)
        {
            var h = (hash ^ (hash >> 16));
            h *= FINAL_MIXER_1;
            h ^= h >> 13;
            h *= FINAL_MIXER_2;
            h ^= h >> 16;
            return h;
        }

        #region Internal 32-bit hashing helpers

        /// <summary>
        /// Rotate a 32-bit unsigned integer to the left by <paramref name="shift"/> bits
        /// </summary>
        /// <param name="original">Original value</param>
        /// <param name="shift">The shift value</param>
        /// <returns>The rotated 32-bit integer</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint RotateLeft32(uint original, int shift)
        {
            return (original << shift) | (original >> (32 - shift));
        }

        #endregion

        /// <summary>
        /// Compute a high-quality hash of an array
        /// </summary>
        /// <param name="a">TBD</param>
        /// <returns>TBD</returns>
        public static int Hash(ReadOnlySpan<byte> a)
        {
            unchecked
            {
                var h = StartHash((uint)a.Length * ArraySeed);
                var c = HIDDEN_MAGIC_A;
                var k = HIDDEN_MAGIC_B;
                var j = 0;
                while (j < a.Length)
                {
                    h = ExtendHash(h, (uint)a[j], c, k);
                    c = NextMagicA(c);
                    k = NextMagicB(k);
                    j += 1;
                }
                return (int)FinalizeHash(h);
            }
        }

        /// <summary>
        /// Compute high-quality hash of a string
        /// </summary>
        /// <param name="s">TBD</param>
        /// <returns>TBD</returns>
        public static int Hash(string s)
        {
            unchecked
            {
                var sChar = s.AsReadOnlySpan();
                var h = StartHash((uint)s.Length * StringSeed);
                var c = HIDDEN_MAGIC_A;
                var k = HIDDEN_MAGIC_B;
                var j = 0;
                while (j + 1 < s.Length)
                {
                    var i = (uint)((sChar[j] << 16) + sChar[j + 1]);
                    h = ExtendHash(h, i, c, k);
                    c = NextMagicA(c);
                    k = NextMagicB(k);
                    j += 2;
                }
                if (j < s.Length) h = ExtendHash(h, sChar[j], c, k);
                return (int)FinalizeHash(h);
            }
        }
    }
}