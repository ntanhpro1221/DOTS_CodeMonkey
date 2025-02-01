using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;

partial struct ActorMoveSystem : ISystem {
    private Job job;

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        job.Init(
            deltaTime: SystemAPI.Time.DeltaTime);
        job.ScheduleParallel();
    }

    [BurstCompile]
    public partial struct Job : IJobEntity {
        private float deltaTime;

        public void Init(float deltaTime) {
            this.deltaTime = deltaTime;
        }

        public void Execute(in ActorMoveData.Data moveData, ref LocalTransform localTrans, ref PhysicsVelocity velocity) {
            // reset value
            velocity.Linear = float3.zero;
            velocity.Angular = float3.zero;

            // calc move stuff
            float3 moveDir = moveData.targetPos - localTrans.Position;
            float moveDis = math.length(moveDir);
            if (moveDis < math.EPSILON) return;
            moveDir = math.normalize(moveDir);

            // do move
            if (moveDis <= deltaTime * moveData.moveSpeed)
                localTrans.Position = moveData.targetPos;
            else velocity.Linear = moveDir * moveData.moveSpeed;

            // rotate stuff
            if (math.length(velocity.Linear) < math.EPSILON) return;

            quaternion curQuat = localTrans.Rotation;
            quaternion targetQuat = quaternion.LookRotation(moveDir, math.up());
            float rotateDis = math.angle(curQuat, targetQuat);
            if (float.IsNaN(rotateDis) || rotateDis < math.EPSILON) return;

            float3 rotateDir = -math.normalize(math.Euler(math.mul(
                math.inverse(curQuat),
                targetQuat)));

            // do rotate
            if (rotateDis <= deltaTime * moveData.rotateSpeed)
                localTrans.Rotation = targetQuat;
            else velocity.Angular = rotateDir * moveData.rotateSpeed;
        }
    }
}

