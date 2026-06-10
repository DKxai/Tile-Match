using _Scripts.Data;

namespace _Scripts.Utils.Event_Bus
{
    public struct LoadSceneEvent
    {
        public readonly SceneType SceneType;

        public LoadSceneEvent(SceneType sceneType)
        {
            this.SceneType = sceneType;
        }
    }
}