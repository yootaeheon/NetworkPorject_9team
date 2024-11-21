using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingBox :MonoBehaviour
{
    [SerializeField] private TMP_Text _loadingText;
    [SerializeField] private float _delayTime;

    Coroutine _loadingTextRoutine;
    private void OnEnable()
    {
        StartCoroutine(LoadingTextRoutine());
    }

    IEnumerator LoadingTextRoutine()
    {
        while (true)
        {
            _loadingText.SetText("로딩 중.".GetText());
            yield return _delayTime.GetDelay();
            _loadingText.SetText("로딩 중..".GetText());
            yield return _delayTime.GetDelay();
            _loadingText.SetText("로딩 중...".GetText());
            yield return _delayTime.GetDelay();
        }
    }
}
