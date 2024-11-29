using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOption : BaseUI
{

    private void Awake()
    {
        Bind();
    }

    private void Start()
    {
        GetUI<Button>("SettingButton").onClick.AddListener(ActivateOption);
    }

    private void ActivateOption()
    {
        OptionPanel.Instance.gameObject.SetActive(true);
    }
}
