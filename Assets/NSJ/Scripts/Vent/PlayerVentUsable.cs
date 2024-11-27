using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerVentUsable : MonoBehaviourPun
{

    private Vent _vent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_enterTriggerRoutine == null)
        {
            _enterTriggerRoutine = StartCoroutine(EnterTriggerRoutine(collision));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(_enterTriggerRoutine != null)
        {
            StopCoroutine(_enterTriggerRoutine);
            _enterTriggerRoutine = null;
        }
    }

    Coroutine _enterTriggerRoutine;
    IEnumerator EnterTriggerRoutine(Collider2D collision)
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Vent vent = collision.GetComponent<Vent>();
                if (vent == null)
                    yield break;
                EnterVent(vent);
            }
            yield return null;  
        }
    }

    private void EnterVent(Vent vent)
    {
        _vent = vent;

        // RPC 아닌거
        Camera.main.transform.SetParent(vent.transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);

        // RPC
        Vector2 tempPos = new Vector2(10000, 10000);
        transform.position = tempPos;
        vent.Enter();

        if (_enterVentRoutine == null)
            _enterVentRoutine = StartCoroutine(EnterVentRoutine());
    }

    Coroutine _enterVentRoutine;
    IEnumerator EnterVentRoutine()
    {
        while (true)
        {
            yield return null;
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ExitVent();
                _enterVentRoutine = null;
                yield break;
            }       
        }
    }

    private void ExitVent()
    {
        // RPC 아닌거
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);

        // RPC
        transform.position = _vent.transform.position;
        _vent.Exit();
    }
}
