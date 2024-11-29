using Photon.Pun;
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
    /// 네트워크 씬 로드
    /// </summary>
    public static void LoadScene(string scene, LoadSceneMode loadSceneMode)
    {
        PhotonView.RPC(nameof(RPCLoadScene), RpcTarget.AllViaServer, scene, (int)loadSceneMode);
    }
    /// <summary>
    /// 네트워크 씬 언로드
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
