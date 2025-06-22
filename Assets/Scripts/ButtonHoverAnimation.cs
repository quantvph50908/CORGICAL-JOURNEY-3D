using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scale Settings")]
    public float hoverScale = 1.1f;
    public float animationDuration = 0.2f;

    [Header("Easing")]
    public Ease easeEnter = Ease.OutBack;
    public Ease easeExit = Ease.InBack;

    private RectTransform rectTransform;
    private Tween scaleTween;
    private Vector3 originalScale;

    private void Awake()
    {
        // Kiểm tra và lấy RectTransform
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            return;
        }
        originalScale = rectTransform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (rectTransform == null) return;
        scaleTween?.Kill(); // Hủy animation cũ
        scaleTween = rectTransform.DOScale(originalScale * hoverScale, animationDuration).SetEase(easeEnter);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (rectTransform == null) return;
        scaleTween?.Kill(); // Hủy animation cũ
        scaleTween = rectTransform.DOScale(originalScale, animationDuration).SetEase(easeExit);
    }
}