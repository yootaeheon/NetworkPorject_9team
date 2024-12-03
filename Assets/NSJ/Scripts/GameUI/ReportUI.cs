using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIs
{
    public class ReportUI :BaseUI
    {
        [SerializeField] public float Duration;


        private Image _reporter => GetUI<Image>("PlayerHeadImage");
        private Image _corpse => GetUI<Image>("PlayerCorpseImage");
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
            GetUI("CorpseReportUI").SetActive(value);
            if (value == true)
            {
                StartCoroutine(DurationRoutine());
            }
        }

        /// <summary>
        /// 색 지정
        /// </summary>
        /// <param name="reporterColor"></param>
        /// <param name="corpseColor"></param>
        public void SetColor(Color reporterColor, Color corpseColor)
        {
            _reporter.color = reporterColor;
            _corpse.color = corpseColor;
        }

        /// <summary>
        /// 지속시간동안만 나타남
        /// </summary>
        IEnumerator DurationRoutine()
        {
            SoundManager.SFXPlay(SoundManager.Data.Report);
            yield return Duration.GetDelay();
            GetUI("CorpseReportUI").SetActive(false);
        }
    }
}

