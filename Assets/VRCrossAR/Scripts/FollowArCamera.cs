using UnityEngine;
using Photon.Pun;

//This script is attached to the AR mesh (camera) so it follows the position of it
public class FollowArCamera : MonoBehaviour
{

    PhotonView pv;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine) return;
        transform.position = PlatformManager.instance.arCamera.position;
        transform.rotation = PlatformManager.instance.arCamera.rotation;
    }
}
