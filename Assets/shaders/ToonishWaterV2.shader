Shader "Water/ToonishWater V2" {
	Properties {
		_Color ("Color", Color) = (0,0,1,1)
	}
	SubShader {
		Tags {
			"RenderType" = "Opaque"
		}
		LOD 200
		
    Pass {
      CGPROGRAM
        // use "vertex" function as the vertex shader
        #pragma vertex vertex

        // use "fragment" function as the pixel (fragment) shader
        #pragma fragment fragment

        // vertex shader inputs
        struct vertexInput
        {
          float2 uv : TEXCOORD0;    // texture coordinate
          float4 position : POSITION; // vertex position
          float4 normal : NORMAL; // position normal
        };

        // vertex shader outputs ("vertex to fragment")
        struct vertexOutput
        {
          float2 uv : TEXCOORD0; // texture coordinate
          float4 projectedPosition : SV_POSITION; // clip space position
          float4 position : TEXCOORD3;
          float4 normal : TEXCOORD1;
          float wavePower : TEXCOORD2;
        };

        float pi = 3.14;
        
        // ---------------------------------------------------------------------------------------------------------
        //  VERTEX SHADER
        // ---------------------------------------------------------------------------------------------------------
        float wave(float time, vertexInput input)
        {
          // Sinus based on uv and time
          // Animate waves
          return sin(input.uv.x * 15.0 + time*40.0);
        }
        
        // vertex shader
        vertexOutput vertex(vertexInput input)
        {
          vertexOutput output;

          // Compute wave power 
          float finalWavePower = wave(_Time, input);

          // Compute new position according to wave power
          float4 animatedPosition = input.position + input.normal * finalWavePower * 0.02;

          // Project normal for the fragment shader
          float4 finalNormal = normalize(mul(UNITY_MATRIX_T_MV, input.normal));

          // Compute the final position for the fragment shader (Model*View)
          float4 finalPosition = mul(UNITY_MATRIX_MV, animatedPosition);

          // Prepare output
          output.projectedPosition = mul(UNITY_MATRIX_MVP, animatedPosition);
          output.position = finalPosition;
          output.normal = finalNormal;
          output.wavePower = finalWavePower;
          output.uv = input.uv;

          return output;
        }

        // ---------------------------------------------------------------------------------------------------------
        //  FRAGMENT SHADER
        // ---------------------------------------------------------------------------------------------------------
        float4 _Color;

        float3 rim(float3 color, float start, float end, float coef, vertexOutput output)
        {
          float3 normal = normalize(output.normal);
          float3 eye = normalize(-_WorldSpaceCameraPos);
          float rim = smoothstep(start, end, 1.0 - dot(normal, eye));
          return clamp(rim, 0.0, 1.0) * coef * color;
        }

        float border(vertexOutput output)
        {
          float3 eye = normalize(-_WorldSpaceCameraPos);
          float3 normal = normalize(output.normal);

          return 1.0 - dot(eye, normal);
        }

        // pixel shader; returns low precision ("fixed4" type)
        // color ("SV_Target" semantic)
        fixed4 fragment(vertexOutput input) : SV_Target
        {
          float3 result = float3(1,1,1);
          float delta = (input.wavePower + 1.0) / 2.0;

          result = input.normal;

          float outer = rim(float3(1.0,1.0,1.0), 0.0, 1.0, 1.0, input).x;

          result = outer * float3(0.4, 0.8, 1.0) + (1.0 - outer) * float3(0.1, 0.0, 0.5);

          result += (border(input) + delta) / 2.0 * float3(0.5, 0.8, 1.0) * 0.4;

          return float4(result, 1.0);
        }
      ENDCG
    }
	}
	FallBack "Diffuse"
}
