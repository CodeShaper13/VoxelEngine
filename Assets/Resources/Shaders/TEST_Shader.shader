// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Debug/Vertex color" {
	Properties
	{
		_Color("Color", Color) = (0,0,0,1)
		_Layer1("Layer1", 2D) = "white" {}
	_Layer2("Layer2", 2D) = "white" {}
	_Layer3("Layer3", 2D) = "white" {}
	}
		SubShader
	{
		Pass
	{
		Fog{ Mode Off }
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

		sampler2D    _Layer1;
	sampler2D    _Layer2;
	sampler2D    _Layer3;
	float         _Color;

	struct appdata
	{
		float4 vertex : POSITION;
		fixed4 color : COLOR;
	};

	struct v2f
	{
		float4 pos : SV_POSITION;
		fixed4 color : COLOR;
		float2 uv_Layer1 : TEXCOORD0;
		float2 uv_Layer2 : TEXCOORD1;
		float2 uv_Layer3 : TEXCOORD2;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.color = v.color;
		return o;
	}

	float4 frag(v2f i) : COLOR
	{
		half4 l1 = tex2D(_Layer1, i.uv_Layer1);
		half4 l2 = tex2D(_Layer2, i.uv_Layer2);
		half4 l3 = tex2D(_Layer3, i.uv_Layer3);

		//float4 color = float4(1, 0, 0, 1);
		//color.rgb = (l1.rgb * i.color.r) + (l2.rgb * i.color.g) + (l3.rgb * i.color.b);
		
		float4 color = i.color;

		return color;
	}
		ENDCG
	}
	}
}