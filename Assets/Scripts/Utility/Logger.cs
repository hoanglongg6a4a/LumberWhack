public static class Logger
{
	public static void Debug(object message) => UnityEngine.Debug.Log(message);

	public static void Warning(object message) => UnityEngine.Debug.LogWarning(message);

	public static void Error(object message) => UnityEngine.Debug.LogError(message);

	public static void Exception(System.Exception exception) => UnityEngine.Debug.LogException(exception);
}
