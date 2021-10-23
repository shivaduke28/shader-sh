Shader "SH/EvalSH"
{
    Properties {}
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "./EvalSH.hlsl"
            ENDCG
        }
    }
}