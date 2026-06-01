using UnityEngine;

namespace Minecraft
{
    public class SoundBankPlayer : MonoBehaviour
    {
        [Header("Sounds")]
        [Tooltip("Audio clips available to play by index.")]
        [SerializeField] private AudioClip[] _sounds;
        [Tooltip("Volume used when playing a clip.")]
        [SerializeField, Range(0f, 1f)] private float _volume = 1f;

        public void PlaySound(int soundIndex)
        {
            if (soundIndex < 0 || soundIndex >= _sounds.Length) { return; }
            if (_sounds[soundIndex] == null) { return; }

            AudioSource.PlayClipAtPoint(_sounds[soundIndex], transform.position, _volume);
        }
    }
}
