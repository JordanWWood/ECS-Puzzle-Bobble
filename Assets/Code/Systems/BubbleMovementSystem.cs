using Unity.Entities;
using UnityEngine;

/**
 * This class the responsible for moving bubbles that have been released
 * by the player
 */
public class BubbleMovementSystem : ComponentSystem {
    private struct BubbleFilter {
        public Transform Transform;
        public BubbleComponent BubbleComponent;
        public SpeedComponent SpeedComponent;
        public CollidableComponent CollidableComponent;
    }

    private struct CollisionFilter {
        public Transform Transform;
        public CollidableComponent CollidableComponent;
    }

    protected override void OnUpdate() {
        foreach (var entity in GetEntities<BubbleFilter>()) {
            if (!entity.BubbleComponent.CanMove) continue;
            
            foreach (var collidableEntities in GetEntities<CollisionFilter>()) {
                if (collidableEntities.CollidableComponent.IsCollidable && collidableEntities.CollidableComponent.IsSticky) {
                    var bx = entity.Transform.position.x;
                    var wx = collidableEntities.Transform.position.x;
                    
                    var by = entity.Transform.position.y;
                    var wy = collidableEntities.Transform.position.y;

                    if (!(Mathf.Abs(by - wy) < .5) || !(Mathf.Abs(bx - wx) < .5)) continue;

                    entity.BubbleComponent.CanMove = false;
                    entity.CollidableComponent.IsSticky = true;
                    entity.CollidableComponent.IsCollidable = true;
                } else if (collidableEntities.CollidableComponent.IsCollidable) {
                    var bx = entity.Transform.position.x;
                    var wx = collidableEntities.Transform.position.x;

                    if (Mathf.Abs(bx - wx) < .5)
                        entity.BubbleComponent.Direction.x = -entity.BubbleComponent.Direction.x;
                } else {
                    if (!collidableEntities.CollidableComponent.IsSticky) continue;
                    
                    var by = entity.Transform.position.y;
                    var wy = collidableEntities.Transform.position.y;

                    if (!(Mathf.Abs(by - wy) < .5)) continue;
                    
                    entity.BubbleComponent.CanMove = false;
                    entity.CollidableComponent.IsSticky = true;
                    entity.CollidableComponent.IsCollidable = true;
                }

                if (!entity.BubbleComponent.CanMove && entity.CollidableComponent.IsCollidable &&
                    entity.CollidableComponent.IsSticky) {
                    var startPos = entity.Transform.position;
                    
                    
                }
            }

            entity.Transform.position +=
                new Vector3(entity.BubbleComponent.Direction.x, entity.BubbleComponent.Direction.y) *
                Time.deltaTime * entity.SpeedComponent.Speed;
        }
    }
}
