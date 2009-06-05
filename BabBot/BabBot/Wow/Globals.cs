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
using BabBot.Manager;

namespace BabBot.Wow
{
    public class Globals
    {
        public const uint GameOffset = 0x010BD5F4; // 3.1.0: 0x010B65F4; // 3.0.9: 0x127F13C
        public const uint MouseOverGuidOffset = 0x011D3F50;

        public const uint NameStorePointer = 0x0113ED00 + 0x8;
                          // 3.1.0: 0x01137CE0 + 8; // 3.0.9 0x11AF470 + 0x8;  // Player name database

        public const uint PlayerBaseOffset1 = 0x34; // 3.1.0: 0x34; // 3.0.9: 0x30
        public const uint PlayerBaseOffset2 = 0x24; // 3.1.0: 0x24; // 3.0.9: 0x28;
        public const uint PlayerCurTargetGuidOffset = 0x011D3F60; // 3.1.0: 0x011CCF38; // 3.0.9: 0x10A68E0
        public const uint PlayerRotationOffset = 0x7A8; // 3.1.0: 0x79C; // 3.0.9: 0x7D8;
        public const uint PlayerXOffset = 0x798; // 3.1.0: 0x798; // 3.0.9: 0x7D0
        public const uint PlayerYOffset = 0x79C; // 3.1.0: 0x79C; // 3.0.9: 0x7D4;
        public const uint PlayerZOffset = 0x7A0; // 3.1.0: 0x79C; // 3.0.9: 0x7D8;
        public static uint cameraOffset = 0x00007834; // 3.1.0: 0x00007834;
        public static uint cameraPointer = 0x0117108C; // 3.1.0: 0x0117108C;
        public static uint ClientConnection;
        public static uint ClientConnectionOffset;
        public static uint ClientConnectionPointer;
        public static uint CurMgr;
        public static uint DescriptorOffset = 0x08;
        public static uint FirstObject = 0xAC;
        public static uint GuidOffset = 0x30;
        public static uint LocalGuidOffset = 0xC0;
        public static uint NextObject = 0x3C;
        private static uint playerBaseOffset;
        public static uint TypeOffset = 0x14;

        public static uint FirstBuff = 0xEBC; // Offset of the first buff from unit base
        public static uint NextBuff = 0x4;  // Offset of next buff from a given buff


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

        // Virtual Method Table offsets
        public static class VMT
        {
            public const uint
                GetPosition = 9 * 4,
                GetFacing   = 10 * 4,
                GetScale    = 11 * 4,
                Interact    = 38 * 4,
                GetName     = 48 * 4;
        }

        // WoW Function Addresses
        public static class Functions
        {
            public const uint
                CastSpellById = 0x004C4DB0, // 3.1.3
                CastSpellByName = 0x004C4DF0, // 3.1.3 TODO: Test this function 
                GetSpellIdByName = 0x006FF4A0, // 3.1.3
                SelectUnit = 0x006EF810, // 3.1.3
                GetUnitRelation = 0x005AA670, // 3.1.3
                CInputControl = 0x0113F8E4, // 3.1.3
                CInputControl_SetFlags = 0x00691BB0, // 3.1.3
                Lua_DoString = 0x0049AAB0, // 3.1.3
                Lua_GetLocalizedText = 0x005A82F0; // 3.1.3

        }

        // Movement flags for use with SetMovementFlag or to compare with current flag
        public enum MovementFlags : int
        {
            // http://www.mmowned.com/forums/wow-memory-editing/147440-better-way-doing-nudge-hacks.html#post958082
            MoveStop = 0x00,
            MoveForward = 0x10,
            MoveBackward = 0x20,
            StrafeLeft = 0x40,
            StrafeRight = 0x80,
            TurnLeft = 0x100,
            TurnRight = 0x200,
            PitchDown = 0x400,
            PitchUp = 0x800,
            Autorun = 0x1000,
            All = 0x1FF0
        }

    }
}