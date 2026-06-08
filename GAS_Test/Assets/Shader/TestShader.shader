Shader "Custom/TestShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags {"RenderType" = "Opaque"}

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
            };

            fixed4 _BaseColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return _BaseColor;
            }

            ENDHLSL
        }
    }
}
