Shader "Graph/PointSurfaceGPU"
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

        #pragma surface ConfigureSurface Standard fullforwardshadows addshadow
        #pragma instancing_options assumeuniformscaling procedural:ConfigureProcedural

        #pragma target 4.5
        
        #include "PointGPU.hlsl"
        
        struct Input
        {
            float3 worldPos;
        };
        
        float _Smoothness;        

        void ConfigureSurface (Input input, inout SurfaceOutputStandard surface) {
			surface.Smoothness = _Smoothness;
			surface.Albedo = saturate(input.worldPos * 0.5 + 0.5);
        
        }
        ENDCG
    }
    FallBack "Diffuse"
}
