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
using System.Text;
using Glider.Common.Objects;
using Pather.Helpers.UI;
using System.Threading;
using System.Text.RegularExpressions;

namespace Pather.Helpers
{
	class Inventory
	{
        private class RankedBag : IComparable
        {
            public int slots = 0; // SlotCount
            public int location = -1; // 0 = equipped, 1 = inventory
            public GContainer bag;
            public GItem item;
            public string slotname;

            public RankedBag(int l, int s, GContainer b, GItem i, string sname)
            {
                location = l;
                slots = s;
                bag = b;
                item = i;
                slotname = sname;
            }

            public int CompareTo(object o)
            {
                RankedBag b = (RankedBag)o;
                if (slots != b.slots)
                {
                    return b.slots - slots;
                }
                else
                    return 0;
            }
        }

		private static Dictionary<string, int> curItemsCache = null;
		// change this to change the cache time, not sure what would be optimal
		private static GSpellTimer curItemCacheTimer = new GSpellTimer(30000);

		public static void ReadyItemCacheTimer()
		{
			curItemCacheTimer.ForceReady();
		}

		/// <summary>
		/// Retrieves the quantities of all distinct items in all of your bags using
		/// cached data if possible.
		/// </summary>
		/// <returns>
		/// A Dictionary with keys corresponding to the names
		/// of each distinct item in your inventory and values corresponding
		/// to the number of that item across all of your bags.
		/// </returns>
		public static Dictionary<string, int> CreateItemCount()
		{
			return CreateItemCount(true);
		}

		/// <summary>
		/// Retrieves the quantities of all distinct items in all of your bags.
		/// </summary>
		/// <param name="useCache">Whether to use cached data, if possible</param>
		/// <returns>
		/// A Dictionary with keys corresponding to the names
		/// of each distinct item in your inventory and values corresponding
		/// to the number of that item across all of your bags.
		/// </returns>
		public static Dictionary<string, int> CreateItemCount(bool useCache)
		{
			// only check it every 30 seconds to try to reduce
			// overhead
			if (curItemsCache != null && !curItemCacheTimer.IsReady && useCache)
				return curItemsCache;

			Dictionary<string, int> items = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
			long[] AllBags = GPlayerSelf.Me.Bags;

			for (int bagNr = 0; bagNr < 5; bagNr++)
			{
				long[] Contents;
				int SlotCount;
				if (bagNr == 0)
				{
					Contents = GContext.Main.Me.BagContents;
					SlotCount = GContext.Main.Me.SlotCount;
				}
				else
				{
					GContainer bag = (GContainer)GObjectList.FindObject(AllBags[bagNr - 1]);
					if (bag != null)
					{
						Contents = bag.BagContents;
						SlotCount = bag.SlotCount;
					}
					else
					{
						SlotCount = 0;
						Contents = null;
					}
				}
				for (int i = 0; i < SlotCount; i++)
				{
					if (Contents[i] == 0)
						continue;
					GItem CurItem = (GItem)GObjectList.FindObject(Contents[i]);
					if (CurItem != null)
					{
						string ItemName = CurItem.Name;
						int ItemCount = CurItem.StackSize;
						int OldCount = 0;
						items.TryGetValue(ItemName, out OldCount);
						items.Remove(ItemName);
						items.Add(ItemName, OldCount + ItemCount);
					}
				}
			}

			curItemCacheTimer.Reset();
			curItemsCache = items;
			return curItemsCache;
		}

		public static int GetItemCount(string name)
		{
			return GetItemCount(name, true);
		}

		public static int GetItemCount(string name, bool useCache)
		{
			name = name.ToLower();
			int ret = 0;

			CreateItemCount(useCache).TryGetValue(name, out ret);

			return ret;
		}

        public static bool IsEquippedBag(long GUID)
        {
            long[] AllBags = GPlayerSelf.Me.Bags;
            foreach (long BagGUID in AllBags)
            {
                if (BagGUID == GUID)
                    return true;
            }
            return false;
        }

        public static bool HasFreeBagSlot()
        {
            int count = 0;
            long[] AllBags = GPlayerSelf.Me.Bags;
            foreach (long CurrentBag in AllBags)
            {
                GContainer bag = (GContainer)GObjectList.FindObject(CurrentBag);
                if (bag != null)
                    count++;
            }

            if (count == 4)
                return false;

            return true;
        }

        public static bool EquipNewBag()
        {
            if (HasFreeBagSlot())
            {
                GItem[] Items = GObjectList.GetItems();
                foreach (GItem Item in Items)
                {
                    if (Item.Type.ToString() == "Container" && !IsEquippedBag(Item.GUID))
                    {
                        GContainer bag = (GContainer)GObjectList.FindObject(Item.GUID);
                        if (bag != null)
                        {
                            BagManager bm = new BagManager();
                            bm.ClickItem(Item, true);
                            for (int p = 1; p <= 4; p++)
                            {
                                if (Popup.IsVisible(p))
                                {
                                    String text = Popup.GetText(p);
                                    PPather.WriteLine("Inventory: Got a popup ('" + text + "')");
                                    if (text.Contains("will bind it to you"))
                                    {
                                        Popup.ClickButton(p, 1);
                                    }
                                    else
                                    {
                                        Popup.ClickButton(p, 2);
                                    }
                                }
                            }
                            bm.CloseAllBags();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool EquipNewBag(GItem Item)
        {
            BagManager bm = new BagManager();
            GContainer bag = (GContainer)GObjectList.FindObject(Item.GUID);
            if (bag != null)
            {
                bm.ClickItem(Item, true);
                for (int p = 1; p <= 4; p++)
                {
                    if (Popup.IsVisible(p))
                    {
                        String text = Popup.GetText(p);
                        PPather.WriteLine("Inventory: Got a popup ('" + text + "')");
                        if (text.Contains("will bind it to you"))
                        {
                            Popup.ClickButton(p, 1);
                        }
                        else
                        {
                            Popup.ClickButton(p, 2);
                        }
                    }
                }
                bm.CloseAllBags();
                return true;
            }
            bm.CloseAllBags();
            return false;
        }

        public static bool HasBetterBag()
        {
            GItem[] Items = GObjectList.GetItems();
            foreach (GItem Item in Items)
            {
                if (Item.Type.ToString() == "Container" && !IsEquippedBag(Item.GUID))
                {
                    GContainer bag = (GContainer)GObjectList.FindObject(Item.GUID);
                    if (bag != null)
                    {
                        long[] AllBags = GPlayerSelf.Me.Bags;
                        foreach (long CurrentBag in AllBags)
                        {
                            GContainer cbag = (GContainer)GObjectList.FindObject(CurrentBag);
                            if (cbag != null)
                            {
                                if (cbag.SlotCount < bag.SlotCount && !cbag.Name.ToString().ToLower().Contains("ammo") && !cbag.Name.ToString().ToLower().Contains("quiver"))
                                    return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static string GetBagSlotName(GItem Item)
        {
            long[] AllBags = GPlayerSelf.Me.Bags;
            for (int i = 0; i < AllBags.Length; i++)
            {
                if (Item.GUID == AllBags[i])
                    return "CharacterBag" + i + "Slot";
            }
            return null;
        }

        public static bool EquipBetterBag(GItem Item, string BagSlotName)
        {
            GContainer bag = (GContainer)GObjectList.FindObject(Item.GUID);
            if (bag != null)
            {
                long[] AllBags = GPlayerSelf.Me.Bags;
                for (int i = 0; i < AllBags.Length; i++)
                {
                    GContainer cbag = (GContainer)GObjectList.FindObject(AllBags[i]);
                    if (cbag != null)
                    {

                        GInterfaceObject OldBag = GContext.Main.Interface.GetByName(BagSlotName);
                        BagManager bm = new BagManager();
                        bm.ClickItem(Item, false);
                        Functions.Click(OldBag, false);
                        for (int p = 1; p <= 4; p++)
                        {
                            if (Popup.IsVisible(p))
                            {
                                String text = Popup.GetText(p);
                                PPather.WriteLine("Inventory: Got a popup ('" + text + "')");
                                if (text.Contains("will bind it to you"))
                                {
                                    Popup.ClickButton(p, 1);
                                }
                                else
                                {
                                    Popup.ClickButton(p, 2);
                                }
                            }
                        }
                        bm.CloseAllBags();
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool EquipBetterBag(GItem Item)
        {
            GContainer bag = (GContainer)GObjectList.FindObject(Item.GUID);
            if (bag != null)
            {
                long[] AllBags = GPlayerSelf.Me.Bags;
                for (int i = 0; i < AllBags.Length; i++)
                {
                    GContainer cbag = (GContainer)GObjectList.FindObject(AllBags[i]);
                    if (cbag != null)
                    {
                        if (cbag.SlotCount < bag.SlotCount && !cbag.Name.ToString().ToLower().Contains("ammo") && !cbag.Name.ToString().ToLower().Contains("quiver"))
                        {
                            GInterfaceObject OldBag = GContext.Main.Interface.GetByName("CharacterBag" + i + "Slot");
                            BagManager bm = new BagManager();
                            bm.ClickItem(Item, false);
                            Functions.Click(OldBag, false);
                            for (int p = 1; p <= 4; p++)
                            {
                                if (Popup.IsVisible(p))
                                {
                                    String text = Popup.GetText(p);
                                    PPather.WriteLine("Inventory: Got a popup ('" + text + "')");
                                    if (text.Contains("will bind it to you"))
                                    {
                                        Popup.ClickButton(p, 1);
                                    }
                                    else
                                    {
                                        Popup.ClickButton(p, 2);
                                    }
                                }
                            }
                            bm.CloseAllBags();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool EquipBetterBag()
        {
            GItem[] Items = GObjectList.GetItems();
            foreach (GItem Item in Items)
            {
                if (Item.Type.ToString() == "Container" && !IsEquippedBag(Item.GUID))
                {
                    GContainer bag = (GContainer)GObjectList.FindObject(Item.GUID);
                    if (bag != null)
                    {
                        long[] AllBags = GPlayerSelf.Me.Bags;
                        for (int i = 0; i < AllBags.Length; i++)
                        {
                            GContainer cbag = (GContainer)GObjectList.FindObject(AllBags[i]);
                            if (cbag != null)
                            {
                                if (cbag.SlotCount < bag.SlotCount && !cbag.Name.ToString().ToLower().Contains("ammo") && !cbag.Name.ToString().ToLower().Contains("quiver"))
                                {
                                    GInterfaceObject OldBag = GContext.Main.Interface.GetByName("CharacterBag" + i + "Slot");
                                    BagManager bm = new BagManager();
                                    bm.ClickItem(Item, false);
                                    Functions.Click(OldBag, false);
                                    for (int p = 1; p <= 4; p++)
                                    {
                                        if (Popup.IsVisible(p))
                                        {
                                            String text = Popup.GetText(p);
                                            PPather.WriteLine("Inventory: Got a popup ('" + text + "')");
                                            if (text.Contains("will bind it to you"))
                                            {
                                                Popup.ClickButton(p, 1);
                                            }
                                            else
                                            {
                                                Popup.ClickButton(p, 2);
                                            }
                                        }
                                    }
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        public static GItem GetItem(string name)
        {
            GItem[] Items = GObjectList.GetItems();
            foreach (GItem Item in Items)
                if (Item.Name.Equals(name)) return Item;
            return null;
        }

        public static bool Equip(EasyItem E, bool EquipBOE, string slot)
        {
            PPather.WriteLine(LOG_CATEGORY.INFORMATION, "Equip: Attempting to equip {0} [{1}]", E.RealName, slot);
            bool placed = false;
            BagManager bm = new BagManager();
            CharacterFrame.ShowFrame();
            GInterfaceObject BagItemObject = null;
            GInterfaceObject TargetSlotObject;

            TargetSlotObject = GContext.Main.Interface.GetByName("Character" + slot);
            //PPather.WriteLine(LOG_CATEGORY.DEBUG, "Equip: TargetSlotObject = {0}", TargetSlotObject.LabelText);
  
            GItem[] Items = bm.GetAllItems();
            foreach (GItem Item in Items)
            {
                if (Item.GUID == E.GUID)
                {
                    Thread.Sleep(500);
                    BagItemObject = bm.GetItem(Item);
                    //PPather.WriteLine(LOG_CATEGORY.DEBUG, "Equip: BagItemObject = {0}", BagItemObject.LabelText);
                    //PPather.WriteLine("item: " + BagItemObject.ToString());
                    Functions.Drag(BagItemObject, TargetSlotObject);
                    placed = true;
                    Thread.Sleep(500);
                    string BOEButton = "StaticPopup1Button2";
                    if (EquipBOE)
                        BOEButton = "StaticPopup1Button1";

                    GInterfaceObject ButtonObject = GContext.Main.Interface.GetByName(BOEButton);
                    if (ButtonObject != null && ButtonObject.IsVisible)
                    {
                        if (!EquipBOE) placed = false;
                        GContext.Main.EnableCursorHook();
                        ButtonObject.Hover();
                        ButtonObject.ClickMouse(false);
                        GContext.Main.DisableCursorHook();
                        GContext.Main.ClearTarget();
                    }
                }
            }

            CharacterFrame.HideFrame();

            /* put the old item in bags */
            if (GContext.Main.Interface.CursorItemType != GCursorItemTypes.None)
                Functions.Click(BagItemObject);

            bm.UpdateItems();
            bm.CloseAllBags();

            if (placed)
            {
                Character.ReplaceCurrentlyEquipped(E, slot);
                return true;
            }

            return false;
        }


        public static bool Equip(EasyItem E, bool EquipBOE)
        {
            PPather.WriteLine(LOG_CATEGORY.INFORMATION,"Equip: Attempting to equip {0}",E.RealName);
            BagManager bm = new BagManager();

            //PPather.WriteLine(String.Format("AutoEquipTask: Equipping {0} (GITem name: {1}", E.RealName, E.GItem.Name));
            if (!bm.ClickItem(E.GItem, true))
            {
                PPather.WriteLine(String.Format("Equip: BagManger failed to click item ({0}). Aborting...",E.RealName));
                GContext.Main.Debug(String.Format("Equip: BagManger failed to click item ({0}). Aborting...",E.RealName));
                return false;
            }
            Thread.Sleep(1000);
            string BOEButton = "StaticPopup1Button2";
            if (EquipBOE)
                BOEButton = "StaticPopup1Button1";
            GInterfaceObject AcceptBOE = GContext.Main.Interface.GetByName(BOEButton);
            if (AcceptBOE != null && AcceptBOE.IsVisible)
            {
                GContext.Main.EnableCursorHook();
                AcceptBOE.Hover();
                AcceptBOE.ClickMouse(false);
                GContext.Main.DisableCursorHook();
                GContext.Main.ClearTarget();
                if (EquipBOE)
                {
                    PPather.WriteLine("Equip: Accepted BOE for " + E.RealName);
                    GContext.Main.Debug("Equip: Accepted BOE for " + E.RealName);
                    bm.CloseAllBags();
                    bm.UpdateItems();
                    //Character.SetCurrent(E.Item.Slot, E, true);
                    Character.ReplaceCurrentlyEquipped(E);
                    return true;
                }
                else
                {
                    PPather.WriteLine("Equip: Declined BOE for " + E.RealName);
                    GContext.Main.Debug("Equip: Declined BOE for " + E.RealName);
                    bm.CloseAllBags();
                    bm.UpdateItems();
                    return false;
                }
            }
            Character.ReplaceCurrentlyEquipped(E);
            bm.UpdateItems();
            bm.CloseAllBags();
            return true;
        }

        public static void EquipBag(GItem bag, bool NewBag, string BagSlotName)
        {
            if (NewBag)
            {
                if (Inventory.EquipNewBag(bag))
                {
                    PPather.WriteLine(String.Format("AutoEquip: Equipped New Bag ({0})", bag.Name));
                    GContext.Main.Debug(String.Format("AutoEquip: Equipped New Bag ({0})", bag.Name));
                    }
            }
            else
                if (Inventory.EquipBetterBag(bag, BagSlotName))
                {
                    PPather.WriteLine(String.Format("AutoEquip: Equipped Better Bag ({0})", bag.Name));
                    GContext.Main.Debug(String.Format("AutoEquip: Equipped Better Bag ({0})", bag.Name));
                    }
        }

        public static void CompareAndEquipBags(EasyItem[] C)
        {
            RankedBag[] RankedBags = new RankedBag[C.Length + 4];
            //PPather.WriteLine(String.Format("AutoEquip: RankedBags.Length = {0}", RankedBags.Length));
            //PPather.WriteLine(String.Format("AutoEquip: containers.Count = {0}", C.Length));
            //PPather.WriteLine(String.Format("AutoEquip: GPlayerSelf.Me.Bags.Length = {0}", 4));

            /* add bags found in inventory */
            GItem[] Items = GObjectList.GetItems();
            List<long> added = new List<long>();
            int nr = 0;
            for (nr = 0; nr < C.Length; nr++)
            {
                if (!Inventory.IsEquippedBag(C[nr].GUID) && !added.Contains(C[nr].GUID))
                {
                    GContainer bag = (GContainer)GObjectList.FindObject(C[nr].GUID);
                    string SlotName = Inventory.GetBagSlotName(C[nr].GItem);
                    RankedBag b = new RankedBag(1, bag.SlotCount, bag, C[nr].GItem, SlotName);
                    RankedBags[nr] = b;
                    //PPather.WriteLine("AutoEquip: (inventory) RankedBags[{0}] = {1} (SlotCount={2}, GUID={3})", nr, C[nr].Item.Name, bag.SlotCount, C[nr].GUID);
                    added.Add(C[nr].GUID);
                }
            }

            /* add already equipped bags */
            nr = C.Length;
            foreach (GItem Item in Items)
            {
                if (Item.Type.ToString() == "Container" && Inventory.IsEquippedBag(Item.GUID) && !added.Contains(Item.GUID))
                {
                    GContainer bag = (GContainer)GObjectList.FindObject(Item.GUID);
                    string SlotName = Inventory.GetBagSlotName(Item);
                    RankedBag b = new RankedBag(0, bag.SlotCount, bag, Item, SlotName);
                    RankedBags[nr] = b;
                    //PPather.WriteLine("AutoEquip: (eqiupped) RankedBags[{0}] = {1} (SlotCount={2}, GUID={3})", nr, Item.Name, bag.SlotCount, Item.GUID);
                    added.Add(Item.GUID);
                    nr++;
                }
            }

            /* add the empty slots */
            int freeslots = 0;
            while (nr < (C.Length + 4))
            {
                RankedBag b = new RankedBag(0, 0, null, null, null);
                RankedBags[nr] = b;
                nr++;
                freeslots++;
            }

            /* sort bags on slotcount */
            Array.Sort(RankedBags);

            /* DEBUG print of ranks */
            //for (int k = 0; k < RankedBags.Length; k++)
            //{
            //    RankedBag rb = RankedBags[k];
            //    if (rb != null)
            //    {
            //        string name = "null";
            //        if (rb.item != null) name = rb.item.Name;
            //        PPather.WriteLine(String.Format("AutoEquip: Rank {0} => {1}({2})", k, name, rb.slots));
            //    }
            //}

            /* get the bag slot names to replace (reverse sort) */
            List<string> slots = new List<string>();
            for (int j = RankedBags.Length - 1; j >= 0; j--)
            {
                RankedBag r = RankedBags[j];
                if (r == null) continue;
                if (r.slotname == null) continue;
                string name = "null";
                if (r.item != null) name = r.item.Name;
                slots.Add(r.slotname);
                //PPather.WriteLine("AutoEquip: Rank {0} of BagSlot to replace is {1} ({2})", slots.Count, r.slotname, name);
            }
            String[] BagSlots = slots.ToArray();
            int ReplaceSlotIndex = 0;

            /* go through RankedBags and make sure the top 4 ranked bags are equipped */
            int index = 0; int equipped = 0;
            while (index < 4)
            {
                //PPather.WriteLine("AutoEquip: index={0}, equipped={1}, freeslots={2}", index, equipped, freeslots);
                RankedBag r = RankedBags[index];
                if (r != null)
                {
                    string name = "null";
                    if (r.item != null) name = r.item.Name;
                    if (r.location == 0)
                    {
                        //PPather.WriteLine("AutoEquip: {0} rank[{1}] is equipped", name, index);
                    }
                    else if (freeslots > 0)
                    {
                        //PPather.WriteLine("AutoEquip: {0} rank[{1}] is new bag", name, index);
                        EquipBag(r.item, true, null);
                        equipped++;
                        freeslots--;
                    }
                    else
                    {
                        //PPather.WriteLine("AutoEquip: {0} rank[{1}] is better bag", name, index);
                        EquipBag(r.item, false, BagSlots[ReplaceSlotIndex]);
                        equipped++;
                        ReplaceSlotIndex++;
                    }
                }
                index++;
            }
        }
        public static void CheckForScrollsAndElixirs()
        {
            /* check inventory for srolls and elixirs */
            Regex elixirPattern = new Regex("Elixir of.*");
            Regex scrollPattern = new Regex("Scroll of.*");
            foreach (KeyValuePair<long, EasyItem> e in Character.InventoryItems)
            {
                int myLevel = GPlayerSelf.Me.Level;
                int itemLevel = Convert.ToInt32(e.Value.Item.Required);
                //PPather.WriteLine("AutoEquip: myLevel = {0} , itemLevel = {1} [{2}]", myLevel, itemLevel, e.Value.Item.Name);
                if (itemLevel <= myLevel)
                {
                    Match m = scrollPattern.Match(e.Value.Item.Name);
                    if (m.Success)
                    {
                        BagManager bm = new BagManager();
                        GItem[] bagItems = bm.GetAllItems();
                        foreach (GItem bagItem in bagItems)
                        {
                            if (bagItem.Name.Equals(e.Value.Item.Name))
                            {
                                PPather.WriteLine("Inventory: using " + bagItem.Name);
                                GContext.Main.Debug("Inventory: using " + bagItem.Name);
                                if (!bm.ClickItem(bagItem, true))
                                {
                                    PPather.WriteLine("Inventory: BagManger failed to click the item. Aborting...");
                                    break;
                                }
                                Thread.Sleep(500);
                                break;
                            }
                        }
                        bm.CloseAllBags();
                        bm.UpdateItems();
                        continue;
                    }
                    m = elixirPattern.Match(e.Value.Item.Name);
                    if (m.Success)
                    {
                        BagManager bm = new BagManager();
                        GItem[] bagItems = bm.GetAllItems();
                        foreach (GItem bagItem in bagItems)
                        {
                            if (bagItem.Name.Equals(e.Value.Item.Name))
                            {
                                PPather.WriteLine("Inventory: using " + bagItem.Name);
                                GContext.Main.Debug("Inventory: using " + bagItem.Name);
                                if (!bm.ClickItem(bagItem, true))
                                {
                                    PPather.WriteLine("Inventory: BagManger failed to click the item. Aborting...");
                                    break;
                                }
                                Thread.Sleep(500);
                                break;
                            }
                        }
                        bm.CloseAllBags();
                        bm.UpdateItems();
                        continue;
                    }
                }
            }
        }

        public static void CheckForBetterGear()
        {
            List<EasyItem> containers = new List<EasyItem>();
            PPather.WriteLine(LOG_CATEGORY.INFORMATION,"Inventory: Reading Inventory Items");
            Character.GetInventoryItems();
            PPather.Debug("\n=====================================\n=== CHECK OF {0} BAG ITEMS START ===\n=====================================\n", Character.InventoryItems.Count);
            foreach (KeyValuePair<long, EasyItem> e in Character.InventoryItems)
            {
                //PPather.WriteLine("AutoEquipTask: Got " + e.Key + " from InventoryItems");
                EasyItem E = e.Value;
                PPather.Debug("\n\n=== AUTOEQUIP FOR {0} START ===", E.RealName);
                if (E.Item.Type.Equals("Containers"))
                {
                    PPather.Debug("AutoEquip: {0} saved for CompareAndEquipBags()", E.RealName);
                    containers.Add(E);
                    PPather.Debug("\n=== AUTOEQUIP FOR {0} END ===\n", E.RealName);
                    continue;
                }
                if (ItemCompare.CompareGear(E, E.RealName))
                {
                    PPather.WriteLine(LOG_CATEGORY.DEBUG,"AutoEquip: Equipped better gear [{0}]", E.RealName);
                }
                PPather.Debug("\n=== AUTOEQUIP FOR {0} END ===\n", E.RealName);
            }

            if (containers.Count > 0)
            {
                EasyItem[] C = containers.ToArray();
                Inventory.CompareAndEquipBags(C);
                containers.Clear();
            }
            PPather.Debug("\n=====================================\n=== CHECK OF {0} BAG ITEMS END ===\n=====================================\n", Character.InventoryItems.Count);
        }
	}
}
