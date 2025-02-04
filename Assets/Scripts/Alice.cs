using UnityEngine;

public class Alice : MonoBehaviour {
    public RectTransform rectTrans;

    [Header("---------------------------------------------------------------------")]
    public bool changeAnchor;
    public Vector2 anchorMin;
    public Vector2 anchorMax;

    [Header("---------------------------------------------------------------------")]
    public bool changePivot;
    public Vector2 pivot;

    [Header("---------------------------------------------------------------------")]
    public bool changeAnchoredPosition;
    public Vector2 anchoredPosition;

    [Header("---------------------------------------------------------------------")]
    public bool changeSizeDelta;
    public Vector2 sizeDelta;

    [Header("---------------------------------------------------------------------")]
    public bool changeOffset;
    public Vector2 offsetMin;
    public Vector2 offsetMax;

    [Header("---------------------------------------------------------------------")]
    public bool changeSizeWithCurrentAnchors;
    public RectTransform.Axis axis;
    public float size;

    [ContextMenu("UncheckAll")]
    public void UncheckAll() {
        changeAnchor = false;

        changePivot = false;

        changeAnchoredPosition = false;

        changeSizeDelta = false;

        changeOffset = false;

        changeSizeWithCurrentAnchors = false;
    }

    [ContextMenu("Bake")]
    public void Bake() {
        if (changeAnchor) {
            rectTrans.anchorMin = anchorMin;
            rectTrans.anchorMax = anchorMax;
        } else {
            anchorMin = rectTrans.anchorMin;
            anchorMax = rectTrans.anchorMax;
        }

        if (changePivot) {
            rectTrans.pivot = pivot;
        } else {
            pivot = rectTrans.pivot;
        }

        if (changeAnchoredPosition) {
            rectTrans.anchoredPosition = anchoredPosition;
        } else {
            anchoredPosition = rectTrans.anchoredPosition;
        }

        if (changeSizeDelta) {
            rectTrans.sizeDelta = sizeDelta;
        } else {
            sizeDelta = rectTrans.sizeDelta;
        }

        if (changeOffset) {
            rectTrans.offsetMin = offsetMin;
            rectTrans.offsetMax = offsetMax;
        } else {
            offsetMin = rectTrans.offsetMin;
            offsetMax = rectTrans.offsetMax;
        }
        
        if (changeSizeWithCurrentAnchors) {
            rectTrans.SetSizeWithCurrentAnchors(axis, size);
        }
    }
}
