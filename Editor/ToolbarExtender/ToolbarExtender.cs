using CookieJarTools.BakersBelt.Editor.Utils;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace CookieJar.BakersBelt.Editor.ToolbarExtender
{
	[InitializeOnLoad]
	public static class ToolbarExtender
	{
		private static VisualElement toolbarLeftZone;
		private static VisualElement toolbarRightZone;
		
		private static readonly StyleSheet styleSheet;
		
		private static readonly List<Func<VisualElement>> leftZoneFunctions = new();
		private static readonly List<Func<VisualElement>> rightZoneFunctions = new();
		
		static ToolbarExtender()
		{
			styleSheet = StyleSheetUtils.LoadStyleSheet("CookieJarToolbarExtender");

			EditorApplication.update -= OnUpdate;
			EditorApplication.update += OnUpdate;
		}

		private static void OnUpdate()
		{
			if (toolbarLeftZone != null && toolbarRightZone != null) return;
			
			var (leftZone, rightZone) = ToolbarHelpers.GetToolbarZones();
			toolbarLeftZone = leftZone;
			toolbarRightZone = rightZone;

			if (styleSheet != null)
			{
				toolbarLeftZone.styleSheets.Add(styleSheet);
				toolbarRightZone.styleSheets.Add(styleSheet);
			}
			
			foreach (var leftZoneFunction in leftZoneFunctions)
			{
				var element = leftZoneFunction();
				toolbarLeftZone.Add(element);
			}
			
			foreach (var rightZoneFunction in rightZoneFunctions)
			{
				var element = rightZoneFunction();
				toolbarRightZone.Add(element);
			}
		}

		public static void AddToLeftToolbar(Func<VisualElement> elementFunc) => leftZoneFunctions.Add(elementFunc);
		public static void AddToRightToolbar(Func<VisualElement> elementFunc) => rightZoneFunctions.Add(elementFunc);
	}
}