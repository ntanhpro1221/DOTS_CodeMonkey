using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoldierMoveHandler : MonoBehaviour {
    private void Update() {
        if (Application.isFocused && Mouse.current.leftButton.wasPressedThisFrame) {
            Vector3? targetPos = GroundClickHelper.GetClickedPos();
            if (targetPos == null) return;
            MoveAllSoldierTo(targetPos.Value);
        }
    }

    private void MoveAllSoldierTo(Vector3 targetPos) {
        var entityQuery = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<ActorMoveData.Data>()
            .WithAll<SelectedTag.Tag>()
            .Build(World.DefaultGameObjectInjectionWorld.EntityManager);
        var entityArray = entityQuery.ToComponentDataArray<ActorMoveData.Data>(Allocator.Temp);

        for (int i = 0; i < entityArray.Length; i++) {
            var data = entityArray[i];
            data.targetPos = targetPos;
            entityArray[i] = data;
        }

        entityQuery.CopyFromComponentDataArray(entityArray);
    }
}
