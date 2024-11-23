using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackendManager : MonoBehaviour
{
    public static BackendManager Instance;

    private FirebaseApp _app;
    public static FirebaseApp App { get{ return Instance._app; } private set { Instance._app = value; } }
    private FirebaseAuth _auth;
    public static FirebaseAuth Auth { get { return Instance._auth; } private set { Instance._auth = value; } }
    private FirebaseDatabase _database;
    public static FirebaseDatabase DataBase { get { return Instance._database; }private set { Instance._database = value; } }

    private UserDate _user;
    public static UserDate User { get { return Instance._user; } set {Instance._user = value; } }

    private Dictionary<string, object> _SettingDic = new Dictionary<string, object>();
    public static Dictionary<string, object> SettingDic { get { return Instance._SettingDic; } }
    private void Awake()
    {
        InitSingleTon();
        CheckDependency();
    }

    /// <summary>
    /// 호환성 체크
    /// </summary>
    private void CheckDependency()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().
            ContinueWithOnMainThread(task =>
            {
                if(task.Result == DependencyStatus.Available) // 호환 가능?
                {
                    Debug.Log("BackendManager : 호환 체크 성공");
                    App = FirebaseApp.DefaultInstance;
                    Auth = FirebaseAuth.DefaultInstance;
                    DataBase = FirebaseDatabase.DefaultInstance;
                }
                else
                {
                    App = null;
                    Auth = null;
                    DataBase = null;
                }
            });
    }

    /// <summary>
    /// 싱글톤 세팅
    /// </summary>
    private void InitSingleTon()
    {
        if(Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
