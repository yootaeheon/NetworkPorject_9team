using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.UI;

public class TestMove : MonoBehaviour
{
    [SerializeField] private float _movSpeed;
    [SerializeField] private Image _tempMission;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
     
    private void Update()
    {
 
        if (Input.GetKeyDown(KeyCode.E))
        {
            //미션 활성화
            _tempMission.gameObject.SetActive(true);
        }

        PlayerInput();


    }

    private void FixedUpdate()
    {
        MoveState();
    }

    private Vector2 _pos;
    private void PlayerInput()
    {
        _pos.x = Input.GetAxisRaw("Horizontal");
        _pos.y = Input.GetAxisRaw("Vertical");
       
    }

    private void MoveState()
    {
        _rb.MovePosition(_rb.position + _pos.normalized * _movSpeed *Time.fixedDeltaTime);
    }


}
