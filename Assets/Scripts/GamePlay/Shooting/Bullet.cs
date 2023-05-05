using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    public string ShooterName;
    public Shoot shoot;

    float power;

    Vector3 pos;

    void Start()
    {
        pos = shoot.GunPoint.transform.right;
        power = shoot.currentGun.bulletSpeed;
    }

    private void Update()
    {
        transform.Translate(pos * power * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            PhotonNetwork.Destroy(gameObject);
            shoot.PlayHitSound();
        }
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("HIT!");
            shoot.PlayHitSound();
            Destroy(gameObject);
        }
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player HIT!");
            Player player = collision.GetComponent<Player>();
            PhotonView view = collision.GetComponent<PhotonView>();

            player.GetDamage(shoot.currentGun.damage, ShooterName);
            shoot.PlayHitSound();
        }
    }



}
