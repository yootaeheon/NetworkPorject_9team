using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;
using Photon.Pun.UtilityScripts;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviourPun
{
    // 플레이어 타입 거위(시민) 오리(임포스터) 
    [SerializeField] public PlayerType playerType;
    [SerializeField] float moveSpeed;  // 이동속도 
    [SerializeField] float Detectradius;  // 탐색 범위
    [SerializeField] LayerMask layerMask;
    //[SerializeField] string playerName;
    // [SerializeField] Color highLightColor;

    [SerializeField] AudioSource audioSource;

    [SerializeField] GameObject IdleBody;
    [SerializeField] GameObject Ghost;
    [SerializeField] GameObject Corpse;



    [SerializeField] SpriteRenderer body;
    [SerializeField] SpriteRenderer GhostRender;
    [SerializeField] SpriteRenderer CorpRender;

    [SerializeField] Animator eyeAnim;
    [SerializeField] Animator bodyAnim;
    [SerializeField] Animator feetAnim;

    [Header("Duck User 어빌리티 능력 팝업창")]
    [SerializeField] private GameObject _abilityObject;

    private int _playerNumber;
    private Vector3 privPos;
    private Vector3 privDir;
    [SerializeField] float threshold = 0.001f;
    private Color randomColor;
    bool isOnMove = false;
    public bool isGhost = false;


    Coroutine coroutine;
    private void Start()
    {

        // 랜덤으로 역할 지정하는 기능이 필요 (대기실 입장에는 필요없고 게임 입장시 필요)
        if (photonView.IsMine == false)  // 소유권자 구분
            return;



        // 랜덤 색 설정 , 추후에 색 지정 기능을 넣으면 랜덤 대신 지정 숫자를 보내면 될듯
        if (LobbyScene.Instance != null)
        {
            // 로비씬에서는 랜덤색
            randomColor = new Color(Random.value, Random.value, Random.value, 1f);
        }
        else
        {
            // 게임씬에서는 저장되있던 색
            randomColor = PlayerDataContainer.Instance.GetPlayerData(PhotonNetwork.LocalPlayer.GetPlayerNumber()).PlayerColor;
        }
        SettingColor(randomColor.r, randomColor.g, randomColor.b);

        StartCoroutine(SetPlayerDataRoutine());
       
    }
    /// <summary>
    /// 플레이어 데이터 세팅
    /// </summary>
    IEnumerator SetPlayerDataRoutine()
    {
        while (true)
        {
            int playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();
            if (playerNumber != -1)
            {
                photonView.RPC(nameof(RPCSetPlayerNumber), RpcTarget.All, playerNumber);
                PlayerDataContainer.Instance.SetPlayerData(playerNumber, PhotonNetwork.LocalPlayer.NickName, playerType, randomColor.r, randomColor.g, randomColor.b, false);
                yield break;
            }
            yield return null;
        }
    }

    /// <summary>
    /// 플레이어컨트롤러에 플레이어 넘버 RPC캐싱
    /// </summary>
    [PunRPC]
    private void RPCSetPlayerNumber(int playerNumber)
    {
        _playerNumber = playerNumber;
    }

    private void Update()
    {

        if (photonView.IsMine == false)  // 소유권자 구분
            return;

        Move();
        MoveCheck();
        FindNearObject();
        DuckAbilityInteraction();
    }

    /// <summary>
    /// 어빌리티 팝업창 오픈 기능
    /// </summary>
    private void DuckAbilityInteraction()
    {
        if (playerType.Equals(PlayerType.Duck))
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                _abilityObject.SetActive(true); 
            }
        }
    }

    public void SettingColor(float r, float g , float b) 
    {
        photonView.RPC("RpcSetColors", RpcTarget.AllBuffered, r,g,b);
    }
    // r 신고 , e 상호작용 , space 살인 
    // 주변 오브젝트 탐색(미니게임 , 사보타지 , 시체 , 긴급회의 , 다른 플레이어) 탐색된 오브젝트에 따라 다른 행동이 가능하게
    // 신고가 되면 시체도 사라져야 함 
    private Collider2D privNearCol = null;
    private Color privColor;
    private void FindNearObject()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, Detectradius, layerMask);
        Collider2D nearCol = null;
        float minDistance = 1000f;
        foreach (Collider2D col in colliders)
        {

            if (col.transform.position != this.transform.position) // 자신 아니고 
            {

                if (col != null)
                {
                    float distance = Vector2.Distance(this.transform.position, col.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;  // 가장 가까운 물체 찾기
                        if (playerType == PlayerType.Goose)
                        {
                            if (col.gameObject.layer == 10)
                            {
                                nearCol = null;
                            }
                            else
                                nearCol = col;
                        }
                        else
                            nearCol = col;
                    }
                }



            }
        }

        if (isGhost == false)
        {
            ObjectHighLight(nearCol);// 선택된 오브젝트 강조 및 선택
            PlayerAction(nearCol);// 선택 오브젝트에 대한 행동 
        }

    }

    private void ObjectHighLight(Collider2D nearCol)
    {
        if (nearCol != privNearCol)
        {

            if (privNearCol != null)
            {
                SpriteRenderer prevRenderer = privNearCol.GetComponent<SpriteRenderer>();
                if (prevRenderer != null)
                    // prevRenderer.color = privColor;    
                    //if (playerType == PlayerType.Goose)
                    //{
                    //    prevRenderer.enabled = true;
                    //}
                    //else
                    prevRenderer.enabled = false;
            }


            if (nearCol != null)
            {
                SpriteRenderer renderer = nearCol.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    // privColor = renderer.color; // 현재 색상 저장
                    // renderer.color = highLightColor; // 하이라이트 색상
                    //if (playerType == PlayerType.Goose)
                    //{
                    //    renderer.enabled = false;
                    //}
                    //else
                    renderer.enabled = true;

                }
            }

            // 현재 가장 가까운 물체를 이전 물체로 저장
            privNearCol = nearCol;
        }
    }

    private void PlayerAction(Collider2D nearCol)
    {
        if (nearCol != null)
        {
            if (nearCol.gameObject.layer == 13) // 나중에 시체용 레이어 더해서 사용하기 
            {

                if (Input.GetKeyDown(KeyCode.R))
                {
                    Debug.Log("신고함!");

                    // 투표 열리는 기능 추가 해야함 

                    nearCol.gameObject.GetComponent<ReportingObject>().Reporting(); //신고시 시체 삭제, 씬 재진입이면 필요없을지도 


                    //GameFlowManager.Instance.ReportingOn();
                    // 신고 On, UI 띄우기 및 씬 전환
                    photonView.RPC(nameof(RPCReport),RpcTarget.All);

                }
            }
            else if (nearCol.gameObject.layer == gameObject.layer)
            {
                coroutine = StartCoroutine(Kill(nearCol));
            }
            else if (nearCol.gameObject.layer == 8)  // 미션 
            {
                if (Input.GetKeyDown(KeyCode.E))
                {

                    nearCol.gameObject.GetComponent<ActiveMission>().GetMission(randomColor,playerType);
                }
            }
            else if (nearCol.gameObject.layer == 10) // 사보타지(임포스터만 가능 ) , 미션 함수 가져올 때 인수로 본인 컬러 넘겨줘야함 
            {
                if (playerType == PlayerType.Duck)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {

                        nearCol.gameObject.GetComponent<ActiveMission>().GetMission(randomColor, playerType);
                    }
                }
            }
        }
        else
        {
            Debug.Log("탐색중");
        }
    }
    IEnumerator Kill(Collider2D col)
    {
        // 로비씬에서는 킬 금지
        if (LobbyScene.Instance != null)
           yield break;

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
        yield return 1f.GetDelay();

    }

    [PunRPC]
    private void RPCReport()
    {
        StartCoroutine(ReportRoutine());
    }

    IEnumerator ReportRoutine()
    {
        GameUI.ShowReport(body.material.color, Random.ColorHSV());
        yield return 3f.GetDelay();
        if (PhotonNetwork.IsMasterClient == true)
        {
            SceneChanger.LoadScene("VoteScene", LoadSceneMode.Additive);
        }
    }

    public void Die()
    {
        StartCoroutine(switchGhost());
    }
    IEnumerator switchGhost()
    {
        photonView.RPC("RpcChildActive", RpcTarget.All, "GooseIdel", false);
        photonView.RPC("RpcChildActive", RpcTarget.All, "Goosecorpse", true);
        yield return new WaitForSeconds(1f);
        photonView.RPC("RpcChildActive", RpcTarget.All, "GoosePolter", true);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;  // 나중에 맵 보고 충돌 바꾸는걸로 해결하는게 나을듯
        PlayerDataContainer.Instance.UpdatePlayerGhostList(_playerNumber);
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
            privDir = new Vector3(0.7f, 0.7f, 0.7f);
            transform.localScale = privDir;

        }
        else if (x > 0) // 오른쪽으로 이동 시
        {
            privDir = new Vector3(-0.7f, 0.7f, 0.7f);
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
                audioSource.Play();

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
                audioSource.Stop();
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


    public void SetJobs()
    {
        Debug.Log("직업변경");
        photonView.RPC("RpcSetJobs", RpcTarget.All, PhotonNetwork.LocalPlayer.GetPlayerNumber());
       
    }
    [PunRPC]
    private void RpcSetJobs(int playerNumber) 
    {
        playerType = PlayerDataContainer.Instance.GetPlayerJob(playerNumber);

        
        // 내 플레이어만 해당 UI 실행
        if (photonView.IsMine == true)
        {
            GameUI.ShowPlayer(playerType);
        }
    }
}
