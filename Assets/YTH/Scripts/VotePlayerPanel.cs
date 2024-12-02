using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VotePlayerPanel : MonoBehaviour
{
    [SerializeField] GameObject[] _panelAnonymImages;

    public GameObject[] PanelAnonymImages { get { return _panelAnonymImages; } }
}
