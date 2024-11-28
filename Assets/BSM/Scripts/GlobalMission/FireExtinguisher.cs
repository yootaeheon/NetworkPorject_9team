using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{

    [SerializeField] private GameObject _powder;
    [SerializeField] private GameObject _powderPoint;

    [SerializeField] private GameObject _fire1;
    [SerializeField] private GameObject _fire2;
    [SerializeField] private GameObject _fire3;


    private (float, float) _fire1PosX = (-396f, -471f);
    private (float, float) _fire1PosY = (-79f, -40f);

    private (float, float) _fire2PosX = (-1f, -110f);
    private (float, float) _fire2PosY = (-54f, -60f);

    private (float, float) _fire3PosX = (293f, 400f);
    private (float, float) _fire3PosY = (-70f, -144f);

    private Animator _powderAnim; 
    private RectTransform _rect;

    private bool isPowder;
    public bool IsPowder
    {
        get
        {
            return isPowder;
        }
        set
        {
            isPowder = value; 
            _powder.SetActive(isPowder); 
        }
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _powderAnim = _powder.GetComponent<Animator>();
        _rect = _powder.GetComponent<RectTransform>();
    }

    private void Update()
    {
        FollowPowderPoint(); 

    }

    private void FollowPowderPoint()
    {
        _powder.transform.position = _powderPoint.transform.position;
    }

    private void FireCheck()
    {
        (float, float) _rectPos = (_rect.anchoredPosition.x, _rect.anchoredPosition.y);

        if(_rectPos.Item1 > _fire1PosX.Item2 || _rectPos.Item1 < _fire1PosX.Item1)
        {

        }



    }

    private IEnumerator FireCoroutine()
    {
        yield return null;
    }

}
