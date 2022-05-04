Shader "SineVFX/PrototypeGridCenterMask" {
	Properties {
		_Texture ("Texture", 2D) = "white" {}
		_TexturePower ("Texture Power", Range(0, 1)) = 1
		_TextureColorTint ("Texture Color Tint", Vector) = (1,1,1,1)
		_TextureTiling ("Texture Tiling", Float) = 10
		_Metallic ("Metallic", Range(0, 1)) = 0
		_Smoothness ("Smoothness", Range(0, 1)) = 0
		_ColorMaskOuterColor ("Color Mask Outer Color", Vector) = (0.5294118,0.5294118,0.5294118,1)
		_ColorMaskDistance ("Color Mask Distance", Float) = 25
		_ColorMaskPower ("Color Mask Power", Float) = 1
		[HideInInspector] __dirty ("", Float) = 1
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
	Fallback "Diffuse"
	//CustomEditor "ASEMaterialInspector"
}