Shader "SineVFX/MatCapPro/SecondLayer" {
	Properties {
		_FinalPower ("Final Power", Float) = 1
		_FinalOpacity ("Final Opacity", Range(0, 1)) = 1
		[Toggle(_FINALOPACITYDITHERENABLED_ON)] _FinalOpacityDitherEnabled ("Final Opacity Dither Enabled", Float) = 0
		[Toggle(_EDGESONLYMODEENABLED_ON)] _EdgesOnlyModeEnabled ("Edges Only Mode Enabled", Float) = 0
		_Ramp ("Ramp", 2D) = "white" {}
		_RampColorTint ("Ramp Color Tint", Vector) = (1,1,1,1)
		_RampTilingExp ("Ramp Tiling Exp", Range(0.2, 4)) = 1
		_EdgeDetectionThickness ("Edge Detection Thickness", Float) = 1
		_MaskGlowExp ("Mask Glow Exp", Range(0.2, 8)) = 1
		_MaskGlowAmount ("Mask Glow Amount", Range(0, 10)) = 0
		_EdgeDetectionExp ("Edge Detection Exp", Range(1, 4)) = 1
		_EdgeGlowAmount ("Edge Glow Amount", Range(0, 10)) = 0
		_EdgeMaskPower ("Edge Mask Power", Range(-100, 100)) = 20
		_EdgeFresnelExp ("Edge Fresnel Exp", Range(0.2, 10)) = 2
		[Toggle(_WIREFRAMEENABLED_ON)] _WireframeEnabled ("Wireframe Enabled", Float) = 0
		_WireframePower ("Wireframe Power", Range(-100, 100)) = 10
		_WireframeThickness ("Wireframe Thickness", Range(0, 0.01)) = 10.92
		_WireframeFresnelExp ("Wireframe Fresnel Exp", Range(0.2, 10)) = 1
		_WireframeGlowAmount ("Wireframe Glow Amount", Range(0, 10)) = 0
		_OpacityTexture ("Opacity Texture", 2D) = "white" {}
		_OpacityTextureChannel ("Opacity Texture Channel", Vector) = (0,0,0,1)
		[HideInInspector] _texcoord ("", 2D) = "white" {}
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