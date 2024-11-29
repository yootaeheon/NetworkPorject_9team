using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUIs
{
    public class PlayerUI : BaseUI
    {

        private GameObject _gooseUI => GetUI("GooseUI");
        private GameObject _duckUI => GetUI("DuckUI");

        private void Awake()
        {
            Bind();
        }

        public void SetUI(PlayerType type)
        {
            _gooseUI.SetActive(type == PlayerType.Goose);
            _duckUI.SetActive(type == PlayerType.Duck);
        }
    }
}

