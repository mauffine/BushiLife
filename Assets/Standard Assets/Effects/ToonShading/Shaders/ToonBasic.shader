Shader "Toon/Basic" {
	Properties{
		_Color("Main Color", Color) = (.5,.5,.5,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	_ToonShade("ToonShader Cubemap(RGB)", CUBE) = "" { Texgen CubeNormal }
	}


		SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" } //Change this
		Pass{
		Name "BASE"
		Cull Off //Remove this
		Blend SrcAlpha OneMinusSrcAlpha //Add This
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest 
#include "UnityCG.cginc"
		sampler2D _MainTex;
	samplerCUBE _ToonShade;
	float4 _MainTex_ST;
	float4 _Color;
	struct appdata {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		float3 normal : NORMAL;
	};
	struct v2f {
		float4 pos : POSITION;
		float2 texcoord : TEXCOORD0;
		float3 cubenormal : TEXCOORD1;
	};
	v2f vert(appdata v) {
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

		float3 n = v.normal;
		float3 viewDir = WorldSpaceViewDir(v.vertex);

		n = dot(viewDir, float3(0, 0, 1)) > 0 ? n : -n;
		

		o.cubenormal = mul(UNITY_MATRIX_MV, float4(n,0));

		return o;
	}
	float4 frag(v2f i) : COLOR{
		float4 col = _Color * tex2D(_MainTex, i.texcoord);
		float4 cube = texCUBE(_ToonShade, i.cubenormal);
		return float4(2.0f * cube.rgb * col.rgb, col.a);
	}
		ENDCG
	}
	}
		SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" } //Change this
		Pass{
		Name "BASE"
		Cull Off //Remove this
		//Cull Back
		Blend SrcAlpha OneMinusSrcAlpha //Add this
		SetTexture[_MainTex]{
		constantColor[_Color]
		Combine texture * constant
	}
		SetTexture[_ToonShade]{
		combine texture * previous DOUBLE, previous
	}
	}
	}
		Fallback "VertexLit"
}