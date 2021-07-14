using Audio;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Initializes the game
    /// </summary>
    public class GameInitializer : MonoBehaviour 
    {
        /// <summary>
        /// Awake is called before Start
        /// </summary>
        void Awake()
        {
            // initialize screen utils
            ScreenUtils.Initialize();
            
            // initialize/load game configuration data
            GameDataUtils.Initialize();
            
            // initialize/add audio manager
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            AudioManager.Initialize(audioSource);
        }
    }
}
