using Unity.Entities;
using UnityEngine;

public class MoveEventData : MonoBehaviour {
    public struct Data : IEventComponentData {
        public bool PostEvent { get; set; }
        public bool IsTriggered { get; set; }
    }

    public class Baker : Baker<MoveEventData> {
        public override void Bake(MoveEventData authoring) {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Data());
        }
    }
}