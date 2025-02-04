using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoldierSelectHandler : SceneSingleton<SoldierSelectHandler> {
    public static event EventHandler OnSelectionStart;
    public static event EventHandler OnSelectionStop;
    public static Rect? SelectionRect {
        get {
            if (!_IsSelecting) return null;
            return RectFromCorner(_SelectionStartPoint, Mouse.current.position.value);
        }
    }

    private static bool _IsSelecting = false;
    private static Vector2 _SelectionStartPoint;

    private void Update() {
        HandleSelectionStart();
        HandleSelectionStop();
    }

    private void OnApplicationFocus(bool focus) {
        if (!focus) {
            _IsSelecting = false;
            OnSelectionStop?.Invoke(this, EventArgs.Empty);
        }
    }

    private static void HandleSelectionStart() {
        if (!Application.isFocused ||
            !Mouse.current.leftButton.wasPressedThisFrame) return;
        _SelectionStartPoint = Mouse.current.position.value;
        _IsSelecting = true;
        OnSelectionStart?.Invoke(Instance, EventArgs.Empty);
    }

    private static void HandleSelectionStop() { 
        if (!Application.isFocused ||
            !Mouse.current.leftButton.wasReleasedThisFrame) return;
        DoSelect(RectFromCorner(_SelectionStartPoint, Mouse.current.position.value));
        _IsSelecting = false;
        OnSelectionStop?.Invoke(Instance, EventArgs.Empty);
    }

    private static void DoSelect(Rect rect) {
        DeselectAll();

        var entityMan = World.DefaultGameObjectInjectionWorld.EntityManager;

        bool isSingleSelect = rect.width + rect.height < 50;
        if (isSingleSelect) {
            var collisionWorld = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<Unity.Physics.PhysicsWorldSingleton>()
                .Build(entityMan)
                .GetSingleton<Unity.Physics.PhysicsWorldSingleton>()
                .CollisionWorld;

            Ray camRay = Camera.main.ScreenPointToRay(Mouse.current.position.value);

            int soldierLayer = 6;
            if (collisionWorld.CastRay(new Unity.Physics.RaycastInput() {
                Start = camRay.GetPoint(0),
                End = camRay.GetPoint(9999),
                Filter = new() {
                    BelongsTo = ~0u,
                    CollidesWith = 1u << soldierLayer,
                }
            }, out Unity.Physics.RaycastHit hit)) {
                entityMan.SetComponentEnabled<SelectedTag.Tag>(hit.Entity, true);
            }
        } else {
            var query = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<LocalTransform>()
                .WithPresent<SelectedTag.Tag>()
                .Build(entityMan);

            var localTransArr = query.ToComponentDataArray<LocalTransform>(Allocator.Temp);
            var entityArr = query.ToEntityArray(Allocator.Temp);

            for (int i = 0; i < localTransArr.Length; i++)
                if (rect.Contains(Camera.main.WorldToScreenPoint(localTransArr[i].Position)))
                    entityMan.SetComponentEnabled<SelectedTag.Tag>(
                        entityArr[i],
                        true);
        }
    }

    private static void DeselectAll() {
        var entityMan = World.DefaultGameObjectInjectionWorld.EntityManager;
        var query = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<SelectedTag.Tag>()
            .Build(entityMan);

        var entityArr = query.ToEntityArray(Allocator.Temp);

        for (int i = 0; i < entityArr.Length; i++)
            entityMan.SetComponentEnabled<SelectedTag.Tag>(
                entityArr[i],
                false);
    }

    private static Rect RectFromCorner(Vector2 pntA, Vector2 pntB) => new(
        new Vector2(
            Math.Min(pntA.x, pntB.x),
            Math.Min(pntA.y, pntB.y)),
        new Vector2(
            Math.Abs(pntA.x - pntB.x),
            Math.Abs(pntA.y - pntB.y)));
}
