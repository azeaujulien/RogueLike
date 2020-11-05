using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomType type;
    public Doorway[] doorways;
    public Collider roomCollider;

    public Bounds RoomBounds {
        get { return roomCollider.bounds; }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(RoomBounds.center, RoomBounds.size - (Vector3.one * 0.1f));
    }
}