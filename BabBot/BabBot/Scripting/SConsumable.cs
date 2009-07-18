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
using System.Linq;
using System.Text;
using BabBot.Bot;
using BabBot.Manager;

namespace BabBot.Scripting
{
    public sealed class SConsumable
    {
        #region Singleton
        private static readonly SConsumable instance = 
            new SConsumable();

        public static SConsumable Instance
        {
            get { return instance; }
        }
        #endregion

        private Wow.WowPlayer player = null;

        private ItemList Food;
        private ItemList Drinks;
        private ItemList Scrolls;

        private SConsumable()
        {
            Food = new ItemList();
            Drinks = new ItemList();
            Scrolls = new ItemList();

            // Initialize the list of food
            InitializeFood();
            // Initialize the list of drinks
            InitializeDrinks();
            // Initialize the list of scrolls
            InitializeScrolls();
        }

        public void Init(Wow.WowPlayer iPlayer)
        {
            player = iPlayer;
        }

        private void InitializeDrinks()
        {
            // Buyable Drinks
            Item i = new Item("Refreshing Spring Water", 0, 1);
            Drinks.Add(i);
            i = new Item("Ice Cold Milk", 5, 1);
            Drinks.Add(i);
            i = new Item("Melon Juice", 15, 1);
            Drinks.Add(i);
            i = new Item("Sweet Nectar", 25, 1);
            Drinks.Add(i);
            i = new Item("Moonberry Juice", 35, 1);
            Drinks.Add(i);
            i = new Item("Morning Glory Dew", 45, 1);
            Drinks.Add(i);
            i = new Item("Footman's Waterskin", 55, 1);
            Drinks.Add(i);
            i = new Item("Grunt's Watersking", 55, 1);
            Drinks.Add(i);
            i = new Item("Filtered Draenic Water", 60, 1);
            Drinks.Add(i);
            i = new Item("Star's Lament", 55, 1);
            Drinks.Add(i);
            i = new Item("Star's Tears", 65, 1);
            Drinks.Add(i);
            i = new Item("Sweetened Goat's Milk", 60, 1);
            Drinks.Add(i);
            i = new Item("Pungent Seal Whey", 70, 1);
            Drinks.Add(i);
            i = new Item("Honeymint Tea", 75, 1);
            Drinks.Add(i);

            // Conjurable Drinks
            i = new Item("Conjured Water", 0, 10);
            Drinks.Add(i);
            i = new Item("Conjured Fresh Water", 5, 10);
            Drinks.Add(i);
            i = new Item("Conjured Purified Water", 15, 10);
            Drinks.Add(i);
            i = new Item("Conjured Spring Water", 25, 10);
            Drinks.Add(i);
            i = new Item("Conjured Mineral Water", 35, 10);
            Drinks.Add(i);
            i = new Item("Conjured Sparkling Water", 45, 10);
            Drinks.Add(i);
            i = new Item("Conjured Crystal Water", 55, 10);
            Drinks.Add(i);
            i = new Item("Conjured Glacier Water", 65, 10);
            Drinks.Add(i);

            Drinks.Sort();
        }

        private void InitializeFood()
        {
            // Buyable Food
            Item i = new Item("Cactus Apple Surprise", 0, 1);
            Food.Add(i);
            i = new Item("Tough Hunk of Bread", 0, 1);
            Food.Add(i);
            i = new Item("Honey Bread", 0, 1);
            Food.Add(i);
            i = new Item("Shiny Red Apple", 0, 1);
            Food.Add(i);
            i = new Item("Darnassian Bleu", 0, 1);
            Food.Add(i);
            i = new Item("Freshly Baked Bread", 5, 1);
            Food.Add(i);
            i = new Item("Tel'Abim Banana", 5, 1);
            Food.Add(i);
            i = new Item("Moist Cornbread", 15, 1);
            Food.Add(i);
            i = new Item("Snapvine Watermelon", 15, 1);
            Food.Add(i);
            i = new Item("Mulgore Spice Bread", 25, 1);
            Food.Add(i);
            i = new Item("Hot Wolf Ribs", 25, 1);
            Food.Add(i);
            i = new Item("Goldenbark Apple", 25, 1);
            Food.Add(i);
            i = new Item("Soft Banana Bread", 35, 1);
            Food.Add(i);
            i = new Item("Moon Harvest Pumpkin", 35, 1);
            Food.Add(i);
            i = new Item("Homemade Cherry Pie", 45, 1);
            Food.Add(i);
            i = new Item("Deep Fried Plantains", 45, 1);
            Food.Add(i);
            i = new Item("Mag'har Grainbread", 55, 1);
            Food.Add(i);
            i = new Item("Skethyl Berries", 55, 1);
            Food.Add(i);
            i = new Item("Bladespire Bagel", 65, 1);
            Food.Add(i);
            i = new Item("Crusty Flatbread", 65, 1);
            Food.Add(i);
            i = new Item("Telaari Grapes", 65, 1);
            Food.Add(i);
            i = new Item("Tundra Berries", 65, 1);
            Food.Add(i);
            i = new Item("Sweet Potato Bread", 75, 1);
            Food.Add(i);
            i = new Item("Savory Snowplum", 75, 1);
            Food.Add(i);


            // Conjurable Food
            i = new Item("Conjured Muffin", 0, 10);
            Food.Add(i);
            i = new Item("Conjured Bread", 5, 10);
            Food.Add(i);
            i = new Item("Conjured Rye", 15, 10);
            Food.Add(i);
            i = new Item("Conjured Pumpernickel", 25, 10);
            Food.Add(i);
            i = new Item("Conjured Sourdough", 35, 10);
            Food.Add(i);
            i = new Item("Conjured Sweet Roll", 45, 10);
            Food.Add(i);
            i = new Item("Conjured Cinnamon Roll", 55, 10);
            Food.Add(i);
            i = new Item("Conjured Croissant", 65, 10);
            Food.Add(i);

            Food.Sort();
        }

        private void InitializeScrolls()
        {
            // TODO: implement
        }

        public bool HasFood()
        {
            foreach (Item i in Food)
            {
                if (i.LevelRequired <= player.Level)
                {
                    if (player.HasItem(i))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool HasDrink()
        {
            foreach (Item i in Drinks)
            {
                if (i.LevelRequired <= player.Level)
                {
                    if (player.HasItem(i))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool HasScroll(string kind)
        {
            foreach (Item i in Scrolls)
            {
                if (i.LevelRequired <= player.Level)
                {
                    if (i.Kind == kind)
                    {
                        if (player.HasItem(i))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool HasBandage()
        {
            // TODO: implement
            return false;
        }

        public void UseBandage()
        {
            // TODO: implement
        }

        public void UseScroll(string kind)
        {
            Dictionary<int, string> RomanNumerals = new Dictionary<int, string>();
            RomanNumerals[1] = "I";
            RomanNumerals[2] = "II";
            RomanNumerals[3] = "III";
            RomanNumerals[4] = "IV";
            RomanNumerals[5] = "V";
            RomanNumerals[6] = "VI";
            RomanNumerals[7] = "VII";

            if (kind == "") return;

            foreach (Item i in Scrolls)
            {
                if (i.LevelRequired <= player.Level)
                {
                    if (i.Name.StartsWith("Scroll of " + kind))
                    {
                        if (player.HasItem(i))
                        {
                            player.UseItem(i);
                        }
                    }
                }
            }
        }

        public Item FindBestDrink()
        {
            foreach (Item i in Drinks)
            {
                if (i.LevelRequired <= player.Level)
                {
                    if (player.HasItem(i))
                    {
                        return i;
                    }
                }
            }
            return null;
        }

        public void UseDrink()
        {
            /// we should go through our list of drinks and use one of them
            Item i = FindBestDrink();
            if (i != null)
            {
                player.UseItem(i);
            }
        }

        public bool NeedFood()
        {
            if (!HasFood())
            {
                return true;
            }
            return false;
        }

        public bool NeedDrink()
        {
            if (!HasDrink())
            {
                return true;
            }
            return false;
        }

        public int? GetIndex(string itemToBuy)
        {
            int idx = 1;
            do
            {
                Item item = player.GetMerchantItemInfo(idx);
                if (item.Name == itemToBuy)
                {
                    return idx;
                }
            } while (player.GetMerchantItemInfo(idx++) != null);
            return null;
        }

        public void MerchantBuy(int stacks, string itemToBuy)
        {
            int? idx = GetIndex(itemToBuy);

            if (idx == null) return;

            Item item = player.GetMerchantItemInfo((int)idx);
            float num = 20.0f / item.Quantity;

            for (int i = 1; i <= stacks; i++)
            {
                player.BuyMerchantItem((int)idx, (int)Math.Round(num, 0));
            }
        }

    }

}
