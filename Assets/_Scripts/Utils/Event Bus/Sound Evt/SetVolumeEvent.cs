using _Scripts.Data.Sounds;

namespace _Scripts.Utils.Event_Bus
{
    public struct SetVolumeEvent
    {
        public readonly MixerType MixerType;
        public readonly float Value;

        public SetVolumeEvent(MixerType mixerType,float value)
        {
            Value = value;
            MixerType = mixerType;
        }
    }
}