using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    public float speed;     //от 0 до 1
    public float time;
    public float moveX;
    public float moveY;

    Vector2 defaultPosition;

    bool movingBack;
 

    private void Start()
    {
        defaultPosition = gameObject.transform.position;
        StartCoroutine(MoveToPosition());

    }

    private void Update()
    {
        if (!movingBack)
            StartCoroutine(MoveToPosition());
        else
            StartCoroutine(MoveBack());

    }

    private IEnumerator MoveToPosition()
    {
        transform.position = Vector2.Lerp(transform.position, new Vector2(defaultPosition.x + moveX, defaultPosition.y + moveY), speed * Time.deltaTime);
        yield return new WaitForSeconds(time);
        movingBack = true;
    }
    private IEnumerator MoveBack()
    {
        transform.position = Vector2.Lerp(transform.position, defaultPosition, speed * Time.deltaTime);
        yield return new WaitForSeconds(time);
        movingBack = false;
    }
}

