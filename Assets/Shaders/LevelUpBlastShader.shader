Shader "Custom/LevelUpBlastShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 1)
		_BlastRadius("BlastRadius", float) = 2
	}
	SubShader
	{
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _Color;
			float _BlastRadius;
			float masterAlpha;
			float2 position;
			float radius;
			float blastRadius;

			struct fragmentInput
			{
				float3 worldPos : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 uv : TEXCOORD1;
			};

			fragmentInput vert(appdata_base v)
			{
				fragmentInput o;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = float4(v.texcoord.xy, 0, 0);
				return o;
			}

			fixed4 frag(fragmentInput fragInput) : SV_Target
			{
				float xCoord = fragInput.worldPos.x - position.x;
				float yCoord = fragInput.worldPos.y - position.y;

				float distance = sqrt(pow(xCoord, 2) + pow(yCoord, 2));
				if (distance < radius)
                {
                    distance += _BlastRadius - radius;
                    float alpha = distance / _BlastRadius;
					return fixed4(_Color.xyz, alpha * masterAlpha);
                }
                else
                {
					return fixed4(0, 0, 0, 0);
                }
			}

			ENDCG
		}
	}
}