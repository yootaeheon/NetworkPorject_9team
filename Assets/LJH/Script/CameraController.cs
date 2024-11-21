using Photon.Pun;
using UnityEngine;


public class CameraController : MonoBehaviourPun
{
    [SerializeField] Vector3 offset;

    public Transform cam;
    

    private void Start()
    {
        cam = Camera.main.transform;
    }
    private void LateUpdate()
    {
        FollowPlayer();
    }
    private void FollowPlayer()
    {

        if (photonView.IsMine == true)
        {
            cam.transform.position = transform.position + offset;

        }

    }
}
