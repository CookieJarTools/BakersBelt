using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace CookieJarTools.BakersBelt.Editor.DebugTools
{
	/// <summary>
	/// The three things everybody does to the console, within reach: open it, empty it, and find
	/// the folder the game actually writes to.
	/// </summary>
	public static class ConsoleUtilityTools
	{
		private const string Path = "Bakers Belt/Console";

		[MainToolbarElement(Path, defaultDockPosition = MainToolbarDockPosition.Right, defaultDockIndex = 13)]
		private static MainToolbarElement Console()
		{
			var content = new MainToolbarContent(EditorGUIUtility.IconContent("d_UnityEditor.ConsoleWindow").image as Texture2D,
				"Console shortcuts");
			return new MainToolbarDropdown(content, Open);
		}

		private static void Open(Rect where)
		{
			var menu = new GenericMenu();
			menu.AddItem(new GUIContent("Open the console"), false, OpenConsole);
			menu.AddItem(new GUIContent("Clear the console"), false, Clear);
			menu.AddSeparator(string.Empty);
			menu.AddItem(new GUIContent("Show the persistent data folder"), false, ShowPersistentData);
			menu.DropDown(where);
		}

		public static void OpenConsole()
		{
			if (!EditorApplication.ExecuteMenuItem("Window/General/Console"))
			{
				Debug.LogWarning("Bakers Belt could not open the console from the menu.");
			}
		}

		/// <summary>Empties the console. Clearing it is internal, so this is the one place reflection is still needed.</summary>
		public static void Clear()
		{
			var entries = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.LogEntries");
			var clear = entries?.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (clear == null)
			{
				Debug.LogWarning("Bakers Belt could not find UnityEditor.LogEntries.Clear, so the console was left alone.");
				return;
			}

			clear.Invoke(null, Array.Empty<object>());
		}

		public static void ShowPersistentData()
		{
			EditorUtility.RevealInFinder(Application.persistentDataPath);
		}
	}
}
