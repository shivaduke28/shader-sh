using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace SH
{
    [Serializable]
    public class SHData
    {
        [SerializeField] SHL2[] coefficients;

        public SphericalHarmonicsL2 ToUnity()
        {
            var sh = new SphericalHarmonicsL2();
            for (var c = 0; c < 3; c++)
            {
                for (var i = 0; i < 9; i++)
                {
                    sh[c, i] = coefficients[c][i];
                }
            }

            return sh;
        }


        public void FromUnity(SphericalHarmonicsL2 sh)
        {
            coefficients = new SHL2[3];

            // RGB
            for (var c = 0; c < 3; c++)
            {
                var l2 = new SHL2();

                for (var i = 0; i < 9; i++)
                {
                    l2[i] = sh[c, i];
                }

                coefficients[c] = l2;
            }
        }

        [Serializable]
        public sealed class SHL2
        {
            [SerializeField] float[] coefficients = new float[9];

            public float this[int index]
            {
                get => coefficients[index];
                set => coefficients[index] = value;
            }
        }
    }
}