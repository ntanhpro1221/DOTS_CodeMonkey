using UnityEngine;
using UnityEngine.InputSystem;

public class GroundClickHelper : SceneSingleton<GroundClickHelper> {
    [SerializeField] private Collider groundCollider;

    public static Vector3? GetClickedPos() {
        Ray clickRay = GameSceneRef.cam.ScreenPointToRay(Mouse.current.position.value);
        if (Instance.groundCollider.Raycast(clickRay, out RaycastHit hitInfo, Mathf.Infinity)) {
            return hitInfo.point;
        } else {
            Debug.LogError("You are not clicking on the ground");
            return null;
        }
    }
}
