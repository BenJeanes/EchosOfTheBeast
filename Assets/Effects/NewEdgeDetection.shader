Shader "Hidden/NewEdgeDetection"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "" {}
	}
		SubShader
	{
		// No culling or depth
		ZTest Always Cull off ZWrite Off
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
#pragma debug
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			
			float4 _MainTex_TexelSize;

			sampler2D _CameraDepthNormalsTexture;
			sampler2D_float _CameraDepthTexture;

			half4 _Sensitivity;
			half4 _EdgeColor;
			half4 _MainColor;
			half _BgFade;
			half _SampleDistance;
			float _Exponent;

			float _Threshold;
			float _Timestep;
			float _BarWidth;

			float4 _Origin;
			float _Distance;


			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 ray : TEXCOORD1;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv[5] : TEXCOORD0;
				float2 uv_depth : TEXCOORD5;
				float4 interpolatedRay : TEXCOORD6;
			};

			half CheckSame(half2 centerNormal, float centerDepth, half4 mSample)
			{
				half2 difference = abs(centerNormal - mSample.xy) * _Sensitivity.y;
				half isSameNormal = (difference.x + difference.y) * _Sensitivity.y < 0.1;

				float sampleDepth = DecodeFloatRG(mSample.zw);
				float zDiff = abs(centerDepth - sampleDepth);
				half isSameDepth = zDiff * _Sensitivity.x < 0.09 * centerDepth;

				return isSameNormal * isSameDepth;
			}

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float2 uv = v.uv.xy;
				o.uv[0] = uv;
				o.uv[1] = uv + _MainTex_TexelSize.xy * half2(1, 1) * _SampleDistance;
				o.uv[2] = uv + _MainTex_TexelSize.xy * half2(-1, -1) * _SampleDistance;
				o.uv[3] = uv + _MainTex_TexelSize.xy * half2(-1, 1) * _SampleDistance;
				o.uv[4] = uv + _MainTex_TexelSize.xy * half2(1, -1) * _SampleDistance;
				o.uv_depth = uv;
				o.interpolatedRay = v.ray;
				return o;
			}			

			fixed4 frag (v2f i) : SV_Target
			{
				float rawDepth = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv_depth));
				float linearDepth = Linear01Depth(rawDepth);
				float4 wsDir = linearDepth * i.interpolatedRay;
				float3 wsPos = _Origin + wsDir;

				half4 sample1 = tex2D(_CameraDepthNormalsTexture, i.uv[1].xy);
				half4 sample2 = tex2D(_CameraDepthNormalsTexture, i.uv[2].xy);
				half4 sample3 = tex2D(_CameraDepthNormalsTexture, i.uv[3].xy);
				half4 sample4 = tex2D(_CameraDepthNormalsTexture, i.uv[4].xy);
				half edge = 1.0;
				edge *= CheckSame(sample1.xy, DecodeFloatRG(sample1.zw), sample2);
				edge *= CheckSame(sample3.xy, DecodeFloatRG(sample3.zw), sample4);

				float dist = distance(wsPos, _Origin);
				half4 black = half4(0, 0, 0, 1);

				if (dist < _Distance && _Distance - _BarWidth < dist)
				{
					if (edge > 0)
					{
						//return lerp(tex2D(_MainTex, i.uv[0].xy), _MainColor, _BgFade);
						if (dist < _Distance && _Distance - (_BarWidth / 10) < dist)
						{
							return 1;
						}
					}									
					else
					{
						float4 returnCol;
						float diff = 1 - (_Distance - dist) / (_BarWidth * 2);
						returnCol = lerp(_EdgeColor, black, 0.1f);
						returnCol *= diff;
						return returnCol;
					}
				}
				return black;				
			}
			ENDCG
		}
	}
}
