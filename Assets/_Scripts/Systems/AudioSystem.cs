using UnityEngine;

namespace _Scripts.Systems
{
    public class AudioSystem: Singleton<AudioSystem>
    {
        [SerializeField] private AudioSource audioSource; 
        [SerializeField] private AudioClip[] audioClips;
        
        
    }
}