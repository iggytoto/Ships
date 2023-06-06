using Unity.Netcode;
using UnityEngine;

namespace Services.Network.Movement
{
    public class TransformState : INetworkSerializable
    {
        public int Tick;
        public Vector3 Position;

        public TransformState()
        {
        }

        public TransformState(int tick, Vector3 position)
        {
            Tick = tick;
            Position = position;
        }


        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            if (serializer.IsReader)
            {
                var reader = serializer.GetFastBufferReader();
                reader.ReadValueSafe(out Tick);
                reader.ReadValueSafe(out Position);
            }
            else
            {
                var writer = serializer.GetFastBufferWriter();
                
                writer.WriteValueSafe(Tick);
                writer.WriteValueSafe(Position);
            }
        }
    }
}