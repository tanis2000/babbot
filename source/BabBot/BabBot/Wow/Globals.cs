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
        public static uint ClientConnection;
        public static uint ClientConnectionOffset;
        public static uint ClientConnectionPointer;
        public static uint CurMgr;
        private static uint playerBaseOffset;

        public static uint PlayerBaseOffset
        {
            get
            {
                if (playerBaseOffset != 0x0)
                    return playerBaseOffset;

                if (ProcessManager.WowProcess != null)
                {
                    try
                    {
                        playerBaseOffset = ProcessManager.WowProcess.ReadUInt(
                            ProcessManager.WowProcess.ReadUInt(
                                ProcessManager.WowProcess.ReadUInt(
                                    ProcessManager.GlobalOffsets.GameOffset) +
                                        ProcessManager.GlobalOffsets.PlayerBaseOffset1) +
                                            ProcessManager.GlobalOffsets.PlayerBaseOffset2);
                        return playerBaseOffset;
                    }
                    catch
                    {
                        ProcessManager.ResetGameStatus();
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
                GetBagPtr = 10 * 4, 
                GetPosition = 11 * 4, // 3.1.3: 9 * 4,
                GetFacing = 12 * 4, // 3.1.3: 10 * 4,
                GetScale = 14 * 4, // 3.1.3: 11 * 4,
                GetModel = 22 * 4, 
                Interact = 41 * 4, // 3.1.3: 38 * 4,
                GetName = 51 * 4;// 3.1.3: 48 * 4;
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