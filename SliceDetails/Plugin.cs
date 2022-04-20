using IPA;
using IPA.Config;
using IPA.Config.Stores;
using SiraUtil.Zenject;
using SliceDetails.Installers;
using SliceDetails.Settings;
using BeatSaberMarkupLanguage.Settings;
using SliceDetails.UI;

using IPALogger = IPA.Logging.Logger;

namespace SliceDetails
{

	[Plugin(RuntimeOptions.DynamicInit), NoEnableDisable]
	public class Plugin
	{
		internal static SettingsStore Settings { get; private set; }
		internal static IPALogger Log { get; private set; }

		[Init]
		public void Init(IPALogger logger, Config config, Zenjector zenject) {
			Settings = config.Generated<SettingsStore>();

			BSMLSettings.instance.AddSettingsMenu("SliceDetails", $"SliceDetails.UI.Views.settingsView.bsml", SettingsViewController.instance);

			Plugin.Log = logger;
			
			zenject.Install<SDAppInstaller>(Location.App);
			zenject.Install<SDMenuInstaller>(Location.Menu);
			zenject.Install<SDGameInstaller>(Location.StandardPlayer);
		}
	}
}
