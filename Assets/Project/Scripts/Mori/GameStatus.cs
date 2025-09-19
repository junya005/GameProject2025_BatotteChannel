using UnityEngine;

public class GameStatus : MonoBehaviour
{
    /// <summary>
    /// 現在の画面状況、モード一覧
    /// </summary>
    public enum GameSceneEnum
    {
        Title,
        Select,
        Tutorial,
        Game,
        Story,
        //Result 別シーン
    }
}
