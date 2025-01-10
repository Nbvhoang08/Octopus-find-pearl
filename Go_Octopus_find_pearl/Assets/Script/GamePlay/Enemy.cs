using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour ,IObserver
{
     public float gridSize = 0.5f;         // Kích thước ô lưới
    public LayerMask wallLayer;          // Layer dùng để kiểm tra tường
    public LayerMask enemyLayer;         // Layer dùng để kiểm tra Enemy khác
    public LayerMask playerLayer;        // Layer dùng để kiểm tra Player
    public float detectionDistance = 1f; // Khoảng cách phát hiện Player
    public Vector2 targetPosition;      // Vị trí mục tiêu tiếp theo của Enemy
    public bool isMoving = false;       // Trạng thái di chuyển của Enemy
    public void Awake()
    {
        Subject.RegisterObserver(this);
    }
    public void OnDestroy()
    {
        Subject.UnregisterObserver(this);
    }

    public virtual void OnNotify(string eventName, object eventData)
    {
        
    }
    public virtual void Start()
    {
        // Chuẩn hóa tọa độ ban đầu của Enemy
        NormalizePosition();
        targetPosition = transform.position;
        isMoving= false;
    }

    public virtual void Update()
    {
        MoveToTarget();
        // Kiểm tra 4 hướng để phát hiện Player
        DetectPlayerAndReact();
    }

    public virtual bool TryMove(Vector2 moveDirection)
    {
        if (isMoving) return false;
        // Kiểm tra nếu có tường hoặc Enemy khác chặn đường
        if (IsWallInDirection(moveDirection) || IsEnemyInDirection(moveDirection))
        {
            return false;
        }
        // Tính toán vị trí tiếp theo
        Vector2 nextPosition = (Vector2)transform.position + moveDirection * gridSize*2;
       
        // Cập nhật vị trí mục tiêu và trạng thái di chuyển
        targetPosition = nextPosition;
        isMoving = true;
        return true;
    }
    private void DetectPlayerAndReact()
    {
        // Kiểm tra 4 hướng: Lên, Xuống, Trái, Phải
        if (IsPlayerInDirection(Vector2.up))
        {
            TryMove(Vector2.down);
        }
        else if (IsPlayerInDirection(Vector2.down))
        {
            TryMove(Vector2.up);
        }
        else if (IsPlayerInDirection(Vector2.left))
        {
            TryMove(Vector2.right);
        }
        else if (IsPlayerInDirection(Vector2.right))
        {
            TryMove(Vector2.left);
        }
    }
    
    public bool IsWallInDirection(Vector2 direction)
    {
        // Phát ray từ vị trí hiện tại theo hướng di chuyển để kiểm tra tường
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, gridSize*2, wallLayer);
        Debug.DrawRay(transform.position, direction * gridSize * 2, Color.blue, 1f);
        return hit.collider != null;
    }

    private bool IsEnemyInDirection(Vector2 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, gridSize * 2, enemyLayer);
        Debug.DrawRay(transform.position, direction * gridSize * 2, Color.green, 0.5f);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject != gameObject)
            {
                Debug.DrawRay(transform.position, direction * gridSize * 2, Color.red, 0.5f);
                return true; // Trúng một Enemy khác
            }
        }

        return false; // Không trúng Enemy nào
    }

    private bool IsPlayerInDirection(Vector2 direction)
    {
        // Phát ray từ vị trí hiện tại theo hướng di chuyển để kiểm tra Player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionDistance, playerLayer);

        if (hit.collider != null)
        {
            // Kiểm tra nếu Player đang ở trạng thái Bulging
            PlayerMoveMent player = hit.collider.GetComponent<PlayerMoveMent>();
            if (player != null && player.Bulging)
            {
                return true;
            }
        }
    
        return false;
    }

    private void MoveToTarget()
    {
        if (isMoving)
        {
            // Di chuyển từ từ đến vị trí mục tiêu
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, gridSize*2/ 0.2f * Time.deltaTime);

            // Kiểm tra nếu đã đến vị trí mục tiêu
            if (Vector2.Distance(transform.position, targetPosition) < 0.001f)
            {
                transform.position = targetPosition; // Snap đúng vào grid
                isMoving = false; // Dừng di chuyển

                // Chuẩn hóa tọa độ
                NormalizePosition();
            }
        }
    }

    private void NormalizePosition()
    {
        // Làm tròn vị trí để đảm bảo luôn là bội số của gridSize
        Vector2 normalizedPosition = transform.position;
        normalizedPosition.x = Mathf.Round(normalizedPosition.x / gridSize) * gridSize;
        normalizedPosition.y = Mathf.Round(normalizedPosition.y / gridSize) * gridSize;
        transform.position = normalizedPosition;
    }
}
