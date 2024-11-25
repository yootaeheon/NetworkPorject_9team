using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    // 플레이어 타입 거위(시민) 오리(임포스터) 
    [SerializeField] PlayerType playerType;
    [SerializeField] float moveSpeed;  // 이동속도 
    [SerializeField] float Detectradius;  // 탐색 범위



    [SerializeField] GameObject IdleBody;
    [SerializeField] GameObject Ghost;
    [SerializeField] GameObject Corpse;



    [SerializeField] SpriteRenderer body;
    [SerializeField] SpriteRenderer GhostRender;
    [SerializeField] SpriteRenderer CorpRender;
    [SerializeField] Color[] colors;
    [SerializeField] Animator eyeAnim;
    [SerializeField] Animator bodyAnim;
    [SerializeField] Animator feetAnim;

    private int count;

    private Vector3 privPos;
    private Vector3 privDir;
    [SerializeField] float threshold = 0.001f;

    bool isOnMove = false;
    public bool isGhost = false;


    private void Start()
    {

        // 랜덤으로 역할 지정하는 기능이 필요 (대기실 입장에는 필요없고 게임 입장시 필요)
        if (photonView.IsMine == false)  // 소유권자 구분
            return;
        playerType = PlayerType.Duck;


        // 랜덤 색 설정 , 추후에 색 지정 기능을 넣으면 랜덤 대신 지정 숫자를 보내면 될듯   
        Color randomColor = new Color(Random.value, Random.value, Random.value, 1f);
        photonView.RPC("RpcSetColors", RpcTarget.AllBuffered, randomColor.r, randomColor.g, randomColor.b);



    }
    private void Update()
    {
        if (photonView.IsMine == false)  // 소유권자 구분
            return;

        Move();
        MoveCheck();
        FindNearObject();
    }

    // r 신고 , e 상호작용 , space 살인 
    // 주변 오브젝트 탐색(미니게임 , 사보타지 , 시체 , 긴급회의 , 다른 플레이어) 탐색된 오브젝트에 따라 다른 행동이 가능하게
    // 신고가 되면 시체도 사라져야 함 
    private void FindNearObject()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, Detectradius);

        foreach (Collider2D col in colliders)
        {
            if (col.tag == "Test")
            {

                if (Input.GetKeyDown(KeyCode.R))
                {
                    Debug.Log("신고함!");
                    
                    // 투표 열리는 기능 추가 
                     
                    col.gameObject.GetComponent<ReportingObject>().Reporting(); //신고 
                }
            }
            else if (col.gameObject.layer == gameObject.layer)
            {
                if (col.gameObject != this.gameObject)
                {
                    if (playerType == PlayerType.Duck)
                    {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {

                            Debug.Log("죽임!"); // 쿨타임 추가해야함 

                            col.gameObject.GetComponent<PlayerController>().Die();
                        }

                    }

                }

            }

        }
    }
    public void Killing()
    {
        // 
    }
    


    public void Die()
    {
        StartCoroutine(switchGhost());
    }
    IEnumerator switchGhost()
    {

        Debug.Log(photonView.ViewID);
        Debug.Log(isGhost);

        photonView.RPC("RpcChildActive", RpcTarget.All, "GooseIdel", false);
        photonView.RPC("RpcChildActive", RpcTarget.All, "Goosecorpse", true);
        yield return new WaitForSeconds(1f);
        photonView.RPC("RpcChildActive", RpcTarget.All, "GoosePolter", true);
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
            transform.localScale = privDir;
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

    [PunRPC]
    private void RpcChildActive(string name, bool isActive)
    {

        if (name == "GooseIdel")
        {
            IdleBody.SetActive(isActive);
        }
        else if (name == "Goosecorpse")
        {
            Corpse.SetActive(isActive);
            Corpse.transform.SetParent(null);
        }
        else if (name == "GoosePolter")
        {
            isGhost = true;
            Ghost.SetActive(isActive);

        }
    }

    [PunRPC]

    private void RpcSetColors(float r, float g, float b)
    {
        Color color = new Color(r, g, b);
        body.color = color;
        GhostRender.color = color;
        CorpRender.color = color;
    }
}
