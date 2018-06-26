using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine;

/**
 * This class the responsible for moving bubbles that have been released
 * by the player
 */
public class BubbleMovementSystem : ComponentSystem {
    private struct Filter {
        public Transform Transform;
        public BubbleComponent BubbleComponent;
        public SpeedComponent SpeedComponent;
    }

    private struct CollisionFilter {
        public Transform Transform;
        public StickComponent StickComponent;
    }

    protected override void OnUpdate() {
        var deltaTime = Time.deltaTime;

        foreach (var entity in GetEntities<Filter>()) {
            if (entity.BubbleComponent.CanMove) {
                foreach (var collidableEntities in GetEntities<CollisionFilter>()) {
                    float bx = entity.Transform.position.x;
                    float wx = collidableEntities.Transform.position.x;

                    Debug.Log(Mathf.Abs(bx - wx));
                    if (Mathf.Abs(bx - wx) < .5) {
                        entity.BubbleComponent.Direction.x = -entity.BubbleComponent.Direction.x;
                    }
                }

                entity.Transform.position +=
                    new Vector3(entity.BubbleComponent.Direction.x, entity.BubbleComponent.Direction.y) *
                    Time.deltaTime * entity.SpeedComponent.Speed;
            }
        }
    }
}