using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        private GameObject _killCoolTimeIcon => GetUI("KillCoolTimeIcon");
        private TMP_Text _killCoolTimeText => GetUI<TMP_Text>("KillCoolTimeText");

        private void Awake()
        {
            Bind();           
        }
        private IEnumerator Start()
        {
            SetActive(false);

            while (true) 
            {
                if(GameLoadingScene.MyPlayerController != null)
                {
                    GameLoadingScene.MyPlayerController.OnChangeRemainCoolDownEvent += UpdateKillCoolTime;
                    UpdateKillCoolTime(GameLoadingScene.MyPlayerController.RemainCoolDown);
                }
                yield return null;
            }
        }

        /// <summary>
        /// 킬쿨타임 업데이트
        /// </summary>
        private void UpdateKillCoolTime(float value)
        {
            _killCoolTimeIcon.SetActive(GameLoadingScene.MyPlayerController.RemainCoolDown > 0);
            _killCoolTimeText.SetText($"{value}");
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

