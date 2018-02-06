
//Shows the grayscale of the depth from the camera.

Shader "Custom/DepthShader2"
{
		CGINCLUDE

		#include "UnityCG.cginc"

		struct v2f
		{
			float4 pos : SV_POSITION;
			float4 projPos : TEXCOORD0; //Screen position of pos
			float4 wPos : TEXCOORD1;
			float4 uv : TEXCOORD2;
		};

		struct vertInput {
			float4 pos : POSITION;
			float4 texcoord1 : TEXCOORD1;
		};

		v2f vert(vertInput input) {
			v2f o;
			o.pos = UnityObjectToClipPos(input.pos);
			o.projPos = UnityObjectToClipPos(input.pos);
			o.wPos = mul(unity_ObjectToWorld, input.pos);
			o.uv =  ComputeScreenPos(input.pos); //float4(input.texcoord1.xy, 0, 0);
			return o;
		}

		uniform int _Points_Length = 9;
		uniform float3 _Points[9];



		float4 frag(v2f i) : COLOR
		{
			float4 c;
			float x = i.projPos.x;
			float y = i.projPos.y;
			float z = i.projPos.z;
			float w = 1;
			
			return float4(i.uv.x, i.uv.y, 0, 1);
			//return float4(x,y,z,1);
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