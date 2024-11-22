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

    // ∞°∏∏»˜ ¿÷¿∏∏È ¡‹ ∂Ø∞‹¡ˆ∞‘ 
    // ±◊∏≤¿⁄ ? 
    private void Start()
    {
        cam = Camera.main.transform;
        Camera.main.orthographicSize = camSize; // ƒ´∏ﬁ∂Û ªÁ¿Ã¡Ó ¡∂¡§ 
    }
    private void LateUpdate()
    {
        FollowPlayer();
        MoveCheck();
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
   
    IEnumerator ZoomInZoomOut()  // øÚ¡˜¿Ã∏È ¡‹ æ∆øÙ ∏ÿ√ﬂ∏È ¡‹ ¿Œ 
    {
        WaitForSeconds time = new WaitForSeconds(0.05f);

        if (isOnMove == true)
        {
            while (camSize <= camSizeMax)
            {
                Debug.Log("¡‹æ∆øÙ");
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
                Debug.Log("¡‹¿Œ");
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
