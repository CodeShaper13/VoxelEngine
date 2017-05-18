// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Unlit alpha-cutout shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "VoxelEngine/Block" {
	Properties{
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		
		// Added:
		_LightMapTex("Light level", 2D) = "white" {}
		_LightColor("Light Color", Color) = (1, 1, 1, 0) // Alpha = 0
		
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}
	
	SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
		LOD 100

		Lighting Off

		Pass{
			//Cull Off
			ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID

				// Added:
				float2 uv2 : TEXCOORD1;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;

				// Added:
				float2 uv2 : TEXCOORD1;

				UNITY_FOG_COORDS(2) // Was 1
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Cutoff;

			// Added:
			sampler2D _LightMapTex;
			float4 _LightColor;

			v2f vert(appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);

				// Added:
				o.uv2 = v.uv2;

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// Added:
				// Adjust the color based on light
				fixed4 col;
				if (_LightColor.a > 0) {
					col = tex2D(_MainTex, i.texcoord) * _LightColor;
				} else {
					col = tex2D(_MainTex, i.texcoord) * tex2D(_LightMapTex, i.uv2);
				};

				//fixed4 col = tex2D(_MainTex, i.texcoord);
				
				clip(col.a - 0.5); // _Cutoff);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
