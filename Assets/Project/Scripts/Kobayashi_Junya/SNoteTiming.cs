using System;
using UnityEngine;
using BatotteChannel.InGame.Notes;

namespace Project.Struct.Notes
{
    [Serializable]
    public struct NoteTiming
    {
        public ENotePlayerState player;
        public float timing;
        public bool useSubBeat;
        public float subBeat;
        public Vector3 position;
    }
}
