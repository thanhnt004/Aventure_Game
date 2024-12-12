// using UnityEngine;

// public class Door:MonoBehaviour
// {
//     [SerializeField] private Transform preRoom;
//     [SerializeField] private Transform nextRoom;
//     [SerializeField] private CameraController cam;
//     private void OnTriggerEnter2D(Collider2D collider)
//     {
//         if(collider.tag == "Player")
//         {
//             if(collider.transform.position.x<transform.position.x)
//                 cam.RemoveToRoom(nextRoom);
//             else
//                 cam.RemoveToRoom(preRoom);
//         }
//     }
// }
