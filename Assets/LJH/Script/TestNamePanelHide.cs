using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class TestNamePanelHide : MonoBehaviour
{
    [SerializeField] float Detectradius = 10;
    [SerializeField] GameObject[] namePanels;
    [SerializeField] LayerMask layerMask = (1 << 0) | (1 << 8) | (1 << 10) | (1 << 14) | (1 << 15) | (1 << 16); // 레이어 마스크 

    private int num = PhotonNetwork.LocalPlayer.GetPlayerNumber();

    private void Start()
    {
        num = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        StartCoroutine(findNameobj());
    }
    IEnumerator findNameobj()
    {
        yield return 3f.GetDelay();
        namePanels = GameObject.FindGameObjectsWithTag("NamePanel");
        StartCoroutine(DelayFindNamePanel());
    }
    IEnumerator DelayFindNamePanel()
    {   
        
        while (true)
        {
            yield return 0.3f.GetDelay();
            if (LobbyScene.Instance == null)
                FindNamePanel();
        }
    }

    private void FindNamePanel()
    {
        for (int i = 0; i < namePanels.Length; i++)
        {
            if (namePanels[i] == null)
                return;
            Vector2 myPos = new Vector2(transform.position.x, transform.position.y - 1.5f);
            Vector2 targetPos = namePanels[i].transform.position;

           
            if (Detectradius > Vector2.Distance(myPos, targetPos))
            {
                
                RaycastHit2D hit = Physics2D.Linecast(myPos, targetPos,layerMask);
                Debug.DrawLine(myPos, targetPos, Color.red);

                if (hit.collider != null&& hit.collider.gameObject)
                {
                    
                    if (hit.collider.gameObject == namePanels[i])
                    {
                        Debug.Log("시야 보임");
                        namePanels[i].layer = 16;
                    }
                    else
                    {
                        
                        Debug.Log("다른 오브젝트에 막힘");
                        namePanels[i].layer = 15;
                    }
                }
                else
                {
                    
                    Debug.Log("히트 없음");
                    namePanels[i].layer = 15;
                }
            }
            else
            {
                
                namePanels[i].layer = 15;
            }
        }



    }
}
