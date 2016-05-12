Shader "Water/ToonishWater" {
	Properties {
		_Color ("Color", Color) = (0,0,1,1)
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
		
		// rim lighting for water bounds
		float rim(Input input, SurfaceOutputStandard output) {
			return 1.0 - dot(normalize(input.viewDir), normalize(output.Normal));
		}

		void surf (Input input, inout SurfaceOutputStandard output) {
			float rimResult = pow(rim(input, output), 1.5);

			output.Albedo = (rimResult) * float3(0.4, 0.8, 1.0) + (1.0 - rimResult) * float3(0.1, 0.0, 0.5);
			//output.Albedo = output.Albedo + rimResult * float3(1, 1, 1);

			// Metallic and smoothness come from slider variables
			output.Metallic = 0;
			output.Smoothness = 1;
			output.Alpha = 1;// -rimResult * (0.2);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
