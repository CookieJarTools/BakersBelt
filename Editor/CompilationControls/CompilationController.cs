using CookieJarTools.BakersBelt.Editor.Toolbar;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.Toolbars;
using UnityEngine;

namespace CookieJarTools.BakersBelt.Editor.CompilationControls
{
	/// <summary>
	/// Holding compilation off while you work through a change that touches several files, and
	/// asking for one when you are ready.
	/// <para>
	/// The lock is a toggle rather than a button with a picture of a padlock on it, so the toolbar
	/// draws it pressed or not the way it draws everything else of that kind.
	/// </para>
	/// <para>
	/// Whether the lock is on is remembered here rather than asked of the editor, because the
	/// editor only counts locks and keeps no public way to read the count. The belt is the one
	/// holding this lock, so the belt is the one that knows. The note is kept in session state,
	/// which outlives a domain reload and is gone when the editor closes, exactly as the lock is.
	/// </para>
	/// </summary>
	public static class CompilationController
	{
		private const string LockPath = "Bakers Belt/Hold compilation";
		private const string CompilePath = "Bakers Belt/Compile now";
		private const string Remembered = "CookieJarTools.BakersBelt.CompilationHeld";

		/// <summary>Whether the belt is holding compilation off.</summary>
		public static bool Held
		{
			get => SessionState.GetBool(Remembered, false);
			private set => SessionState.SetBool(Remembered, value);
		}

		[MainToolbarElement(LockPath, defaultDockPosition = MainToolbarDockPosition.Right, defaultDockIndex = 10)]
		private static MainToolbarElement Lock()
		{
			var content = new MainToolbarContent(Picture("AssemblyLock"),
				"Hold compilation off. Unity waits until you let go, so a change across several " +
				"files cannot reload the domain half way through it");
			return new MainToolbarToggle(content, Held, Holding);
		}

		[MainToolbarElement(CompilePath, defaultDockPosition = MainToolbarDockPosition.Right, defaultDockIndex = 11)]
		private static MainToolbarElement CompileNow()
		{
			return new MainToolbarButton(new MainToolbarContent(Picture("d_Refresh"), "Compile now"), Compile);
		}

		/// <summary>Takes the lock, or lets it go.</summary>
		public static void Holding(bool held)
		{
			if (held == Held)
			{
				return;
			}

			if (held)
			{
				EditorApplication.LockReloadAssemblies();
			}
			else
			{
				EditorApplication.UnlockReloadAssemblies();
			}

			Held = held;
			MainToolbar.Refresh(LockPath);
		}

		/// <summary>Asks for a compile, letting the lock go first if the belt is holding one.</summary>
		public static void Compile()
		{
			// Asking while the lock is on would do nothing and say nothing, which looks like a
			// button that does not work.
			Holding(false);
			CompilationPipeline.RequestScriptCompilation();
		}

		private static Texture2D Picture(string named)
		{
			return EditorGUIUtility.IconContent(named).image as Texture2D;
		}
	}
}
