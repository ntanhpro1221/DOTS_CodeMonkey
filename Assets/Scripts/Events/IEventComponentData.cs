using Unity.Entities;
using UnityEngine.Scripting;

/// <summary>
/// Remember update me in <see cref="EventResetSystem"/>
/// </summary>
[RequireImplementors]
public interface IEventComponentData : IComponentData {
    /// <summary>
    /// Set true to post event (do not set to false otherwise :3)
    /// </summary>
    bool PostEvent { get; set; }

    /// <summary>
    /// Is this event triggered
    /// </summary>
    bool IsTriggered { get; set; }
}
