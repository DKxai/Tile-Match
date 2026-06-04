using UnityEngine;

namespace _Scripts.Systems
{
    public class AudioSystem: PersistentSingleton<AudioSystem>
    {
        [SerializeField] private AudioSource audioSource; 
        [SerializeField] private AudioClip[] audioClips;
        
        
    }
}