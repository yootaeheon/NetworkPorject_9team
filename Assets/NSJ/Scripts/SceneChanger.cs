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
    /// 포톤 LoadLevel
    /// </summary>
    /// <param name="scene"></param>
    public static void LoadLevel(int scene)
    {
        PhotonNetwork.LoadLevel(scene);
    }

    /// <summary>
    /// 포톤 LoadLevel
    /// </summary>
    /// <param name="scene"></param>
    public static void LoadLevel(string scene)
    {
        PhotonNetwork.LoadLevel(scene);
    }

    /// <summary>
    /// 네트워크 씬 로드
    /// </summary>
    public static void LoadScene(string scene, LoadSceneMode loadSceneMode)
    {
        PhotonView.RPC(nameof(RPCLoadSceneString), RpcTarget.All, scene, (int)loadSceneMode);
    }
    /// <summary>
    /// 네트워크 씬 로드
    /// </summary>
    public static void LoadScene(int scene,LoadSceneMode loadSceneMode)
    {
        PhotonView.RPC(nameof(RPCLoadSceneInt), RpcTarget.All, scene, (int)loadSceneMode);
    }
    /// <summary>
    /// 네트워크 씬 언로드
    /// </summary>
    /// <param name="scene"></param>
    public static void UnLoadScene(string scene)
    {
        PhotonView.RPC(nameof(RPCUnLoadSceneString), RpcTarget.All, scene);
    }
    /// <summary>
    /// 네트워크 씬 언로드
    /// </summary>
    /// <param name="scene"></param>
    public static void UnLoadScene(int scene)
    {
        PhotonView.RPC(nameof(RPCUnLoadSceneInt), RpcTarget.All, scene);
    }

    [PunRPC]
    public void RPCLoadSceneString(string scene, int loadSceneMode)
    {
        SceneManager.LoadSceneAsync(scene, (LoadSceneMode)loadSceneMode);
    }
    [PunRPC]
    public void RPCLoadSceneInt(int scene, int loadSceneMode)
    {
        SceneManager.LoadSceneAsync(scene, (LoadSceneMode)loadSceneMode);
    }
    [PunRPC]
    public void RPCUnLoadSceneString(string scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }
    [PunRPC]
    public void RPCUnLoadSceneInt(int scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }


    private void InitSingleTon()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
