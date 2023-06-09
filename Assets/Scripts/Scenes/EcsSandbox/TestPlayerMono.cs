using Unity.Entities;
using UnityEngine;

    public class TestPlayerMono : MonoBehaviour
    {
        public int Level;
    }
    
    public class TestPlayerBaker : Baker<TestPlayerMono>
    {
        public override void Bake(TestPlayerMono authoring)
        {
            var testPlayer = new TestPlayer {Level = authoring.Level};
            
            AddComponent(testPlayer);
        }
    }
