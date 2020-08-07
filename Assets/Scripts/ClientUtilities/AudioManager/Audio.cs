using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClientUtilities.AudioMangaer
{
    public class Audio : MonoBehaviour
    {
        private AudioSource source = null;


        public AudioSource Source
        {
            get { return source; }
            set
            {
                Debug.Assert(value != null, "Source cannot be null");

                source = value;
            }
        }

        public AudioClip Clip
        {
            get { return source.clip; }
            set
            {
                source.clip = value;
                AlreadyPlayed = false;
            }
        }

        public bool AutoUnload
        {
            get;
            set;
        }

        public bool Loop
        {
            get { return source.loop; }
            set { source.loop = value; }
        }

        public bool Mute
        {
            get { return source.mute; }
            set { source.mute = value; }
        }

        public float Volume
        {
            get { return source.volume; }
            set { source.volume = value; }
        }

        public bool IsFinished
        {
            get { return AlreadyPlayed && !source.isPlaying; }
        }

        public bool AlreadyPlayed
        {
            get;
            private set;
        }


        private void Update()
        {
            if (AlreadyPlayed && AutoUnload && !source.isPlaying)
                AudioManager.Instance.Unload(this);
        }


        public void Play()
        {
            source.Play();

            AlreadyPlayed = true;
        }

        public void Stop()
        {
            source.Stop();

            AlreadyPlayed = false;
        }

        public void Unload()
        {
            Stop();
            AudioManager.Instance.Unload(this);
        }
    }
}