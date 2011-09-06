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
using System.Threading;

using Glider.Common.Objects;
using Pather.Graph;

namespace Pather.Helpers
{
	class Mount
	{
		public const int MIN_MOUNT_LEVEL = 30;

        public static void MountIfNeeded(double distanceToTravel, Nullable<bool> taskMountPreference)
        {
            if (ShouldMount(distanceToTravel, taskMountPreference) )
            {
                MountUp();
            }
        }

        public static bool ShouldMount(double distance, Nullable<bool> taskMountPreference )
        {
            //TODO: This should be a Time check, not purely distance.
            // It takes X seconds to mount up and dismount. Those + the Estimate travel time needs
            // to exceed an travel time for walking there.

            // mount if we're really far away
            if (distance >= PPather.PatherSettings.MountRange)
            {
                if (PPather.PatherSettings.UseMount != "Never Mount")
                {
                    if (PPather.PatherSettings.UseMount == "Always Mount" ||
                        (PPather.PatherSettings.UseMount == "Let Task Decide" &&
                        taskMountPreference.HasValue && taskMountPreference.Value == true))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool IsMounted()
        {
            return IsMounted(GPlayerSelf.Me);
        }

        public static bool IsMounted(GUnit Unit)
        {

            // Mount test based on spell ID - 5 June 2008
            int[] mountBuffsID = {
                48778, // Acherus Deathcharger
                35022, //	Black Hawkstrider
                6896, //	Black Ram
                17461, //	Black Ram
                22718, //	Black War Kodo
                22720, //	Black War Ram
                22721, //	Black War Raptor
                22717, //	Black War Steed
                22723, //	Black War Tiger
                22724, //	Black War Wolf
                578, //	Black Wolf
                35020, //	Blue Hawkstrider
                10969, //	Blue Mechanostrider
                33630, //	Blue Mechanostrider
                6897, //	Blue Ram
                17463, //	Blue Skeletal Horse
                43899, //	Brewfest Ram
                34406, //	Brown Elekk
                458, //	Brown Horse
                18990, //	Brown Kodo
                6899, //	Brown Ram
                17464, //	Brown Skeletal Horse
                6654, //	Brown Wolf
                39315, //	Cobalt Riding Talbuk
                34896, //	Cobalt War Talbuk
                39316, //	Dark Riding Talbuk
                34790, //	Dark War Talbuk
                17481, //	Deathcharger
                6653, //	Dire Wolf
                8395, //	Emerald Raptor
                36702, //	Fiery Warhorse
                23509, //	Frostwolf Howler
                16060, //	Golden Sabercat
                35710, //	Gray Elekk
                18989, //	Gray Kodo
                6777, //	Gray Ram
                459, //	Gray Wolf
                35713, //	Great Blue Elekk
                23249, //	Great Brown Kodo
                34407, //	Great Elite Elekk
                23248, //	Great Gray Kodo
                35712, //	Great Green Elekk
                35714, //	Great Purple Elekk
                23247, //	Great White Kodo
                18991, //	Green Kodo
                15780, //	Green Mechanostrider
                17453, //	Green Mechanostrider
                17465, //	Green Skeletal Warhorse
                824, //	Horse Riding
                6743, //	Horse Riding
                17459, //	Icy Blue Mechanostrider
                16262, //	Improved Ghost Wolf
                16287, //	Improved Ghost Wolf
                10795, //	Ivory Raptor
                17450, //	Ivory Raptor
                18995, //	Kodo Riding
                18996, //	Kodo Riding
                16084, //	Mottled Red Raptor
                16055, //	Nightsaber
                10798, //	Obsidian Raptor
                42936, //	Ornery Ram
                44370, //	Pink Elekk Call
                472, //	Pinto Horse
                35711, //	Purple Elekk
                35018, //	Purple Hawkstrider
                17455, //	Purple Mechanostrider
                23246, //	Purple Skeletal Warhorse
                17456, //	Red & Blue Mechanostrider
                34795, //	Red Hawkstrider
                10873, //	Red Mechanostrider
                17462, //	Red Skeletal Horse
                22722, //	Red Skeletal Warhorse
                43883, //	Rental Racing Ram
                18363, //	Riding Kodo
                30174, //	Riding Turtle
                39317, //	Silver Riding Talbuk
                34898, //	Silver War Talbuk
                8980, //	Skeletal Horse
                10921, //	Skeletal Horse Riding
                29059, //	Skeletal Steed
                42776, //	Spectral Tiger
                10789, //	Spotted Frostsaber
                15781, //	Steel Mechanostrider
                23510, //	Stormpike Battle Charger
                8394, //	Striped Frostsaber
                10793, //	Striped Nightsaber
                897, //	Summon Angry Programmer
                44369, //	Summon Baby Pink Elekk
                31700, //	Summon Black Qiraji Battle Tank
                26655, //	Summon Black Qiraji Battle Tank
                26656, //	Summon Black Qiraji Battle Tank
                25863, //	Summon Black Qiraji Battle Tank
                25953, //	Summon Blue Qiraji Battle Tank
                32723, //	Summon Bonechewer Riding Wolf
                34767, //	Summon Charger
                23214, //	Summon Charger
                23215, //	Summon Charger
                34766, //	Summon Charger
                23261, //	Summon Darkreaver's Fallen Charger
                31331, //	Summon Dire Wolf
                23161, //	Summon Dreadsteed
                38311, //	Summon Eclipsion Hawkstrider
                1710, //	Summon Felsteed
                5784, //	Summon Felsteed
                5968, //	Summon Ghost Saber
                6084, //	Summon Ghost Saber
                26056, //	Summon Green Qiraji Battle Tank
                30829, //	Summon Kessel's Elekk
                30837, //	Summon Kessel's Elekk
                30840, //	Summon Kessel's Elekk Trigger
                39782, //	Summon Lightsworn Elekk
                18166, //	Summon Magram Ravager
                26054, //	Summon Red Qiraji Battle Tank
                41543, //	Summon Reins (Jorus)
                41544, //	Summon Reins (Malfas)
                41546, //	Summon Reins (Onyxien)
                41547, //	Summon Reins (Suraku)
                41548, //	Summon Reins (Voranaku)
                41549, //	Summon Reins (Zoya)
                39783, //	Summon Scryer Hawkstrider
                7910, //	Summon Tamed Raptor
                7915, //	Summon Tamed Turtle
                4946, //	Summon Tamed Wolf
                13819, //	Summon Warhorse
                13820, //	Summon Warhorse
                34768, //	Summon Warhorse
                34769, //	Summon Warhorse
                23227, // Swift Palomino
                23241, //	Swift Blue Raptor
                43900, //	Swift Brewfest Ram
                23238, //	Swift Brown Ram
                23229, //	Swift Brown Steed
                23250, //	Swift Brown Wolf
                23220, //	Swift Dawnsaber
                23221, //	Swift Frostsaber
                23239, //	Swift Gray Ram
                23252, //	Swift Gray Wolf
                35025, //	Swift Green Hawkstrider
                23225, //	Swift Green Mechanostrider
                23219, //	Swift Mistsaber
                23242, //	Swift Olive Raptor
                23243, //	Swift Orange Raptor
                33660, //	Swift Pink Hawkstrider
                35027, //	Swift Purple Hawkstrider
                24242, //	Swift Razzashi Raptor
                42777, //	Swift Spectral Tiger
                23338, //	Swift Stormsaber
                23251, //	Swift Timber Wolf
                35028, //	Swift War Hawkstrider
                23223, //	Swift White Mechanostrider
                23240, //	Swift White Ram
                23228, //	Swift White Steed
                23222, //	Swift Yellow Mechanostrider
                24252, //	Swift Zulian Tiger
                39318, //	Tan Riding Talbuk
                34899, //	Tan War Talbuk
                16059, //	Tawny Sabercat
                18992, //	Teal Kodo
                22480, //	Tender Wolf Steak
                10790, //	Tiger
                10796, //	Turquoise Raptor
                17454, //	Unpainted Mechanostrider
                10799, //	Violet Raptor
                15779, //	White Mechanostrider
                6898, //	White Ram
                39319, //	White Riding Talbuk
                34897, //	White War Talbuk
                17229, //	Winterspring Frostsaber
                60114, //   Armored Brown Bear (Alliance)
                60116, //   Armored Brown Bear (Horde)
                 };

            GPlayerSelf.Me.Refresh(true);
            Unit.SetBuffsDirty();            
            return Unit.HasBuff(mountBuffsID);
		}

		public static bool HaveMount()
		{
			return new GInterfaceHelper().IsKeyPopulated("Common.Mount");
		}

		static GSpellTimer mountTimer = new GSpellTimer(15000, true);

		public static bool MountUp()
		{
			if (GPlayerSelf.Me.Level < MIN_MOUNT_LEVEL)
				return false;
			if (IsMounted())
				return true;
			if (GPlayerSelf.Me.IsInCombat)
				return false;
			if (!HaveMount())
				return false;

			// only try to mount every so often
			if (!mountTimer.IsReady)
			{
				return false;
			}

			// check that we're not inside
            Spot mySpot = null;

			try
			{
				mySpot = PPather.world.GetSpot(
					new Location(GPlayerSelf.Me.Location));

				if (mySpot.GetFlag(Spot.FLAG_INDOORS))
				{
					//PPather.WriteLine("Not mounting, we're inside");
					mountTimer.Reset();
					return false;
				}
			}
			catch
			{
				// if we got an exception something must be up
				return false;
			}


			PPather.mover.Stop();

			//PPather.WriteLine("Mount up");
			//buff.Snapshot();

			//mountBuffID = 0;

			GContext.Main.CastSpell("Common.Mount");

			Thread.Sleep(100);

			string badMount = null;

			if (GContext.Main.RedMessage.Contains("while swimming"))
			{
				badMount = "Trying to mount while swimming";
			}
			else if (GContext.Main.RedMessage.Contains("can't mount here"))
			{
				badMount = "Trying to mount inside";
				mySpot.SetFlag(Spot.FLAG_INDOORS, true);
			}

			if (null != badMount)
			{
				PPather.WriteLine(badMount);
				mountTimer.Reset();
				return false;
			}

			while (GPlayerSelf.Me.IsCasting)
			{
				Thread.Sleep(100);
			}

			if (!IsMounted())
			{
				mountTimer.Reset();
				return false;
			}

			return true;
		}

        static GSpellTimer dismountTimer = new GSpellTimer(10000, true);

		public static void Dismount()
		{
			if (IsMounted() && dismountTimer.IsReady)
			{
				PPather.WriteLine("Dismounting");
				GContext.Main.SendKey("Common.Mount");
                dismountTimer.Reset();
			}
		}
	}
}
        