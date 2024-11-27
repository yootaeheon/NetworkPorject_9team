using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSJ_Test
{
    public class TestGameManager : MonoBehaviour
    {
        public static TestGameManager Instance;

        private Camera _miniMap;
        public static Camera MiniMap { get { return Instance._miniMap; } set { Instance._miniMap = value; } }


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }
    }
}
