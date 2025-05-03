using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public float FadeDuration=1f;
    public FadeType CurentFadeType;


    private int _fadeAmount = Shader.PropertyToID("_Value");

    private int _useShutter = Shader.PropertyToID("_UseShutter");
    private int _useRadialWipe = Shader.PropertyToID("_useRadialWipe");
    private int _usePlainBlack = Shader.PropertyToID("_UsePlainBlack");
    private int _useGoop = Shader.PropertyToID("_UseGoop");

    private int? _lastEffect;

    private Image _image;

    private Material _material;


    public enum FadeType
    {
        Shutters,
        RadialWipe,
        PlainBlack,
        Goop
    }


    private void Awake()
    {
        _image = GetComponent<Image>();
        _material = _image.material;


        _lastEffect = _useShutter;
    }

  public void nextFade()
    {
        FadeOut(CurentFadeType);
    }
    public void prevFade()
    {
        FAdeIn(CurentFadeType);
    }

    public void FAdeIn(FadeType fadeType)
    {
        ChangeFadeEffect(fadeType);
        StartFadeIn();
    }

    private void StartFadeIn()
    {
        _material.SetFloat(_fadeAmount, 1f);

        StartCoroutine(HandleFade(0f, 1f));
    }
    public void FadeOut(FadeType fadeType)
    {
        ChangeFadeEffect(fadeType);
        StartFadeOut();
    }

    private void StartFadeOut()
    {
       _material.SetFloat(_fadeAmount, 0f);

        StartCoroutine(HandleFade(1f,0f));
    }

    private IEnumerator HandleFade(float targetAmount, float startAmount)
    {
        float elapsedTime = 0f;
        while(elapsedTime < FadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float lerpedAmount =Mathf.Lerp(startAmount, targetAmount, (elapsedTime/FadeDuration));
            _material.SetFloat(_fadeAmount, lerpedAmount);

            yield return null;
        }
        _material.SetFloat(_fadeAmount, targetAmount);
    }

    private void ChangeFadeEffect(FadeType fadeType)
    {
        if (_lastEffect.HasValue)   
        {
            _material.SetFloat(_lastEffect.Value, 0f);
        }


        switch (fadeType)
        {
            case FadeType.Shutters:
                SwitchEffect(_useShutter);
                break;

            case FadeType.RadialWipe:
                SwitchEffect(_useRadialWipe);
                break;
            case FadeType.PlainBlack:
                SwitchEffect(_usePlainBlack);
                break;

            case FadeType.Goop:
                SwitchEffect(_useGoop);
                break;

        }

        
    }

    private void SwitchEffect(int effectToTorn)
    {
        _material.SetFloat(effectToTorn, 1f);
        _lastEffect = effectToTorn;
    }
}
