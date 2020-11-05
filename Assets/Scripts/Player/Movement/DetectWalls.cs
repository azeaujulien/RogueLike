using UnityEngine;

public class DetectWalls : MonoBehaviour
{
    public PlayerMovement player;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ground") && !other.CompareTag("Player")) {
            player.StopPlayer();
        }
    }
}
