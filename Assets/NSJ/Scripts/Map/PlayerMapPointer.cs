using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMapPointer : MonoBehaviour
{
    private Renderer _renderer;

    private PlayerController _myPlayer;
    private TextPlayerController _myTextPlayer;
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _myPlayer = GetComponentInParent<PlayerController>();

        if (_myPlayer != null)
        {
            SetPointer(_myPlayer);        
        }
        else
        {
            _myTextPlayer = GetComponentInParent<TextPlayerController>();
            if (_myTextPlayer != null)
            {
                SetPointer(_myTextPlayer);
            }
        }
    }

    private void SetPointer<T>(T player) where T :MonoBehaviourPun
    {
        if (player.photonView.IsMine == true)
        {
            _renderer.material.color = Color.green;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
