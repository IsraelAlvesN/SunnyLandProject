using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    public static FadeController _fadeInstance;
    public Image _fadeImage;
    public Color _initialColor;
    public Color _finalColor;
    public float _fadeDuration;
    public bool _isFade;
    private float _time;

    private void Awake()
    {
        _fadeInstance = this;
    }

    IEnumerator InitialFade()
    {
        _isFade = true;
        _time = 0f;
        while (_time <= _fadeDuration)
        {
            _fadeImage.color = Color.Lerp(_initialColor, _finalColor, (_time / _fadeDuration));
            _time += Time.deltaTime;
            yield return null;
        }

        _isFade = false;
    }

    void Start()
    {
        StartCoroutine(InitialFade());
    }

    void Update()
    {
        
    }
}
