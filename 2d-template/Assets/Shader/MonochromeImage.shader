Shader "Custom/MonochromeImage"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _IsMonochrome ("Is Monochrome", Float) = 0
        _StencilComp ("Stencil Comparison", Float) = 3
        _Stencil ("Stencil ID", Float) = 3
        _StencilOp ("Stencil Operation", Float) = 0.000000
        _StencilWriteMask ("Stencil Write Mask", Float) = 0
        _StencilReadMask ("Stencil Read Mask", Float) = 3
        _ColorMask ("Color Mask", Float) = 15.000000
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        Stencil  // 🔹 UGUI Mask를 위한 Stencil 설정 추가
        {
            Ref [_Stencil]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
            Comp [_StencilComp]
            Pass [_StencilOp]
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            Lighting Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float _IsMonochrome;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                if (_IsMonochrome > 0.5)
                {
                    float gray = dot(col.rgb, float3(0.299, 0.587, 0.114));
                    col.rgb = gray.xxx;
                }
                return col;
            }
            ENDCG
        }
    }
}
