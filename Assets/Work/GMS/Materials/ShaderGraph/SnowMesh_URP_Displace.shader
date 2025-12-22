Shader "Custom/SnowMesh_URP_Displace"
{
    Properties
    {
        _SnowMask ("Snow Mask", 2D) = "black" {}
        _SnowTex ("Snow Albedo", 2D) = "white" {}
        _DisplaceStrength ("Displace Strength", Float) = 0.3
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
        }

        Pass
        {
            Name "ForwardUnlit"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_SnowMask);
            SAMPLER(sampler_SnowMask);

            TEXTURE2D(_SnowTex);
            SAMPLER(sampler_SnowTex);

            float _DisplaceStrength;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                float mask = SAMPLE_TEXTURE2D_LOD(
                    _SnowMask,
                    sampler_SnowMask,
                    IN.uv,
                    0
                ).r;

                float dig = 1.0 - mask;

                float3 pos = IN.positionOS.xyz;
                pos.y -= dig * _DisplaceStrength;

                OUT.positionCS = TransformObjectToHClip(pos);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                return SAMPLE_TEXTURE2D(
                    _SnowTex,
                    sampler_SnowTex,
                    IN.uv
                );
            }
            ENDHLSL
        }
    }
}
