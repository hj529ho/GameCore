/// <summary>
/// Singleton.Get<T>() where T : class, new()
/// </summary>
public static class Singleton
{
    public static T Get<T>() where T : class, new() {
        var instance = SingletonImpl<T>.Instance;
        if (instance == null) {
            instance = new T();
            SingletonImpl<T>.Instance = instance;
        }
        return instance;
    } 

    private class SingletonImpl<T> where T : class, new() {
        public static T Instance;
    }
}