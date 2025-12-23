Shader "UI/IrisFade"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Color", Color) = (0,0,0,1)
        _Radius("Radius", Range(0,1)) = 1
        _Softness("Softness", Range(0.0001,0.5)) = 0.08
        _Noise("Noise", Range(0,1)) = 0.12
        _NoiseScale("NoiseScale", Range(1,500)) = 180
        _Center("Center", Vector) = (0.5,0.5,0,0)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Overlay"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _Color;
            float _Radius;
            float _Softness;
            float _Noise;
            float _NoiseScale;
            float4 _Center;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float hash21(float2 p)
            {
                p = frac(p * float2(123.34, 456.21));
                p += dot(p, p + 45.32);
                return frac(p.x * p.y);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 c = _Center.xy;

                float2 d = uv - c;

                float aspect = _ScreenParams.x / _ScreenParams.y;

                float2 dd = d;
                dd.x *= aspect;

                float maxDist = length(float2(0.5 * aspect, 0.5));
                float dist = length(dd) / maxDist;

                float n = hash21(uv * _NoiseScale) * 2 - 1;
                dist += n * _Noise * 0.05;

                float edge = smoothstep(_Radius - _Softness, _Radius + _Softness, dist);

                float closed = 1.0 - smoothstep(0.0, _Softness, _Radius);
                edge = lerp(edge, 1.0, closed);

                fixed4 col = _Color;
                col.a *= saturate(edge);
                return col;
            }
            ENDHLSL
        }
    }
}
