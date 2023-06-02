
using UnityEngine;

namespace Services.Cannon
{
    public interface IBulletController
    {
        void Fire(Vector3 velocity, bool isGhost);
    }
}