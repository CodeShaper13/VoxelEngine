Shader "VoxelEngine/Block" {
	Properties {
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		
		// Added:
		_LightColor("Light Color", Color) = (1, 1, 1, 0) // Alpha = 0
		
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}
	
	SubShader {
		Tags { "Queue" = "Transparent" }
		LOD 100

		Lighting Off

		Pass {
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert

			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID

				// Added:
				fixed4 color : COLOR;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;

				// Added:
				fixed4 color : COLOR;

				UNITY_FOG_COORDS(1)
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
				o.color = v.color;

				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				// Adjust the color based on light
				fixed4 col;

				if (_LightColor.a > 0) {
					// Entites.
					col = tex2D(_MainTex, i.texcoord) * _LightColor;
				} else {
					// UVs for block light:  UNUSED! col = tex2D(_MainTex, i.texcoord) * tex2D(_LightMapTex, i.uv2);

					// Vertex colors for block light:
					col = tex2D(_MainTex, i.texcoord) * i.color;
				};
				
				// Discard pixel if less than 0
				clip(col.a - 0.1); // _Cutoff);

				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
