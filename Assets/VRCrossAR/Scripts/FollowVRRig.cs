using UnityEngine;
using Photon.Pun;

//This script is attached to the VR body, to ensure each part is following the correct tracker. This is done only if the body is owned by the player
//and replicated around the network with the Photon Transform View component
public class FollowVRRig : MonoBehaviour
{
    [SerializeField] Transform[] body;

    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine) return;
        for (int i = 0; i < body.Length; i++)
        {
            body[i].localPosition = PlatformManager.instance.vrRigParts[i].localPosition;
            body[i].localRotation = PlatformManager.instance.vrRigParts[i].localRotation;
        }
    }
}
