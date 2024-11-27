using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Vent : MonoBehaviourPun
{
    public Vent[] MoveableVents;

    public event UnityAction<Vent> OnChangeVentEvent;

    [SerializeField] private Transform _canvas;
    [SerializeField] private GameObject _dirArrowPrefab;

    private List<GameObject> _arrows = new List<GameObject> ();
    private Animator animator;
    private int animatorHash = Animator.StringToHash("Vent");


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void Enter()
    {
        // 갈수있는 벤트 수 만큼 생성
        foreach(Vent vent in MoveableVents)
        {
            GameObject arrow = Instantiate(_dirArrowPrefab, _canvas);
            Vector2 newPos = vent.transform.position - arrow.transform.position;
            float rotZ = Mathf.Atan2(newPos.y, newPos.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0,0,rotZ);

            arrow.transform.position = transform.position;
        }

        //RPC
        photonView.RPC(nameof(RPCVent), RpcTarget.AllViaServer);
    }

    public void Exit()
    {

        //RPC
        photonView.RPC(nameof(RPCVent), RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void RPCVent()
    {
        animator.Play(animatorHash);
    }
}
