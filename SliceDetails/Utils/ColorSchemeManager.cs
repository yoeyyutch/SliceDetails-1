using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static PlayerSaveData;

namespace SliceDetails.Utils
{
	internal class ColorSchemeManager
	{
		private static ColorScheme _colorScheme;

		public static ColorScheme GetMainColorScheme()
		{
			if (_colorScheme == null)
			{
				ColorSchemesSettings colorSchemesSettings = ReflectionUtil.GetField<PlayerData, PlayerDataModel>(Resources.FindObjectsOfTypeAll<PlayerDataModel>().FirstOrDefault(), "_playerData").colorSchemesSettings;
				_colorScheme = colorSchemesSettings.GetSelectedColorScheme();
			}
			return _colorScheme;
		}

		public static Material SliceMaterial(ColorType colorType)
		{
			Material m = new(Shader.Find("Sprites/Default"));
			Color a = GetMainColorScheme().saberAColor;
			Color b = GetMainColorScheme().saberBColor;
			Color cA = new(a.r, a.g, a.b, Plugin.Settings.SliceTransparency);
			Color cB = new(b.r, b.g, b.b, Plugin.Settings.SliceTransparency);
			if (colorType == ColorType.ColorA)
				m.color = cA;
			else if (colorType == ColorType.ColorB)
				m.color = cB;
			return m;

		}
		public static Material SliceMaterial(int colorType)
		{
			Material m = new(Shader.Find("Sprites/Default"));
			Color a = GetMainColorScheme().saberAColor;
			Color b = GetMainColorScheme().saberBColor;
			Color cA = new(a.r, a.g, a.b, .25f);
			Color cB = new(b.r, b.g, b.b, .25f);
			if (colorType == (int)ColorType.ColorA)
				m.color = cA;
			else if (colorType == (int)ColorType.ColorB)
				m.color = cB;
			return m;

		}
	}
}
