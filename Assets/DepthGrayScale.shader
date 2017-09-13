// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

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
#pragma fragmentoption ARB_precision_hint_nicest


	//uniform sampler2D _CameraDepthTexture; //the depth texture

	struct v2f
	{
		float4 pos : SV_POSITION;
		float4 projPos : TEXCOORD0; //Screen position of pos
		float4 wPos : TEXCOORD1;
	};

	struct vertInput {
		float4 pos : POSITION;
	};

	v2f vert(vertInput input) {
		v2f o;
		o.pos = UnityObjectToClipPos(input.pos);
		o.projPos = UnityObjectToClipPos(input.pos);
		o.wPos = mul(unity_ObjectToWorld, input.pos);
		return o;
	}

	

	

	float4 frag(v2f i) : COLOR
	{
		float4 c;
		float x = i.wPos.x;
		float y = i.wPos.y;
		float z = i.wPos.z;
		
		float w = 1;

		return float4(x,y,z,1);
	}

		ENDCG
	}
	}
}