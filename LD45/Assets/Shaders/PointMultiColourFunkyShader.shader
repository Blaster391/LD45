Shader "Hidden/PointMultiColourFunkyShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_OriginalTex("OriginalTex", 2D) = "white" {}
		_point("Point", Vector) = (0.5,0.5,1,1)
		_radius("Radius", Float) = 0.1
		_Colour_One("1 Color", Color) = (1,0,0,1)
		_Colour_Two("2 Color", Color) = (0,1,0,1)
		_Colour_Three("3 Color", Color) = (0,0,1,1)
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
			uniform sampler2D _OriginalTex;
			uniform float2 _point;
			uniform float _radius;
			uniform float4 _Colour_One;
			uniform float4 _Colour_Two;
			uniform float4 _Colour_Three;

			float4 frag(v2f_img i) : COLOR{
				float4 inColour = tex2D(_MainTex, i.uv);

				float2 pointDiff = _point - i.uv;
				pointDiff.x = pointDiff.x * ( _ScreenParams.x / _ScreenParams.y);

				float power = _radius - sqrt(pointDiff.x * pointDiff.x + pointDiff.y * pointDiff.y);

				float4 col = inColour;
				if (power > 0)
				{
					//Do multicolour
					power = power * 1 / _radius;
				//	col = float4(power, power, power, 1) + inColour;
					float4 originalCol = tex2D(_OriginalTex, i.uv);

					float4 funkColour = originalCol.r * _Colour_One + originalCol.g * _Colour_Two + originalCol.b * _Colour_Three;
					col = (funkColour * power) + (inColour * (1 - power));
				}

				col.a = inColour.a;

                return col;
            }
            ENDCG
        }
    }
}
