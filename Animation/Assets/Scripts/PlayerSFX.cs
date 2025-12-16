using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] List<AudioClip> _attackClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> _jumpClips = new List<AudioClip>();
    [SerializeField] AudioClip _clip;

    public void PlaySFX(AudioClip clip)
    {
        if(_audioSource != null && clip != null)
            _audioSource.PlayOneShot(clip);
    }


    public void PlayAttackSFX()
    {
        if(_attackClips.Count == 0) return;
        PlaySFX(_attackClips[Random.Range(0, _attackClips.Count)]);
    }

    public void PlayJumpSFX()
    {
        if(_jumpClips.Count == 0) return;
        PlaySFX(_jumpClips[Random.Range(0, _jumpClips.Count)]);
    }




}
