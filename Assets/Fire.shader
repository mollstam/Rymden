Shader "Custom/Fire" {
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Alpha (A)", 2D) = "white" {}
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_Distortion ("Distortion", Float) = 1
		_HsyncFactor ("HSync Factor", Float) = 0
		_Bloom ("Bloom", Float) = 0
		_Blur ("Blur", Float) = 0

		_ColorMask ("Color Mask", Float) = 15
	}

	SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType"="Plane"
		}

		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Offset -1, -1
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		Pass
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma glsl
				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex : POSITION;
					half4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex : POSITION;
					half4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				fixed4 _Color;
				float _Distortion;
				float _HsyncFactor;
				float _Bloom;
				float _Blur;

				const float EPSILON = 0.000001;

				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				
#ifdef UNITY_HALF_TEXEL_OFFSET
					o.vertex.xy -= (_ScreenParams.zw-1.0);
#endif

					o.color = v.color;
					return o;
				}				

				float simplexnoise(float3 uv, float res)
				{
					const float3 s = float3(1e0, 1e2, 1e3);
	
					uv *= res;
	
					float3 uv0 = floor(fmod(uv, res))*s;
					float3 uv1 = floor(fmod(uv+float3(1, 1, 1), res))*s;
	
					float3 f = frac(uv); f = f*f*(3.0-2.0*f);

					float4 v = float4(uv0.x+uv0.y+uv0.z, uv1.x+uv0.y+uv0.z,
		      					  uv0.x+uv1.y+uv0.z, uv1.x+uv1.y+uv0.z);

					float4 r = frac(sin(v*1e-1)*1e3);
					float r0 = lerp(lerp(r.x, r.y, f.x), lerp(r.z, r.w, f.x), f.y);
	
					r = frac(sin((v + uv1.z - uv0.z)*1e-1)*1e3);
					float r1 = lerp(lerp(r.x, r.y, f.x), lerp(r.z, r.w, f.x), f.y);
	
					return lerp(r0, r1, f.z)*2.-1.;
				}

				half4 frag (v2f i) : COLOR
				{
					float2 p = -.5 + i.texcoord;
					//p.x *= iResolution.x/iResolution.y;
	
					float color = 3.0 - (3.*length(2.3*p));
	
					float3 coord = float3(atan2(p.x,p.y)/6.2832+.5, length(p)*.4, .5);
					float time = _Time.y * 3.0f;

					for(int i = 1; i <= 7; i++)
					{
						float power = pow(3.0, float(i));
						color += (1.5 / power) * simplexnoise(coord + float3(0.,-time*.05, time*.01), power*16.);
					}

					if (p.y < 0)
						return float4(0,0,0,0);

					return float4( color, pow(max(color,0.),2.)*0.4, pow(max(color,0.),3.)*0.15 , color);
				}
			ENDCG
		}
	}
}
