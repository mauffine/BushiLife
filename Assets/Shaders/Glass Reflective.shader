Shader "Glass Reflective" {
	Properties{
		_SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess("Shininess", Range(0.01, 1)) = 0.078125
		_ReflectColor("Reflection Color", Color) = (1,1,1,0.5)
		_Cube("Reflection Cubemap", Cube) = "black" { TexGen CubeReflect }
	}
		SubShader{
		Tags{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
	}

		Cull Off
		LOD 300

		CGPROGRAM
#pragma surface surf BlinnPhong decal:add nolightmap

		samplerCUBE _Cube;

	fixed4 _ReflectColor;
	half _Shininess;
	sampler2D _MainTex;

	struct Input {
		float3 worldRefl;
		float2 uv_MainTex;
		float3 viewDir;
		float3 worldNormal;
		float2 uv_BumpMap;
		INTERNAL_DATA
	};

	void surf(Input IN, inout SurfaceOutput o) {
		o.Albedo = 0;
		o.Gloss = 1;
		o.Specular = _Shininess;

		fixed4 reflcol = texCUBE(_Cube, IN.worldRefl);
		o.Emission = reflcol.rgb * _ReflectColor.rgb;
		o.Alpha = reflcol.a * _ReflectColor.a; 
	}
	ENDCG
	}
		FallBack "Transparent/VertexLit"
}