
//Shows the grayscale of the depth from the camera.

Shader "Custom/DepthShader4"
{
	CGINCLUDE

#include "UnityCG.cginc"

	struct v2f
	{
		float4 pos : SV_POSITION;
		float4 uv : TEXCOORD0;
		float2 depth : TEXCOORD1;
	};

	struct vertInput {
		float4 pos : POSITION;
		float4 texcoord0 : TEXCOORD0;
	};

	v2f vert(vertInput input) {
		v2f o;
		o.pos = UnityObjectToClipPos(input.pos);
		o.uv = float4(input.texcoord0.xy, 0, 0);
		UNITY_TRANSFER_DEPTH(o.depth);
		return o;
	}

	sampler2D _CameraDepthTexture;
	float4 frag(v2f i) : COLOR
	{
		
		UNITY_OUTPUT_DEPTH(i.depth);

	}

		ENDCG

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
			#pragma fragmentoption ARB_precision_hint_nicest
			ENDCG
		}
	}
}