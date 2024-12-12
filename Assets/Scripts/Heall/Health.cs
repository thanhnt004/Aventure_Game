using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float maxHealth;
    private float currentHealth;
    private Animator anim;
    private bool dead;
    public float heal {get => currentHealth ;}
    //
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numbeOfFlash;
    private SpriteRenderer spriteRen;
    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;
    void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        spriteRen = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void resPawn()
    {
        dead = false;
        ChangeHealth(maxHealth);
        anim.ResetTrigger("die");
        anim.Play("Idle");
        StartCoroutine(Invunerability());
        foreach (Behaviour component in components)
                {
                    component.enabled = true;
                }
    }
    public void ChangeHealth(float amount)
    {
    //     if (amount < 0)
    // {
    //     if (isInvincible)
    // {
    //     return;
    // }
    //     isInvincible = true;
    //     damageCooldown = timeInvincible;
    // }
        if(invulnerable == true) return;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if(amount<0)
        {
            if(currentHealth>0)
            {
                //player hurt
                anim.SetTrigger("hurt");
                StartCoroutine(Invunerability());
            }
            else
            {
                if(!dead)
                {
                foreach (Behaviour component in components)
                {
                    component.enabled = false;
                }
                GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                dead = true;
                anim.SetTrigger("die");
                }
               //die
            }
        }
    }
    private IEnumerator Invunerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(7,8,true);
    for(int i = 0 ;i<= numbeOfFlash;i++)
    {
        spriteRen.color = new Color(1,0,0,0.5f);
        yield return new WaitForSeconds(iFramesDuration/(numbeOfFlash*2));
        spriteRen.color = Color.white;
        yield return new WaitForSeconds(iFramesDuration/(numbeOfFlash*2));
    }
        Physics2D.IgnoreLayerCollision(7,8,false);
        invulnerable = false;
    }
     private void DeadActive()
     {
        gameObject.SetActive(false);
     }
}
