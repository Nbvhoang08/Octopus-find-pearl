using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class PlayerMoveMent : MonoBehaviour
{
    public float gridSize = 0.5f;         // Kích thước của một ô lưới (bội số của 0.5)
    public float moveCooldown = 0.2f;    // Thời gian chờ giữa các lần di chuyển
    public LayerMask wallLayer;          // Layer dùng để kiểm tra tường
     public LayerMask obtaclelayer;  
    private bool isPlayerInSmoke = false; // Check if Player is in the smoke
    private Vector2 targetPosition;      // Vị trí mục tiêu tiếp theo của đối tượng
    private bool isMoving = false;       // Kiểm tra xem đối tượng có đang di chuyển hay không
    private float lastMoveTime;          // Thời gian lần di chuyển cuối
    public bool Bulging;
    public int BulgingStep;
    public SpriteRenderer sprite;
    public Animator anim;
    public List<GameObject> stepIndicators;        // Danh sách các GameObject để hiển thị bước đi
    public bool IsDeath;
    
    void Start()
    {
        // Chuẩn hóa tọa độ ban đầu của nhân vật
        NormalizePosition();
        targetPosition = transform.position; // Đặt vị trí mục tiêu bằng vị trí hiện tại
        sprite = GetComponent<SpriteRenderer>();
        ResetStepIndicators();
        IsDeath = false;
    }

    void Update()
    {
        if(IsDeath) return;
        anim.SetBool("Bulging",Bulging);
        anim.SetBool("IsWriggle",isWriggle);
       
        MoveToTarget();
    }

    // Hàm di chuyển chung, dùng tham số int để xác định hướng
    public void Move(int direction)
    {
        if(direction ==3)
        {
            sprite.flipX = false;
        }else if(direction ==2)
        {
            sprite.flipX = true;
        }
        if (isMoving || Time.time - lastMoveTime < moveCooldown) return;
        // Xác định hướng di chuyển
        Vector2 moveDirection = Vector2.zero;
        // Nếu Player đang trong khói
        if (isPlayerInSmoke && !Bulging)
        {
         // Ưu tiên đi lên
            moveDirection = Vector2.up;

        // Kiểm tra nếu không thể đi lên (gặp tường)
            if (IsWallInDirection(moveDirection))
            {
                switch (direction)
                {
                case 2: // Trái
                    moveDirection = Vector2.left;
                    break;
                case 3: // Phải
                    moveDirection = Vector2.right;
                    break;
                default:
                    return;
                }
            }
        }
        else
        {
            // Di chuyển bình thường (nếu Bulging hoặc không ở trong khói)
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
                    return;
            }
            if(Bulging && CheckDirection(moveDirection))
            {
                return;
            }
            // Kiểm tra nếu có tường ở hướng di chuyển
            if (IsWallInDirection(moveDirection))
            {
                return;
            }
           
        }
            // Cập nhật vị trí mục tiêu và trạng thái di chuyển
            Vector2 playerNextPosition = (Vector2)transform.position + moveDirection * gridSize*2;
            targetPosition = playerNextPosition;
            lastMoveTime = Time.time; // Cập nhật thời gian lần di chuyển cuối
            isMoving = true;
            SoundManager.Instance.PlayVFXSound(4);
            Subject.NotifyObservers("move");
            if(Bulging)
            {
                if(BulgingStep >0){
                    BulgingStep --;
                    UpdateStepIndicators();
                }else{
                    Subject.NotifyObservers("normal");
                    Bulging = false;
                    ResetStepIndicators();
                }
            }
    }

    // Kiểm tra xem có tường ở hướng di chuyển không bằng Raycast
    bool IsWallInDirection(Vector2 direction)
    {
        // Phát ray từ vị trí hiện tại theo hướng di chuyển
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, gridSize*2, wallLayer);
        return hit.collider != null;
    }
    bool CheckDirection(Vector2 direction)
    {
        // Vẽ tia ray để trực quan hóa
        Debug.DrawRay((Vector2)transform.position + direction/2, direction * gridSize * 4, Color.green, 0.5f);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, gridSize * 4, obtaclelayer);

        int obstacleCount = 0;
        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject != gameObject)
            {
                obstacleCount++;
            }

            if (obstacleCount > 1)
            {
                Debug.DrawRay(transform.position, direction * gridSize * 4, Color.red, 0.5f);
                return true;
            }
        }

        return false;
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
    // Detect if Player enters the smoke area
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Smoke"))
        {
            isPlayerInSmoke = true;
        }else if(other.CompareTag("Cave"))
        {
            Wriggle();
        }
    }
    public bool isWriggle;
    public void Wriggle()
    {
        if(!isWriggle) isWriggle = true;
    }
    public GameObject DeathEffect;
    public GameObject PassEffect;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Bubble"))
        {
            if(!Bulging)
            {
                Bulging = true;
                Subject.NotifyObservers("bulging");
                BulgingStep = 5;
                UpdateStepIndicators();
                SoundManager.Instance.PlayVFXSound(0);
            }
        }
        else if(other.CompareTag("pearl"))
        {
            Destroy(other.gameObject);
            StartCoroutine(passLevel());
            Instantiate(PassEffect, transform.position ,Quaternion.identity);
             SoundManager.Instance.PlayVFXSound(1);
            LevelManager.Instance.SaveGame();
        }else if(other.CompareTag("Enemy") && !Bulging)
        {
            if(!IsDeath)
            {
                sprite.enabled = false;
                StartCoroutine(ReLoad());
                Instantiate(DeathEffect, transform.position ,Quaternion.identity);
                 SoundManager.Instance.PlayVFXSound(3);
            }
        }
    }
    IEnumerator passLevel()
    {
        yield return new WaitForSeconds(1);
        UIManager.Instance.OpenUI<Pass>();
        Time.timeScale = 0;
    }
    IEnumerator ReLoad()
    {
        yield return new WaitForSeconds(1f);
        ReloadCurrentScene();
    }
    public void ReloadCurrentScene()
    {
        // Lấy tên của scene hiện tại 
        string currentSceneName = SceneManager.GetActiveScene().name;
        //Tải lại scene hiện tại
        SceneManager.LoadScene(currentSceneName);
    }





    // Detect if Player exits the smoke area
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Smoke"))
        {
            isPlayerInSmoke = false;
        }else if(isWriggle)
        {
            isWriggle = false;
        }
        
    }
    private void UpdateStepIndicators()
    {
        // Bật/tắt các step indicator dựa trên số bước
        for (int i = 0; i < stepIndicators.Count; i++)
        {
            stepIndicators[i].SetActive(i <= BulgingStep-1);
        }
    }

    private void ResetStepIndicators()
    {
        // Tắt tất cả các step indicator
        foreach (GameObject stepIndicator in stepIndicators)
        {
            stepIndicator.SetActive(false);
        }
    }
}
