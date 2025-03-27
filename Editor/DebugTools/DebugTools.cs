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
    public static class DebugTools
    {
        static DebugTools()
        {
            ToolbarExtender.AddToRightToolbar(CreateUiToolkitDebugElement);
        }

        private static VisualElement CreateUiToolkitDebugElement()
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

            var debugDropdown = new ToolbarMenu
            {
                text = "Debug Tools"
            };

            debugDropdown.menu.AppendAction("Open UI Toolkit Debugger", action =>
            {
                var debuggerWindowType = System.Type.GetType("UnityEditor.UIElements.Debugger.UIElementsDebugger, UnityEditor.UIElementsModule");
                if (debuggerWindowType != null)
                {
                    EditorWindow.GetWindow(debuggerWindowType);
                }
                else
                {
                    Debug.LogError("UIBuilderDebugger window type not found.");
                }
            });

            debugDropdown.menu.AppendAction("Toggle Inspector Debug Mode", action =>
            {
                ToggleInspectorDebugMode();
            });

            root.Add(debugDropdown);
            return root;
        }

        /// <summary>
        /// Toggles the Inspector's mode between Normal (0) and Debug (1).
        /// </summary>
        private static void ToggleInspectorDebugMode()
        {
            var inspectorWindowType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.InspectorWindow");
            if (inspectorWindowType == null)
            {
                Debug.LogError("InspectorWindow type not found.");
                return;
            }

            var inspectors = Resources.FindObjectsOfTypeAll(inspectorWindowType);
            foreach (var inspector in inspectors)
            {
                var inspectorModeField = inspectorWindowType.GetField("m_InspectorMode", BindingFlags.Instance | BindingFlags.NonPublic);
                if (inspectorModeField == null) continue;
                
                var currentMode = (int)inspectorModeField.GetValue(inspector);
                var newMode = currentMode == 0 ? 1 : 0;
                inspectorModeField.SetValue(inspector, newMode);
                    
                inspector.GetType().GetMethod("Repaint", BindingFlags.Instance | BindingFlags.Public)?.Invoke(inspector, null);
            }
        }
    }
}
