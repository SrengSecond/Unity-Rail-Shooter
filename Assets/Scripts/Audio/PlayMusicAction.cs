using UnityEngine;

namespace Audio
{
    public class PlayMusicAction : MonoBehaviour
    {
        [SerializeField] private AudioGetter audioSfx;
        [SerializeField] private float delay;

        private void OnEnable()
        {
            this.DelayAction(delegate
            {
                AudioPlayer.Instance.PlaySFX(audioSfx);
            }, delay);
        }

    }
}