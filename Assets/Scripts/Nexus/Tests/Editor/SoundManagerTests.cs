using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Nexus.SoundSystem;

public class SoundManagerTests
{
    private SoundManager soundManager;
    private AudioClip testClip;

    [SetUp]
    public void SetUp()
    {
        soundManager = SoundManager.Instance;
        List<SoundChannelConfig> configs = new List<SoundChannelConfig>() { new SoundChannelConfig() { type = SoundType.Music, Volume = 1, IsMuted = false }, new SoundChannelConfig() { type = SoundType.SFX, Volume = 1, IsMuted = false }, new SoundChannelConfig() { type = SoundType.Dialogue, Volume = 1, IsMuted = false } };
        soundManager.InitializeChannelConfigs(configs);
        testClip = AudioClip.Create("TestClip", 44100, 1, 44100, false);
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up
        SoundManager.DestroyInstance();
        Object.DestroyImmediate(testClip);
    }

    [Test]
    public void PlaySound_WithValidClip_PlaysSound()
    {
        soundManager.SetVolume(SoundType.SFX, 1f); 
        soundManager.Mute(SoundType.SFX, false);
        soundManager.PlaySound(testClip, SoundType.SFX);
    }

    [Test]
    public void SetVolume_UpdatesVolumeCorrectly()
    {
        float newVolume = 0.5f;

        soundManager.SetVolume(SoundType.Music, newVolume);
        
    }

    [Test]
    public void Mute_WhenCalled_MutesSoundType()
    {
        soundManager.Mute(SoundType.Dialogue, true);
    }
}
