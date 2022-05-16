using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class oyuncu : MonoBehaviourPunCallbacks
{
    public GameObject parctefek;
    private PhotonView pw;
    private Animator _anim;
    public TMP_Text nick;
    public TMP_Text saglikText;
    public GameObject canvas;
    public float saglik = 100f;
    public Slider saglikBar;
    public GameObject firePoint;
    private Hashtable props;
    void Start()
    {
        pw = GetComponent<PhotonView>();
        _anim = GetComponent<Animator>();
        nick.text = pw.Owner.NickName;
        saglikText.text = saglik.ToString();
        /*
        CameraWork _cameraWork = GetComponent<CameraWork>();

        if (_cameraWork != null)
        {
            if (pw.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        */
        PhotonNetwork.LocalPlayer.AddScore(0);
        props = new Hashtable()
        {
            {"sira",1},
            {"kazanma",0},
            {"kaybetme",0}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    // Update is called once per frame
    void Update()
    {
        if (pw.IsMine)
        {
            canvas.transform.rotation = Quaternion.Euler(90,0,0);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       
            RaycastHit hit;
        
            if (Physics.Raycast(ray, out hit))        {
           
                Vector3 dir = hit.point - transform.position;            
                dir.y = 0;             
                transform.rotation = Quaternion.LookRotation(dir * Time.deltaTime * 2f);         
                Debug.DrawLine(transform.position, hit.point);
            
            }
            float x = Input.GetAxis("Horizontal") * Time.deltaTime * 20f;
            float y = Input.GetAxis("Vertical") * Time.deltaTime * 20f;
            transform.Translate(x, 0, y);
       

            if (Input.GetButtonDown("Fire1"))
            {
                Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);

                //Instantiate(parctefek, transform.position, Quaternion.Euler(-90f, transform.eulerAngles.y, 0f));
                PhotonNetwork.Instantiate("AtesParct", firePoint.transform.position,
                    Quaternion.Euler(-90f, transform.eulerAngles.y, 0f));
                
            }

            if (Input.GetKey(KeyCode.E))
            {
                _anim.SetBool("buyu",true);
            }
            else
            {
                _anim.SetBool("buyu",false);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PhotonNetwork.LocalPlayer.SetScore(PhotonNetwork.LocalPlayer.GetScore()+1);
                Debug.Log("Skor arttırıldı.");
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log(PhotonNetwork.LocalPlayer.GetScore());
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("sira", out object siraDeger);
                Debug.Log(siraDeger);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                props.TryGetValue("sira", out object siraDeger);
                props["sira"] = (int)siraDeger + 1;
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
                Debug.Log("Sıra arttırıldı.");
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                PhotonNetwork.LocalPlayer.CustomProperties.Remove("sira");
                props.Remove("sira");
                Debug.Log("Sıra özelliği kaldırıldı");
            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                props.Add("sira",1);
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
                Debug.Log("Sıra özelliği eklendi");
            }

            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    p.CustomProperties.TryGetValue("sira", out object siraDeger);
                    Debug.Log("Nick: " + p.NickName + " Sıra: " + siraDeger);
                }
            }
        }
    }

    [PunRPC]
    void hasarAl(int damage)
    {
        saglik -= damage;
        saglikText.text = saglik.ToString();
        saglikBar.value = saglik;
        if (saglik <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        Debug.Log("Değer Güncellendi!");
    }
}
