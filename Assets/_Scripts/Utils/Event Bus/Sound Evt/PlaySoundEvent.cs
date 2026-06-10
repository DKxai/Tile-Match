using _Scripts.Data.Sounds;

namespace _Scripts.Utils.Event_Bus
{
    public struct PlaySoundEvent
    {
        public readonly SoundType Type;
        public PlaySoundEvent(SoundType type)
        {
            Type = type;
        }
    }
}