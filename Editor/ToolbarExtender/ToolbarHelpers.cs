using UnityEngine;
using System.Reflection;
using UnityEngine.UIElements;

namespace CookieJar.BakersBelt.Editor.ToolbarExtender
{
	internal static class ToolbarHelpers
	{
		public static (VisualElement toolbarLeftZone, VisualElement toolbarRightZone) GetToolbarZones()
		{
			var toolbarType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.Toolbar");
			var toolbars = Resources.FindObjectsOfTypeAll(toolbarType);
			var currentToolbar = toolbars.Length > 0 ? toolbars[0] : null;
			
			if (currentToolbar == null) return (null, null);
			
			var root = toolbarType.GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
			var rawRoot = root.GetValue(currentToolbar);
			var rootElement = rawRoot as VisualElement;
			
			var toolbarLeftZone = rootElement.Q("ToolbarZoneLeftAlign");
			var toolbarRightZone = rootElement.Q("ToolbarZoneRightAlign");
			
			return (toolbarLeftZone, toolbarRightZone);
		}
	}
}