Shader "Hidden/MultiColourShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Colour_One("Some Color", Color) = (1,1,1,1)
		_Colour_Two("Some Color", Color) = (1,1,1,1)
		_Colour_Three("Some Color", Color) = (1,1,1,1)
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
			uniform fixed4 _Colour_One;
			uniform fixed4 _Colour_Two;
			uniform fixed4 _Colour_Three;

			float4 frag(v2f_img i) : COLOR{
				float4 inColour = tex2D(_MainTex, i.uv);
                //fixed4 inColour = tex2D(_MainTex, i.uv);
				float total = inColour.r + inColour.b + inColour.g;
				float4 col = fixed4(0, 0, 0, 1);
				if (total != 0)
				{
					//col = ((_Colour_One * inColour.r) + (_Colour_Two + inColour.g) + (_Colour_Three + inColour.b)) / total;
			
					col = (_Colour_One * inColour.r);// / total;
					col = col + (_Colour_Two * inColour.g);// / total;
					col = col + (_Colour_Three * inColour.b);// / total;
					col.a = 1;

				}
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
