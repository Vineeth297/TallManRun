Shader "Skybox/ProceduralDesaturated" {
	Properties {
		[KeywordEnum(None, Simple, High Quality)] _SunDisk ("Sun", Float) = 2
		_SunSize ("Sun Size", Range(0, 1)) = 0.04
		_AtmosphereThickness ("Atmosphere Thickness", Range(0, 5)) = 1
		_SkyTint ("Sky Tint", Vector) = (0.5,0.5,0.5,1)
		_GroundColor ("Ground", Vector) = (0.369,0.349,0.341,1)
		_Exposure ("Exposure", Range(0, 8)) = 1.3
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
}