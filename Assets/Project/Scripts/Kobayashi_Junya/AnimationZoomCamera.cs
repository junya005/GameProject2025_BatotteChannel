using System.Collections;
using UnityEngine;

namespace BatotteChannel.InGame.Animation
{
    /// <summary>カメラのズームに関するアニメーションクラス(カメラコンポーネントの併用を想定)</summary>
    [RequireComponent(typeof(Camera))]
    public class AnimationZoomCamera : MonoBehaviour
    {
        #region 変数

        /// <summary>カメラコンポーネントを格納、Startで取得するため設定の必要なし</summary>
        private Camera _camera;

        #endregion

        #region 関数

        public IEnumerator ZoomCamera(Vector3 startPos, Vector3 endPos,
                                        float startSize = 2.7f, float endSize = 5.4f,
                                        float duration = 3.0f)
        {
            _camera = GetComponent<Camera>();
            float timeFrameSec = 0.0f;
            while (timeFrameSec < duration)
            {
                float t = timeFrameSec / duration;
                _camera.orthographicSize = Mathf.Lerp(startSize, endSize, t);
                transform.position = Vector3.Lerp(startPos, endPos, t);
                timeFrameSec += Time.deltaTime;
                yield return null;
            }
        }

        #endregion

        #region イベント関数

        private void Start()
        {
            _camera = GetComponent<Camera>();
        }

        #endregion

    }
}
