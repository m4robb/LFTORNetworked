using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Realtime;
using Photon.Pun;

public class PunComm : MonoBehaviour
{ 
    
    

    public UnityEvent EventStartPerformance;

    public void StartPerformanceReceive()
    {
        if (EventStartPerformance != null) EventStartPerformance.Invoke();
    }

    public void StartPerformanceSend()
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("StartPerformanceReceive", RpcTarget.All);
    }

}
