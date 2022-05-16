using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;

public class SunucuYonetim : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI logText;
    public InputField kullaniciAdi;
    public InputField odaAdi;
    public GameObject girisPanel;
    public GameObject oyuncularPanel;
    public Text oyuncularText;
    public Text puanlarText;
    public Button odaKurButton;
    public Button randomButton;
    private bool canList = false;
    void Start()
    {        
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Bağlantı koptu");      
    }
    public override void OnConnectedToMaster()
    {

        Debug.Log("Server'e Bağlanıldı.");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {       

        Debug.Log("Lobiye bağlanıldı.");
        odaKurButton.interactable = true;
        randomButton.interactable = true;

    }  
    public override void OnJoinedRoom()
    {
        Debug.Log("Odaya Girildi.");
        canList = true;
        girisPanel.SetActive(false);
        PhotonNetwork.Instantiate("Oyuncu", Vector3.zero, Quaternion.identity);
    }
    public override void OnLeftLobby()
    {
        Debug.Log("Lobiden Çıkıldı.");

    }
    public override void OnLeftRoom()
    {
        Debug.Log("Odadan Çıkıldı.");      
    }  

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Odaya girilemedi." + message + " - " + returnCode);   
    
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Random Odaya girilemedi." + message + " - " + returnCode);

    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Oda oluşturulamadı." + message + " - " + returnCode);
    }

    public void odaKur()
    {
        PhotonNetwork.NickName = kullaniciAdi.text;
        PhotonNetwork.JoinOrCreateRoom(odaAdi.text, new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
    }

    public void randomKatil()
    {
        PhotonNetwork.NickName = kullaniciAdi.text;
        PhotonNetwork.JoinRandomRoom();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            if (canList)
            {
                oyuncularPanel.SetActive(true);
                oyuncularText.text = "Oyuncu Listesi\n";
                puanlarText.text = "Skor\n";
                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    if (p.IsMasterClient)
                    {
                        oyuncularText.text += p.NickName + " (Mod)\n";
                        puanlarText.text += p.GetScore() + "\n";
                    }
                    else
                    {
                        oyuncularText.text += p.NickName + "\n";
                        puanlarText.text += p.GetScore() + "\n";
                    }
                }
            }
        }
        else
        {
            oyuncularPanel.SetActive(false);
        }
    }
}
