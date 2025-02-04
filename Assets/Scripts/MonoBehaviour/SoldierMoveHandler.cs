using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoldierMoveHandler : MonoBehaviour {
    [SerializeField] private float ringRadius = 2.2f;
    [SerializeField] private float disInRing = 2f;

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
        var targetPosArr = GenGroupPosition(targetPos, entityArray.Length);

        for (int i = 0; i < entityArray.Length; i++) {
            var data = entityArray[i];
            data.targetPos = targetPosArr[i];
            entityArray[i] = data;
        }

        entityQuery.CopyFromComponentDataArray(entityArray);
    }

    private NativeArray<float3> GenGroupPosition(float3 center, int groupSize) {
        NativeArray<float3> groupPos = new(groupSize, Allocator.Temp);

        for (int unitId = 0, ringId = 0; unitId < groupSize; ++ringId) {
            float radius = ringId * ringRadius;
            float ringPerimeter = math.PI2 * radius;
            int ringCapacity = math.min(
                groupSize - unitId, 
                math.max(
                    1, 
                    (int)math.floor(ringPerimeter / disInRing)));
            float angleBetweenEachActor = math.PI2 / ringCapacity;
            for (; ringCapacity > 0; --ringCapacity, ++unitId)
                groupPos[unitId] = 
                    center + 
                    math.rotate(
                        quaternion.RotateY(angleBetweenEachActor * ringCapacity), 
                        new(radius, 0, 0));
        }

        return groupPos;
    }
}
