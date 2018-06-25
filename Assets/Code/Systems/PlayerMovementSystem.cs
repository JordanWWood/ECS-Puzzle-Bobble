using System;
using Unity.Entities;
using UnityEngine;

public class PlayerMovementSystem : ComponentSystem {
    private struct Filter {
        public Transform Transform;
        public InputComponent InputComponent;
    }

    protected override void OnUpdate() {
        var deltaTime = Time.deltaTime;

        foreach (var entity in GetEntities<Filter>()) {
            Quaternion rot = entity.Transform.rotation *
                Quaternion.AngleAxis(deltaTime * entity.InputComponent.Direction * entity.InputComponent.Speed,
                    Vector3.back);
            
            if (rot.eulerAngles.z < 270 && rot.eulerAngles.z > 90) continue;
            entity.Transform.rotation = rot;
        }
    }
}
