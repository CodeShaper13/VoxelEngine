Shader "VoxelEngine/ChunkShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_LightMapTex("Light level", 2D) = "white" {}
		_LightColor("Light Color", Color) = (1, 1, 1, 0) // Alpha = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _LightMapTex;
			float4 _LightColor;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = v.uv2;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture				
				fixed4 col;
				if (_LightColor.a > 0) {
					col = tex2D(_MainTex, i.uv) * _LightColor;
				} else {
					col = tex2D(_MainTex, i.uv) * tex2D(_LightMapTex, i.uv2);
				};
				
				//fixed4 col = tex2D(_MainTex, i.uv);

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
