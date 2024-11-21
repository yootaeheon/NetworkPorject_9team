using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using UnityEngine.UIElements;
public class PlayerController : MonoBehaviourPun
{
    [SerializeField] float moveSpeed;
    [SerializeField] SpriteRenderer body;

    [SerializeField] Color[] colors;

    [SerializeField] Animator eyeAnim;
    [SerializeField] Animator bodyAnim;
    [SerializeField] Animator feetAnim;
    private int count;

    private Vector3 privPos;
    private Vector3 privDir;
    [SerializeField] float threshold = 0.001f;
    bool isOnMove = false;


    private void Start()
    {
        count = PhotonNetwork.ViewCount - 1;
        body.color = new Color(1f, 0f, 0f, 1f);
        Debug.Log($"플레이어 넘버{count}");

    }
    private void Update()
    {
        if (photonView.IsMine == false)  // 소유권자 구분
            return;

        Move();
        MoveCheck(); 
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 moveDir = new Vector2(x, y).normalized;

        if (moveDir == Vector2.zero)
            return;

        transform.Translate(moveDir * moveSpeed * Time.deltaTime);

        if (x < 0) // 왼쪽으로 이동 시
        {   
            privDir = new Vector3(1, 1, 1);
            transform.localScale = privDir;
        }
        else if (x > 0) // 오른쪽으로 이동 시
        {
            privDir = new Vector3(-1, 1, 1);
            transform.localScale = privDir;
        }
        else  // 이전 방향을 유지하게 
        {
            transform.localScale =privDir;  
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
                eyeAnim.SetBool("Running", true);
                bodyAnim.SetBool("Running", true);
                feetAnim.SetBool("Running", true);

            }
        }
        else
        {
            if (isOnMove) // 상태가 변경된 경우에만 애니메이션 업데이트
            {
                isOnMove = false;
                eyeAnim.SetBool("Running", false);
                bodyAnim.SetBool("Running", false);
                feetAnim.SetBool("Running", false);
            }
        }
        privPos = transform.position;
    }
}
