using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BatotteChannel.InGame.Players
{
    /// <summary>プレイヤーの参加処理を行うクラス</summary>
    [RequireComponent(typeof(PlayerInputManager))]
    public class JoinPlayer : MonoBehaviour
    {
        /// <summary>プレイヤーを格納するリスト</summary>
        private List<GameObject> _players = new List<GameObject>();

        #region イベント関数

        private void Start()
        {
            // プレイヤー生成時にOnPlayerJoindを実行するよう設定
            if (PlayerInputManager.instance != null)
                PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
        }

        /// <summary>
        /// プレイヤーインスタンスを取得する(プレイヤー生成時に自動実行)
        /// </summary>
        /// <param name="playerInput">参加したプレイヤーインスタンスのPlayerInput</param>
        private void OnPlayerJoined(PlayerInput playerInput)
        {
            GameObject playerInstance = playerInput.gameObject;
            Debug.Log(playerInstance);

            // リストに取得したプレイヤーを格納
            _players.Add(playerInstance);
        }

        #endregion
    }
}
