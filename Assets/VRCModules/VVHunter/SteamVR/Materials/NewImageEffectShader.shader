// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "UI/PS-Overlay_Mode" {
	Properties{
		[KeywordEnum(None,Darken,Multiply,ColorBurn,LinearBurn,Lighten,Screen,ColorDodge,LinearDodge)] _Overlay("Overlay mode 1", Float) = 0
		[KeywordEnum(None, Overlay,SoftLight,HardLight,VividLight,LinearLight,PinLight,Difference,Exclusion)] _Overlay2("Overlay mode 2", Float) = 0
		_TintColor1("Color1",Color) = (1,1,1,1)
		_TintColor2("Color2",Color) = (1,1,1,1)
		_MainTex("Particle Texture", 2D) = "white" {}
	}

		Category{
			Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }
			Blend One OneMinusSrcColor
		//    Blend OneMinusSrcColor One
		//	Blend SrcAlpha OneMinusSrcColor
		//	Blend SrcAlpha One
		//    Blend One One
		//    Blend One Zero
		//    Blend Zero One
		//    ColorMask RGB
		//    Cull Off 
			Lighting Off
			ZWrite Off

			SubShader {
				Pass {
					CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma target 2.0
					#pragma multi_compile _OVERLAY_NONE _OVERLAY_DARKEN _OVERLAY_MULTIPLY _OVERLAY_COLORBURN _OVERLAY_LINEARBURN _OVERLAY_LIGHTEN _OVERLAY_SCREEN _OVERLAY_COLORDODGE _OVERLAY_LINEARDODGE 
					#pragma multi_compile _OVERLAY2_NONE _OVERLAY2_OVERLAY _OVERLAY2_SOFTLIGHT _OVERLAY2_HARDLIGHT _OVERLAY2_VIVIDLIGHT _OVERLAY2_LINEARLIGHT _OVERLAY2_PINLIGHT _OVERLAY2_DIFFERENCE _OVERLAY2_EXCLUSION
					#include "UnityCG.cginc"

					sampler2D _MainTex;
					float4 _MainTex_ST;
					fixed4 _TintColor1;
					fixed4 _TintColor2;
					struct appdata_t {
						float4 vertex : POSITION;
						fixed4 color : COLOR;
						float2 texcoord : TEXCOORD0;
					};

					struct v2f {
						float4 vertex : SV_POSITION;
						fixed4 color : COLOR;
						float2 texcoord : TEXCOORD0;
					};

					v2f vert(appdata_t v)
					{
						v2f o = (v2f)0;
						o.vertex = UnityObjectToClipPos(v.vertex);// UnityObjectToClipPos(v.vertex);
						o.color = v.color;
						o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
						return o;
					}

					fixed4 frag(v2f i) : SV_Target
					{
						half4 tex = tex2D(_MainTex, i.texcoord);
						half4 col = i.color * tex * _TintColor2;
						//                col.rgb *= col.a;

										//Darken 变暗
										#if _OVERLAY_DARKEN
											col.rgb = min(col.rgb,col.rgb);
										#endif

											//Multiply 正片叠底
											#if _OVERLAY_MULTIPLY
												col.rgb = col.rgb * col.rgb;
												col.rgb *= col.rgb * 2;

											#endif

												//ColorBurn 颜色加深
												#if _OVERLAY_COLORBURN
													col.rgb = 1.0 - (1.0 - col.rgb) / col.rgb;
												#endif

													//LinearBurn 线性加深
													#if _OVERLAY_LINEARBURN
														col.rgb = col.rgb + col.rgb - 1.0;
													#endif

														//Lighten 变亮
														#if _OVERLAY_LIGHTEN
															col.rgb = max(col.rgb,col.rgb);
															//col = col*_TintColor1;
														#endif

														//Screen 滤色
														#if _OVERLAY_SCREEN
															col.rgb = 1.0 - (1.0 - col.rgb) * (1.0 - col.rgb);
														#endif

															//ColorDodge 颜色减淡
															#if _OVERLAY_COLORDODGE
																col.rgb = col.rgb / (1.0 - col.rgb);
															#endif

																//LinearDodge(Add) 线性减淡（添加）
																#if _OVERLAY_LINEARDODGE
																	col.rgb = col.rgb + col.rgb;
																#endif

																	//Overlay 叠加
																	#if _OVERLAY2_OVERLAY
																		col.rgb = step(0.5, col.rgb);
																		col.rgb = lerp((col.rgb*col.rgb * 2), (1.0 - (2.0*(1.0 - col.rgb)*(1.0 - col.rgb))), col.rgb);
																	#endif

																		//Soft Light 柔光
																		#if _OVERLAY2_SOFTLIGHT
																			if (col.r > .5) { col.r = col.r * (1.0 - (1.0 - col.r)*(1.0 - 2 * col.r)); }
																			else { col.r = 1.0 - (1.0 - col.r)*(1.0 - (col.r*2.0*col.r)); }

																			if (col.g > .5) { col.g = col.g * (1.0 - (1.0 - col.g)*(1.0 - 2 * col.g)); }
																			else { col.g = 1.0 - (1.0 - col.g)*(1.0 - (col.g*2.0*col.g)); }

																			if (col.b > .5) { col.b = col.b * (1.0 - (1.0 - col.b)*(1.0 - 2 * col.b)); }
																			else { col.b = 1.0 - (1.0 - col.b)*(1.0 - (col.b*2.0*col.b)); }
																		#endif

																			//Hard Light 强光
																			#if _OVERLAY2_HARDLIGHT
																				if (col.r > .5) { col.r = 1.0 - (1.0 - col.r)*(1 - 2 * col.r); }
																				else { col.r = col.r * 2 * col.r; }

																				if (col.g > .5) { col.g = 1.0 - (1.0 - col.g)*(1 - 2 * col.g); }
																				else { col.g = col.g * 2 * col.g; }

																				if (col.b > .5) { col.b = 1.0 - (1.0 - col.b)*(1 - 2 * col.b); }
																				else { col.b = col.b * 2 * col.b; }
																			#endif

																				//Vivid Light 亮光
																				#if _OVERLAY2_VIVIDLIGHT
																					if (col.r > .5) { col.r = 1.0 - (1.0 - col.r) / (2 * (col.r - .5)); }
																					else { col.r = col.r / (1 - 2 * col.r); }

																					if (col.g > .5) { col.g = 1.0 - (1.0 - col.g) / (2 * (col.g - .5)); }
																					else { col.g = col.g / (1 - 2 * col.g); }

																					if (col.b > .5) { col.b = 1.0 - (1.0 - col.b) / (2 * (col.b - .5)); }
																					else { col.b = col.b / (1 - 2 * col.b); }
																				#endif			

																					//Linear Light 线性光
																					#if _OVERLAY2_LINEARLIGHT
																						if (col.r > .5) { col.r = col.r + 2.0*(col.r - .5); }
																						else { col.r = col.r + 2 * col.r - 1; }

																						if (col.g > .5) { col.g = col.g + 2.0*(col.g - .5); }
																						else { col.g = col.g + 2 * col.g - 1; }

																						if (col.b > .5) { col.b = col.b + 2.0*(col.b - .5); }
																						else { col.b = col.b + 2 * col.b - 1; }
																					#endif

																						//Pin Light 点光
																						#if _OVERLAY2_PINLIGHT
																							if (col.r > .5) { col.r = max(col.r,2 * (col.r - .5)); }
																							else { col.r = min(col.r,2 * col.r); }

																							if (col.g > .5) { col.g = max(col.g,2 * (col.g - .5)); }
																							else { col.g = min(col.g,2 * col.g); }

																							if (col.b > .5) { col.b = max(col.b,2 * (col.b - .5)); }
																							else { col.b = min(col.b,2 * col.b); }
																						#endif

																							//Difference 差值	
																							#if _OVERLAY2_DIFFERENCE
																								col.rgb = abs(col.rgb - col.rgb);
																							#endif

																								//Exclusion 排除	
																								#if _OVERLAY2_EXCLUSION
																									col.rgb = .5 - 2 * (col.rgb - 0.5) * (col.rgb - 0.5);
																								#endif

																								return col + _TintColor1;
																							}
																							ENDCG
																						}
																					}
	}
}

