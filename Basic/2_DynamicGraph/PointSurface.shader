Shader "Graph/Point Surface"
{
    Properties {
		_Smoothness ("Smoothness", Range(0,1)) = 0.5
	}
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha // Enable alpha blending
        CGPROGRAM

        #pragma surface ConfigureSurface Standard fullforwardshadows

        #pragma target 3.0

        struct Input
        {
            float3 worldPos;
        };
        
        float _Smoothness;

        void ConfigureSurface (Input input, inout SurfaceOutputStandard surface) {
			surface.Smoothness = _Smoothness;
			surface.Albedo.rg = saturate(input.worldPos.xy * 0.5 + 0.5);
        
        }
        ENDCG
    }
    FallBack "Diffuse"
}
