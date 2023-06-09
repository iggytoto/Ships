using Unity.Entities;
using Unity.NetCode;

public struct TestPlayer : IComponentData
{
    [GhostField]
    public int Level;
}









