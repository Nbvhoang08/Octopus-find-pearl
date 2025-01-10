using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoor : MonoBehaviour
{
   public GameObject door;         // Đối tượng cửa cần mở/đóng
    public float doorSpeed = 2f;    // Tốc độ mở/đóng cửa
    public SpriteRenderer sprite;
    private Vector3 originalScale;  // Scale ban đầu của cửa
    private bool isTriggered = false; // Cờ kiểm tra nếu có đối tượng trong trigger

    void Start()
    {
        if (door == null)
        {
            Debug.LogError("Door object is not assigned!");
            return;
        }
        sprite = GetComponent<SpriteRenderer>();
        // Lưu lại scale ban đầu của cửa
        originalScale = door.transform.localScale;
    }

    void Update()
    {
        if (isTriggered)
        {
            // Giảm scale Y của cửa dần về 0
            door.transform.localScale = Vector3.Lerp(door.transform.localScale, 
                                                     new Vector3(originalScale.x, 0, originalScale.z), 
                                                     Time.deltaTime * doorSpeed);
        }
        else
        {
            // Khôi phục scale Y của cửa dần về giá trị ban đầu
            door.transform.localScale = Vector3.Lerp(door.transform.localScale, 
                                                     originalScale, 
                                                     Time.deltaTime * doorSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
    
        // Khi có đối tượng đi vào vùng trigger
        if (other.CompareTag("Player")|| other.CompareTag("Enemy")) // Hoặc thay bằng tag phù hợp
        {
            isTriggered = true;
            sprite.enabled = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // Khi đối tượng rời khỏi vùng trigger
        if (other.CompareTag("Player")||other.CompareTag("Enemy")) // Hoặc thay bằng tag phù hợp
        {
            isTriggered = false;
            sprite.enabled = true;
        }
    }
}
