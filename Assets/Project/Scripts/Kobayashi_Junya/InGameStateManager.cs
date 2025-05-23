using UnityEngine;

namespace BatotteChannel.GameState
{
    /// <summary>InGameのステート</summary>
    public enum InGameState
    {
        Ready,
        PlayMusic,
        End
    }

    /// <summary>
    /// InGameの状態を表すクラス
    /// シングルトンパターンを使用する
    /// InGameManagerと併用して使用することを想定
    /// </summary>
    public class InGameStateManager : SceneSingleton<InGameStateManager>
    {
        /// <summary>現在のInGame状態</summary>
        private InGameState _currentInGameState;

        /// <summary>現在のInGame状態のゲッタープロパティ</summary>
        public InGameState CurrentInGameState
        {
            get { return _currentInGameState; }
        }

        /// <summary>
        /// 現在のInGame状態のセッター関数
        /// </summary>
        /// <param name="inGameState">設定したい値 InGameState.Ready,PlayMusic,End のいずれか</param>
        public void SetCurrentInGameState(InGameState inGameState)
        {
            _currentInGameState = inGameState;
#if UNITY_EDITOR
            Debug.Log($"現在のゲームステートを{_currentInGameState}に変更しました");
#endif
        }
    }
}
