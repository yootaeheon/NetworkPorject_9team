using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PhotonView))]
public class SceneChanger : MonoBehaviourPun
{
    public static SceneChanger Instance;

    private void Awake()
    {
        InitSingleTon();
    }

    public void LoadScene(string scene, LoadSceneMode loadSceneMode)
    {
        photonView.RPC(nameof(RPCLoadScene), RpcTarget.AllViaServer, scene, (int)loadSceneMode);
    }
    public void UnLoadScene(string scene)
    {
        photonView.RPC(nameof(RPCUnLoadScene), RpcTarget.AllViaServer, scene);
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
