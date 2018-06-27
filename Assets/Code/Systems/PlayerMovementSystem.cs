using Unity.Entities;
using UnityEngine;

/**
 * This class is responsible for moving the player's mesh to reflect where
 * it is aiming
 */
public class PlayerMovementSystem : ComponentSystem {
    private struct Filter {
        public Transform Transform;
        public InputComponent InputComponent;
        public SpeedComponent SpeedComponent;
    }

    protected override void OnUpdate() {
        var deltaTime = Time.deltaTime;

        foreach (var entity in GetEntities<Filter>()) {
            var rot = entity.Transform.rotation *
                Quaternion.AngleAxis(deltaTime * entity.InputComponent.Direction * entity.SpeedComponent.Speed,
                    Vector3.back);
            
            if (rot.eulerAngles.z < 280 && rot.eulerAngles.z > 80) continue;
            entity.Transform.rotation = rot;
        }
    }
}

