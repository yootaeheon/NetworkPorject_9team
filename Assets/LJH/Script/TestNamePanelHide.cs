using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class TestNamePanelHide : MonoBehaviour
{
    [SerializeField] float Detectradius = 10;
    [SerializeField] GameObject[] namePanels;

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
            //if (PlayerDataContainer.Instance.GetPlayerData(num).IsGhost == true)
            //{

            //}
            //else 
            //{
                FindNamePanel();
            //}
        }
    }

    private void FindNamePanel() 
    {   
        for (int i = 0; i < namePanels.Length; i++)
        {
            Vector2 myPos = new Vector2(transform.position.x, transform.position.y - 1.5f);
            Vector2 targetPos = new Vector2(namePanels[i].transform.position.x, namePanels[i].transform.position.y - 1.5f);
            if (Detectradius > Vector2.Distance(myPos, targetPos))
            {
               
                namePanels[i].layer = 16;
                Vector2 dir = targetPos - myPos;
                RaycastHit2D hit = Physics2D.Raycast(myPos, dir.normalized, Detectradius);

                if (hit.collider != null)
                {
                    if (hit.collider.gameObject == namePanels[i])
                    {
                        GameObject obj = hit.collider.gameObject;
                        obj.layer = 16;


                    }
                }
            }
            else
            {   
             
                namePanels[i].layer = 15;
              
            }
        }
    }
}
