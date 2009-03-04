using System;
using System.Collections.Generic;
using System.Text;
using BabBot.Manager;

namespace BabBot.Wow
{
    public class Globals
    {
        public const uint GameOffset = 0x127F13C;
        public const uint PlayerHealthOffset = 0xFF4;
        public const uint PlayerMaxHealthOffset = 0x26E4;
        public const uint PlayerManaOffset = 0xFF8;
        public const uint PlayerMaxManaOffset = 0x26E8;
        public const uint PlayerXpOffset = 0x3630;
        private static uint playerBaseOffset = 0x0;
        public const uint PlayerBaseOffset1 = 0x30;
        public const uint PlayerBaseOffset2 = 0x28;
        public const uint PlayerXOffset = 0x7D0;
        public const uint PlayerYOffset = 0x7D4;
        public const uint PlayerZOffset = 0x7D8;
        public const uint PlayerCurTargetGuidOffset = 0x10A68E0;

        // ObjectManager
        public static uint ClientConnectionPointer = 0x0;
        public static uint ClientConnectionOffset = 0x0;
        public static uint ClientConnection = 0x0;
        public static uint CurMgr = 0x0;
        public static uint LocalGuidOffset = 0xC0;
        public static uint FirstObject = 0xAC;
        public static uint GuidOffset = 0x30;
        public static uint NextObject = 60;
        public static uint TypeOffset = 20;
        public static uint DescriptorOffset = 0x08;
        public static uint UnitFieldOffset = 0x110;


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
