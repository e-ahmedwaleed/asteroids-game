using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    /// <summary>
    /// The audio manager
    /// </summary>
    public static class AudioManager
    {
        static AudioSource audioSource;
        static Dictionary<AudioClipName, AudioClip> audioClips =
            new Dictionary<AudioClipName, AudioClip>();

        /// <summary>
        /// Initializes the audio manager
        /// </summary>
        /// <param name="source">audio source</param>
        public static void Initialize(AudioSource source)
        {
            audioSource = source;
            audioClips.Add(AudioClipName.Damage, 
                Resources.Load<AudioClip>(@"Audio/Damage"));
            audioClips.Add(AudioClipName.Explode,
                Resources.Load<AudioClip>(@"Audio/Explode"));
            audioClips.Add(AudioClipName.Shot,
                Resources.Load<AudioClip>(@"Audio/Shot"));
        }

        /// <summary>
        /// Plays the audio clip with the given name
        /// </summary>
        /// <param name="name">name of the audio clip to play</param>
        public static void Play(AudioClipName name)
        {
            audioSource.PlayOneShot(audioClips[name]);
        }
    }
}
