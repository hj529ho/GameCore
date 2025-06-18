namespace Core.Manager
{
    public partial class Managers
    {
        #region Core
        private readonly Core.Manager.DataManager _data = new();
        public static Core.Manager.DataManager Data => Instance._data;

        private readonly Core.Manager.EventManager _event = new();
        public static Core.Manager.EventManager Event => Instance._event;

        private readonly Core.Manager.InputManager _input = new();
        public static Core.Manager.InputManager Input => Instance._input;

        private readonly Core.Manager.LoadingSceneManager _scene = new();
        public static Core.Manager.LoadingSceneManager Scene => Instance._scene;

        private readonly Core.Manager.LocalizationManager _localization = new();
        public static Core.Manager.LocalizationManager Localization => Instance._localization;

        private readonly Core.Manager.PoolManager _pool = new();
        public static Core.Manager.PoolManager Pool => Instance._pool;

        private readonly Core.Manager.ResourceManager _resource = new();
        public static Core.Manager.ResourceManager Resource => Instance._resource;

        private readonly Core.Manager.SoundManager _sound = new();
        public static Core.Manager.SoundManager Sound => Instance._sound;

        private readonly Core.Manager.StoryManager _story = new();
        public static Core.Manager.StoryManager Story => Instance._story;

        private readonly Core.Manager.UIManager _uI = new();
        public static Core.Manager.UIManager UI => Instance._uI;

        #endregion
    }
}