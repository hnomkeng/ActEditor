using System;
using System.Globalization;
using ErrorManager;
using GRF.FileFormats.ActFormat;
using TokeiLibrary;
using TokeiLibrary.WPF;

namespace Scripts {
    public class Script : IActScript {
		public const int Magnify = 2;
		
		public object DisplayName {
			get { return "Magnify"; }
		}
		
		public string Group {
			get { return "Scripts"; }
		}
		
		public string InputGesture {
			get { return "Ctrl-Shift-M" ; }
		}
		
		public string Image {
			get { return "scale.png"; }
		}
		
		public void Execute(Act act, int selectedActionIndex, int selectedFrameIndex, int[] selectedLayerIndexes) {
			if (act == null) return;
			
			var effect = new EffectConfiguration("Magnify");
			effect.AddProperty("ScaleX", 2f, 0, 10f);
			effect.AddProperty("ScaleY", 2f, 0, 10f);
			effect.AddProperty("Anchors", 1, 0, 1);
			effect.Apply(actInput => {
				actInput.Commands.Backup(_ => {
					float scaleX = effect.GetProperty<float>("ScaleX");
					float scaleY = effect.GetProperty<float>("ScaleY");
					int anchors = effect.GetProperty<int>("Anchors");

					foreach (Layer layer in actInput.GetAllLayers()) {
						layer.OffsetX = (int)(layer.OffsetX * scaleX);
						layer.OffsetY = (int)(layer.OffsetY * scaleY);
						layer.ScaleX *= scaleX;
						layer.ScaleY *= scaleY;
					}
					
					if (anchors == 1) {
						actInput.AllAnchors(anchor => {
							anchor.OffsetX = (int)(anchor.OffsetX * scaleX);
							anchor.OffsetY = (int)(anchor.OffsetY * scaleY);
						});
					}
				}, "Magnify");
			});
			effect.Display(act, selectedActionIndex);
		}
		
		public bool CanExecute(Act act, int selectedActionIndex, int selectedFrameIndex, int[] selectedLayerIndexes) {
			return act != null;
		}
	}
}
