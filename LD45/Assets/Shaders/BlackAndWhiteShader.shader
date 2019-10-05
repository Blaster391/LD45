Shader "Hidden/BlackAndWhiteShader"
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

			float4 frag(v2f_img i) : COLOR{
				float baseAmount = 1.0f;
				float colourAmount = 0.0f;

				float4 inColour = tex2D(_MainTex, i.uv);
				float base = max(max(inColour.r, inColour.b), inColour.g);
				//total = total / 3;
				float4 col = fixed4(base, base, base, 1) * baseAmount + inColour * colourAmount;

				if (inColour.r < 0.5f && base > 0.5f)
				{
					float4 brown = float4(0.5, 0.2, 0.16, 1);

					col = brown * base;;
				}

                return col;
            }
            ENDCG
        }
    }
}
