using System.Collections.Generic;
using Nexus.PoolSystem;
using Nexus.Singleton;
using UnityEngine;

namespace Nexus.SoundSystem
{
    public class SoundManager : Singleton<SoundManager>
    {
        public const string SOUND_BLUE_PRINT = "Audio/AudioPrefab";
        

        private Dictionary<SoundType, SoundChannelConfig> channelConfigMap;



        public void InitializeChannelConfigs( List<SoundChannelConfig> soundChannelConfigs )
        {
            channelConfigMap = new Dictionary<SoundType, SoundChannelConfig>();
            for( int i = 0; i < soundChannelConfigs.Count; i++ )
            {
                var soundItem = soundChannelConfigs[i];
                if (channelConfigMap.ContainsKey(soundItem.type))
                {
                    continue;
                }
                channelConfigMap.Add(soundItem.type,soundItem);
            }
            
            PoolManager.Instance.CreatePool(SOUND_BLUE_PRINT,10);
        }

        public void PlaySound( AudioClip clip, SoundType type, float volumeScale = 1f )
        {
            if( channelConfigMap.TryGetValue(type, out var channelConfig) && !channelConfig.IsMuted )
            {
                GameObject audioItemObject = PoolManager.Instance.GetObjectFromPool(SOUND_BLUE_PRINT);
                if( audioItemObject != null )
                {
                    SoundIem audioItem = audioItemObject.GetComponent<SoundIem>();
                    if( audioItem == null )
                    {
                         audioItem = audioItemObject.AddComponent<SoundIem>();
                        
                    }
                    float finalVolume = channelConfig.Volume * volumeScale; 
                    audioItem.PlaySound(clip, finalVolume);
                }
            }
        }

        public void SetVolume( SoundType type, float volume )
        {
            if( channelConfigMap.TryGetValue(type, out var channelConfig) )
            {
                channelConfig.Volume = volume;
            }
        }

        public void Mute( SoundType type, bool mute )
        {
            if( channelConfigMap.TryGetValue(type, out var channelConfig) )
            {
                channelConfig.IsMuted = mute;
            }
        }
    }
}