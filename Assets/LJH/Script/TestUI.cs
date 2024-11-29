using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            GameUI.ShowPlayer(PlayerType.Goose);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            GameUI.ShowPlayer(PlayerType.Duck);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameUI.ShowGameStart(PlayerType.Goose);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            GameUI.ShowGameStart(PlayerType.Duck);
        }
    }
}
