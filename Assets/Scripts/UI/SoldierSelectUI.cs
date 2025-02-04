using System;
using UnityEngine;
using UnityEngine.UI;

public class SoldierSelectUI : MonoBehaviour {
    [SerializeField] private Transform canvasTrans;
    [SerializeField] private Image selectRectImg;
    [SerializeField] private RectTransform selectRectTrans;

    private void Start() {
        SoldierSelectHandler.OnSelectionStart += OnSelectionStart;
        SoldierSelectHandler.OnSelectionStop += OnSelectionStop;
    }

    private void Update() {
        SyncSelectRect();
    }

    private void OnDestroy() {
        SoldierSelectHandler.OnSelectionStart -= OnSelectionStart;
        SoldierSelectHandler.OnSelectionStop -= OnSelectionStop;
    }

    private void OnSelectionStart(object _, EventArgs __) {
        selectRectImg.enabled = true;
    }

    private void OnSelectionStop(object _, EventArgs __) { 
        selectRectImg.enabled = false;
    }

    private void SyncSelectRect() {
        Rect? selectRect = SoldierSelectHandler.SelectionRect;
        if (selectRect == null) return;

        selectRect = new(
            selectRect.Value.x / canvasTrans.localScale.x,
            selectRect.Value.y / canvasTrans.localScale.y,
            selectRect.Value.width / canvasTrans.localScale.x,
            selectRect.Value.height / canvasTrans.localScale.y);

        selectRectTrans.anchorMin =
        selectRectTrans.anchorMax =
        selectRectTrans.pivot = Vector3.zero;

        selectRectTrans.anchoredPosition = selectRect.Value.position;
        selectRectTrans.sizeDelta = selectRect.Value.size;
    }
}
