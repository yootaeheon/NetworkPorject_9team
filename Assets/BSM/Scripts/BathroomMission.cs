using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI; 

public class BathroomMission : MonoBehaviour
{
    private int _clearCount = 5;

    private MissionController _missionController;
    private MissionState _missionState;

    private Dictionary<string, Component> _strainDict = new Dictionary<string, Component>();

    private void Awake() => Init();

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
    }


    private void Update()
    {
        _missionController.PlayerInput();
        RemoveTrain();
    }

    private void RemoveTrain()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject go = _missionController._searchObj.gameObject;

            if (go == null)
                return;

            Image image = go.GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.35f);

            if(image.color.a < 0)
            {
                _clearCount--;

                //미션 클리어 점수 증가 
            }

        }

    }

}
