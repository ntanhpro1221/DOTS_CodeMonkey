using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

public class SelectedTag : MonoBehaviour {
    public GameObject visualizer;

    public struct Tag : IComponentData, IEnableableComponent {
        public Entity visualizer;
    }

    public class Baker : Baker<SelectedTag> {
        public override void Bake(SelectedTag authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            Entity visualizer = GetEntity(authoring.visualizer, TransformUsageFlags.Dynamic);
            AddComponent(entity, new Tag() {
                visualizer = visualizer,
            });
            SetComponentEnabled<Tag>(entity, false);
        }
    }
}