using UnityEngine;

public class PlayerResapwn : MonoBehaviour
{
    private AudioManager audioManager;
    private Transform curentCheckPoint;//check point gan day nhat
    private Health playerHeal;//mau nguoi choi
    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        playerHeal = GetComponent<Health>();
    }
    public void ressPawn()
    {
        transform.position = curentCheckPoint.position;
        playerHeal.resPawn();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.transform.tag =="checkpoint")
        {
            curentCheckPoint = collider.transform;
            AudioManager.instance.PlaySFX(AudioManager.instance.checkPoint);
            collider.GetComponent<Collider2D>().enabled = false;
            collider.GetComponent<Animator>().SetTrigger("active");
        }
    }
}
