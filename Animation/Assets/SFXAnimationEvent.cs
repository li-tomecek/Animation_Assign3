using UnityEngine;

public class SFXAnimationEvent : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _clip;

    public void PlaySFX()
    {
        if(_audioSource != null && _clip != null)
            _audioSource.PlayOneShot(_clip);
    }
}
