using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RotateWeapon : MonoBehaviour
{
    public float offset;
    Vector3 LocalScale;
    private PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (view.IsMine)
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotateZ + offset);

            LocalScale = Vector3.one;

            if (rotateZ > 90 || rotateZ < -90)
                LocalScale.y = -1f;
            else
                LocalScale.y = +1f;

            transform.localScale = LocalScale;
        }
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(transform.rotation);
    //        stream.SendNext(transform.localScale);
    //    }
    //    else
    //    {
    //        transform.rotation = (Quaternion)stream.ReceiveNext();
    //        transform.localScale = (Vector3)stream.ReceiveNext();
    //    }
    //}
}
