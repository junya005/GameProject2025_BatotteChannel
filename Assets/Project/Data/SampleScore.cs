using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScore : MusicalScoreData
{
    public override List<int> generateTimingForPlayerOne => new List<int>()
    {
        1,
        5,
        10,
        16,
        18,
        20
    };

    public override List<int> generateTimingForPlayerTwo => new List<int>()
    {
        3,
        8,
        14,
        19,
        21,
        24
    };
}
