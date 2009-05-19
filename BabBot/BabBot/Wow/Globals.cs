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
using System.Text;
using BabBot.Manager;

namespace BabBot.Wow
{
    public class Globals
    {
        public const uint GameOffset = 0x010B65F4; // 3.0.9: 0x127F13C
        //public const uint PlayerHealthOffset = 0xFF4;
        //public const uint PlayerMaxHealthOffset = 0x26E4;
        //public const uint PlayerManaOffset = 0xFF8;
        //public const uint PlayerMaxManaOffset = 0x26E8;
        //public const uint PlayerXpOffset = 0x3630;
        private static uint playerBaseOffset = 0x0;
        public const uint PlayerBaseOffset1 = 0x34; // 3.0.9: 0x30
        public const uint PlayerBaseOffset2 = 0x24; // 3.0.9: 0x28;
        public const uint PlayerXOffset = 0x798; // 3.0.9: 0x7D0
        public const uint PlayerYOffset = 0x79C; // 3.0.9: 0x7D4;
        public const uint PlayerZOffset = 0x7A0; // 3.0.9: 0x7D8;
        public const uint PlayerRotationOffset = 0x7A8; // 3.0.9: 0x7D8;
        public const uint PlayerCurTargetGuidOffset = 0x011CCF38; // 3.0.9: 0x10A68E0
        public const uint NameStorePointer = 0x01137CE0 + 8; // 3.0.9 0x11AF470 + 0x8;  // Player name database

        // ObjectManager
        public static uint ClientConnectionPointer = 0x0;
        public static uint ClientConnectionOffset = 0x0;
        public static uint ClientConnection = 0x0;
        public static uint CurMgr = 0x0;
        public static uint LocalGuidOffset = 0xC0;
        public static uint FirstObject = 0xAC;
        public static uint GuidOffset = 0x30;
        public static uint NextObject = 0x3C;
        public static uint TypeOffset = 0x14;
        public static uint DescriptorOffset = 0x08;
        public static uint cameraPointer = 0x0117108C;
        public static uint cameraOffset = 0x00007834;


        public static uint PlayerBaseOffset
        {
            get
            {
                if (playerBaseOffset != 0x0)
                {
                    return playerBaseOffset;
                }

                if (ProcessManager.WowProcess != null)
                {
                    try
                    {
                        playerBaseOffset =
                            ProcessManager.WowProcess.ReadUInt(
                                ProcessManager.WowProcess.ReadUInt(ProcessManager.WowProcess.ReadUInt(GameOffset) +
                                                                   PlayerBaseOffset1) + PlayerBaseOffset2);
                        return playerBaseOffset;
                    }
                    catch
                    {
                        ProcessManager.InGame = false;
                        throw new Exception("Cannot read PlayerBaseOffset. Have we quit the game?");
                    }
                }

                throw new Exception("Trying to read the PlayerBaseOffset with an uninitialized process");
                
            }
        }
    }
}
