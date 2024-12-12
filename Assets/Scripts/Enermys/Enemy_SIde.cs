using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_SIde : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float moveDistance;
    [SerializeField] private float speed;
    private bool moveLeft;
    private float leftEdge;
    private float rightEdge;
    void Start()
    {
        leftEdge = transform.position.x - moveDistance;
        rightEdge = transform.position.x + moveDistance;
        moveLeft = true;
    }
    void Update()
    {
        if (moveLeft)
        {
            if (transform.position.x > leftEdge)
            {
                transform.position = new Vector3(transform.position.x - speed*Time.deltaTime,transform.position.y,transform.position.z);
            }
            else
                moveLeft = false;
        }

        else
        {
            if (transform.position.x < rightEdge)
            {
                transform.position = new Vector3(transform.position.x + speed*Time.deltaTime,transform.position.y,transform.position.z);
            }
            else
                moveLeft = true;
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            collider.GetComponent<Health>().ChangeHealth(-damage);
        }
        
    }
}
