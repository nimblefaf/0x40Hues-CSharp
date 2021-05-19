//------------------------------------------------------------------------------
// <auto-generated>
//     ???? ??? ?????? ??????????.
//     ??????????? ??????:4.0.30319.42000
//
//     ????????? ? ???? ????? ????? ???????? ? ???????????? ?????? ? ????? ???????? ? ??????
//     ????????? ????????? ????.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;


namespace MOPS.Shaders {
	
	public class ColorBlend_HardLightEffect : ShaderEffect {
		public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(ColorBlend_HardLightEffect), 0);
		public static readonly DependencyProperty BlendProperty = DependencyProperty.Register("Blend", typeof(Color), typeof(ColorBlend_HardLightEffect), new UIPropertyMetadata(Color.FromArgb(179, 255, 255, 255), PixelShaderConstantCallback(0)));
		public SolidColorBrush blendBrush = new SolidColorBrush();
		public ColorBlend_HardLightEffect() {
			PixelShader pixelShader = new PixelShader();
			pixelShader.UriSource = new Uri("effects/ColorBlend_HardLightEffect.ps", UriKind.Relative);
			this.PixelShader = pixelShader;

			this.UpdateShaderValue(InputProperty);
			this.UpdateShaderValue(BlendProperty);
		}
		public Brush Input {
			get {
				return ((Brush)(this.GetValue(InputProperty)));
			}
			set {
				this.SetValue(InputProperty, value);
			}
		}
		/// <summary>Color to blend image with</summary>
		public Color Blend {
			get {
				return ((Color)(this.GetValue(BlendProperty)));
			}
			set {
				this.SetValue(BlendProperty, value);
			}
		}




		//public SolidColorBrush BlendBrush
  //      {
		//	get
  //          {
		//		return blendBrush;
  //          }
		//	set
  //          {
		//		blendBrush = value;
		//		this.SetValue(BlendProperty, Color.FromArgb(179, value.Color.R, value.Color.G, value.Color.B));
		//	}
  //      }
	}
}