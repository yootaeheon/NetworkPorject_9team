using NSJ_Test;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] GameObject _light;
    private void Start()
    {
        if (TestGame.Instance != null) 
        {
            _light.SetActive(false);
        }
        else
        {
            _light.SetActive(true);
        }
    }
}
