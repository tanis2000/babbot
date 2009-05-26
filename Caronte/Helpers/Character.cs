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
using System.Linq;
using System.Text;
using Glider.Common.Objects;
using Pather.Helpers.UI;
using System.Threading;

/*
 * Contributor(s): toblakai
 */ 
namespace Pather.Helpers
{
    /// <summary>
    /// This class represents the character info frame. Used for example by
    /// AutoEquip to compare new loot with what the toon is currently 
    /// wearing in the different slots. Also contains bag slots and
    /// some stats.
    /// </summary>
    public static class Character
    {
        public static class Slots
        {
            public const string Head = "HeadSlot";
            public const string Neck = "NeckSlot";
            public const string Shoulder = "ShoulderSlot";
            public const string Back = "BackSlot";
            public const string Chest = "ChestSlot";
            public const string Shirt = "ShirtSlot";
            public const string Wrist = "WristSlot";
            public const string Tabard = "TabardSlot";
            public const string Hands = "HandsSlot";
            public const string Waist = "WaistSlot";
            public const string Legs = "LegsSlot";
            public const string Feet = "FeetSlot";
            public const string Finger = "Finger";
            public const string Finger0 = "Finger0Slot";
            public const string Finger1 = "Finger1Slot";
            public const string Trinket = "Trinket";
            public const string Trinket0 = "Trinket0Slot";
            public const string Trinket1 = "Trinket1Slot";
            public const string MainHand = "MainHandSlot";
            public const string OneHand = "OneHand";
            public const string SecondaryHand = "SecondaryHandSlot";
            public const string Ranged = "RangedSlot";

            public static string get(string slot)
            {
                slot = slot.ToLower();
                slot = slot.Replace("-", "");
                slot = slot.Replace(" ", "");
                switch (slot)
                {
                    case "head": return Head;
                    case "neck": return Neck;
                    case "shoulder": return Shoulder;
                    case "back": return Back;
                    case "chest": return Chest;
                    case "shirt": return Shirt;
                    case "tabard": return Tabard;
                    case "wrist": return Wrist;
                    case "hands": return Hands;
                    case "waist": return Waist;
                    case "legs": return Legs;
                    case "feet": return Feet;
                    case "finger": return Finger;
                    case "finger0": return Finger0;
                    case "finger1": return Finger1;
                    case "trinket": return Trinket;
                    case "trinket0": return Trinket0;
                    case "trinket1": return Trinket1;
                    case "main": return MainHand;
                    case "mainhand": return MainHand;
                    case "twohand": return MainHand;
                    case "heldinmainhand": return MainHand;
                    case "heldinonehand": return OneHand;
                    case "onehand": return OneHand;
                    case "offhand": return SecondaryHand;
                    case "heldinoffhand": return SecondaryHand;
                    case "secondaryhand": return SecondaryHand;
                    case "shield": return SecondaryHand;
                    case "ranged": return Ranged;
                    case "relic": return Ranged;
                    default: PPather.Debug("Slots: Uknown (not equippable?): {0}", slot); return slot;
                }
            }
        }

        public static Dictionary<string, EasyItem> CurrentlyEquipped = new Dictionary<string, EasyItem>();
        private static Dictionary<string, EasyItem> FutureItems = new Dictionary<string, EasyItem>();
        public static List<GContainer> Bags = new List<GContainer>();
        public static Dictionary<long, EasyItem> InventoryItems = new Dictionary<long, EasyItem>();
        private static List<String> CharacterSlots = null;
        private static BagManager bm = new BagManager();

        private static List<string> GetCharacterSlots()
        {
            List<string> slots = new List<string>();
            slots.Add("HeadSlot");
            slots.Add("NeckSlot");
            slots.Add("ShoulderSlot");
            slots.Add("BackSlot");
            slots.Add("ChestSlot");
            slots.Add("ShirtSlot");
            slots.Add("TabardSlot");
            slots.Add("WristSlot");
            slots.Add("HandsSlot");
            slots.Add("WaistSlot");
            slots.Add("LegsSlot");
            slots.Add("FeetSlot");
            slots.Add("Finger0Slot");
            slots.Add("Finger1Slot");
            slots.Add("Trinket0Slot");
            slots.Add("Trinket1Slot");
            slots.Add("MainHandSlot");
            slots.Add("SecondaryHandSlot");
            slots.Add("RangedSlot");
            return slots;
        }

        public static void GetEquippedItems()
        {
            GItemHelper GIH = new GItemHelper();

            if (CharacterSlots == null)
                CharacterSlots = GetCharacterSlots();

            CharacterFrame.ShowFrame();
            foreach (string slot in CharacterSlots)
            {
                string tooltipName = "none";
                PPather.Debug("\n\n == SLOT LOOKUP FOR {0} START ==", slot);
                List<string> tooltip = CharacterFrame.GetTooltip(slot);
                if (tooltip != null && tooltip.Count > 0)
                {
                    //foreach (string tip in tooltip)
                    //    PPather.WriteLine("Character: Tooltip[{0}]\t=>\t{1}", slot, tip);
                    tooltipName = tooltip[tooltip.Count - 1];
                }
                PPather.Debug("Character: Name for {0} = {1}", slot, tooltipName);
                if (tooltip == null || IsEmptySlotName(tooltipName))
                {
                    AddNullSlot(slot);
                }
                else
                {
                    Item i = ItemManager.get(tooltipName);
                    if (i == null)
                    {
                        AddNullSlot(slot);
                    }
                    else
                    {
                        GItem Item = (GItem)GObjectList.FindObject(GIH.GetEquippedGUID(slot));
                        if (Item == null) continue;
                        EasyItem E = new EasyItem(Item, i, Item.GUID, tooltipName);
                        try
                        {
                            CurrentlyEquipped.Add(slot,E);
                            PPather.WriteLine(LOG_CATEGORY.INFORMATION,"Character: Item [{0}] equipped in '{1}'", tooltipName, slot);
                            PPather.Debug(String.Format("ToolTip[{0}]: {1}", i.Name, CleanToolTip(tooltip)));
                        }
                        catch (ArgumentException)
                        {
                            PPather.Debug("Character: " + tooltipName + " is already added to CurrentlyEquipped (skipping)");
                        }
                    }
                }
                PPather.Debug("\n=== SLOT LOOKUP FOR {0} END ===\n", slot);
            }
            CharacterFrame.HideFrame();
        }

        /// <summary>
        /// Checks for the tooltip given when hovering an empty slot in the character frame.
        /// </summary>
        /// <param name="tooltip_name">value of first index of tooltip string array tooltip[0]</param>
        /// <returns></returns>
        public static bool IsEmptySlotName(string tooltip_name)
        {
            switch (tooltip_name.Trim(new char[] { ' ' }))
            {
                case "(no text)": return true;
                case "Head": return true;
                case "Neck": return true;
                case "Shoulders": return true;
                case "Back": return true;
                case "Chest": return true;
                case "Shirt": return true;
                case "Tabard": return true;
                case "Wrist": return true;
                case "Hands": return true;
                case "Waist": return true;
                case "Legs": return true;
                case "Feet": return true;
                case "Finger": return true;
                case "Trinket": return true;
                case "Main Hand": return true;
                case "Off Hand": return true;
                case "Ranged": return true;
                case "Relic": return true;
                case "Ammo": return true;
                default: return false;
            }
        }

        public static bool IsEmptyBagSlotName(string tooltip_name)
        {
            switch (tooltip_name)
            {
                case "Equip Container": return true;
                default: return false;
            }
        }

        public static void AddNullSlot(string slot)
        {
            try
            {
                CurrentlyEquipped.Add(slot, null);
                PPather.Debug("Character: No currently equipped item for slot '" + slot + "'");
            }
            catch (ArgumentException)
            {
                //PPather.WriteLine("Character: Already added 'null' to slot '" + slot + "'");
            }
        }

        public static void RemoveCurrentlyEquipped(EasyItem E)
        {
            if (E == null) return;
            if (CurrentlyEquipped == null || CurrentlyEquipped.Count < 1) return;
            if (CurrentlyEquipped.ContainsKey(E.Item.Slot))
            {
                CurrentlyEquipped.Remove(E.Item.Slot);
            }
        }

        public static void ReplaceCurrentlyEquipped(EasyItem E, string slot)
        {
            if (E == null) return;
            EasyItem w = null;
            if (CurrentlyEquipped == null || CurrentlyEquipped.Count < 1) return;
            if (CurrentlyEquipped.ContainsKey(slot))
            {
                w = GetCurrent(slot);
                CurrentlyEquipped.Remove(slot);
            }

            SetCurrent(slot, E);
            string oldName = "null";
            if (w != null && w.Item != null) oldName = w.GItem.Name;
            if (w != null && w.GItem != null)
            {
                PPather.AutoEquipForceSell.Add(w.GItem.GUID);
                GContext.Main.Debug(String.Format("Character: Added {0} to ForceSell list",w.GItem.Name));
            }
            PPather.WriteLine(String.Format("Character: Replaced {0} with {1} in CurrentlyEquipped", oldName, E.GItem.Name));
        } 
        
        public static void ReplaceCurrentlyEquipped(EasyItem E)
        {
            if (E == null) return;
            EasyItem w = null;
            if (CurrentlyEquipped == null || CurrentlyEquipped.Count < 1) return;
            if (CurrentlyEquipped.ContainsKey(E.Item.Slot))
            {
                w = GetCurrent(E.Item.Slot);
                CurrentlyEquipped.Remove(E.Item.Slot);
            }

            SetCurrent(E.Item.Slot, E);
            string oldName = "null";
            if (w != null && w.Item != null) oldName = w.GItem.Name;
            if (w != null && w.GItem != null)
            {
                PPather.AutoEquipForceSell.Add(w.GItem.GUID);
                GContext.Main.Debug(String.Format("Character: Added {0} to ForceSell list", w.GItem.Name));
            }
            PPather.WriteLine(String.Format("Character: Replaced {0} with {1} in CurrentlyEquipped", oldName, E.GItem.Name));
            //EasyItem w = GetCurrent(E.Item.Slot, true);
            //if (w == null) CurrentlyEquipped.Add(E.Item.Slot,E);
            //else
            //{
            //    CurrentlyEquipped.Remove(w.Item.Slot);
            //    SetCurrent(E.Item.Slot,E);
            //    PPather.WriteLine(String.Format("Character: Replaced {0} with {1} in CurrentlyEquipped", w.Item.Name, E.RealName));
            //}
        }

        public static void GetInventoryItems()
        {
            InventoryItems.Clear();
            bm.UpdateItemsSorted();
            for (int j = 0; j < bm.Bags.Length; j++ )
            {
                //PPather.Debug("Charcater: Bags.Length = {0}",bm.Bags.Length);
                BagManager.Bag b = bm.Bags[j];
                if (b.count < 1) continue;
                //PPather.Debug("Characater: Bags[{0}] contains {1} items",j,b.I.Length);
                bm.OpenBag(b.bag);
                GItem[] Items = null;
                if(b.I != null)
                    Items = b.I;
                if (Items == null || Items.Length < 1) continue;
                foreach (GItem Item in Items)
                {
                    if (Item == null || Item.GUID == 0) continue;
                    PPather.Debug("\n\n == ITEM LOOKUP FOR {0} START ==", Item.Name);
                    List<string> tooltip = GetTooltipOnOpenBags(Item);
                    if (tooltip == null || tooltip.Count < 1)
                    {
                        PPather.Debug("Character.GetInventoryItems: tooltip for {0} is emtpy", Item.Name);
                    }
                    else
                    {
                        string tooltipName = tooltip[tooltip.Count - 1];
                        PPather.Debug("Character.GetInventoryItems: tooltip[0] for {0} is {1}", Item.Name,tooltipName);
                        Item i = ItemManager.get(tooltipName);
                        if (i == null)
                        {
                            PPather.Debug("Character: ItemManager return null, {0} most likely not equippable", tooltipName);
                            continue;
                        }
                        try
                        {
                            EasyItem E = new EasyItem(Item, i, Item.GUID, tooltipName);
                            InventoryItems.Add(Item.GUID,E);
                            PPather.WriteLine(Pather.LOG_CATEGORY.INFORMATION,"Character: {0} added to InventoryItems", tooltipName);
                            //PPather.WriteLine(String.Format("DEBUG: Item.Type = {0} , Item.Quality = {1}",E.GItem.Type.ToString(), E.GItem.Definition.Quality.ToString())); 
                            //PPather.WriteLine(String.Format("Charcter: Item [{0}] found in bags", i.Name));
                            //PPather.WriteLine(String.Format("ToolTip[{0}]: {1}", i.Name, CleanToolTip(tooltip)));
                        }
                        catch (ArgumentException)
                        {
                            PPather.Debug("Character.GetInventoryItems: {0}({1}) is already added to InventoryItems (skipping)",tooltipName, Item.GUID);
                        }
                    }
                    PPather.Debug("\n=== ITEM LOOKUP FOR {0} END ===\n",Item.Name);
                }
            }
            bm.CloseAllBags();
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

        public static List<string> GetTooltipOnOpenBags(GItem item)
        {
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
            return text;
        }

        public static EasyItem GetCurrent(string s)
        {
            EasyItem E;
            CurrentlyEquipped.TryGetValue(s, out E);
            if (E != null)
                PPather.Debug("Character: GetCurrent for {0} is {1}", s, E.RealName);
            else
               PPather.Debug("Character: GetCurrent for {0} is {1}", s, "null");
            return E;
        }
        public static void SetCurrent(string slot, EasyItem i) { CurrentlyEquipped[slot] = i; }

        public static string Conform(string slot)
        {
            /* really, this is pretty stupid.
             * 
             * First of all, this  method should be in the WowHeadParser. 
             * There, we need to catch all different ways we get slots back
             * from wowhead. I've seen both Off-Hand and "Held in One Hand" 
             * for example. The names below are they used by glider, so it's
             * sort of good to convert everything to those internally.
             * 
             * We also needs to remember to check against both finger slots
             * or trinket slots if we get an idem for that.
             * 
             * Also, the silly getters and setters above... I changed it and now
             * it doesn't make sense. The overloads that take a bool are ment to 
             * trigger the use of this Conform method. So we can send in slot names
             * from wowhead and get the correct name back. But it's all sillyness.
             * 
             * Conform all slot names at once in the WowHead method when we fetch
             * the items, then just remove all the overload methods above.
             * 
             */

            if (slot.ToLower().Equals("head")) return "HeadSlot";
            if (slot.ToLower().Equals("neck")) return "NeckSlot";
            if (slot.ToLower().Equals("shoulder")) return "ShoulderSlot";
            if (slot.ToLower().Equals("back")) return "BackSlot";
            if (slot.ToLower().Equals("chest")) return "ChestSlot";
            if (slot.ToLower().Equals("shirt")) return "ShirtSlot";
            if (slot.ToLower().Equals("tabard")) return "TabardSlot";
            if (slot.ToLower().Equals("wrist")) return "WristSlot";
            if (slot.ToLower().Equals("hands")) return "HandsSlot";
            if (slot.ToLower().Equals("waist")) return "WaistSlot";
            if (slot.ToLower().Equals("legs")) return "LegsSlot";
            if (slot.ToLower().Equals("feet")) return "FeetSlot";
            if (slot.ToLower().Equals("finger0")) return "Finger0Slot";
            if (slot.ToLower().Equals("finger1")) return "Finger1Slot";
            if (slot.ToLower().Equals("trinket0")) return "Trinket0Slot";
            if (slot.ToLower().Equals("trinket1")) return "Trinket1Slot";
            if (slot.ToLower().Equals("main") || slot.ToLower().Equals("mainhand") || slot.ToLower().Equals("two hand") || slot.ToLower().Equals("two-hand") || slot.ToLower().Equals("held in main hand") || slot.ToLower().Equals("held in one hand")) return "MainHandSlot";
            if (slot.ToLower().Equals("onehand") || slot.ToLower().Equals("shield") || slot.ToLower().Equals("off hand") || slot.ToLower().Equals("offhand") || slot.ToLower().Equals("secondaryhand") || slot.ToLower().Equals("off-hand") || slot.ToLower().Equals("held in off-hand") || slot.ToLower().Equals("held in off hand")) return "SecondaryHandSlot";
            if (slot.ToLower().Equals("ranged")) return "RangedSlot";
            return slot;
        }

        private static string G2I(long guid)
        {
            GObject[] items = GObjectList.GetItems();
            foreach (GObject item in items)
            {
                if (item.GUID.Equals(guid))
                    return item.Name;
            }
            return null;
        }

        private static void TestSlotName(string slot)
        {
            GItemHelper GIH = new GItemHelper();
            long guid = GIH.GetEquippedGUID(slot);
            string name = G2I(guid);
            PPather.WriteLine(String.Format("{0} [{1}] -> {2}", slot, guid, name));
        }
    }
}
