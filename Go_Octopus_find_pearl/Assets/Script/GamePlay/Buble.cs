using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buble : MonoBehaviour , IObserver
{
    // Start is called before the first frame update
    public Collider2D col;
    public GameObject Bubble;

    void Awake()
    {
        Subject.RegisterObserver(this);
    }
    void Start()
    {
        col = GetComponent<Collider2D>();
    }
    void OnDestroy()
    {
        Subject.UnregisterObserver(this);
    }
    public void OnNotify(string eventName,object eventData)
    {
        if (eventName == "bulging") // Khi nhận trạng thái bulging từ Player
        {
            
        }
        else if (eventName == "normal") // Khi Player trở lại trạng thái bình thường
        {
          
        }
    }
    void bubbleFloating()
    {
        Bubble.SetActive(true);
        col.enabled = true;
    }

    void Unfloating()
    {
        Bubble.SetActive(false);
        col.enabled = false;
    }
    
}
