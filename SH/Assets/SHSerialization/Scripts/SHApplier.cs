using UnityEngine;
using UnityEngine.Rendering;

namespace SH
{
    public static class SHApplier
    {
        static class ShaderProperty
        {
            public static readonly int[] SHA =
            {
                Shader.PropertyToID("unity_SHAr"),
                Shader.PropertyToID("unity_SHAg"),
                Shader.PropertyToID("unity_SHAb")
            };

            public static readonly int[] SHB =
            {
                Shader.PropertyToID("unity_SHBr"),
                Shader.PropertyToID("unity_SHBg"),
                Shader.PropertyToID("unity_SHBb")
            };

            public static readonly int SHC = Shader.PropertyToID("unity_SHC");
        }

        public static void Apply(MaterialPropertyBlock propertyBlock, SphericalHarmonicsL2 sh)
        {
            // Constant + Linear
            for (var i = 0; i < 3; i++)
            {
                propertyBlock.SetVector(ShaderProperty.SHA[i], new Vector4(
                    sh[i, 3], sh[i, 1], sh[i, 2], sh[i, 0] - sh[i, 6]
                ));
            }

            // Quadratic polynomials
            for (var i = 0; i < 3; i++)
            {
                propertyBlock.SetVector(ShaderProperty.SHB[i], new Vector4(
                    sh[i, 4], sh[i, 5], sh[i, 6] * 3, sh[i, 7]
                ));
            }

            // Final quadratic polynomial
            propertyBlock.SetVector(ShaderProperty.SHC, new Vector4(
                sh[0, 8], sh[1, 8], sh[2, 8], 1
            ));
        }
    }
}