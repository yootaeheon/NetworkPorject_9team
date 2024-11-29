using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PhotonView))]
public class SceneChanger : MonoBehaviourPun
{
    public static SceneChanger Instance;

    private PhotonView _photonView;
    private static PhotonView PhotonView { get { return Instance._photonView; } }

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        InitSingleTon();
    }

    /// <summary>
    /// Æ÷Åæ LoadLevel
    /// </summary>
    /// <param name="scene"></param>
    public static void LoadLevel(int scene)
    {
        PhotonNetwork.LoadLevel(scene);
    }

    /// <summary>
    /// Æ÷Åæ LoadLevel
    /// </summary>
    /// <param name="scene"></param>
    public static void LoadLevel(string scene)
    {
        PhotonNetwork.LoadLevel(scene);
    }

    /// <summary>
    /// ³×Æ®¿öÅ© ¾À ·Îµå
    /// </summary>
    public static void LoadScene(string scene, LoadSceneMode loadSceneMode)
    {
        PhotonView.RPC(nameof(RPCLoadScene), RpcTarget.AllViaServer, scene, (int)loadSceneMode);
    }
    /// <summary>
    /// ³×Æ®¿öÅ© ¾À ¾ð·Îµå
    /// </summary>
    /// <param name="scene"></param>
    public static void UnLoadScene(string scene)
    {
        PhotonView.RPC(nameof(RPCUnLoadScene), RpcTarget.AllViaServer, scene);
    }

    [PunRPC]
    public void RPCLoadScene(string scene, int loadSceneMode)
    {
        SceneManager.LoadSceneAsync(scene, (LoadSceneMode)loadSceneMode);
    }
    [PunRPC]
    public void RPCUnLoadScene(string scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }


    private void InitSingleTon()
    {
        if (Instance == null)
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
