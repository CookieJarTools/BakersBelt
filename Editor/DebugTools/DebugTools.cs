using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace CookieJarTools.BakersBelt.Editor.DebugTools
{
	/// <summary>
	/// The switches you want while something is behaving oddly: every inspector in debug mode, and
	/// the UI Toolkit debugger, without going hunting through menus for either.
	/// </summary>
	public static class DebugTools
	{
		private const string Path = "Bakers Belt/Debug";

		[MainToolbarElement(Path, defaultDockPosition = MainToolbarDockPosition.Right, defaultDockIndex = 12)]
		private static MainToolbarElement Debugging()
		{
			var content = new MainToolbarContent(EditorGUIUtility.IconContent("d_UnityEditor.DebugInspectorWindow").image as Texture2D,
				"Debug switches");
			return new MainToolbarDropdown(content, Open);
		}

		private static void Open(Rect where)
		{
			var menu = new GenericMenu();
			menu.AddItem(new GUIContent("Inspectors in debug mode"), AnyInspectorDebugging(), Flip);
			menu.AddItem(new GUIContent("UI Toolkit debugger"), false, OpenUiDebugger);
			menu.DropDown(where);
		}

		/// <summary>
		/// Puts every inspector into debug mode, or takes them all out of it. The mode is a private
		/// field on the window, so there is no way to ask for it politely.
		/// </summary>
		public static void Flip()
		{
			var wanted = AnyInspectorDebugging() ? 0 : 1;
			foreach (var inspector in Inspectors())
			{
				var field = Mode(inspector);
				if (field == null)
				{
					continue;
				}

				field.SetValue(inspector, wanted);
				((EditorWindow)inspector).Repaint();
			}
		}

		public static void OpenUiDebugger()
		{
			// The menu item is the supported way in, and it works out which panel to attach to.
			if (!EditorApplication.ExecuteMenuItem("Window/UI Toolkit/Debugger"))
			{
				Debug.LogWarning("Bakers Belt could not open the UI Toolkit debugger from the menu.");
			}
		}

		private static bool AnyInspectorDebugging()
		{
			foreach (var inspector in Inspectors())
			{
				var field = Mode(inspector);
				if (field != null && (int)field.GetValue(inspector) != 0)
				{
					return true;
				}
			}

			return false;
		}

		private static UnityEngine.Object[] Inspectors()
		{
			var type = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.InspectorWindow");
			return type == null ? Array.Empty<UnityEngine.Object>() : Resources.FindObjectsOfTypeAll(type);
		}

		private static FieldInfo Mode(UnityEngine.Object inspector)
		{
			return inspector.GetType().GetField("m_InspectorMode", BindingFlags.Instance | BindingFlags.NonPublic);
		}
	}
}
