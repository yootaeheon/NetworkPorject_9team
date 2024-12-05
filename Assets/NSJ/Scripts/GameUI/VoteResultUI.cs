using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIs
{
    public class VoteResultUI : BaseUI
    {
        public enum Result { Kick, Skip }

        [SerializeField] public float _duration;

        private Image _playerImage => GetUI<Image>("Goose");
        private TMP_Text _nameText => GetUI<TMP_Text>("NameText");
        private TMP_Text _jobText => GetUI<TMP_Text>("JobText");
        private void Awake()
        {
            Bind();

        }
        private void Start()
        {
            SetActiveKick(false) ;
            SetActiveSkip(false);
        }

        public void SetActiveKick(bool value)
        {
            GetUI("VoteKickUI").SetActive(value);
            if (value)
            {
                StartCoroutine(DurationKickRoutine());
            }
        }

        public void SetActiveSkip(bool value)
        {
            GetUI("VoteSkipUI").SetActive(value);
            if (value)
            {
                StartCoroutine(DurationSkipRoutine());
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
        IEnumerator DurationKickRoutine()
        {
            yield return _duration.GetDelay();
            GetUI("VoteKickUI").SetActive(false);
        }

        /// <summary>
        /// 지속시간동안만 나타남
        /// </summary>
        IEnumerator DurationSkipRoutine()
        {
            yield return _duration.GetDelay();
            GetUI("VoteSkipUI").SetActive(false);
        }
    }
}

