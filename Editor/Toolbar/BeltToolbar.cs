using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace CookieJarTools.BakersBelt.Editor.Toolbar
{
	/// <summary>
	/// Puts things on Unity's main toolbar through the toolbar API rather than by reaching into
	/// the editor's own visual tree.
	/// <para>
	/// Unity now declares toolbar elements with an attribute on a static method, which it reads
	/// once as the editor loads. That leaves no way to hand it an element later, so the belt keeps
	/// a small number of slots on each side: every slot is declared up front, each one shows
	/// whatever has been put in it, and an empty slot shows nothing at all.
	/// </para>
	/// <para>
	/// Only the kinds Unity provides can go on the toolbar: a button, a dropdown and a toggle.
	/// Arbitrary elements of your own are no longer possible for anybody, because the one method
	/// that would draw them is internal to the editor.
	/// </para>
	/// </summary>
	public static class BeltToolbar
	{
		internal const string LeftPath = "Bakers Belt/Left of play/Slot ";
		internal const string RightPath = "Bakers Belt/Right of play/Slot ";
		internal const int Slots = 6;

		private static readonly List<BeltItem> left = new List<BeltItem>();
		private static readonly List<BeltItem> right = new List<BeltItem>();

		/// <summary>A button, on the left of the play controls or the right of them.</summary>
		public static void AddButton(string text, string tooltip, Action pressed, bool onTheRight = true)
		{
			Put(new BeltItem(BeltItemKind.Button, text, tooltip) { Pressed = pressed }, onTheRight);
		}

		/// <summary>A button whose icon comes from Unity's own set, named as `EditorGUIUtility.IconContent` names it.</summary>
		public static void AddIconButton(string icon, string tooltip, Action pressed, bool onTheRight = true)
		{
			Put(new BeltItem(BeltItemKind.Button, null, tooltip) { Icon = icon, Pressed = pressed }, onTheRight);
		}

		/// <summary>A dropdown. Fill the menu it hands you when it is opened.</summary>
		public static void AddDropdown(string text, string tooltip, Action<GenericMenu> filling, bool onTheRight = true)
		{
			Put(new BeltItem(BeltItemKind.Dropdown, text, tooltip) { Filling = filling }, onTheRight);
		}

		/// <summary>A switch that stays as you left it.</summary>
		public static void AddToggle(string text, string tooltip, bool on, Action<bool> changed, bool onTheRight = true)
		{
			Put(new BeltItem(BeltItemKind.Toggle, text, tooltip) { On = on, Changed = changed }, onTheRight);
		}

		/// <summary>
		/// Draws a slot again, for when what is in it has changed its mind about how it should look.
		/// </summary>
		public static void Refresh(BeltItem item)
		{
			var side = right.Contains(item) ? right : left;
			var at = side.IndexOf(item);
			if (at < 0)
			{
				return;
			}

			MainToolbar.Refresh((side == right ? RightPath : LeftPath) + (at + 1));
		}

		internal static BeltItem At(bool onTheRight, int slot)
		{
			var side = onTheRight ? right : left;
			return slot < side.Count ? side[slot] : null;
		}

		private static void Put(BeltItem item, bool onTheRight)
		{
			var side = onTheRight ? right : left;
			if (side.Count >= Slots)
			{
				Debug.LogWarning("Bakers Belt holds " + Slots + " things a side and that side is full, so " +
								 (item.Text ?? item.Tooltip) + " was left off.");
				return;
			}

			side.Add(item);
			var path = (onTheRight ? RightPath : LeftPath) + side.Count;
			MainToolbar.Refresh(path);
			BeltFirstRun.Ensure(path);
		}
	}
}
