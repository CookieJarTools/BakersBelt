using CookieJar.BakersBelt.Editor.ToolbarExtender;
using CookieJarTools.BakersBelt.Editor.Utils;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine.UIElements;

namespace CookieJar.BakersBelt.CompilationControls
{
	[InitializeOnLoad]
	public static class CompilationController
	{
		private static Image toggleCompilationButton;
		
		static CompilationController()
		{
			ToolbarExtender.AddToRightToolbar(CreateCompilationControls);
		}

		static VisualElement CreateCompilationControls()
		{
			var root = new VisualElement
			{
				style =
				{
					flexDirection = FlexDirection.Row
				}
			};
			
			var styleSheet = StyleSheetUtils.LoadStyleSheet("CookieJarCompilationControlsStyleSheet");
			if (styleSheet != null) root.styleSheets.Add(styleSheet);

			toggleCompilationButton = new Image
			{
				image = EditorGUIUtility.IconContent("LockIcon-On").image,
				tooltip = "Toggle Compilation"
			};
			toggleCompilationButton.AddToClassList("lock");
			toggleCompilationButton.RegisterCallback<ClickEvent>(_ => ToggleCompilationLock());
			root.Add(toggleCompilationButton);
			
			var forceCompilationButton = new Image
			{
				image = EditorGUIUtility.IconContent("d_ModelImporter Icon").image,
				tooltip = "Force Compilation"
			};
			forceCompilationButton.AddToClassList("force");
			forceCompilationButton.RegisterCallback<ClickEvent>(_ => ForceRecompile());
			root.Add(forceCompilationButton);

			return root;
		}
		
		public static void ToggleCompilationLock()
		{
			if (!AssemblyReloadUtility.CanReloadAssemblies())
			{
				EditorApplication.UnlockReloadAssemblies();
				toggleCompilationButton.image = EditorGUIUtility.IconContent("LockIcon-On").image;
			}
			else
			{
				EditorApplication.LockReloadAssemblies();
				toggleCompilationButton.image = EditorGUIUtility.IconContent("LockIcon").image;
			}
			toggleCompilationButton.ToggleInClassList("locked");
		}
		
		public static void ForceRecompile()
		{
			CompilationPipeline.RequestScriptCompilation();
			AssetDatabase.Refresh();
		}
	}
}