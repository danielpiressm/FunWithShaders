// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//Shows the grayscale of the depth from the camera.

Shader "Custom/DepthShader"
{
	SubShader
	{

		Pass
	{
		ZTest Always  ZWrite On
		Fog{ Mode off }

		CGPROGRAM
#pragma target 3.0
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

	//uniform sampler2D _CameraDepthTexture; //the depth texture

	struct v2f
	{
		float4 pos : SV_POSITION;
		float4 projPos : TEXCOORD0; //Screen position of pos
		float4 wPos : TEXCOORD1;
	};

	v2f vert(appdata_base v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.projPos = ComputeScreenPos(o.pos);
		o.wPos = mul(unity_ObjectToWorld, v.vertex);

		return o;
	}

	

	float4 frag(v2f i) : COLOR
	{
		float4 c;
		float x = i.pos.x;
		float y = i.pos.y;
		float z = i.pos.z;
		
		float w = 1;

		return float4(x,y,z,1);
	}

		ENDCG
	}
	}
}