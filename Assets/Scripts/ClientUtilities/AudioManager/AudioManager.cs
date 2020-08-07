
using Assets.Scripts.ClientUtilities.Pool;
using ClientUtilities.ResourceManager;
using ClientUtilities.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClientUtilities.AudioMangaer
{
    public class AudioManager : MonoBehaviorSingleton<AudioManager>
    {
        public class AudioPool : ObjectPool<Audio>
        {

        }

        private class AudioPlayerList : List<Audio>
        { }

        private class DefaultAudioClip
        {
            public AudioClip Clip;
        }


        public enum SoundTypes
        {
            Music,
            Effect
        }


        private AudioPlayerList musicsList = new AudioPlayerList();
        private AudioPlayerList effectsList = new AudioPlayerList();

        private Dictionary<SoundTypes, bool> specificMuteState = new Dictionary<SoundTypes, bool>();
        private Dictionary<SoundTypes, float> specificVolumeState = new Dictionary<SoundTypes, float>();

        private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
        private AudioPool pool = null;

        public bool Mute
        {
            get { return AudioListener.pause; }
            set { AudioListener.pause = value; }
        }

        public float MasterVolume
        {
            get { return AudioListener.volume; }
            set { AudioListener.volume = value; }
        }

        private void Awake()
        {
            pool = new AudioPool();
            pool.InitiliazePool("Audio/AudioPrefab", 10);
        }

        public Audio Load(string Path, SoundTypes Type)
        {
            AudioClip clip = null;


            if (audioClips.ContainsKey(Path))
            {
                clip = audioClips[Path];

            }
            else
            {
                clip = GameResourceManager.Instance.LoadAudioClip(Path);
                if (clip != null)
                    audioClips.Add(Path, clip);
            }

            return Load(clip, Type);
        }

        public Audio Load(AudioClip Clip, SoundTypes Type)
        {
            if (Clip == null)
                return null;

            Audio instance = pool.GetFromPool();
            instance.Source = instance.GetComponent<AudioSource>();
            instance.Source.clip = Clip;
            instance.Volume = GetVolume(Type);
            instance.Mute = GetMute(Type);
            instance.Loop = false;

            switch (Type)
            {
                case SoundTypes.Music:
                    musicsList.Add(instance);
                    break;
                case SoundTypes.Effect:
                    effectsList.Add(instance);
                    break;
            }

            return instance;
        }

        public void Unload(Audio Instance)
        {
            musicsList.Remove(Instance);
            effectsList.Remove(Instance);

            pool.SendToPool(Instance);
        }

        public Audio Register(AudioSource Source, SoundTypes Type)
        {
            Audio instance = gameObject.AddComponent<Audio>();
            instance.Source = Source;
            instance.Clip = Source.clip;

            switch (Type)
            {
                case SoundTypes.Music:
                    musicsList.Add(instance);
                    break;
                case SoundTypes.Effect:
                    effectsList.Add(instance);
                    break;
            }

            return instance;
        }

        public bool GetMute(SoundTypes Type)
        {
            if (specificMuteState.ContainsKey(Type))
                return specificMuteState[Type];

            return false;
        }

        public void SetMute(bool Value, SoundTypes Type)
        {
            AudioPlayerList list = null;

            switch (Type)
            {
                case SoundTypes.Music:
                    list = musicsList;
                    break;
                case SoundTypes.Effect:
                    list = effectsList;
                    break;
            }

            specificMuteState[Type] = Value;

            for (int i = 0; i < list.Count; ++i)
                list[i].Mute = Value;
        }

        public float GetVolume(SoundTypes Type)
        {
            if (specificVolumeState.ContainsKey(Type))
                return specificVolumeState[Type];

            return 0.0F;
        }

        public void SetVolume(float Value, SoundTypes Type)
        {
            AudioPlayerList list = null;

            switch (Type)
            {
                case SoundTypes.Music:
                    list = musicsList;
                    break;
                case SoundTypes.Effect:
                    list = effectsList;
                    break;
            }

            specificVolumeState[Type] = Value;

            for (int i = 0; i < list.Count; ++i)
                list[i].Volume = Value;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

    }
}
