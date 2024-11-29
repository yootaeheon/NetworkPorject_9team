using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIs
{
    public class VoteResultUI : BaseUI
    {
        [SerializeField] private float _duration;

        private Image _playerImage => GetUI<Image>("Goose");
        private TMP_Text _nameText => GetUI<TMP_Text>("NameText");
        private TMP_Text _jobText => GetUI<TMP_Text>("JobText");
        private void Awake()
        {
            Bind();
        }

        public void SetActive(bool value)
        {
            GetUI("VoteResultUI").SetActive(value);
            if (value)
            {
                StartCoroutine(DurationRoutine());
            }
        }
        public void SetUI(Color playerColor, string name, PlayerType type)
        {
            _playerImage.color = playerColor;
            _nameText.SetText($"{name}는 더 아름다운 세상으로 떠났습니다.");

            string jobText = type == PlayerType.Goose ? "오리가 아니었" : "오리였"; 
            _jobText.SetText($"그는 {jobText}습니다.");
        }

        /// <summary>
        /// 지속시간동안만 나타남
        /// </summary>
        IEnumerator DurationRoutine()
        {
            yield return _duration.GetDelay();
            GetUI("VoteResultUI").SetActive(false);
        }
    }
}

