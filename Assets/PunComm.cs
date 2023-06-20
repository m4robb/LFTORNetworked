using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PunComm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartPerformance()
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("ChatMessage", RpcTarget.All, "jup", "and jup.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
