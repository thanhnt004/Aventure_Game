using UnityEngine;


public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    private float lookAhead;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // public void RemoveToRoom(Transform room)
    // {
    //     currentPosX = room.position.x;
    //     //move room
    //      transform.position = Vector3.SmoothDamp(transform.position,new Vector3(currentPosX,transform.position.y,transform.position.z)
    //     ,ref velocity, speed );
    // }
    // // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x + lookAhead,player.position.y,player.position.z - 10);
        lookAhead = Mathf.Lerp(lookAhead,(aheadDistance*player.localScale.x), speed * Time.deltaTime);

    }
}
