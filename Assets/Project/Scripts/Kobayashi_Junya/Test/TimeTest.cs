using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTest : MonoBehaviour
{
    public static float time;

    private static bool isCount;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isCount)
            time += Time.deltaTime;
    }

    public static void SetIsCount(bool value)
    {
        isCount = value;
    }
}
