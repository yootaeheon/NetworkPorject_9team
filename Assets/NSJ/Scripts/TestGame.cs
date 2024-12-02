using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSJ_Test
{
    public class TestGame : MonoBehaviour
    {
        public static TestGame Instance;

        [SerializeField] private bool _isTest;

        private void Awake()
        {
            if (_isTest == false)
            {
                Destroy(gameObject);
                return;
            }
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
