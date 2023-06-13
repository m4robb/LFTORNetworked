using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using System.Collections;
using Photon.Voice.PUN;

//This script handles the two different modes: AR & VR
public class PlatformManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject vrRig;
    [SerializeField] GameObject arRig;
    [SerializeField] GameObject arSession;
    [SerializeField] GameObject cameraMesh;
    [SerializeField] GameObject photonVoiceSetup;

    public Transform[] vrRigParts;
    public Transform arCamera;

    [SerializeField] GameObject vrBody;
    enum Mode { VR, AR};
    [Tooltip("Choose the mode before building, also you should change the XR Plugin Manager settings")]
    [SerializeField] Mode mode;

    public static PlatformManager instance;

    void Awake()
    {
        //If not connected go to lobby to connect
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(0);
        }
        instance = this;
    }

    private void Start()
    {
        if(mode == Mode.VR)
        {
            vrRig.SetActive(true);
            CreateVRBody();
        }
        else if(mode == Mode.AR)
        {
            arSession.SetActive(true);
            arRig.SetActive(true);
            CreateARBody();
        }
    }

    void CreateVRBody()
    {
        GameObject GOBody = PhotonNetwork.Instantiate(vrBody.name, vrRig.gameObject.transform.position, vrRig.gameObject.transform.rotation);
        GOBody.transform.parent = vrRig.transform;
        GameObject GOVoice= PhotonNetwork.Instantiate(photonVoiceSetup.name, vrRig.gameObject.transform.position, vrRig.gameObject.transform.rotation);
        //GOVoice.transform.parent = vrRig.transform;
    }

    void CreateARBody()
    {
        PhotonNetwork.Instantiate(cameraMesh.name, transform.position, transform.rotation);
        PhotonNetwork.Instantiate(photonVoiceSetup.name, transform.position, transform.rotation);
    }

    //If disconnected from server, return to lobby to reconnect.
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        GoToScene(0);
    }

    void GoToScene(int n)
    {
        StartCoroutine(LoadScene(n));
    }

    IEnumerator LoadScene(int n)
    {
        yield return new WaitForSeconds(0.5f);

        AsyncOperation async = SceneManager.LoadSceneAsync(n);
        async.allowSceneActivation = false;

        yield return new WaitForSeconds(1);
        async.allowSceneActivation = true;
        if (n == 0) //if going back to menu destroy instance
        {
            Destroy(gameObject);
        }
    }

    //So we stop loading scenes if we quit app
    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }
}
