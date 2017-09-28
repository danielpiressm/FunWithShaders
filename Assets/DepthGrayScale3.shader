
//Shows the grayscale of the depth from the camera.

Shader "Custom/DepthShader3"
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

	uniform int _Points_Length = 9;
	uniform float3 _Points[9];

	sampler2D _floatArray;
	sampler2D _lastFrameArray;
	int sizeImage = 4;
	float dt = 0.1;
	float s = 0.0;

	float4 frag(v2f i) : COLOR
	{
		float4 c = tex2D(_floatArray, i.uv);
		float4 a = tex2D(_lastFrameArray, i.uv);
		
		float x1 = c.x;
		float y1 = c.y;
		float z1 = c.z;
		float F1 = c.w;
		float u1 = floor(F1);
		float v1 = ((F1 - u1)*1000.0)/255.0;

		float x2 = a.x;
		float y2 = a.y;
		float z2 = a.z;
		float F2 = a.w;
		float u2 = floor(F2);
		float v2 = ((F2 - u2)*1000.0)/255.0;
		

		float OF = 0.0f;
		float TTC = 0.0f;
		float d = 2.5;
		float pixelSize = 0.27;
		float mapSize = 256;
		float m = 100;




		float magVector1 = pow(x1, 2) + pow(y1, 2) + pow(z1, 2);
		magVector1 = sqrt(magVector1);
		float magVector2 = pow(x2, 2) + pow(y2, 2) + pow(z2, 2);
		magVector2 = sqrt(magVector2);

		float speed = abs(magVector1 - magVector2)/dt;

		//float magVector2 = pow()

		float tt = z1 / speed;
		 
		TTC = exp(-pow(tt, 2) / (2 * pow(d,2)));

		float distanceX = (x1 - x2)*mapSize;
		float distanceY = (y1 - y2)*mapSize;
		float speedPixel = sqrt(pow(distanceX, 2) + pow(distanceY, 2))*pixelSize / dt;
		OF = 1 - exp(-pow(speedPixel, 2) / (2 * pow(m, 2)));

		float deltazinho = s / 2;
		float w = exp(pow(deltazinho - i.depth, 2) / pow(0.25*deltazinho, 2));
		//return float4(TTC, OF, w, 0);
		//v = v / (2);
		//return float4(x1, y1, z1, F1);
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