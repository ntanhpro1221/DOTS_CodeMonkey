using UnityEngine;

public class GameSceneRef : SceneSingleton<GameSceneRef> {
    protected override void Awake() {
        base.Awake();
        cam = Camera.main;
    }

    public static Camera cam;
}
