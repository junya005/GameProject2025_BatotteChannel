using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_InsEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject _goodEff;
    [SerializeField]
    private GameObject _missEff;
    [Button]
    public void GoodEffButton()
    {
        Instantiate(_goodEff, transform.position, Quaternion.identity);
    }
    [Button]
    public void MissEffButton()
    {
        Instantiate(_missEff, transform.position, Quaternion.identity);
    }
}
