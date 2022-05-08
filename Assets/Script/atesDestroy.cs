using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class atesDestroy : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(yokEt());
    }

    IEnumerator yokEt()
    {
        yield return new WaitForSeconds(2f);
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.transform.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PhotonView>().RPC("hasarAl",RpcTarget.All,10);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
