using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : Enemy
{
    public LayerMask groundLayer; // Layer để kiểm tra ground
    private bool isBeingPushed = false; // Trạng thái bị Player đẩy
    public override void OnNotify(string eventName, object eventData)
    {
        base.OnNotify(eventName, eventData);
        if(eventName == "move")
        {

            if (!IsOnGround() && !isMoving)
            {
                TryMove(Vector2.down); // Tự động rơi xuống nếu không đứng trên ground
            }
        }
    }
    public override void Update()
    {
        base.Update(); // Gọi Update của lớp cha
    }

    private bool IsOnGround()
    {
        // Kiểm tra nếu Crab đang đứng trên ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, gridSize, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * gridSize*2, Color.yellow, 0.5f);
       
        return hit.collider != null;
    }

    public override bool TryMove(Vector2 moveDirection)
    {
        if (moveDirection == Vector2.down)
        {
            isBeingPushed = false; // Reset trạng thái bị đẩy khi tự động rơi
        }
        else
        {
            isBeingPushed = true; // Đặt trạng thái bị đẩy khi bị Player tác động
        }
        return base.TryMove(moveDirection);
    }
}
