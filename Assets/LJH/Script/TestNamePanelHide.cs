using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class TestNamePanelHide : MonoBehaviour
{
    [SerializeField] float Detectradius = 30;
    [SerializeField] GameObject[] namePanels;



    private void Start()
    {
        StartCoroutine(findNameobj());
       // StartCoroutine(hideNamePanel());
    }
    IEnumerator findNameobj() 
    {
        yield return 0.5f.GetDelay();
        namePanels = GameObject.FindGameObjectsWithTag("Test");
    }

    private void Update()
    {   
      FindNamePanel3();
    }
    private void FindNamePanel3() 
    {
        for (int i = 0; i < namePanels.Length; i++)
        {
            if (Detectradius > Vector2.Distance(transform.position, namePanels[i].transform.position))
            {
                Vector2 dir = namePanels[i].transform.position - transform.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Detectradius);

                Debug.DrawLine(transform.position, namePanels[i].transform.position, Color.cyan);
                namePanels[i].layer = 0;

                if (hit.collider != null)
                {
                    if (hit.collider.gameObject == namePanels[i]) 
                    {
                        hit.collider.gameObject.layer = 0;
                    }
                }
                else 
                {
                    namePanels[i].layer = 11;
                }
            }
            else
            {
                namePanels[i].layer = 11;
              
            }
        }
    }
}
