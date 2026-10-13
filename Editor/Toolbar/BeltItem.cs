using System;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace CookieJarTools.BakersBelt.Editor.Toolbar
{
	/// <summary>One thing on the toolbar: what it says, what it does, and which kind it is.</summary>
	public sealed class BeltItem
	{
		internal BeltItem(BeltItemKind kind, string text, string tooltip)
		{
			Kind = kind;
			Text = text;
			Tooltip = tooltip;
		}

		public BeltItemKind Kind { get; }

		public string Text { get; set; }

		public string Tooltip { get; set; }

		/// <summary>An icon from Unity's own set, named the way `EditorGUIUtility.IconContent` names them.</summary>
		public string Icon { get; set; }

		public bool On { get; set; }

		internal Action Pressed { get; set; }

		internal Action<GenericMenu> Filling { get; set; }

		internal Action<bool> Changed { get; set; }

		/// <summary>What it is, said the way the toolbar wants to hear it.</summary>
		internal MainToolbarContent Content()
		{
			var picture = string.IsNullOrEmpty(Icon) ? null : EditorGUIUtility.IconContent(Icon).image as Texture2D;
			if (picture == null)
			{
				return new MainToolbarContent(Text ?? string.Empty, Tooltip);
			}

			return string.IsNullOrEmpty(Text)
				? new MainToolbarContent(picture, Tooltip)
				: new MainToolbarContent(Text, picture, Tooltip);
		}

		/// <summary>The toolbar's own element for this, which is the only kind of element it takes.</summary>
		internal MainToolbarElement Build()
		{
			if (Kind == BeltItemKind.Dropdown)
			{
				return new MainToolbarDropdown(Content(), Open);
			}

			if (Kind == BeltItemKind.Toggle)
			{
				return new MainToolbarToggle(Content(), On, Flipped);
			}

			return new MainToolbarButton(Content(), () => Pressed?.Invoke());
		}

		private void Open(Rect where)
		{
			var menu = new GenericMenu();
			Filling?.Invoke(menu);
			menu.DropDown(where);
		}

		private void Flipped(bool now)
		{
			On = now;
			Changed?.Invoke(now);
		}
	}
}
