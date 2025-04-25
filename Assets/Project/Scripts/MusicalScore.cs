using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MusicalScoreData
{
    public abstract List<int> generateTimingForPlayerOne { get; }

    public abstract List<int> generateTimingForPlayerTwo { get; }
}
