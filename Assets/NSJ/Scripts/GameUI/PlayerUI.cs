using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIs
{
    public class PlayerUI : BaseUI
    {
        public Button KillButton => GetUI<Button>("KillButton");
        public Button SabotageButton => GetUI<Button>("SabotageButton");
        public Button EnterVentButton => GetUI<Button>("EnterVentButton");
        public Button ExitVent => GetUI<Button>("ExitVentButton");
        public Button MissionButton => GetUI<Button>("MissionButton");
        public Button ReportButton => GetUI<Button>("ReportButton");


        private GameObject _gooseUI => GetUI("GooseUI");
        private GameObject _duckUI => GetUI("DuckUI");

        private void Awake()
        {
            Bind();
        }

        public void SetActive(bool value)
        {
            GetUI("PlayerUI").SetActive(value);
        }
        public void SetUI(PlayerType type)
        {
            _gooseUI.SetActive(type == PlayerType.Goose);
            _duckUI.SetActive(type == PlayerType.Duck);
        }
    }
}

