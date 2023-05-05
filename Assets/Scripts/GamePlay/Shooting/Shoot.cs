using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;        //Photon.Pun

public class Shoot : MonoBehaviour, IPunObservable
{
    public AudioClip NoBullet;
    public AudioClip ReloadSound;
    public AudioClip ShootSound;
    public AudioClip HitSound;
    public Transform GunPoint;
    public Camera MainCamera;
    public GameObject BulletPrefab;
    public Gun currentGun;
    public Text AmmoText;

    bool canShoot;
    bool reloading;
    int firstAmmo;
    int secondAmmo;

    private bool isShooting;    //Photon.Pun
    private PhotonView view;     //Photon.Pun


    private void Start()
    {
        view = GetComponent<PhotonView>();       //Photon.Pun
        canShoot = true;
        reloading = false;

        if (MainCamera == null)
            MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        if (AmmoText == null)
            AmmoText = GameObject.Find("AmmoText").GetComponent<Text>();

        ChangeGun(currentGun);
    }

    private void Update()
    {
        if (GameManager.instance.chat.textMessage.text != "")
            return;

        if (!GameManager.instance.canShoot)
            return;

        //Photon.Pun

        if (view.IsMine)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (canShoot)
                    Shooting();
            }
            else
                isShooting = false;

            if (Input.GetKey(KeyCode.R))
            {
                if (firstAmmo != currentGun.firstAmmo)
                    if (!reloading)
                        StartCoroutine(Reload());
            }
        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isShooting);
        }
        else
        {
            isShooting = (bool)stream.ReceiveNext();
        }
    }

    void ChangeGun(Gun gun)
    {
        firstAmmo = gun.firstAmmo;
        secondAmmo = gun.secondAmmo;

        UpdateAmmoUI();
    }

    void PlayShootSound()
    {
        SoundManager.instance.PlaySingle(ShootSound);
    }

    void PlayReloadSound()
    {
        SoundManager.instance.PlaySingle(ReloadSound);
    }

    void PlayNoBulletSound()
    {
        SoundManager.instance.PlaySingle(NoBullet);
    }

    public void PlayHitSound()
    {
        SoundManager.instance.PlaySingle(HitSound);
    }


    void Shooting()
    {
        if (SpendAmmo() == false)       //если нет патронов то не стреляем
            return;
        if (reloading)
            return;

        Vector2 mousePos = MainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        GameObject bullet = SpawnBullet();
        bullet.AddComponent<Bullet>().shoot = this;
        bullet.GetComponent<Bullet>().ShooterName = view.Owner.NickName;
        isShooting = true;                               //Photon.Pun
        PlayShootSound();


        StartCoroutine(FireRate());
    }

    GameObject SpawnBullet()
    {
        GameObject bullet;
        bullet = PhotonNetwork.Instantiate(BulletPrefab.name, GunPoint.transform.position, Quaternion.identity);
        return bullet;
    }

    IEnumerator FireRate()
    {
        canShoot = false;
        yield return new WaitForSeconds(currentGun.fireRate);
        canShoot = true;
    }

    IEnumerator Reload()
    { 
        canShoot = false;
        reloading = true;
        PlayReloadSound();
        yield return new WaitForSeconds(currentGun.reloadTime);
        
        if (secondAmmo < currentGun.firstAmmo)
        {
            firstAmmo = secondAmmo;
            secondAmmo = 0;
        }
        else
        {
            int difference = currentGun.firstAmmo - firstAmmo;
            firstAmmo = currentGun.firstAmmo;
            secondAmmo -= difference;
        }
        reloading = false;
        canShoot = true;

        UpdateAmmoUI();
    }

    bool SpendAmmo()        
    {
        if (firstAmmo != 0)
        {
            firstAmmo--;
            UpdateAmmoUI();
            return true;
        }
        PlayNoBulletSound();
        canShoot = false;
        return false;

    }

    void UpdateAmmoUI()
    {
        AmmoText.text = firstAmmo + "/" + secondAmmo;
    }

}
