/*
  This file is part of PPather.

    PPather is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    PPather is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with PPather.  If not, see <http://www.gnu.org/licenses/>.

*/
using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

using Glider.Common.Objects;
using Pather;

namespace Pather.Helpers.UI
{
	public class CharacterFrame
	{
		private static string cFRAME = "PaperDollFrame";
        private static string cSLOT = "Character";
        private static string cBUTTON = "CharacterMicroButton";
        public static List<string> text = new List<string>();
        public static int ttcount = 0;

        public static List<string> GetTooltip(string slot)
        {
            text.Clear();
            GInterfaceObject SlotObj = GContext.Main.Interface.GetByName(cSLOT + slot);
            if (SlotObj != null)
            {
                GContext.Main.EnableCursorHook();
                SlotObj.Hover();
                Thread.Sleep(100);
                GInterfaceObject ToolTip = GContext.Main.Interface.GetByName("GameTooltip");
                if (ToolTip != null)
                //    RecursiveToolTip(ToolTip);
                {
                    foreach (GInterfaceObject child in ToolTip.Children)
                    {
                        if (child.ToString().Contains("GameTooltipText"))
                        text.Add(child.LabelText);
                    }
                }
                GContext.Main.DisableCursorHook();
            }          
            return text;
        }

        public static void RecursiveToolTip(GInterfaceObject gobj)
        {
            Thread.Sleep(150);
            if (gobj != null)
            {
                PPather.WriteLine("Tooltip{0}\t=>\t{1}", ttcount, gobj.LabelText);
                ttcount++;
                if(gobj.LabelText != null)
                    text.Add(gobj.LabelText);

                GInterfaceObject[] children = null;

                try
                {
                    children = gobj.Children;
                }
                catch
                { }

                if (children != null && children.Length > 0)
                {
                    foreach (GInterfaceObject child in children)
                    {
                        if (child != null)
                        {
                            if (child.LabelText != null)
                            {
                                PPather.WriteLine("Tooltip{0}\t=>\t{1}", ttcount, child.LabelText);
                                text.Add(child.LabelText);
                                ttcount++;
                            }
                            RecursiveToolTip(child);
                        }
                    }
                }
            }
        }


        public static GInterfaceObject GetCharacterSlot(string slot)
        {
            return GContext.Main.Interface.GetByName(cSLOT + slot);
        }

        public static string CleanToolTip(List<string> tooltip)
        {
            string cleaned = "";
            int tipnr = 0;
            foreach (string tip in tooltip)
            {
                if (tip.Length < 2) continue;
                if (tip.Contains("(read failed)")) continue;
                if (tip.Contains("(no text)")) continue;
                cleaned += tip;
                if (tipnr < tooltip.Count)
                    cleaned += "|";
                tipnr++;
            }
            return cleaned;
        }

		// The talent frame doesn't actually exist in memory
		// until it's been opened at least once
		private static bool FrameExists()
		{
			if (GetFrame() == null)
				return false;
			return true;
		}

		// external frame opening
		public static void ShowFrame()
		{
			if (!GetFrame().IsVisible)
				ToggleFrame();
		}

		// external frame closing
		public static void HideFrame()
		{
			if (GetFrame().IsVisible)
				ToggleFrame();
		}

		public static bool IsVisible()
		{
			if (CharacterFrame.FrameExists())
				return CharacterFrame.GetFrame().IsVisible;
			return false;
		}

		// Internal frame handling
		private static GInterfaceObject GetFrame()
		{
			return GContext.Main.Interface.GetByName(cFRAME);
		}

		// Internal frame handling
		private static void ToggleFrame()
		{
			GInterfaceObject micro = GContext.Main.Interface.GetByName(cBUTTON);

			if (micro.IsVisible)
			{
				Functions.Click(micro, false);
				Thread.Sleep(500);
			}
		}
	}
}
