using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    /// <summary>
    /// 現在の画面状況、モード
    /// </summary>
    public enum GameSceneEnum
    {
        Title,
        Select,
        Tutorial,
        Game,
        //Result 別シーン
    }
}