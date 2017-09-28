
//Shows the grayscale of the depth from the camera.

Shader "Custom/DepthShader"
{
	CGINCLUDE

#include "UnityCG.cginc"

	struct v2f
	{
		float4 pos : SV_POSITION;
		float4 uv : TEXCOORD0;
		float4 projPos : TEXCOORD1; //Screen position of pos
		float4 wPos : TEXCOORD2;
		float2 depth : TEXCOORD3;
	};

	struct vertInput {
		float4 pos : POSITION;
		float4 texcoord0 : TEXCOORD0;
	};

	v2f vert(vertInput input) {
		v2f o;
		o.pos = UnityObjectToClipPos(input.pos);
		o.projPos = mul(UNITY_MATRIX_IT_MV, input.pos);
		o.wPos = mul(unity_ObjectToWorld, input.pos);
		o.uv = float4(input.texcoord0.xy, 0, 0);
		UNITY_TRANSFER_DEPTH(o.depth);

		return o;
	}

	uniform int _Points_Length = 9;
	uniform float3 _Points[9];
	int sizeImage = 4;
	sampler2D _CameraDepthTexture;


	half4 frag(v2f i) : COLOR
	{
		float4 c;
		float x = i.wPos.x;
		float y = i.wPos.y;
		float z = i.wPos.z;
		float x1 = i.uv.x;
		float y1 = i.uv.y;
		float u =  round(x1 * (sizeImage - 1));
		float v =  (y1 * (sizeImage - 1)) / 1000.0;
		float f =  u +  v;

		//float4 depthMap = tex2D(_CameraDepthTexture, i.uv);

		return float4(x, y, z, 1);
		//return float4(x, y, z, 1);
		//UNITY_OUTPUT_DEPTH(i.depth);
		//return float4(i.depth.x,i.depth.y,x, z);
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