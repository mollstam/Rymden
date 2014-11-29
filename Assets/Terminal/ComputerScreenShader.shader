﻿Shader "Custom/ComputerScreenShader" {
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

				float2 radialDistortion(float2 coord)
		        {
		                float2 cc = coord - 0.5;
		                float dist = dot(cc, cc) * _Distortion;
		                return (coord + cc * (1.0 + dist) * dist);
		        }

		        /* Credit due to Cathode.app guy! <3 */

		        float pictureWave(float variable, float repeatCount, float speed)
				{
				    float waveLength = repeatCount * variable;
				    float waveSpeed = speed * _Time;
				    
				    return sin(waveLength + waveSpeed);
				}

		        float pictureHsync(vec2 coords)
				{
				    float coordSum = coords.x + coords.y;
				    float coordProduct = coords.x * coords.y;

				    float variable = coords.y;
				    float speed = 2000.0 * _HsyncFactor;
				    
				    float amplitude = 0.25 + 0.25 * pictureWave(coordSum, 2.0, 500.0);
				    float repeatCount = 12.0 + 10.0 * pictureWave(coordProduct, 1.0, 200.0);
				    float carrierWave = amplitude * pictureWave(variable, repeatCount, speed);

				    float tilt = (variable + 0.5) * amplitude;

				    float finalWave = carrierWave + tilt;
				    float hsync = 0.09;

				    return hsync * finalWave;
				}



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

				half4 frag (v2f i) : COLOR
				{
					half4 col = i.color;

					float2 texcoord = radialDistortion(i.texcoord);
					
					if (_HsyncFactor > 0)
						texcoord.x -= pictureHsync(texcoord);

					col.a *= tex2D(_MainTex, texcoord).a;
					col = col * _Color;
					clip (col.a - 0.01);
					return col;
				}
			ENDCG
		}
	}
}
