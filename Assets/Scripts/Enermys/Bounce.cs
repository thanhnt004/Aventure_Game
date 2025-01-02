using Unity.VisualScripting;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float BounceForce;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            collider.GetComponent<Rigidbody2D>().AddForce(Vector3.up * BounceForce,ForceMode2D.Impulse);
           animator.SetTrigger("active");
        }
    }  
}
