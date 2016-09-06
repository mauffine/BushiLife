CGINCLUDE
#include "UnityCG.cginc"
struct appdata {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
};
struct v2f {
	float4 pos : POSITION;
	float4 color : COLOR;
};
uniform float4 _Color; //Add This
uniform float _Outline;
uniform float4 _OutlineColor;
v2f vert(appdata v) {
	v2f o;
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
	norm.x *= UNITY_MATRIX_P[0][0];
	norm.y *= UNITY_MATRIX_P[1][1];
	o.pos.xy += norm.xy * o.pos.z * _Outline;
	o.color = float4(_OutlineColor.rgb, _Color.a); //Change this
	return o;
}
SubShader{
	Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" } //Change this if you like
	UsePass "Toon/Basic/BASE"
	Pass{
	Name "OUTLINE"
	Tags{ "LightMode" = "Always" }
	Cull Front
	ZWrite On
	ColorMask RGB
	Blend SrcAlpha OneMinusSrcAlpha
	CGPROGRAM
#pragma vertex vert
#pragma exclude_renderers gles xbox360 ps3
	ENDCG
	SetTexture[_MainTex]{ combine primary }
}
Fallback "Toon/Basic"