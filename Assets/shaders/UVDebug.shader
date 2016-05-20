Shader "Custom/UV Debug" {
	Properties {
		_Color ("Color", Color) = (0,0,1,1)
    _MainTex("Texture", 2D) = "white" {}
	}
	SubShader {
		Tags {
			"RenderType" = "Opaque"
		}
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		
		struct Input {
			float3 worldNormal;
      float2 uv_MainTex;
			float3 viewDir;
			float3 worldPos;
		};

		fixed4 _Color;
		
		void surf (Input input, inout SurfaceOutputStandard output) {
			output.Albedo = float3(input.uv_MainTex.x, input.uv_MainTex.y, 1);

			output.Metallic = 0;
			output.Smoothness = 1;
			output.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
