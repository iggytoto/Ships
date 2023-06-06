using UnityEngine;

namespace Services.Network.Movement
{
    public class InputState
    {
        public int Tick;
        public Vector3 MovementInput;

        public InputState(int tick, Vector3 movementInput)
        {
            Tick = tick;
            MovementInput = movementInput;
        }
    }
}