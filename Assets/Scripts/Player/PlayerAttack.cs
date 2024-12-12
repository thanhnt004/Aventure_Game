using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float atackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireBalls;
    [SerializeField] int NumOfAttack;
    private Animator anim;
    private float cooldownTimer; 
    private AudioManager audioManager;
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;
    }
    public void Attack()
    {
        if(cooldownTimer>atackCooldown&&NumOfAttack>0)
        {
        audioManager.PlaySFX(audioManager.atk);
        NumOfAttack--;
        anim.SetTrigger("atk");
        cooldownTimer = 0;
        fireBalls[findFireBall()].transform.position = firePoint.position;
        fireBalls[findFireBall()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
        }
    }
    private int findFireBall()
    {
        for(int i = 0;i<fireBalls.Length;i++)
        {
            if(!fireBalls[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
