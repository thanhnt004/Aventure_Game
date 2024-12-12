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
    private Vector3[] directions = new Vector3[4];
    private Vector3 destination;
    private float checkTimer;
    private bool attacking;

    private void OnEnable()
    {
        Stop();
    }
    private void Update()
    {
        //Move spikehead to destination only if attacking
        
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

        //Check if spikehead sees player in all 4 directions
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
        directions[0] = transform.right * range; //Right direction
        directions[1] = -transform.right * range; //Left direction
        directions[2] = transform.up * range; //Up direction
        directions[3] = -transform.up * range; //Down direction
    }
    private void Stop()
    {
        destination = transform.position; //Set destination as current position so it doesn't move
        attacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);
        if(collider.tag == "Enemy")
            return;
        Stop(); //Stop spikehead once he hits something
    }
}
