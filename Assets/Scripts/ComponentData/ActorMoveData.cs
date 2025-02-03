using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ActorMoveData : MonoBehaviour {
    public float moveSpeed;
    public float rotateSpeed;
    public float3 targetPos;

    public struct Data : IComponentData {
        public float moveSpeed;
        public float rotateSpeed;
        public float3 targetPos;
    }
 
    public class Baker : Baker<ActorMoveData> {
        public override void Bake(ActorMoveData authoring) {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Data() {
                moveSpeed = authoring.moveSpeed,
                rotateSpeed = authoring.rotateSpeed,
                targetPos = authoring.targetPos = authoring.transform.position,
            });
        }
    }
}
