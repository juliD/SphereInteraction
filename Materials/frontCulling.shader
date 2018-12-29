Shader "Unlit/frontCulling"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Radius("Circle Radius", float) = 0.05
		_Width("Texture Width", int) = 2048
		_Height("Texture Height", int) = 1024
		_PointX("Hit X", float) = 0.5
		_PointY("Hit Y", float) = 0.5
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				Cull Front
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
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				sampler2D _RedTex;
				float4 _MainTex_ST;
				float4 _RedTex_ST;

				float _Radius;
				int _Width;
				int _Height;
				float _PointX;
				float _PointY;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);

					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture
					fixed4 col = tex2D(_MainTex, i.uv);
					fixed4 main_color = tex2D(_MainTex, i.uv);

					float aspect = (float)_Width / ((_Height == 0) ? _Width : (float)_Height);


					float x = (_PointX - i.uv.x)*aspect;
					float y = (_PointY - i.uv.y);

					float d = sqrt(pow(x, 2) + pow(y, 2));

					if (d > _Radius) {
						return fixed4(main_color.r, main_color.g, main_color.b, 1.0);
					}
					else {
						return fixed4(1, 0.01, 0.01, 1.0);
					}

				}
				ENDCG
			}
		}
}
