using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;

partial struct ActorSelectVisualizeSystem : ISystem {
    private Job job;

    [BurstCompile]
    void OnCreate(ref SystemState state) {
        job.Init(
            state.GetComponentLookup<SelectedTag.Tag>(true),
            state.GetComponentLookup<MaterialMeshInfo>(false));
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        job.Update(ref state);
        job.ScheduleParallel();
    }

    [WithPresent(typeof(SelectedTag.Tag))]
    [BurstCompile]
    public partial struct Job : IJobEntity {
        [ReadOnly] private ComponentLookup<SelectedTag.Tag> selectedTagLookup;
        [NativeDisableParallelForRestriction]
        private ComponentLookup<MaterialMeshInfo> materialInfoLookup;
    
        public void Init(
            in ComponentLookup<SelectedTag.Tag> selectedTagLookup,
            in ComponentLookup<MaterialMeshInfo> materialInfoLookup) {
            this.selectedTagLookup = selectedTagLookup;
            this.materialInfoLookup = materialInfoLookup;
        }

        public void Update(ref SystemState state) {
            selectedTagLookup.Update(ref state);
            materialInfoLookup.Update(ref state);
        }

        public void Execute(Entity entity, in SelectedTag.Tag selectedTag) {
            materialInfoLookup.SetComponentEnabled(
                selectedTag.visualizer,
                selectedTagLookup.IsComponentEnabled(entity));
        }
    }
}
