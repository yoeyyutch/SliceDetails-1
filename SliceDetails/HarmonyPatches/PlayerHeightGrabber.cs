using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using TMPro;

namespace SliceDetails.HarmonyPatches
{
	public class PlayerHeightGrabber
	{
		public static float PlayerHeight = 1.7f;

		[HarmonyPatch(typeof(PlayerHeightSettingsController), "Init")]
		class PleyerHeightSettingsControllerInit
		{
			public static void Postfix(float ____value)
			{
				if (PlayerHeight != ____value)
				{
					PlayerHeight = ____value;
				}
			}
		}

		[HarmonyPatch(typeof(PlayerHeightSettingsController), "RefreshUI")]
		class PleyerHeightSettingsControllerRefreshUI
		{
			public static void Postfix(TextMeshProUGUI ____text, float ____value)
			{
				if (PlayerHeight != ____value)
				{
					PlayerHeight = ____value;
				}
				____text.text = string.Format("{0:0.00}m", ____value);

			}
		}


	}
}
