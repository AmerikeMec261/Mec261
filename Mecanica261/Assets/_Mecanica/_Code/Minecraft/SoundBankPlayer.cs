using UnityEngine;

namespace Minecraft
{
    public class SoundBankPlayer : MonoBehaviour
    {
        [Header("Sounds")]
        [SerializeField] private AudioClip[] _sounds;
        [SerializeField, Range(0f, 1f)] private float _volume = 1f;

        public void PlaySound(int soundIndex)
        {
            if (soundIndex < 0 || soundIndex >= _sounds.Length) { return; }
            if (_sounds[soundIndex] == null) { return; }

            AudioSource.PlayClipAtPoint(_sounds[soundIndex], transform.position, _volume);
        }
    }
}
