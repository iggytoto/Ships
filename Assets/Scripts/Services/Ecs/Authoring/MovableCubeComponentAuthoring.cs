using Unity.Entities;
using UnityEngine;

namespace Services.Ecs.Authoring
{
    public struct MovableCubeComponent : IComponentData
    {
    }

    [DisallowMultipleComponent]
    public class MovableCubeComponentAuthoring : MonoBehaviour
    {
        class MovableCubeComponentBaker : Baker<MovableCubeComponentAuthoring>
        {
            public override void Bake(MovableCubeComponentAuthoring authoring)
            {
                MovableCubeComponent component = default(MovableCubeComponent);
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, component);
            }
        }
    }
}