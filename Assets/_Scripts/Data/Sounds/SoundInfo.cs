using System;
using UnityEngine;

namespace _Scripts.Data.Sounds
{
    [Serializable]
    public class SoundInfo
    {
        public SoundType SoundType;
        public MixerType MixerType;
        public AudioClip SoundClip;
    }
}