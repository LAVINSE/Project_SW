using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SwUtilsLog
{
    #region 함수
    [System.Diagnostics.Conditional("DebugMode")]
    public static void Log(object message)
    {
#if DebugMode
        Debug.Log(message);
#endif // DebugMode
    }

    [System.Diagnostics.Conditional("DebugMode")]
    public static void LogWarning(object message)
    {
#if DebugMode
        Debug.LogWarning(message);
#endif // DebugMode
    }

    [System.Diagnostics.Conditional("DebugMode")]
    public static void LogError(object message)
    {
#if DebugMode
        Debug.LogError(message);
#endif // DebugMode
    }
    #endregion // 함수
}
