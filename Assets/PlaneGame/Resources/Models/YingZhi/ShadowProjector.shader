// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'

Shader "Custom/PostEffect/ShadowProjector" {
	Properties {
		_ShadowTex ("ShadowTex", 2D) = "gray" {}
		_ShadowColor ("Shadow Color", Color) = (0,0,0,1)
		_bulerWidth ("BulerWidth", float) = 1
		_ShadowMask ("ShadowMask",2D) = "white"{}
	}

	SubShader {

		Tags { "Queue"="AlphaTest+1" }

		Pass {
			ZWrite Off
			ColorMask RGB
			Blend DstColor Zero
			Offset -1, -1

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"


			half4x4 unity_Projector;
			sampler2D _ShadowTex;
			sampler2D _ShadowMask;
			uniform half4 _ShadowTex_TexelSize;
			//float _bulerWidth;
			half4 _ShadowColor;

			struct v2f {
				half4 pos:POSITION;
				half4 sproj:TEXCOORD0;
			};


			v2f vert(half4 vertex:POSITION) {
				v2f o;
				o.pos = UnityObjectToClipPos(vertex);
				o.sproj = mul(unity_Projector, vertex);

				return o;
			}

			fixed4 frag(v2f i):COLOR{
				
				fixed4 shadowCol = tex2Dproj(_ShadowTex, UNITY_PROJ_COORD(i.sproj));
				fixed maskCol = tex2Dproj(_ShadowMask, UNITY_PROJ_COORD(i.sproj)).r;
				fixed a = (shadowCol * maskCol).a;

				float4 uv4= UNITY_PROJ_COORD(i.sproj);
				float2 uv = uv4.xy / uv4.w ;

				//blur来柔化边缘
				a += tex2D(_ShadowTex, saturate(uv + _ShadowTex_TexelSize.xy * 1 * float2(1,0))).a;
				a += tex2D(_ShadowTex, saturate(uv + _ShadowTex_TexelSize.xy * 1 * float2(0,1))).a;
				a += tex2D(_ShadowTex, saturate(uv + _ShadowTex_TexelSize.xy * 1 * float2(-1,0))).a;
				a += tex2D(_ShadowTex, saturate(uv + _ShadowTex_TexelSize.xy * 1 * float2(0,-1))).a;

				a = a/5;
				return  float4(1, 1, 1, 1) * (1 - _ShadowColor.a * a * (1 - _ShadowColor));
				/*if(a > 0)
				{
					return  float4(1,1,1,1) * (1 - 0.5 * a);
				}
				else
				{
					return float4(1,1,1,1) ;
				}*/		
				//return  fixed4(1,1,1,1) * (1 - _ShadowColor.a * a * (1 - _ShadowColor));
			}

			ENDCG
		}
	} 
	FallBack "Diffuse"
}
