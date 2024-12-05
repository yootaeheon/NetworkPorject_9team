using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIs
{
    public class GameStartUI : BaseUI
    {
        [SerializeField] public float Duration;

        private GameObject _gooseUI => GetUI("GooseBackGround");
        private GameObject _duckUI => GetUI("DuckBackGround");

        private Image _playerImage => GetUI<Image>("PlayerBody");

        private void Awake()
        {
            Bind();
        }
        private void Start()
        {
            SetActive(false);
        }

        public void SetActive(bool value)
        {
            GetUI("GameStartUI").SetActive(value);
            if (value == true)
            {
                StartCoroutine(DurationRoutine());
            }
        }

        /// <summary>
        /// 거위 화면 또는 오리 화면 띄우기
        /// 플레이어 색상 또한 지정
        /// </summary>
        public void SetUI(PlayerType type, Color color)
        {
            SoundManager.SFXPlay(type == PlayerType.Goose ? SoundManager.Data.GooseIntro : SoundManager.Data.DuckIntro);

            _gooseUI.SetActive(type == PlayerType.Goose);
            _duckUI.SetActive(type == PlayerType.Duck);

            _playerImage.color = color;
        }

        /// <summary>
        /// 지속시간동안만 나타남
        /// </summary>
        IEnumerator DurationRoutine()
        {
            yield return Duration.GetDelay();
            GetUI("GameStartUI").SetActive(false);
        }
    }
}

