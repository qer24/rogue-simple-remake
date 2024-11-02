using System.Globalization;

namespace RogueProject.Utils;

public static class Logger
{
    private static bool _initialized = false;

    private const string LOG_PATH = "debug.log";

    private static void Init()
    {
        _initialized = true;

        File.WriteAllText(LOG_PATH, string.Empty);
        Log("Logger initialized, starting log...");
        Log(DateTime.Now.ToString(CultureInfo.InvariantCulture));
    }

    public static void Log(string message)
    {
        if (!_initialized)
        {
            Init();
        }

        File.AppendAllText(LOG_PATH, message + Environment.NewLine);
    }
}
