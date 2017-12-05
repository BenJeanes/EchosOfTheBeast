Shader "ImageEffect/EchoShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_LeadCol("Lead Color", Color) = (1,0,0,1)
		_MidCol("Mid Color", Color) = (0,1,0,1)
		_RearCol("Rear Color", Color) = (0,0,1,1)
		_Transparency("Transparency", Range(0.0,1.0)) = 0.5
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct VertIN
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					float4 ray : TEXCOORD1;
				};

				struct VertOut
				{
					float4 vertex : SV_POSITION;
					float2 uv : TEXCOORD0;
					float2 uv_depth : TEXCOORD1;
					float4 interpolatedRay : TEXCOORD2;
				};

				float4 _MainTex_TexelSize;
				float4 _CameraWS;

				VertOut vert(VertIN v)
				{
					VertOut o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv.xy;
					o.uv_depth = v.uv.xy;

					#if UNITY_UV_STARTS_AT_TOP			
					if (_MainTex_TexelSize.y < 0)
					{
						o.uv.y = 1 - o.uv.y;
					}
					#endif
					o.interpolatedRay = v.ray;
					return o;
				}

				sampler2D _MainTex;
				sampler2D_float _CameraDepthTexture;

				float4 _LeadCol;
				float4 _MidCol;
				float4 _RearCol;

				float4 _WorldSpaceScannerPos;
				float _EchoDistance;
				float _EchoWidth;
				float _LeadSharpness;
				float _EchoRange;
				float fateRate;			

				float _Transparency;

				half4 frag(VertOut i) : SV_Target
				{
					half4 col = tex2D(_MainTex, i.uv);
					
					float rawDepth = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv_depth));
					float linearDepth = Linear01Depth(rawDepth);
					float4 wsDir = linearDepth * i.interpolatedRay;
					float3 wsPos = _WorldSpaceCameraPos + wsDir;

					half4 echoCol = half4(0, 0, 0, 0);					
					
					float dist = distance(wsPos, _WorldSpaceScannerPos);
					if (dist < _EchoDistance && dist > _EchoDistance - _EchoWidth && linearDepth < 1)
					{
						/*float diff = 1 - (_EchoDistance - dist) / (_EchoWidth);
						half4 edge = lerp(_MidCol, _LeadCol, pow(diff, _LeadSharpness));
						echoCol = lerp(_RearCol, edge, diff);
						echoCol *= diff;*/
						echoCol = _MidCol;
						/*if (_EchoDistance > _EchoRange)
						{
							float fade = _EchoDistance - _EchoRange;
							echoCol.a -= fade;						
						}*/
						echoCol.a = _Transparency;
						return echoCol;
					}
					
					return col;
				}
			ENDCG
		}
	}
}
