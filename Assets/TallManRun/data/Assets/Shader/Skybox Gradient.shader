Shader "Skybox Gradient" {
	Properties {
		_Top ("Top", Vector) = (1,1,1,0)
		_Bottom ("Bottom", Vector) = (0,0,0,0)
		_mult ("mult", Float) = 1
		_pwer ("pwer", Float) = 1
		[Toggle(_SCREENSPACE_ON)] _Screenspace ("Screen space", Float) = 0
	}
	
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard fullforwardshadows
#pragma target 3.0
		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}
		ENDCG
	}
	//CustomEditor "ASEMaterialInspector"
}