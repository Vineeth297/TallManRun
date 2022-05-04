Shader "ShapeFX/Particles/Alpha Blended" {
	Properties {
		_MainTex ("Particle Texture", 2D) = "white" {}
		[Toggle(SHFX_NOTEXTURE)] _NoTexture ("No Texture", Float) = 0
		[Toggle(SHFX_DISABLE_SOFT_PARTICLES)] _NoSoftParticles ("Disable Soft Particles", Float) = 0
		_InvFade ("Soft Particles Factor", Range(0.01, 3)) = 1
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