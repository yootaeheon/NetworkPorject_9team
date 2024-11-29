using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPlayerController : MonoBehaviourPun
{
    [SerializeField] private float _moveSpeed = 5;

    private void Update()
    {
        if(photonView.IsMine == true)
        {
            Move();
        }
    }
    private void Move()
    {
        Vector3 moveDir = new Vector3();
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.y = Input.GetAxisRaw("Vertical");
        if (moveDir == Vector3.zero)
            return;

        transform.Translate(moveDir*_moveSpeed* Time.deltaTime, Space.World);
    }
}
