using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using UnityEngine.UIElements;
public class PlayerController : MonoBehaviourPun
{
    // 플레이어 타입 거위(시민) 오리(임포스터) 
    [SerializeField] PlayerType playerType;
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
        playerType = PlayerType.Goose;   // 랜덤으로 역할 지정하는 기능이 필요 (대기실 입장에는 필요없고 게임 입장시 필요)

        count = PhotonNetwork.ViewCount - 1;  // 들어온 순서대로 색 지정 
        body.color = colors[count];
        Debug.Log($"플레이어 넘버{count}");

    }
    private void Update()
    {
        if (photonView.IsMine == false)  // 소유권자 구분
            return;

        Move();
        MoveCheck(); 
    }


    // 오버랩 스피어로 주변 오브젝트 탐색(미니게임 , 사보타지 , 시체 , 긴급회의 , 다른 플레이어) 탐색된 오브젝트에 따라 다른 행동이 가능하게

    private void FindNearObject() 
    {

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
