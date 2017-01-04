Shader "MetaBalls/MetaBallsShader"
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
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.0

			#include "UnityCG.cginc"

			uniform fixed4 metaBalls[50];
			uniform float width;
			uniform float height;
			uniform float radius;
			uniform int numBalls;
			

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			

			fixed4 frag (v2f i) : SV_Target
			{
				float contrib = 0;
				for (int idx = 0; idx < numBalls; idx++)
				{
					float4 metallBall = metaBalls[idx];
					float xTex = metallBall.x / width;
					float yTex = metallBall.y / height;

					contrib += (radius * radius) / ((xTex - i.uv.x)*(xTex - i.uv.x) + (yTex - i.uv.y)*(yTex - i.uv.y));
					
				};

				if (contrib > 1)
				{
					return fixed4(1, 0, 0, 1);
				}
				else
				{
					return fixed4(1, 1, 1, 1);
				}

				

				/*
				float4 metallBall = metaBalls[0];
				float xTex = metallBall.x / width;
				float yTex = metallBall.y / height;

				if ((xTex - i.uv.x)*(xTex - i.uv.x) + (yTex - i.uv.y)*(yTex - i.uv.y) >= radius * radius)
				{
					return fixed4(1, 0, 0, 1);
				}; 

				return fixed4(0, 0, 0, 0);
				*/
			

				//fixed4 col = tex2D(_MainTex, i.uv);
				// just invert the colors
			//	col = 1 - col;
				//return fixed4(1,1,1,1);
				//return fixed4(temp,temp,temp,temp);
				//return metaBalls[0];


			}
			ENDCG
		}
	}
}
