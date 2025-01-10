using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : Enemy
{
   private Vector2 moveDirection = Vector2.right; // Hướng di chuyển mặc định
    private SpriteRenderer spriteRenderer;         // Để thay đổi màu và flip X
    [SerializeField] private bool isBulging = false;                // Trạng thái khi Player ở trạng thái bulging

    public override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void OnNotify(string eventName, object eventData)
    {
        base.OnNotify(eventName, eventData);

        if (eventName == "move" && !isBulging) // Chỉ di chuyển khi không ở trạng thái bulging
        {
            if (!isMoving)
            {
                if (IsWallInDirection(moveDirection))
                {
                    // Đổi hướng khi gặp tường
                    moveDirection = moveDirection == Vector2.right ? Vector2.left : Vector2.right;
                    FlipSpriteBasedOnDirection(); // Đổi flipX theo hướng di chuyển
                }
                TryMove(moveDirection); // Di chuyển theo phương ngang
            }
        }
        else if (eventName == "bulging") // Khi nhận trạng thái bulging từ Player
        {
            StartCoroutine(HandleBulgingState());
        }
        else if (eventName == "normal") // Khi Player trở lại trạng thái bình thường
        {
            StopBulgingState();
        }
    }

    private IEnumerator HandleBulgingState()
    {
        isBulging = true;
        gameObject.layer = LayerMask.NameToLayer("Wall"); // Đổi layer thành Wall
        if (spriteRenderer)
        {
            // Hiệu ứng nhấp nháy màu đỏ
            while (isBulging)
            {
                spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(0.2f);
                spriteRenderer.color = Color.white;
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    private void StopBulgingState()
    {
        isBulging = false;
        gameObject.layer = LayerMask.NameToLayer("Enemy"); // Đổi layer lại thành Enemy
        if (spriteRenderer)
        {
            spriteRenderer.color = Color.white; // Reset màu về mặc định
        }
    }

    private void FlipSpriteBasedOnDirection()
    {
        if (spriteRenderer)
        {
            spriteRenderer.flipX = moveDirection == Vector2.left; // Flip sprite nếu hướng di chuyển là trái
        }
    }
    
    
}
