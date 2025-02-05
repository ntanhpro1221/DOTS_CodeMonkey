using Unity.Entities;

[UpdateInGroup(typeof(LateSimulationSystemGroup), OrderLast = true)]
public partial class EventResetSystem : SystemBase {
    protected override void OnUpdate() {
        Entities.ForEach((ref MoveEventData.Data data) => { // Just change data type for other event
            data.IsTriggered = data.PostEvent;
            data.PostEvent = false;
        }).WithBurst().ScheduleParallel();
    }
}
