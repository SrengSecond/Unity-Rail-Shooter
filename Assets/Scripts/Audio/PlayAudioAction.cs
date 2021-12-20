using UnityEngine;

namespace Audio
{
    public class PlayAudioAction : MonoBehaviour
    {
        [SerializeField] private AudioGetter audioSfx;
        [SerializeField] private bool twoDSound;
        [SerializeField] private float delay;

        private void OnEnable()
        {
            this.DelayAction(delegate
            {
                AudioPlayer.Instance.PlaySFX(audioSfx, twoDSound ? null : transform);
            }, delay);
        }

    }
}