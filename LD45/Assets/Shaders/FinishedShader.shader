Shader "Hidden/FinishedShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_OriginalTex("OriginalTex", 2D) = "white" {}
		_time("Time", Float) = 0
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
			uniform float _time;

			float4 frag(v2f_img i) : COLOR{
				
				//float2 pointDiff = _point - i.uv;
				//pointDiff.x = pointDiff.x * ( _ScreenParams.x / _ScreenParams.y);

				//float power = _radius - sqrt(pointDiff.x * pointDiff.x + pointDiff.y * pointDiff.y);

				//float4 col = inColour;
				//if (power > 0)
				//{
				//	//Do multicolour
				//	power = power * 1 / _radius;
				////	col = float4(power, power, power, 1) + inColour;
				//	float4 originalCol = tex2D(_OriginalTex, i.uv);
				//	col = (originalCol * power) + (inColour * (1 - power));
				//}
				//col.a = inColour.a;

				//General
				float4 inColour = tex2D(_MainTex, i.uv);
				float4 col = inColour;

				float2 center = float2(0.5, 0.5);
				float2 pointDiff = center - i.uv;
				pointDiff.x = pointDiff.x * (_ScreenParams.x / _ScreenParams.y);

				float4 originalCol = tex2D(_OriginalTex, i.uv);

				//Stage 1 Real Colour
				float stage1Time = _time;
				float maxStage1Time = 5.0;

				float stage1Power = stage1Time / maxStage1Time - sqrt(pointDiff.x * pointDiff.x + pointDiff.y * pointDiff.y);
				if (stage1Power > 1)
				{
					stage1Power = 1;
				}
				if (stage1Power < 0)
				{
					stage1Power = 0;
				}

				col = (originalCol * stage1Power) + (col * (1 - stage1Power));


				//Stage 2 Orange Burst
				float stage2Time =   _time - maxStage1Time;
				float maxStage2Time = 5.0;
				if (stage2Time > 0)
				{
					float4 orange = float4(1, 0, 0, 1);
					float stage2Power = stage2Time / maxStage2Time - sqrt(pointDiff.x * pointDiff.x + pointDiff.y * pointDiff.y);

					if (stage2Power > 1)
					{
						stage2Power = 1;
					}

					col = (orange * stage2Power) + (col * (1 - stage2Power));
				}
				


				//Stage 3 Fade To Black
				float stage3Time =  _time - maxStage1Time - maxStage2Time;
				float maxStage3Time = 5.0;
				if (stage3Time > 0)
				{
					

					float stage3Power = stage3Time / maxStage3Time;

					float4 black = float4(0, 0, 0, 1);

					if (stage3Power > 1)
					{
						stage3Power = 1;
					}

					col = (black * stage3Power) + (col * (1 - stage3Power));
				}

                return col;
            }
            ENDCG
        }
    }
}
