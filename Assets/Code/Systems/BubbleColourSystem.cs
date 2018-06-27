
using Unity.Entities;
using UnityEngine;

public class BubbleColourSystem : ComponentSystem  {

    private struct BubbleFilter {
        public MeshRenderer Renderer;
        public BubbleComponent BubbleComponent;
    }
    
    private struct ColourData {
        public int Length;
        public ComponentArray<ColourComponent> ColourComponents;
    }
    
    [Inject] private ColourData _colourData;
    
    protected override void OnUpdate() {
        
        foreach (var entity in GetEntities<BubbleFilter>()) {
            if (entity.BubbleComponent.ColourIndex != -1) continue;
            
            var num = Random.Range(0, _colourData.ColourComponents[0].Colours.Length);

            entity.Renderer.materials[0].color = _colourData.ColourComponents[0].Colours[num];
            entity.BubbleComponent.ColourIndex = num;
        }
    }
}
