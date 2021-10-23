#ifndef EVAL_SH_INCLUDED
#define EVAL_SH_INCLUDED

#include "UnityCG.cginc"
struct appdata
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
};

struct v2f
{
    float4 vertex : SV_POSITION;
    float3 normal : TEXCOORD1;
};

v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.normal = UnityObjectToWorldNormal(v.normal);
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    fixed4 col;
    half4 normal = half4(normalize(i.normal), 1);
    col.rgb = ShadeSH9(normal);
    return col;
}

// The source code below is copied from UnityCG.cginc for reader's convinience.
#if 0
/*
Unity built-in shader source. Copyright (c) 2016 Unity Technologies.
Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

// normal should be normalized, w=1.0
half3 SHEvalLinearL0L1(half4 normal)
{
    half3 x;

    // Linear (L1) + constant (L0) polynomial terms
    x.r = dot(unity_SHAr, normal);
    x.g = dot(unity_SHAg, normal);
    x.b = dot(unity_SHAb, normal);

    return x;
}

// normal should be normalized, w=1.0
half3 SHEvalLinearL2(half4 normal)
{
    half3 x1, x2;
    // 4 of the quadratic (L2) polynomials
    half4 vB = normal.xyzz * normal.yzzx;
    x1.r = dot(unity_SHBr, vB);
    x1.g = dot(unity_SHBg, vB);
    x1.b = dot(unity_SHBb, vB);

    // Final (5th) quadratic (L2) polynomial
    half vC = normal.x * normal.x - normal.y * normal.y;
    x2 = unity_SHC.rgb * vC;

    return x1 + x2;
}

// normal should be normalized, w=1.0
// output in active color space
half3 ShadeSH9(half4 normal)
{
    // Linear + constant polynomial terms
    half3 res = SHEvalLinearL0L1(normal);

    // Quadratic polynomials
    res += SHEvalLinearL2(normal);

#ifdef UNITY_COLORSPACE_GAMMA
        res = LinearToGammaSpace (res);
#endif

    return res;
}
#endif
#endif