using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace BatotteChannel.Tutorial
{
    /// <summary>チュートリアルの流れを制御するクラス</summary>
    public class TutorialFlowManager : MonoBehaviour
    {
        [SerializeField, Label("タイトル&セレクト画面のマネージャー")]
        private TitleSelectManager _titleSelectManager;

        [SerializeField, Label("チュートリアル用ノーツ生成管理クラス")]
        private TutorialNotesManager _tutorialNotesManager;

        [SerializeField]
        private TutorialPlayerManager _tutorialPlayerManager;

        [SerializeField]
        private TutorialLog _tutorialLog;

        /// <summary>
        /// チュートリアルが開始されたかのフラグ
        /// </summary>
        private bool _isStartTutorial;

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            CheckTutrorialState();
        }

        /// <summary>
        /// 初期化する
        /// </summary>
        private void Initialize()
        {
            _isStartTutorial = false;

            _tutorialPlayerManager._skipTutorialEventHandler += OnSkipTutorial;
        }

        /// <summary>
        /// ゲーム状態がチュートリアルになったかチェックする
        /// </summary>
        private void CheckTutrorialState()
        {
            if (_isStartTutorial == true) { return; }
            if (_titleSelectManager.GameSceneState != GameStatus.GameSceneEnum.Tutorial) return;

            _isStartTutorial = true;
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;
            PlayTutorialAnimation(token);
        }

        /// <summary>
        /// チュートリアルを再生する
        /// </summary>
        public async UniTask PlayTutorialAnimation(CancellationToken token)
        {
            Debug.Log("チュートリアルを開始");

            // 準備
            _tutorialNotesManager.SetCanGenerate(true);
            _tutorialLog.SetLogSprite(0);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
            _tutorialPlayerManager.SetCanPlayersInput(true);

            // タイミングの説明
            await UniTask.Delay(TimeSpan.FromSeconds(2.0f), cancellationToken: token);
            await _tutorialNotesManager.GenerateNoteByCount(0, 2, token: token);
            await UniTask.Delay(TimeSpan.FromSeconds(3.0f), cancellationToken: token);

            // 番号の説明
            _tutorialLog.SetLogSpriteWithAnimation(1);
            await _tutorialNotesManager.GenerateNoteOrderBySpecified(1, 3, token: token);
            await UniTask.Delay(TimeSpan.FromSeconds(3.0f), cancellationToken: token);

            // スタンの説明
            _tutorialLog.SetLogSpriteWithAnimation(2);
            await _tutorialNotesManager.GenerateNoteOrderBySpecified(1, 2, true, 4.0f, token);
            await UniTask.Delay(TimeSpan.FromSeconds(3.0f), cancellationToken: token);
            _tutorialNotesManager.SetCanGenerate(false);

            _titleSelectManager.TransitionCanvas(GameStatus.GameSceneEnum.Tutorial, GameStatus.GameSceneEnum.Select);
        }

        public void OnSkipTutorial()
        {
            _tutorialPlayerManager.SetCanPlayersInput(false);
            _cancellationTokenSource.Cancel();
            _tutorialNotesManager.SetCanGenerate(false);
            _tutorialNotesManager.gameObject.SetActive(false);
            _titleSelectManager.TransitionCanvas(GameStatus.GameSceneEnum.Tutorial, GameStatus.GameSceneEnum.Select);
        }
    }
}
