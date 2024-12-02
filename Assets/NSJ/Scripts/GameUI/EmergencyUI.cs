using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIs
{
    public class EmergencyUI : BaseUI
    {
        [SerializeField] public float Duration;

        private Image _playerArm => GetUI<Image>("PlayerArmImage");

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
            GetUI("EmergencyCallUI").SetActive(value);
            if (value == true)
            {
                StartCoroutine(DurationRoutine());
            }
        }

        public void SetColor(Color playerColor)
        {
            _playerArm.color = playerColor;
        }

        /// <summary>
        /// 지속시간동안만 나타남
        /// </summary>
        IEnumerator DurationRoutine()
        {
            yield return Duration.GetDelay();
            GetUI("EmergencyCallUI").SetActive(false);
        }
    }

}
