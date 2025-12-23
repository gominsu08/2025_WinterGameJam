using UnityEngine;

public class PlayerCameraFollower : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y, player.position.z) + offset;
    }
}
