using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using UnityEngine.UI;

public class oyuncu : MonoBehaviour
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
    void Start()
    {
        pw = GetComponent<PhotonView>();
        _anim = GetComponent<Animator>();
        nick.text = pw.Owner.NickName;
        saglikText.text = saglik.ToString();

        CameraWork _cameraWork = GetComponent<CameraWork>();

        if (_cameraWork != null)
        {
            if (pw.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
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
   
}
