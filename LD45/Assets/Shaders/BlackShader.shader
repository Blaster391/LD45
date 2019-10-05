Shader "Hidden/BlackShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
			 #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

			uniform sampler2D _MainTex;

			float4 frag(v2f_img i) : COLOR{
                return float4(0,0,0,1);
            }
            ENDCG
        }
    }
}
