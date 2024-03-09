using Nexus.PoolSystem;
using UnityEngine;

namespace Nexus.SoundSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundIem : MonoBehaviour
    {
        private AudioSource audioSource;
        private PoolManager poolManager;

        public void Initialize(PoolManager poolManager)
        {
            this.poolManager = poolManager;
        }

        public void PlaySound(AudioClip clip, float volume)
        {
            if( audioSource == null )
            {
                audioSource = GetComponent<AudioSource>();
            }

            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
            Invoke(nameof(ReturnToPool), clip.length);
        }

        private void ReturnToPool()
        {
            if (poolManager != null)
            {
                poolManager.ReturnObjectToPool(SoundManager.SOUND_BLUE_PRINT,  gameObject);
            }
        }
    }

}
