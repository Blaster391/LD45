Shader "Hidden/PointMultiColourShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_point("Point", Vector) = (0.5,0.5,1,1)
		_radius("Radius", Float) = 0.1
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
			uniform float2 _point;
			uniform float _radius;

			float4 frag(v2f_img i) : COLOR{
				float4 inColour = tex2D(_MainTex, i.uv);
				float2 pointDiff = _point - i.uv;

				float power = _radius - sqrt(pointDiff.x * pointDiff.x + pointDiff.y * pointDiff.y);

				float4 col = inColour;
				if (power > 0)
				{
					//Do multicolour
					power = power * 1 / _radius;
					col = float4(power, power, power, 1) + inColour;
				}

                return col;
            }
            ENDCG
        }
    }
}
