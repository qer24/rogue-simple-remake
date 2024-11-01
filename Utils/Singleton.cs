using RogueProject.Models;

namespace RogueProject.Utils;

public class Singleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            _instance ??= Activator.CreateInstance<T>();

            return _instance;
        }
    }
}
