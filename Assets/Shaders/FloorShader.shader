Shader "Unlit/FloorShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        [Toggle] _IsCeiling ("Is Ceiling", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 color : COLOR0;
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD0;
            };

            float4 _Color;
            float _IsCeiling;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = _Color;
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 col = i.color;
                float yDifference = 0.5f - (i.screenPos.y / i.screenPos.w);
                if (_IsCeiling)
                    yDifference = -yDifference;
                col.rgb *= 2.0f * yDifference;
                
                return col;
            }
            ENDCG
        }
    }
}
