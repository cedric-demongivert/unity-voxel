Shader "Custom/GridShader" {
	Properties {
		_GridColor ("Grid Color", Color) = (1,1,1,1)
		_CellSize ("Cell Size", Float) = 1
		_LineWidth ("Line Width", float) = 0.2
		_ShowRadius ("Show Radius", float) = 50
	}
	SubShader {
		Pass {
			Tags { "RenderType"="Opaque" }
			LOD 200
			Blend SrcAlpha OneMinusSrcAlpha
		
			CGPROGRAM
			#pragma vertex vertexShader
			#pragma fragment fragmentShader

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0
		
			struct VertexInput {
				float4 vertex : POSITION;
			};

			struct FragInput {
				float4 vertex : SV_POSITION;
				float4 worldLocation : TEXCOORD0;
			};

			fixed4 _GridColor;
			fixed4 _NoneColor = fixed4(0,0,0,0);
			float _CellSize;
			float _LineWidth;
			float _ShowRadius;

			// Vertex Shader.
			// Only compute projection.
			FragInput vertexShader (VertexInput input) {
				FragInput output;

				output.vertex = mul( UNITY_MATRIX_MVP, input.vertex );
				output.worldLocation = mul(_Object2World, input.vertex);

				return output;
			}

			float gridLightness(float4 vertex, float cellSize, float accuracy) {
				float x = vertex.x % cellSize;
				float z = vertex.z % cellSize;

				if(x < 0) x += cellSize;
				if(z < 0) z += cellSize;

				float xlightness = 0;
				float zlightness = 0;

				if(x >= cellSize - accuracy) {
					xlightness = (accuracy - (cellSize - x))/accuracy;
				}
				else if(x <= accuracy) {
					xlightness = (accuracy - x)/accuracy;
				}

				if(z >= cellSize - accuracy) {
					zlightness = (accuracy - (cellSize - z))/accuracy;
				}
				else if(z <= accuracy) {
					zlightness = (accuracy - z)/accuracy;
				}
				
				float result = max(xlightness, zlightness);

				if(result > 1) return 1;
				else return result;
			}

			// Fragment Shader.
			// Paint Grid.
			fixed4 fragmentShader (FragInput input) : SV_Target {

				float3 cameraPos = _WorldSpaceCameraPos;
				
				float zoom = 1;
				float showRadius = _ShowRadius * _ShowRadius;
				float alpha = (showRadius - (input.worldLocation.x*input.worldLocation.x + input.worldLocation.z*input.worldLocation.z)) / showRadius;

				if(alpha < 0) alpha = 0;

				/*float zoom = 1;

				if(cameraPos.y > 50) {
					zoom = 0; //(cameraPos.y - 20);
				}*/

				float baseLight = gridLightness(input.worldLocation, _CellSize * zoom, (_LineWidth/2) * zoom);
				float secdLight = gridLightness(input.worldLocation, _CellSize * 5 * zoom, (_LineWidth/2) * zoom);
				float repLight = gridLightness(input.worldLocation, _CellSize * 200000 * zoom, (_LineWidth/2) * zoom);

				if(baseLight + secdLight <= 0) return _NoneColor;
				else {
					fixed4 result = _GridColor * (baseLight + secdLight);

					result.a *= alpha;

					if(repLight > 0) {
						result.r = 1;
						result.g = 0;
						result.b = 0;
						result.a += 0.1;
					}

					return result;
				}
			}

			ENDCG
		}
	}
}
