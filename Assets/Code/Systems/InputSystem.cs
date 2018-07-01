using System;
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

    // Dependancy injection to find GameObjects that match our structs.
    [Inject] private PlayerData _playerData;
    [Inject] private BubbleData _bubbleData;

    private long _lastRelease = 0;

    protected override void OnUpdate() {
        var horizontal = Input.GetAxis("Horizontal");
        var release = Input.GetAxis("Jump");

        for (var i = 0; i < _playerData.Length; i++)
            _playerData.InputComponents[i].Direction = horizontal;

        for (var i = 0; i < _bubbleData.Length; i++) {
            // Only allow the player to release a bubble once every 1.75 seconds and make sure to only act upon the bubble the player is holding
            if (_bubbleData.BubbleComponents[i].BeenHeld != false || release != 1 ||
                !(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - _lastRelease >= 1750)) continue;

            _bubbleData.BubbleComponents[i].CanMove = true;
            _bubbleData.BubbleComponents[i].BeenHeld = true;

            _lastRelease = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

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