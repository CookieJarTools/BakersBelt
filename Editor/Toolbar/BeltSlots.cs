using UnityEditor.Toolbars;

namespace CookieJarTools.BakersBelt.Editor.Toolbar
{
	/// <summary>
	/// The slots themselves. Unity reads these methods once as the editor loads, so they have to
	/// exist whether anything has been put in them or not; a slot with nothing in it is not shown.
	/// <para>
	/// There is no neater way round the repetition. The attribute wants a compile time path and a
	/// method of its own for each element, so a loop cannot declare them.
	/// </para>
	/// </summary>
	internal static class BeltSlots
	{
		[MainToolbarElement(BeltToolbar.LeftPath + "1", defaultDockPosition = MainToolbarDockPosition.Left, defaultDockIndex = 100)]
		private static MainToolbarElement Left0() => Slot(false, 0);

		[MainToolbarElement(BeltToolbar.LeftPath + "2", defaultDockPosition = MainToolbarDockPosition.Left, defaultDockIndex = 101)]
		private static MainToolbarElement Left1() => Slot(false, 1);

		[MainToolbarElement(BeltToolbar.LeftPath + "3", defaultDockPosition = MainToolbarDockPosition.Left, defaultDockIndex = 102)]
		private static MainToolbarElement Left2() => Slot(false, 2);

		[MainToolbarElement(BeltToolbar.LeftPath + "4", defaultDockPosition = MainToolbarDockPosition.Left, defaultDockIndex = 103)]
		private static MainToolbarElement Left3() => Slot(false, 3);

		[MainToolbarElement(BeltToolbar.LeftPath + "5", defaultDockPosition = MainToolbarDockPosition.Left, defaultDockIndex = 104)]
		private static MainToolbarElement Left4() => Slot(false, 4);

		[MainToolbarElement(BeltToolbar.LeftPath + "6", defaultDockPosition = MainToolbarDockPosition.Left, defaultDockIndex = 105)]
		private static MainToolbarElement Left5() => Slot(false, 5);

		[MainToolbarElement(BeltToolbar.RightPath + "1", defaultDockPosition = MainToolbarDockPosition.Right, defaultDockIndex = 100)]
		private static MainToolbarElement Right0() => Slot(true, 0);

		[MainToolbarElement(BeltToolbar.RightPath + "2", defaultDockPosition = MainToolbarDockPosition.Right, defaultDockIndex = 101)]
		private static MainToolbarElement Right1() => Slot(true, 1);

		[MainToolbarElement(BeltToolbar.RightPath + "3", defaultDockPosition = MainToolbarDockPosition.Right, defaultDockIndex = 102)]
		private static MainToolbarElement Right2() => Slot(true, 2);

		[MainToolbarElement(BeltToolbar.RightPath + "4", defaultDockPosition = MainToolbarDockPosition.Right, defaultDockIndex = 103)]
		private static MainToolbarElement Right3() => Slot(true, 3);

		[MainToolbarElement(BeltToolbar.RightPath + "5", defaultDockPosition = MainToolbarDockPosition.Right, defaultDockIndex = 104)]
		private static MainToolbarElement Right4() => Slot(true, 4);

		[MainToolbarElement(BeltToolbar.RightPath + "6", defaultDockPosition = MainToolbarDockPosition.Right, defaultDockIndex = 105)]
		private static MainToolbarElement Right5() => Slot(true, 5);

		private static MainToolbarElement Slot(bool onTheRight, int index)
		{
			var item = BeltToolbar.At(onTheRight, index);
			if (item != null)
			{
				return item.Build();
			}

			// Nothing has been put here. The slot still has to hand something back, so it hands
			// back something that is never shown.
			return new MainToolbarButton(new MainToolbarContent(string.Empty), () => { }) { displayed = false };
		}
	}
}
