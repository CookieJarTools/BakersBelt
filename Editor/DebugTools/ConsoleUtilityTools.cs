using System;
using System.Reflection;
using CookieJar.BakersBelt.Editor.ToolbarExtender;
using CookieJarTools.BakersBelt.Editor.Utils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CookieJarTools.BakersBelt.Editor.DebugTools
{
    [InitializeOnLoad]
    public static class ConsoleUtilityTools
    {
        static ConsoleUtilityTools()
        {
            ToolbarExtender.AddToRightToolbar(CreateConsoleUtilityDropdown);
        }

        private static VisualElement CreateConsoleUtilityDropdown()
        {
            var root = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row
                }
            };
            
            var styleSheet = StyleSheetUtils.LoadStyleSheet("CookieJarDebugToolsStyleSheet");
            if (styleSheet != null) root.styleSheets.Add(styleSheet);

            var utilitiesMenu = new ToolbarMenu
            {
                text = "Console Utility",
                tooltip = "Quick console shortcuts"
            };

            utilitiesMenu.menu.AppendAction("Open Console Window", _ => OpenConsoleWindow());
            utilitiesMenu.menu.AppendAction("Clear Console", _ => ClearConsole());
            utilitiesMenu.menu.AppendAction("Reveal Persistent Data Path", _ => RevealPersistentDataPath());

            root.Add(utilitiesMenu);
            return root;
        }

        private static void OpenConsoleWindow()
        {
            var consoleWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
            if (consoleWindowType == null)
            {
                Debug.LogError("Console window type not found. Unable to open console.");
                return;
            }

            EditorWindow.GetWindow(consoleWindowType);
        }

        private static void ClearConsole()
        {
            var logEntriesType = Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
            var clearMethod = logEntriesType?.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (clearMethod == null)
            {
                Debug.LogError("LogEntries.Clear reflection lookup failed. Unable to clear console.");
                return;
            }

            clearMethod.Invoke(null, null);
        }

        private static void RevealPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
    }
}
