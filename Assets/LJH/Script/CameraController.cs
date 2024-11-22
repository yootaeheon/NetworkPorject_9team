using Photon.Pun;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class CameraController : MonoBehaviourPun
{
    [SerializeField] Vector3 offset;

    [SerializeField] float camSize;
    [SerializeField] float camSizeMax = 10f;
    [SerializeField] float camSizeMin = 5f;

    public Transform cam;

    
    private Vector3 privPos;
    private float threshold = 0.001f;
    private bool isOnMove = false;



    
    // 가만히 있으면 줌 땡겨지게 
    // 그림자 ? 
    private void Start()
    {
        cam = Camera.main.transform;
        Camera.main.orthographicSize = camSize; // 카메라 사이즈 조정 

       
    }
    private void LateUpdate()
    {   
        FollowPlayer();
        if (photonView.IsMine == true)
        {
            MoveCheck();
            if (transform.GetComponent<PlayerController>().isGhost == true)
            {
                Camera.main.cullingMask = -1;
            }

        }
    }
    private void FollowPlayer()
    {

        if (photonView.IsMine == true)
        {
            cam.transform.position = transform.position + offset;
        }
        

    }

    private void MoveCheck()
    {

        float distance = Vector3.Distance(transform.position, privPos);

        if (distance > threshold)
        {
            if (!isOnMove)
            {
               
                isOnMove = true;
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = StartCoroutine(ZoomInZoomOut());

            }
        }
        else
        {
            if (isOnMove) 
            {
               
                isOnMove = false;
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = StartCoroutine(ZoomInZoomOut());
            }
        }
        privPos = transform.position;
    }

    Coroutine coroutine;
   
    IEnumerator ZoomInZoomOut()  // 움직이면 줌 아웃 멈추면 줌 인 
    {
        WaitForSeconds time = new WaitForSeconds(0.05f);

        if (isOnMove == true)
        {
            while (camSize <= camSizeMax)
            {
                
                yield return time;
                camSize += 0.05f;
                Camera.main.orthographicSize = camSize;
                if (isOnMove == false)
                {
                    break;
                }
            }
        }
        else if(isOnMove == false) 
        {
            while (camSize >= camSizeMin) 
            {
               
                yield return time;
                camSize -= 0.05f;
                Camera.main.orthographicSize = camSize;
                if (isOnMove == true) 
                {
                    break;
                }
            }
        }

    }
}
