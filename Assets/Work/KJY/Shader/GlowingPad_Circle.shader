Shader "Tycoon/GlowingPad_CircleEdgeOnly_URP"
{
    Properties
    {
        _GlowColor("Glow Color", Color) = (1, 0.1, 0.9, 1)

        _Radius("Radius (Local)", Range(0.05, 1.0)) = 0.5
        _EdgeWidth("Edge Width", Range(0.001, 0.5)) = 0.12
        _EdgeSoftness("Edge Softness", Range(0.001, 0.5)) = 0.08

        _GlowIntensity("Glow Intensity", Range(0, 20)) = 6
        _PulseSpeed("Pulse Speed", Range(0, 10)) = 2.0
        _PulseAmount("Pulse Amount", Range(0, 2)) = 0.5

        _ScanSpeed("Scan Speed", Range(0, 10)) = 1.2
        _ScanStrength("Scan Strength", Range(0, 2)) = 0.5

        _TopOnly("Top Only (1=Top)", Range(0, 1)) = 1
        _TopCut("Top Normal Cut", Range(0, 1)) = 0.6
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalRenderPipeline"
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        Pass
        {
            Name "Forward"
            Tags { "LightMode"="UniversalForward" }

            Cull Back
            ZWrite Off
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float4 _GlowColor;

            float _Radius;
            float _EdgeWidth;
            float _EdgeSoftness;

            float _GlowIntensity;
            float _PulseSpeed;
            float _PulseAmount;

            float _ScanSpeed;
            float _ScanStrength;

            float _TopOnly;
            float _TopCut;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionOS : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
            };

            float hash21(float2 p)
            {
                p = frac(p * float2(123.34, 456.21));
                p += dot(p, p + 34.345);
                return frac(p.x * p.y);
            }

            Varyings vert(Attributes v)
            {
                Varyings o;

                VertexPositionInputs posInputs = GetVertexPositionInputs(v.positionOS.xyz);
                VertexNormalInputs nInputs = GetVertexNormalInputs(v.normalOS);

                o.positionHCS = posInputs.positionCS;
                o.positionWS = posInputs.positionWS;
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);

                o.positionOS = v.positionOS.xyz;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float3 N = normalize(i.normalWS);

                float topFace = saturate(dot(N, float3(0, 1, 0)));
                float topMask = lerp(1.0, step(_TopCut, topFace), _TopOnly);

                float2 p = i.positionOS.xz;

                float dist = length(p);

                float outer = _Radius;
                float inner = max(0.0, _Radius - _EdgeWidth);

                float ringOuter = smoothstep(outer - _EdgeSoftness, outer, dist);
                float ringInner = 1.0 - smoothstep(inner, inner + _EdgeSoftness, dist);

                float ring = saturate(ringOuter * ringInner);

                ring *= topMask;

                float t = _Time.y;

                float pulse = 1.0 + sin(t * _PulseSpeed) * _PulseAmount;

                float2 uv = i.positionWS.xz * 0.2;
                float n = (hash21(uv * 20 + t) - 0.5) * 0.2;

                float scanPhase = frac(t * _ScanSpeed);
                float scan = 1.0 - abs(frac((uv.x + uv.y) + scanPhase) - 0.5) * 2.0;
                scan = saturate(scan) * _ScanStrength;

                float intensity = _GlowIntensity * pulse * saturate(0.75 + scan + n);

                float a = saturate(ring);

                float3 col = _GlowColor.rgb * (intensity * a);

                return half4(col, a);
            }
            ENDHLSL
        }
    }
}
