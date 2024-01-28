﻿using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ADVMESAI.Data.Scripts.MESAPISpawning
{
    public class ADVMESWave_Packets
    {

        [ProtoInclude(1000, typeof(ChatCommandPacket))]
        [ProtoInclude(1001, typeof(CounterUpdatePacket))]
        [ProtoContract]
        public class Packet
        {
            public Packet()
            {
            }
        }

        [ProtoContract]
        public class ChatCommandPacket : Packet
        {
            [ProtoMember(1)]
            public string CommandString;

            public ChatCommandPacket()
            {
            }

            public ChatCommandPacket(string CommandString)
            {
                this.CommandString = CommandString;
            }
        }

        [ProtoContract]
        public class CounterUpdatePacket : Packet
        {
            [ProtoMember(1)]
            public int CounterValue;

            public CounterUpdatePacket()
            {
            }

            public CounterUpdatePacket(int counterValue)
            {
                this.CounterValue = counterValue;
            }
        }

        public class SpawnGroupInfo
        {
            public int SpawnTime { get; set; }
            public Dictionary<string, int> Prefabs { get; private set; }
            public Vector3D SpawnCoordinates { get; private set; } // Add this property

            public SpawnGroupInfo(int spawnTime, Dictionary<string, int> prefabs, Vector3D spawnCoordinates)
            {
                SpawnTime = spawnTime;
                Prefabs = prefabs;
                SpawnCoordinates = spawnCoordinates; // Initialize the spawn coordinates
            }
        }

    }
}
