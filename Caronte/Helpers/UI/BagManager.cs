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
	public class BagManager
	{
		/*
		  This is designed to just keep one bag open at a time. 
		  Will make it more robust. 

		  Bag buttons: 
		  MainMenuBarBackpackButton
		  CharacterBag0Slot
		  CharacterBag1Slot
		  CharacterBag2Slot
		  CharacterBag3Slot

		 */
		private string[] BagButtonNames = new string[]
        {
            "MainMenuBarBackpackButton",
            "CharacterBag0Slot",
            "CharacterBag1Slot",
            "CharacterBag2Slot",
            "CharacterBag3Slot"
        };

	    private string[] BagFrameNames = new string[]
	    {
             "1",
             "2",
             "3",
             "4",
             "5"
	    };

	    private int tries;
		private int CurrentOpenBag = -1;
		private class BagItem
		{
			public int bag;
			public int slot;
			public GItem item;
			public BagItem(int b, int s, GItem i)
			{
				bag = b;
				slot = s;
				item = i;
			}
		}

        public class Bag
        {
            public int bag;
            public int count;
            public GItem[] I;
            public Bag(int b, GItem[] i)
            {
                bag = b;
                I = i;
            }
            public void Add(GItem i, int index)
            {
                I[index] = i;
                count++;
            }
        }


		private GItem[] Items;
		private BagItem[] BagItems;
        public Bag[] Bags;
        
		/*public BagManager() {
			// make sure all bags are closed

			CloseAllBags();
			UpdateItems();

			// check all contents
		}*/

        public void UpdateItemsSorted()
        {
            //PPather.WriteLine("UpdateItemsSorted()");
            int[] count = new int[5] {0,0,0,0,0};
            UpdateItems();
            for (int i = 0; i < BagItems.Length; i++)
            {
                BagItem it = BagItems[i];
                //PPather.WriteLine("UpdateItemsSorted: Adding {0} to Bag {1} (count = {2})", it.item.Name, it.bag, count[it.bag] + 1);
                count[it.bag]++;
            }

            int bags = GPlayerSelf.Me.Bags.Length+1;

            long[] AllBags = GPlayerSelf.Me.Bags;
            for (int bagNr = 0; bagNr < 5; bagNr++)
			{
				int SlotCount;
				if (bagNr == 0)
					SlotCount = GContext.Main.Me.SlotCount;
				else
                {
					GContainer bag = (GContainer)GObjectList.FindObject(AllBags[bagNr - 1]);
					if (bag != null)
						SlotCount = bag.SlotCount;
					else
						SlotCount = 0;
				}
                count[bagNr] = SlotCount;
            }

            //PPather.WriteLine("UpdateItemsSorted: Creating new Bag array of size {0}", bags);
            Bags = new Bag[bags];
            for(int i=0; i<bags; i++)
            {
                //PPather.WriteLine("UpdateItemsSorted: Creating new GItem array of size {0} to Bag {1}", count[i], i);
                GItem[] gits = new GItem[count[i]];
                //PPather.WriteLine("UpdateItemsSorted: Adding gits to Bag {0}", i);
                Bags[i] = new Bag(i,gits);
            }

            for (int i = 0; i < BagItems.Length; i++)
            {
                BagItem it = BagItems[i];
                //PPather.WriteLine("UpdateItemsSorted: Bag[{0}].Add({1}, {2})", it.bag, it.item.Name, it.slot-1);
                Bags[it.bag].Add(it.item, it.slot-1);
            }
        }

		public void UpdateItems()
		{
			List<GItem> ItemList = new List<GItem>();
			List<BagItem> BagItemList = new List<BagItem>();
			long[] AllBags = GPlayerSelf.Me.Bags;

			long[] Contents;
			int SlotCount;

			for (int bag = 0; bag <= 4; bag++)
			{
				SlotCount = 0;
				if (bag == 0)
				{
					Contents = GContext.Main.Me.BagContents;
					SlotCount = GContext.Main.Me.SlotCount;
				}
				else
				{
					GContainer container = (GContainer)GObjectList.FindObject(AllBags[bag - 1]);
					if (container != null)
					{
						Contents = container.BagContents;
						SlotCount = container.SlotCount;
					}
					else
						Contents = null;
				}
				if (Contents != null)
				{
					for (int i = 0; i < Contents.Length; i++)
					{
						GItem CurItem = (GItem)GObjectList.FindObject(Contents[i]);
						if (CurItem != null)
						{
							ItemList.Add(CurItem);
							BagItem bi = new BagItem(bag, SlotCount - i, CurItem);
							BagItemList.Add(bi);
						}
					}
				}
			}


			Items = ItemList.ToArray();
			BagItems = BagItemList.ToArray();
		}

		public void CloseAllBags()
		{
            //foreach (string BagKey in BagButtonNames)
            //{
            //    GInterfaceObject CurBag = GContext.Main.Interface.GetByName(BagKey);
            //    if (CurBag != null)
            //    {
            //        if (CurBag.IsFiring)
            //        {
            //            Functions.Click(CurBag, false);
            //            Thread.Sleep(300);
            //            PPather.WriteLine("BagManager: Close bag " + BagKey);
            //        }
            //    }
            //}
            foreach(string BagFrame in BagFrameNames)
            {
                if(GContext.Main.Interface.GetByName("ContainerFrame" + BagFrame) != null)
                {
                    if(GContext.Main.Interface.GetByName("ContainerFrame" + BagFrame).IsVisible)
                    {
                        Functions.Click(GContext.Main.Interface.GetByName("ContainerFrame" + BagFrame + "CloseButton" ));
                        //PPather.Writeline(LOG_CATEGORY.DEBUG, "BagManager: Closed bag {0}",BagFrame);
                    }
                }
            }
		}

		public void OpenBag(int nr)
		{

			if (nr == CurrentOpenBag)
			{
				// verify it is open
				GInterfaceObject CurBag = GContext.Main.Interface.GetByName(BagButtonNames[nr]);
				if (CurBag == null || !CurBag.IsFiring)
				{
					PPather.WriteLine(LOG_CATEGORY.INFORMATION,"BagManager: Something is very fishy with the bags. Close them all and try again");
					CloseAllBags();
				    CurrentOpenBag = -1;
                    Thread.Sleep(250); // allow a short wait after closing the bag and opening the new one.
				}
			}

			if (nr != CurrentOpenBag)
			{
				CloseAllBags();
                Thread.Sleep(250);

				GInterfaceObject CurBag = GContext.Main.Interface.GetByName(BagButtonNames[nr]);
				if (CurBag != null)
				{
					Functions.Click(CurBag, false);
					//Thread.Sleep(300);
					CurrentOpenBag = nr;
                    Thread.Sleep(125);
					//PPather.WriteLine("BagManager: Open bag " + BagButtonNames[nr]);
				}
			}
		}

		//BagNr is 0-4, ItemNr is 1-BagSlots
		public bool ClickItem(int BagNr, int ItemNr, GItem item, bool RightMouse)
		{
          ClickItem:
            if(tries >= 4)
            {
                tries = 1;
                PPather.WriteLine(LOG_CATEGORY.INFORMATION, "BagManager: Tried to click {0} 3 times now and it didn't work. Aborting :-(",item.Name);
                return false;
            }
			OpenBag(BagNr);
		    Thread.Sleep(100);
			//PPather.WriteLine("BagManager: Click item " + BagNr + " " + ItemNr);
			String itemStr = "ContainerFrame1Item" + ItemNr;
			GInterfaceObject ItemObj = GContext.Main.Interface.GetByName(itemStr);
			if (ItemObj != null)
			{
                GContext.Main.EnableCursorHook();
                ItemObj.Hover();
                Thread.Sleep(100);
                GInterfaceObject ToolTip = GContext.Main.Interface.GetByName("GameTooltip");
                if(ToolTip != null)
                {
                    GInterfaceObject child = ToolTip.Children[0];
                    
                    // PPather.WriteLine(child.ToString() + " with labeltext " + child.LabelText);
                    if (child.ToString().Contains("GameTooltipTextLeft1"))
                    {
                        if (child.LabelText != item.Name)
                        {
                            PPather.WriteLine(LOG_CATEGORY.INFORMATION, "BagManger: Tried to click item {0} but found {1}. Will try again...",item.Name, child.LabelText);
                            Thread.Sleep(100);
                            GContext.Main.DisableCursorHook();
                            CloseAllBags();
                            CurrentOpenBag = -1;
                            // PPather.WriteLine("BagManager: Retry number " + tries);
                            tries++;
                            Thread.Sleep(150);
                            goto ClickItem;
                        }
                    }
                }
                GContext.Main.DisableCursorHook();
			    Functions.Click(ItemObj, RightMouse);
			    return true;
			}
		    return false;
		}

       	public bool ClickItem(GItem item, bool RightMouse)
		{
			UpdateItems();

			// search for it
			for (int i = 0; i < BagItems.Length; i++)
			{
				BagItem it = BagItems[i];
				if (it.item == item)
				{
					return ClickItem(it.bag, it.slot, item, RightMouse);
				}
			}
       	    return false;
		}

        public GInterfaceObject GetItem(int BagNr, int ItemNr)
        {
            //PPather.WriteLine("BagManager: Opening bag {0}",BagNr);
            OpenBag(BagNr);
			//PPather.WriteLine("BagManager: Click item " + BagNr + " " + ItemNr);
			String itemStr = "ContainerFrame1Item" + ItemNr;
			GInterfaceObject ItemObj = GContext.Main.Interface.GetByName(itemStr);
            return ItemObj;
        }

        public GInterfaceObject GetItem(GItem item)
        {
            UpdateItems();
            //PPather.WriteLine("you want {0}",item.Name);
			// search for it
			for (int i = 0; i < BagItems.Length; i++)
			{
				BagItem it = BagItems[i];
				if (it.item == item)
				{
                    //PPather.WriteLine("Getting more info");
                    //PPather.WriteLine("Bag: {0}, Slot: {1}",it.bag,it.slot);
					GInterfaceObject obj = GetItem(it.bag, it.slot);
                    return obj;
				}
			}
            return null;
        }

        public static List<string> GetTooltip(GItem item)
        {
            BagManager bm = new BagManager();
            List<string> text = new List<string>();
            GInterfaceObject ItemObj = bm.GetItem(item);
            if (ItemObj != null)
            {
                GContext.Main.EnableCursorHook();
                ItemObj.Hover();
                Thread.Sleep(50);
                GInterfaceObject ToolTip = GContext.Main.Interface.GetByName("GameTooltip");
                if (ToolTip != null)
                {
                    foreach (GInterfaceObject child in ToolTip.Children)
                    {
                        if (child.ToString().Contains("GameTooltipText"))
                            text.Add(child.LabelText);
                    }
                }
                GContext.Main.DisableCursorHook();
            }
            bm.CloseAllBags();
            return text;
        }

		public void CastSpellItem(GItem item)
		{
			ClickItem(item, true);
			Thread.Sleep(500);
			while (GContext.Main.Me.IsCasting)
			{
				Thread.Sleep(100);
			}
		}

		public GItem[] GetAllItems()
		{
			UpdateItems();
			return Items;
		}
	}
}
