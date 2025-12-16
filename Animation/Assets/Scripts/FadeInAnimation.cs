using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeInAnimation : MonoBehaviour
{
    [SerializeField] float fadeDuration;
    [SerializeField] Image fadeBackground;
    
    void Start()
    {
        fadeBackground.DOFade(0, fadeDuration);
    }

}
