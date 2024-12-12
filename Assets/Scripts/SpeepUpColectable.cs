using UnityEngine;

public class SpeepUpColectable : MonoBehaviour
{
    [SerializeField] private float value;
    [SerializeField] private float time;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player") )
        {
            collider.GetComponent<PlayerMovement>().SpeepUp(time,value);
            gameObject.SetActive(false);
        }
    }
}
