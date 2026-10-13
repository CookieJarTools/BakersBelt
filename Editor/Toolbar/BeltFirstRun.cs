using System.Reflection;
using UnityEditor;
using UnityEditor.Overlays;

namespace CookieJarTools.BakersBelt.Editor.Toolbar
{
	/// <summary>
	/// Shows the belt's own elements the first time it is installed, and never again.
	/// <para>
	/// A toolbar element declared by a package arrives switched off: Unity registers it, then
	/// leaves it to the person to turn on from the toolbar's own menu. There is no public way to
	/// say "on by default", so this asks the toolbar for the element once, through the one
	/// internal method that can hand it over, and sets it visible.
	/// </para>
	/// <para>
	/// It is remembered per element per person, so a deliberately hidden element stays hidden. If
	/// the internal method ever goes, nothing breaks and nothing is logged: the elements are then
	/// simply off until somebody turns them on, which is where Unity leaves every other package.
	/// </para>
	/// </summary>
	internal static class BeltFirstRun
	{
		private const string Remembered = "CookieJarTools.BakersBelt.Shown.";

		private static readonly string[] Ours =
		{
			"Bakers Belt/Hold compilation",
			"Bakers Belt/Compile now",
			"Bakers Belt/Debug",
			"Bakers Belt/Console",
		};

		[InitializeOnLoadMethod]
		private static void Settle()
		{
			// The toolbar is built after the domain is, so asking now would find nothing.
			EditorApplication.delayCall += Once;
		}

		/// <summary>
		/// Shows a slot the first time something is put in it. The toolbar is not necessarily
		/// built yet when a package fills a slot, so this waits for the editor to come round again.
		/// </summary>
		internal static void Ensure(string path)
		{
			EditorApplication.delayCall += () => Maybe(path);
		}

		private static void Once()
		{
			foreach (var path in Ours)
			{
				Maybe(path);
			}
		}

		private static void Maybe(string path)
		{
			var key = Remembered + path;
			if (EditorPrefs.GetBool(key, false))
			{
				return;
			}

			if (Show(path))
			{
				EditorPrefs.SetBool(key, true);
			}
		}

		private static bool Show(string path)
		{
			var toolbar = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.Toolbars.MainToolbar");
			var ask = toolbar?.GetMethod("TryGetOverlay", BindingFlags.Static | BindingFlags.NonPublic);
			if (ask == null)
			{
				return false;
			}

			var arguments = new object[] { path, null };
			if (!(bool)ask.Invoke(null, arguments) || !(arguments[1] is Overlay overlay))
			{
				return false;
			}

			overlay.displayed = true;
			return true;
		}
	}
}
