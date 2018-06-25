using Unity.Entities;
using UnityEngine;

public class PlayerMovementSystem : ComponentSystem {
    private struct Filter {
        public InputComponent InputComponent;
    }

    protected override void OnUpdate() {
        var deltaTime = Time.deltaTime;

        foreach (var entity in GetEntities<Filter>()) {
            // Rotate Player
        }
    }
}
