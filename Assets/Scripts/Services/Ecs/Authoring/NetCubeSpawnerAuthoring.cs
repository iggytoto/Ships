using Unity.Entities;
using UnityEngine;

namespace Services.Ecs.Authoring
{
    public struct NetCubeSpawner : IComponentData
    {
        public Entity Cube;
    }

    [DisallowMultipleComponent]
    public class NetCubeSpawnerAuthoring : MonoBehaviour
    {
        public GameObject Cube;
    
        public class NetCubeSpawnerBaker : Baker<NetCubeSpawnerAuthoring>
        {
            public override void Bake(NetCubeSpawnerAuthoring authoring)
            {
                Debug.Log("I was baked");
                NetCubeSpawner component = default(NetCubeSpawner);
        
                component.Cube = GetEntity(authoring.Cube, TransformUsageFlags.Dynamic);
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, component);
            }
        }
    }
}