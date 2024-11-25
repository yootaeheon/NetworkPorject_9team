using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportingObject : MonoBehaviourPun
{
    public void DeleteObject() 
    {

       Destroy(gameObject);
    }
}
