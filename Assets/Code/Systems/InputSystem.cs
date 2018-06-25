using Unity.Entities;
using UnityEngine;

public class InputSystem : ComponentSystem {
    private struct PlayerData {
        public int Length;
        public ComponentArray<InputComponent> InputComponents;
    }
    
    private struct BubbleData {
        public int Length;
        public ComponentArray<BubbleComponent> BubbleComponents;
    }

    [Inject] private PlayerData _playerData;
    [Inject] private BubbleData _bubbleData;
    
    protected override void OnUpdate() {
        var horizontal = Input.GetAxis("Horizontal");

        for (int i = 0; i < _playerData.Length; i++)
            _playerData.InputComponents[i].Direction = horizontal;
    }
}
