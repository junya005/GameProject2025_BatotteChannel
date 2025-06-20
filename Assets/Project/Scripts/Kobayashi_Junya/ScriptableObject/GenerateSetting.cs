using BatotteChannel.InGame.Players;
using UnityEngine;

namespace BatotteChannel.DataAssets
{
    /// <summary>
    /// 譜面生成タイミングと生成座標のデータアセットを作成できる
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GenerateSettingScriptableObject")]
    public class GenerateSettingScriptableObject : ScriptableObject
    {
        /*
        注意：変数名の変更や初期化処理をt追記すると作成したデータが全てデフォルト値に戻ります。
        */

        /// <summary>生成対象のプレイヤー</summary>
        public PlayerNumberState player;

        /// <summary>生成タイミング</summary>
        public float timing;

        /// <summary>
        /// SubBeatを使用するか
        /// </summary>
        public bool isUseSubBeat = false;

        /// <summary>
        /// もっと細かい生成タイミング、IsUseSubBeatがtrueになっていなければ無効
        /// </summary>
        public int subTiming;

        /// <summary>生成座標</summary>
        public Vector3 generatePos;
    }
}
