using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeHead : EnemyDamage
{
   [Header("SpikeHead Attributes")]
    [SerializeField] private int activeTime;
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private bool doc;
    [SerializeField] private bool ngang;
    private Vector3[] directions;
    private Vector3 destination;
    private float checkTimer;
    private bool attacking;

    private void OnEnable()
    {
        Stop();
    }
    private void Awake()
    {
        if(doc == true&&ngang == true)
            directions = new Vector3[4];
        else
            directions = new Vector3[2];

    }
    void FixedUpdate()
    {
            if (attacking)
                transform.Translate(destination * Time.deltaTime * speed);
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
                CheckForPlayer();
        }
    }
    private void CheckForPlayer()
    {
        CalculateDirections();
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if (hit.collider != null && !attacking &&activeTime>0)
            {
                attacking = true;
                destination = directions[i];
                checkTimer = 0;
                activeTime--;
            }
        }
    }
    private void CalculateDirections()
    {
        if(doc == true && ngang == true)
        {
            directions[0] = transform.right * range; 
            directions[1] = -transform.right * range;
            directions[2] = transform.up * range; 
            directions[3] = -transform.up * range; 
        }
        else if(doc == true && ngang == false)
        {
            directions[0] = transform.up * range; 
            directions[1] = -transform.up * range;
        }
        else if(ngang == true && doc ==false)
        {
            directions[0] = transform.right * range; 
            directions[1] = -transform.right * range;
        }
        
    }
    private void Stop()
    {
        destination = transform.position; 
        attacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);
        if(collider.tag == "Enemy")
            return;
        Stop(); 
    }
}
