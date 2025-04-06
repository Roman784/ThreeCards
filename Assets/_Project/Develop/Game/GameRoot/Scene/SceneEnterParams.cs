namespace GameRoot
{
    public abstract class SceneEnterParams
    {
        public string SceneName { get; }
        public string ExitSceneName { get; }

        public SceneEnterParams(string sceneName, string exitSceneName)
        {
            SceneName = sceneName;
            ExitSceneName = exitSceneName;
        }

        public T As<T>() where T : SceneEnterParams
        {
            return (T)this;
        }
    }
}
