using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveMent : MonoBehaviour
{
   public float gridSize = 0.5f;         // Kích thước của một ô lưới (bội số của 0.5)
    public float moveCooldown = 0.2f;    // Thời gian chờ giữa các lần di chuyển
    public LayerMask wallLayer;          // Layer dùng để kiểm tra tường

    private Vector2 targetPosition;      // Vị trí mục tiêu tiếp theo của đối tượng
    private bool isMoving = false;       // Kiểm tra xem đối tượng có đang di chuyển hay không
    private float lastMoveTime;          // Thời gian lần di chuyển cuối

    void Start()
    {
        // Chuẩn hóa tọa độ ban đầu của nhân vật
        NormalizePosition();
        targetPosition = transform.position; // Đặt vị trí mục tiêu bằng vị trí hiện tại
    }

    void Update()
    {
        MoveToTarget();
    }

    // Hàm di chuyển chung, dùng tham số int để xác định hướng
    public void Move(int direction)
    {
        if (isMoving || Time.time - lastMoveTime < moveCooldown) return;

        // Xác định hướng đi dựa trên tham số truyền vào
        Vector2 moveDirection = Vector2.zero;
        switch (direction)
        {
            case 0: // Lên
                moveDirection = Vector2.up;
                break;
            case 1: // Xuống
                moveDirection = Vector2.down;
                break;
            case 2: // Trái
                moveDirection = Vector2.left;
                break;
            case 3: // Phải
                moveDirection = Vector2.right;
                break;
            default:
                Debug.LogError("Hướng không hợp lệ!");
                return;
        }

        // Kiểm tra nếu có tường ở 4 hướng (theo moveDirection)
        Vector2 nextPosition = (Vector2)transform.position + moveDirection * gridSize*2;
        if (IsWallInDirection(moveDirection))
        {
            return;
        }

        // Cập nhật vị trí mục tiêu
        targetPosition = nextPosition;
        lastMoveTime = Time.time; // Cập nhật thời gian lần di chuyển cuối
        isMoving = true;
    }

    // Kiểm tra xem có tường ở hướng di chuyển không bằng Raycast
    bool IsWallInDirection(Vector2 direction)
    {
        // Phát ray từ vị trí hiện tại theo hướng di chuyển
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, gridSize*2, wallLayer);
        return hit.collider != null;
    }

    void MoveToTarget()
    {
        if (isMoving)
        {
            // Di chuyển từ từ đến vị trí mục tiêu
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, gridSize*2 / moveCooldown * Time.deltaTime);

            // Kiểm tra nếu đã đến vị trí mục tiêu
            if (Vector2.Distance(transform.position, targetPosition) < 0.001f)
            {
                transform.position = targetPosition; // Snap đúng vào grid
                isMoving = false; // Dừng di chuyển

                // Chuẩn hóa tọa độ để tránh sai lệch
                NormalizePosition();
            }
        }
    }

    // Chuẩn hóa tọa độ để đảm bảo luôn là bội số của gridSize
    void NormalizePosition()
    {
        Vector2 normalizedPosition = transform.position;
        normalizedPosition.x = Mathf.Round(normalizedPosition.x / gridSize) * gridSize;
        normalizedPosition.y = Mathf.Round(normalizedPosition.y / gridSize) * gridSize;
        transform.position = normalizedPosition;
    }
}
