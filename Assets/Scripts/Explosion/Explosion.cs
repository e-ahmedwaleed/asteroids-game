using UnityEngine;

namespace Explosion
{
    /// <summary>
    ///     An explosion
    /// </summary>
    public class Explosion : MonoBehaviour
    {
        // cached for efficiency
        private Animator _anim;

        /// <summary>
        ///     Use this for initialization
        /// </summary>
        private void Start()
        {
            _anim = GetComponent<Animator>();
        }

        /// <summary>
        ///     Update is called once per frame
        /// </summary>
        private void Update()
        {
            // destroy the game object if the explosion has finished its animation
            if (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                Destroy(gameObject);
        }
    }
}