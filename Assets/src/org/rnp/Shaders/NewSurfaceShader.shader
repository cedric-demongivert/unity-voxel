Shader "Custom/test" {
	
	Properties{
		_Color ("Color (rgb)", Color)= (1,1,1,1)
		_MainTexture ("Texture", 2D)= "white"{}
	}


	SubShader{
		Pass{
				Color[_Color]

				SetTexture[_MainTexture]{
					
					Combine texture 
				}
			}

	}

	
}
