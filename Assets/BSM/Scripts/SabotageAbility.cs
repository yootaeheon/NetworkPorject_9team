using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SabotageAbility : BaseMission
{ 
    [SerializeField] private PlayerType _playerType;

    [SerializeField] private GameObject _abilityObject;
    [SerializeField] private Image _armColor;

    private void Start()
    {
        //PlayerController _playerController = GetComponent<PlayerController>();
        //_armColor.color = Color.red;
    }

    private void Update()
    {

        if (_playerType.Equals(PlayerType.Duck))
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                _abilityObject.SetActive(true);
                
            }
        }

    }

}
