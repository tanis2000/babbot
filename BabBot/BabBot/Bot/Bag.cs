/*
    This file is part of BabBot.

    BabBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    BabBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BabBot.  If not, see <http://www.gnu.org/licenses/>.
  
    Copyright 2009 BabBot Team
*/
using System;
using System.Collections.Generic;
using BabBot.Manager;
using System.Text.RegularExpressions;

namespace BabBot.Bot
{
    public class BagList : List<Bag>
    {
    }

    public class Bag
    {

        // http://www.wowwiki.com/API_GetContainerItemLink

        public int BagID;
        public ItemLinkList Items;

        public Bag(int _bagID)
        {
            BagID = _bagID;
            Items = new ItemLinkList();
        }

        public void UpdateBagItems()
        {
            if (IsValidBag)
            {
                Items.Clear();
                int TotalSlot = NumberOfSlots + 1;
                for (int slot = 1; slot < TotalSlot; slot++)
                {
                    string strItemlink = GetContainerItemLink(slot);
                    if (strItemlink != "null")
                    {
                        ItemLink itemlink = new ItemLink(slot, strItemlink);
                        Items.Add(itemlink);
                    }
                }
            }
        }

        private string GetContainerItemLink(int slot)
        {
            ProcessManager.Injector.Lua_DoString(string.Format("ItemLink = GetContainerItemLink({0}, {1});", BagID, slot));
            string local = ProcessManager.Injector.Lua_GetLocalizedText("ItemLink");

            // |cff9d9d9d|Hitem:7073:0:0:0:0:0:0:0|h[Broken Fang]|h|r
            if (local != "null")
            {
                int start = local.IndexOf('[') + 1;
                int len = local.LastIndexOf(']') - start;
                return local.Substring(start, len);
            }

            return local;
        }

        /// <summary>
        /// Return false if there is not bag ?? check
        /// </summary>
        public bool IsValidBag
        {
            get 
            {
                return BagName != "null" ? true : false;
            }
        }

        /// <summary>
        /// Get the name of one of the player's bags.
        /// </summary>
        public string BagName
        {
            get
            {
                ProcessManager.Injector.Lua_DoString(string.Format("bagName = GetBagName({0});", BagID));
                return ProcessManager.Injector.Lua_GetLocalizedText("bagName");
            }
        }

        /// <summary>
        /// Returns the total number of slots in the bag.
        /// </summary>
        public int NumberOfSlots
        {
            get
            {
                ProcessManager.Injector.Lua_DoString(string.Format("numberOfSlots = GetContainerNumSlots({0});", BagID));
                string local = ProcessManager.Injector.Lua_GetLocalizedText("numberOfSlots");
                return Convert.ToInt32(local);
            }
        }

        /// <summary>
        /// Returns the total number of free slots in the bag.
        /// </summary>
        public int NumberOfFreeSlots
        {
            get
            {
                ProcessManager.Injector.Lua_DoString(string.Format("numberOfFreeSlots, BagType = GetContainerNumFreeSlots({0});", BagID));
                string local = ProcessManager.Injector.Lua_GetLocalizedText("numberOfFreeSlots");
                return Convert.ToInt32(local);
            }
        }

        public ItemLink Find(string name)
        {
            foreach (ItemLink il in Items)
            {
                if (il.ItemName == name) return il;
            }

            return null;
        }

    }

    #region ItemLink 

    public class ItemLinkList : List<ItemLink>
    {
    }

    public class ItemLink
    {

        /*
            http://www.wowwiki.com/API_GetContainerItemInfo
            Returns
            texture 
            String - the texture for the item in the specified bag slot
            itemCount 
            Number - the number of items in the specified bag slot
            locked 
            Boolean - 1 if the item is locked by the server, nil otherwise.
            quality 
            Number - the numeric quality of the item
            readable 
            Boolean - 1 if the item can be "read" (as in a book)        
        */

        public int Slot;
        public string ItemName;

        public ItemLink(int slot, string itemname)
        {
            Slot = slot;
            ItemName = itemname;
        }
    }

    #endregion

}
