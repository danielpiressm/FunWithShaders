
//Shows the grayscale of the depth from the camera.

Shader "Custom/DepthShader3"
{
	CGINCLUDE

#include "UnityCG.cginc"

	struct v2f
	{
		float4 pos : SV_POSITION;
		float4 uv : TEXCOORD0;
	};

	struct vertInput {
		float4 pos : POSITION;
		float4 texcoord0 : TEXCOORD0;
	};

	v2f vert(vertInput input) {
		v2f o;
		o.pos = UnityObjectToClipPos(input.pos);
		o.uv = float4(input.texcoord0.xy, 0, 0);
		return o;
	}

	uniform int _Points_Length = 9;
	uniform float3 _Points[9];

	float _arrayOfU[65536];
	float _arrayOfV[65536];

	sampler2D _floatArray;
	int sizeImage = 4;


	float4 frag(v2f i) : COLOR
	{
		float4 c = tex2D(_floatArray, i.uv);
		
		float x = c.x;
		float y = c.y;
		float z = c.z;
		float F = c.w;
		float u = floor(F);
		float v = (F - u);
		//v = v / (2);
		return float4(x, y, z, F);// float4(0,0,0,1);
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