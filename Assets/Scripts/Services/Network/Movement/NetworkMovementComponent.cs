using System;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Utils.DataTypes;

namespace Services.Network.Movement
{
    public class NetworkMovementComponent : NetworkBehaviour
    {
        [SerializeField] private CharacterController _cc;
        [SerializeField] private float _speed = 15f;

        private int _tick = 0;
        private float _tickRate = 1f / 30f;
        private float _tickDeltaTime = 0f;
        private const int BUFFER_SIZE = 1024;

        //Only need to replicate input from user if local and server states are different
        private CircularBuffer<int, InputState> _inputStates = new(BUFFER_SIZE, state => state.Tick);
        private CircularBuffer<int, TransformState> _transformStates = new(BUFFER_SIZE, state => state.Tick);
        
        public NetworkVariable<TransformState> ServerTransformState = new();
        private TransformState _previousTransformState;

        //FOR DEBUG ONLY
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private Color _color;

        void OnEnable() => ServerTransformState.OnValueChanged += OnServerStateChanged;
        void OnDisable() => ServerTransformState.OnValueChanged -= OnServerStateChanged;

        
        //Server reconciliation algorithm. 
        private void OnServerStateChanged(TransformState previousvalue, TransformState serverState)
        {
            //_previousTransformState = previousvalue;
            if (!IsLocalPlayer) return;

            if (previousvalue == null)
            {
                _previousTransformState = serverState;
            }

            var calculatedState = _transformStates.FindByKey(serverState.Tick);

            if (calculatedState != null && calculatedState.Position != serverState.Position)
            {
                Debug.Log("Correcting client position");
                //Teleport player to the server position
                TeleportPlayer(serverState);
                //Replay the inputs that happens after

                var inputStates = _inputStates
                    .Where(input => input.Tick > serverState.Tick)
                    .OrderBy(input => input.Tick);
                
                foreach (var inputState in inputStates)
                {
                    MovePlayer(inputState.MovementInput);

                    var transformState = new TransformState(inputState.Tick, transform.position);
                    _transformStates.ReplaceByKey(inputState.Tick, transformState);
                }
            }
        }

        private void TeleportPlayer(TransformState serverState)
        {
            //hack we need to disable before teleporting
            _cc.enabled = false;
            transform.position = serverState.Position;
            _cc.enabled = true;

            _transformStates.ReplaceByKey(serverState.Tick, serverState);
        }

        public void ProcessLocalPlayerMovement(Vector3 movementInput)
        {
            _tickDeltaTime += Time.deltaTime;
            if (_tickDeltaTime <= _tickRate) return;
            
            if (!IsServer)
            {
                MovePlayerServerRpc(_tick, movementInput);
                MovePlayer(movementInput);
            }
            else
            {
                MovePlayer(movementInput);
                ReplaceState(_tick, transform.position);
            }

            var inputState = new InputState(_tick, movementInput);
            var transformState = new TransformState(_tick, transform.position);
            _inputStates.Add(inputState);
            _transformStates.Add(transformState);

            _tickDeltaTime -= _tickRate;
            _tick++;
        }

        public void ProcessSimulatedPlayerMovement()
        {
            _tickDeltaTime += Time.deltaTime;
            if (_tickDeltaTime <= _tickRate) return;
            
            if (ServerTransformState.Value != null)
            {
                //just update position from the server
                transform.position = ServerTransformState.Value.Position;
            }

            _tickDeltaTime -= _tickRate;
            _tick++;
        }

        [ServerRpc]
        private void MovePlayerServerRpc(int tick, Vector3 movementInput)
        {
            // if (_tick != _previousTransformState.Tick + 1) .Log("Loss package");
            MovePlayer(movementInput);
            ReplaceState(tick, transform.position);
        }

        private void ReplaceState(int tick, Vector3 position)
        {
            var state = new TransformState(tick, position);
            
            _previousTransformState = ServerTransformState.Value;
            ServerTransformState.Value = state;
        }

        private void MovePlayer(Vector3 movementInput)
        {
            _cc.Move(movementInput * _speed * _tickRate);
        }

        private void OnDrawGizmos()
        {
            if (ServerTransformState.Value != null)
            {
                Gizmos.color = _color;
                Gizmos.DrawMesh(_meshFilter.mesh, ServerTransformState.Value.Position);
            }
        }
    }
}