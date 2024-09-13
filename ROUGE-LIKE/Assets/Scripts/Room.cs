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

    public void OpenDoor(Vector2Int direction, Color doorColor)
    {
        if(direction == Vector2Int.up)
        {
            topDoor.GetComponent<SpriteRenderer>().color = doorColor;
            topDoor.GetComponent<BoxCollider2D>().enabled = false;
        }
        if (direction == Vector2Int.down)
        {
            bottomDoor.GetComponent<SpriteRenderer>().color = doorColor;
            bottomDoor.GetComponent<BoxCollider2D>().enabled = false;
        }
        if (direction == Vector2Int.left)
        {
            leftDoor.GetComponent<SpriteRenderer>().color = doorColor;
            leftDoor.GetComponent<BoxCollider2D>().enabled = false;
        }
        if (direction == Vector2Int.right)
        {
            rightDoor.GetComponent<SpriteRenderer>().color = doorColor;
            rightDoor.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            gameObject.transform.GetChild(2).gameObject.SetActive(true);
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
