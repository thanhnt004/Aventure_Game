using Unity.VisualScripting;
using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    [SerializeField] private float value;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            collider.GetComponent<Health>().ChangeHealth(value);
            gameObject.SetActive(false);
        }
    }
}
