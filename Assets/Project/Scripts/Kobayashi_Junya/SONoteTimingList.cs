using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Project.Struct.Notes;
using UnityEngine;

namespace Project.DataAsset.Notes
{
    [Serializable, CreateAssetMenu(fileName = "NewNoteTimingList",
                                    menuName = "DataAssets/NoteTimingList")]
    public class SONoteTimingList : ScriptableObject
    {
        [Label("難易度")]
        public EDifficultyState difficultyState;
        [Label("譜面データ")]
        public List<NoteTiming> timingList;
    }
}
