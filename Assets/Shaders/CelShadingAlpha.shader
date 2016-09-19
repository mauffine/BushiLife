Shader "Custom/CelShadingForwardWithAlpha" {
	Properties{
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Albedo (RGBA)", 2D) = "white" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}
		SubShader{
		Tags{

		"RenderType" = "Transparent"
		"IgnoreProjector" = "True"
		"Queue" = "Transparent"
	}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200
		
		CGPROGRAM 
		#pragma surface surf CelShadingForward alpha
		#pragma target 3.0
		float _Cutoff;

		half4 LightingCelShadingForward(SurfaceOutput s, half3 lightDir, half atten) {
		half NdotL = dot(s.Normal, lightDir);
		if (NdotL <= 0.0) NdotL = 0;
		else NdotL = 1;
		half4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 2);
		c.a = s.Alpha;
		if (c.a < _Cutoff) discard;
		return c;
	}

	sampler2D _MainTex;
	fixed4 _Color;

	struct Input {
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		// Albedo comes from a texture tinted by color
		fixed4 texColor = tex2D(_MainTex, IN.uv_MainTex);
		fixed4 c = texColor * _Color;
		o.Albedo = c.rgb;
		o.Alpha = texColor.a;
	}
	ENDCG
	SetTexture[_MainTex]{ combine primary }
}
Fallback "Toon/Basic"