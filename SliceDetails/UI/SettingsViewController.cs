using BeatSaberMarkupLanguage.Attributes;
//using BeatSaberMarkupLanguage.Components;
//using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMarkupLanguage.Components.Settings;
//using BeatSaberMarkupLanguage.GameplaySetup;
//using System.ComponentModel;


namespace SliceDetails.UI
{
	internal class SettingsViewController : PersistentSingleton<SettingsViewController>
	{
		[UIComponent("distance-slider")]
		private SliderSetting Distance_Slider;

		[UIValue("distance_value")]
		private float Distance_Value
		{
			get => Plugin.Settings.SliceDistance;
			set => Plugin.Settings.SliceDistance = value;
		}

		[UIAction("set_distance_value")]
		void Set_Distance(float value)
		{
			Distance_Value = value;
		}

		[UIAction("distance_slider_formatter")]
		private string Distance_Slider_Formatter(float value) => value.ToString("0.##") + " m";
		


		//[UIValue("slice-length")]
		//public float SliceLength
		//{
		//	get { return Plugin.Settings.SliceLength; }
		//	set { Plugin.Settings.SliceLength = value; }
		//}

		//[UIValue("slice-width")]
		//public float SliceWidth
		//{
		//	get { return Plugin.Settings.SliceWidth; }
		//	set { Plugin.Settings.SliceWidth = value; }
		//}

		//[UIValue("slice-transparency")]
		//public float SliceTransparency
		//{
		//	get { return Plugin.Settings.SliceTransparency; }
		//	set { Plugin.Settings.SliceTransparency = value; }
		//}

		[UIValue("show-liveview")]
		public bool ShowLiveView
		{
			get { return Plugin.Settings.ShowLiveView; }
			set { Plugin.Settings.ShowLiveView = value; }
		}

		[UIValue("show-pause")]
		public bool ShowInPauseMenu {
			get { return Plugin.Settings.ShowInPauseMenu; }
			set { Plugin.Settings.ShowInPauseMenu = value; }
		}

		[UIValue("show-completion")]
		public bool ShowInCompletionScreen
		{
			get { return Plugin.Settings.ShowInCompletionScreen; }
			set { Plugin.Settings.ShowInCompletionScreen = value; }
		}

		[UIValue("show-handles")]
		public bool ShowHandle
		{
			get { return Plugin.Settings.ShowHandle; }
			set { Plugin.Settings.ShowHandle = value; }
		}

		[UIValue("true-offsets")]
		public bool TrueCutOffsets
		{
			get { return Plugin.Settings.TrueCutOffsets; }
			set { Plugin.Settings.TrueCutOffsets = value; }
		}
	}
}
