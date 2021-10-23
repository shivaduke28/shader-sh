using System;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Mathf;

namespace SH
{
    public static class SHUtility
    {
        static readonly int[] _idSHA = {
            Shader.PropertyToID("unity_SHAr"),
            Shader.PropertyToID("unity_SHAg"),
            Shader.PropertyToID("unity_SHAb")
        };

        static readonly int[] _idSHB = {
            Shader.PropertyToID("unity_SHBr"),
            Shader.PropertyToID("unity_SHBg"),
            Shader.PropertyToID("unity_SHBb")
        };
        static readonly int _idSHC = Shader.PropertyToID("unity_SHC");

        // ref: https://github.com/keijiro/LightProbeUtility
        // NOTE: orders of coefficients in quadratic polynomials had been changed.
        public static void ApplySH(MaterialPropertyBlock propertyBlock, SphericalHarmonicsL2 sh)
        {
            // Constant + Linear
            for (var i = 0; i < 3; i++)
                propertyBlock.SetVector(_idSHA[i], new Vector4(
                    sh[i, 3], sh[i, 1], sh[i, 2], sh[i, 0] - sh[i, 6]
                ));

            // Quadratic polynomials
            for (var i = 0; i < 3; i++)
                propertyBlock.SetVector(_idSHB[i], new Vector4(
                    sh[i, 4], sh[i, 5], sh[i, 6] * 3, sh[i, 7]
                ));

            // Final quadratic polynomial
            propertyBlock.SetVector(_idSHC, new Vector4(
                 sh[0, 8], sh[1, 8], sh[2, 8], 1
            ));

        }

        public static void ApplySH(Material material, SphericalHarmonicsL2 sh)
        {
            // Constant + Linear
            for (var i = 0; i < 3; i++)
                material.SetVector(_idSHA[i], new Vector4(
                    sh[i, 3], sh[i, 1], sh[i, 2], sh[i, 0] - sh[i, 6]
                ));

            // Quadratic polynomials
            for (var i = 0; i < 3; i++)
                material.SetVector(_idSHB[i], new Vector4(
                    sh[i, 4], sh[i, 5], sh[i, 6] * 3, sh[i, 7]
                ));

            // Final quadratic polynomial
            material.SetVector(_idSHC, new Vector4(
                 sh[0, 8], sh[1, 8], sh[2, 8], 1
            ));
        }


        // Reimplementation of SphericalHarmonicsL2.AddDirectionalLight for learning purposes.
        public static void AddDirectionalLightSH(ref SphericalHarmonicsL2 sh, int shIndex, Vector3 dir, Color color, float intensity)
        {
            var lDotY = EvalSH(shIndex, dir);    // L_{lm} = dot(L, Y_{lm}) is equal to the evaluation at dir.
            var coeff = lDotY * ACoeff[shIndex]; // (L \ast A)_{lm} = dot(L \ast A,Y), where A(t) = max(0,cos(t))
            coeff *= SHCoeff[shIndex];           // fold the leading constants of SH polynomials into lighting coefficients
            for (var colorIndex = 0; colorIndex < 3; colorIndex++)
            {
                sh[colorIndex, shIndex] += coeff * color.linear[colorIndex] * intensity;
            }
        }

        // For A(t) = max(0, cos(t)),
        // A_{lm} = dot(A,Y_{lm}) vanishes when m != 0.
        // ACoeff is the array of
        // \hat{A}_l = sqrt(4*PI/(2l+1)) * A_{l0}
        // so that the SH coefficient of the convolution product of A and L is
        // caluculated as
        // (A \ast L)_{lm} = \hat{A}_l * L_{lm}
        // see https://cseweb.ucsd.edu/~ravir/papers/envmap/
        static readonly float[] ACoeff = {
            PI,
            2f * PI / 3f,
            2f * PI / 3f,
            2f * PI / 3f,
            PI * 0.25f,
            PI * 0.25f,
            PI * 0.25f,
            PI * 0.25f,
            PI * 0.25f,
        };

        static readonly float[] SHCoeff =
        {
            0.5f * Sqrt(1 / PI),
            Sqrt(3f / (4f * PI)),
            Sqrt(3f / (4f * PI)),
            Sqrt(3f / (4f * PI)),
            0.5f * Sqrt(15f / PI),
            0.5f * Sqrt(15f / PI),
            0.25f * Sqrt(5f / PI),
            0.5f * Sqrt(15f / PI),
            0.25f * Sqrt(15f / PI)
        };

        public static float Y0(Vector3 p) => SHCoeff[0]; // Y_{00}
        public static float Y1(Vector3 p) => SHCoeff[1] * p.y; // Y_{1,-1}
        public static float Y2(Vector3 p) => SHCoeff[2] * p.z; // Y_{1,0}
        public static float Y3(Vector3 p) => SHCoeff[3] * p.x; // Y_{1,1}
        public static float Y4(Vector3 p) => SHCoeff[4] * p.x * p.y; // Y_{2,-2}
        public static float Y5(Vector3 p) => SHCoeff[5] * p.y * p.z; // Y_{2,-1}
        public static float Y6(Vector3 p) => SHCoeff[6] * (3f * p.z * p.z - 1f); // Y_{2, 0}
        public static float Y7(Vector3 p) => SHCoeff[7] * p.z * p.x; // Y_{2,1}
        public static float Y8(Vector3 p) => SHCoeff[8] * (p.x * p.x - p.y * p.y); // Y_{2,2}

        public static float EvalSH(int i, Vector3 p)
        {
            switch (i)
            {
                case 0:
                    return Y0(p);
                case 1:
                    return Y1(p);
                case 2:
                    return Y2(p);
                case 3:
                    return Y3(p);
                case 4:
                    return Y4(p);
                case 5:
                    return Y5(p);
                case 6:
                    return Y6(p);
                case 7:
                    return Y7(p);
                case 8:
                    return Y8(p);
                default:
                    throw new ArgumentException();
            }
        }
    }
}