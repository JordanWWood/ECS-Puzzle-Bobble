using Unity.Entities;
using UnityEngine;

/**
 * This class is responsible for keeping track of player input.
 */
public class InputSystem : ComponentSystem {
    private struct PlayerData {
        public int Length;
        public ComponentArray<InputComponent> InputComponents;
    }

    private struct BubbleData {
        public int Length;
        public ComponentArray<BubbleComponent> BubbleComponents;
    }

    private struct PlayerFilter {
        public Transform Transform;
        public InputComponent InputComponent;
        public SpeedComponent SpeedComponent;
    }

    [Inject] private PlayerData _playerData;
    [Inject] private BubbleData _bubbleData;

    protected override void OnUpdate() {
        var horizontal = Input.GetAxis("Horizontal");
        var release = Input.GetAxis("Jump");

        for (int i = 0; i < _playerData.Length; i++)
            _playerData.InputComponents[i].Direction = horizontal;

        for (int i = 0; i < _bubbleData.Length; i++) {
            if (_bubbleData.BubbleComponents[i].BeenHeld == false && release == 1) {
                _bubbleData.BubbleComponents[i].CanMove = true;
                _bubbleData.BubbleComponents[i].BeenHeld = true;

                /**
                 * There should be only one player object so it should be safe to assume the first one is okay.
                 * However, if we add muliplayer this will not be the case
                 *
                 * TODO make this compatible with multiple players
                 */
                foreach (var entity in GetEntities<PlayerFilter>()) { 
                    _bubbleData.BubbleComponents[i].Direction = new Vector2((entity.Transform.rotation * Vector3.up).x,
                        (entity.Transform.rotation * Vector3.up).y);
                }
            }
        }
    }
}