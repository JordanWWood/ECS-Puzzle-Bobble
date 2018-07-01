using Boo.Lang;
using Unity.Entities;
using UnityEngine;

/**
 * This system is responsible for spawning bubbles, both randomly at the
 * beginning of the game and for the player there after
 */
public class BubbleSpawningSystem : ComponentSystem {
    public struct Filter {
        public Transform TransformComponent;
        public BubbleSpawnerComponent BubbleSpawnerComponent;
    }

    protected override void OnUpdate() {
        var objects = GetEntities<Filter>();

        foreach (var entity in objects)
            if (entity.BubbleSpawnerComponent.IsPlayer) {
                if (entity.BubbleSpawnerComponent.Object != null &&
                    entity.BubbleSpawnerComponent.Object.transform.position != entity.TransformComponent.position)
                    entity.BubbleSpawnerComponent.Object = null;

                if (entity.BubbleSpawnerComponent.Object != null) continue;
                if (objects[1].BubbleSpawnerComponent.Object == null) continue;

                entity.BubbleSpawnerComponent.Object = objects[1].BubbleSpawnerComponent.Object;

                objects[1].BubbleSpawnerComponent.Object = null;
                entity.BubbleSpawnerComponent.Object.transform.SetPositionAndRotation(
                    entity.TransformComponent.position,
                    entity.TransformComponent.rotation);

                if (entity.BubbleSpawnerComponent.Object != null)
                    entity.BubbleSpawnerComponent.Object.GetComponent<BubbleComponent>().BeenHeld = false;
            }
            else if (!entity.BubbleSpawnerComponent.IsPlayer) {
                if (entity.BubbleSpawnerComponent.Object != null) continue;
                
                entity.BubbleSpawnerComponent.Object =
                    Object.Instantiate(entity.BubbleSpawnerComponent.Prefab, entity.TransformComponent.position, entity.TransformComponent.rotation);

                entity.BubbleSpawnerComponent.Object.SetActive(true);
                entity.BubbleSpawnerComponent.Object.GetComponent<BubbleComponent>().BeenHeld = true;
            }
    }
}