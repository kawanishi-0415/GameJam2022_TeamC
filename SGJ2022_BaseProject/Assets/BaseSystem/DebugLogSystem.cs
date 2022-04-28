using System.Diagnostics;
using System.Runtime.CompilerServices;

/// <summary>
/// GAME_DEBUGが定義されているときのみに呼び出される
/// </summary>
public static class GameDebug
{
    [Conditional("GAME_DEBUG")]
    public static void Log(object o)
    {
        UnityEngine.Debug.Log(o);
    }
    [Conditional("GAME_DEBUG")]
    public static void LogWarning(object o)
    {
        UnityEngine.Debug.LogWarning(o);
    }
    [Conditional("GAME_DEBUG")]
    public static void LogError(object o)
    {
        UnityEngine.Debug.LogError(o);
    }
    [Conditional("GAME_DEBUG")]
    public static void DetailsLog(string message = "",
        [CallerFilePath] string filePath = "",
        [CallerMemberName] string methodName = "",
        [CallerLineNumber] int line = 0)
    {
        UnityEngine.Debug.Log($"{filePath}\nMethod:{methodName}|Line:{line}|Message:{message}");
    }
}