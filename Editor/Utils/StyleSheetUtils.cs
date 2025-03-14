using UnityEditor;
using UnityEngine.UIElements;

namespace CookieJarTools.BakersBelt.Editor.Utils
{
	public static class StyleSheetUtils
	{
		public static StyleSheet LoadStyleSheet(string name)
		{
			StyleSheet styleSheet = null;
			var assetPaths = AssetDatabase.FindAssets($"t:StyleSheet {name}");
			if (assetPaths.Length > 0)
			{
				var guidToAssetPath = AssetDatabase.GUIDToAssetPath(assetPaths[0]);
				var loadAsset = AssetDatabase.LoadAssetAtPath(guidToAssetPath, typeof(StyleSheet));
				if (loadAsset != null)
				{
					styleSheet = (StyleSheet)loadAsset;
				}
			}
			return styleSheet;
		}
	}
}