using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject topDoor;
    [SerializeField] GameObject rightDoor;
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject bottomDoor;

    public Vector2Int RoomIndex { get; set; }

    public void OpenDoor(Vector2Int direction)
    {
        if(direction == Vector2Int.up)
        {
            topDoor.GetComponent<SpriteRenderer>().color = Color.black;
            topDoor.GetComponent<BoxCollider2D>().enabled = false;
        }
        if (direction == Vector2Int.down)
        {
            bottomDoor.GetComponent<SpriteRenderer>().color = Color.black;
            bottomDoor.GetComponent<BoxCollider2D>().enabled = false;
        }
        if (direction == Vector2Int.left)
        {
            leftDoor.GetComponent<SpriteRenderer>().color = Color.black;
            leftDoor.GetComponent<BoxCollider2D>().enabled = false;
        }
        if (direction == Vector2Int.right)
        {
            rightDoor.GetComponent<SpriteRenderer>().color = Color.black;
            rightDoor.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
