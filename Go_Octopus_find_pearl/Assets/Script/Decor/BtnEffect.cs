using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Import DoTween namespace
using System.Collections;
public class BtnEffect : MonoBehaviour
{
      [SerializeField] private float amplitude = 0.5f; // Biên độ sóng (độ lên xuống)
    [SerializeField] private float duration = 1f;   // Thời gian một chu kỳ
    private RectTransform _rectTransform;

    private void Awake()
    {
        // Lấy RectTransform của button
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForLayoutAndStartEffect());
    }

    private void OnDisable()
    {
        // Dừng tất cả các hiệu ứng khi button bị vô hiệu hóa
        if (_rectTransform != null)
        {
            _rectTransform.DOKill();
        }
    }

    private IEnumerator WaitForLayoutAndStartEffect()
    {
        // Đợi một frame để đảm bảo layout đã được tính toán
        yield return null;

        // Lấy vị trí ban đầu của button sau khi layout đã được tính toán
        Vector2 originalPosition = _rectTransform.anchoredPosition;

        // Kiểm tra giá trị originalPosition
        Debug.Log("Original Position: " + originalPosition);

        // Tạo hiệu ứng nhấp nhô (dao động chỉ trên trục Y tại vị trí Y ban đầu)
        _rectTransform.DOAnchorPosY(originalPosition.y + amplitude, duration)
            .SetEase(Ease.InOutSine) // Làm mượt hiệu ứng theo sóng sin
            .SetLoops(-1, LoopType.Yoyo); // Lặp vô hạn, kiểu Yoyo (lên và xuống)
    }

}
