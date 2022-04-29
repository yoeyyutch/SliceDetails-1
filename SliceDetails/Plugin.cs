using IPA;
using IPA.Config;
using IPA.Config.Stores;
using SiraUtil.Zenject;
using SliceDetails.Installers;
using SliceDetails.Settings;
using BeatSaberMarkupLanguage.Settings;
using SliceDetails.UI;
using System.Diagnostics;
using HarmonyLib;
using System.Reflection;

namespace SliceDetails
{

	[Plugin(RuntimeOptions.DynamicInit), NoEnableDisable]
	public class Plugin
	{
		internal static SettingsStore Settings { get; private set; }
		public static Harmony harmony;
	 

		[Init]
		public void Init(IPA.Logging.Logger logger, Config config, Zenjector zenject) {
			Settings = config.Generated<SettingsStore>();
			harmony = new Harmony("yoeyyutch.slice-details-1");
			

			BSMLSettings.instance.AddSettingsMenu("SliceDetails", $"SliceDetails.UI.Views.settingsView.bsml", SettingsViewController.instance);
			
			zenject.UseLogger(logger);
			
			zenject.Install<SDAppInstaller>(Location.App);
			zenject.Install<SDMenuInstaller>(Location.Menu);
			zenject.Install<SDGameInstaller>(Location.StandardPlayer);
		}

		[OnEnable]
		public void OnEnable()
		{
			harmony.PatchAll(Assembly.GetExecutingAssembly());

		}

		[OnDisable]
		public void OnApplicationQuit()
		{
			harmony.UnpatchSelf();
		}
	}
}
