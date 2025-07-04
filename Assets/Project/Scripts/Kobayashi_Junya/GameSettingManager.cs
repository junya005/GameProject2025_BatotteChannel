using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingManager : Singleton<GameSettingManager>
{
    // フレームレート上限の選択肢
    public enum EnumFrameRateLimitState
    {
        Thirty = 30,
        Sixty = 60
    }

    // フレームレート上限数 変更する場合はSetAppFrameRateLimit関数を使用すること
    private EnumFrameRateLimitState _appFrameRateLimit = EnumFrameRateLimitState.Thirty;

    // フレームレート上限のゲッター
    public EnumFrameRateLimitState AppFrameRateLimit { get { return _appFrameRateLimit; } }

    /// <summary>
    /// フレームレート上限のセッター
    /// 同時にアプリケーションのフレーム上限をセットします
    /// </summary>
    /// <param name="value"></param>
    public void SetAppFrameRateLimit(EnumFrameRateLimitState value)
    {
        _appFrameRateLimit = value;
        Application.targetFrameRate = (int)_appFrameRateLimit;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetAppFrameRateLimit(_appFrameRateLimit);
    }
}
