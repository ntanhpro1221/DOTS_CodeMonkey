using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

partial struct ActorMoveSystem : ISystem {
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        foreach (var (
            localTrans,
            moveData,
            velocity) in SystemAPI.Query<
                RefRW<LocalTransform>,
                RefRO<ActorMoveData.Data>,
                RefRW<PhysicsVelocity>>()) {

            // reset value
            velocity.ValueRW.Linear = float3.zero;
            velocity.ValueRW.Angular = float3.zero;

            // calc move stuff
            float3 moveDir =
                moveData.ValueRO.targetPos -
                localTrans.ValueRO.Position;
            float moveDis = math.length(moveDir);
            if (moveDis < math.EPSILON) continue;
            moveDir = math.normalize(moveDir);

            // do move
            if (moveDis <= SystemAPI.Time.DeltaTime * moveData.ValueRO.moveSpeed)
                localTrans.ValueRW.Position = moveData.ValueRO.targetPos;
            else velocity.ValueRW.Linear = moveDir * moveData.ValueRO.moveSpeed;

            // rotate stuff
            if (math.length(velocity.ValueRO.Linear) < math.EPSILON) continue;

            quaternion curQuat = localTrans.ValueRO.Rotation;
            quaternion targetQuat = quaternion.LookRotation(moveDir, math.up());
            float rotateDis = math.angle(curQuat, targetQuat);
            if (float.IsNaN(rotateDis) || rotateDis < math.EPSILON) continue;

            float3 rotateDir = -math.normalize(math.Euler(math.mul(
                math.inverse(curQuat),
                targetQuat)));

            // do rotate
            if (rotateDis <= SystemAPI.Time.DeltaTime * moveData.ValueRO.rotateSpeed)
                localTrans.ValueRW.Rotation = targetQuat;
            else velocity.ValueRW.Angular = rotateDir * moveData.ValueRO.rotateSpeed;
        }
    }
}
