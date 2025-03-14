using System;
using System.Reflection;
using UnityEditor;

namespace CookieJar.BakersBelt.CompilationControls
{
	internal static class AssemblyReloadUtility
	{
		private static readonly MethodInfo canReloadAssembliesMethod;

		static AssemblyReloadUtility()
		{
			// Retrieve the internal 'CanReloadAssemblies' method using reflection
			canReloadAssembliesMethod = typeof(EditorApplication).GetMethod(
				"CanReloadAssemblies",
				BindingFlags.Static | BindingFlags.NonPublic
			);

			if (canReloadAssembliesMethod == null)
			{
				throw new InvalidOperationException("Unable to find the 'CanReloadAssemblies' method.");
			}
		}

		public static bool CanReloadAssemblies()
		{
			// Invoke the internal method and return its result
			return (bool)canReloadAssembliesMethod.Invoke(null, null);
		}
	}
}