using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CannonController  : MonoBehaviour
{
    public Transform barrelPivot;
    public BulletController _bulletController;
    public ParticleSystem particleSystem;
    public Transform bulletSpawn;
    public float gunPower = 3f;


    public Transform camera;

    public float reloadingTimeSec = 2f;
    private float _previousFire = 0f;

    public Projection _projection;
    

    // Update is called once per frame
    void Update()
    {
        _projection.SimulateTrajectory(_bulletController, bulletSpawn.position, bulletSpawn.forward * gunPower);
        
        if (Input.GetKeyDown(KeyCode.J))
        {
            gunPower += 1f;
        }
        else if(Input.GetKeyDown(KeyCode.M))
        {
            gunPower -= 1f;
        }
        

        //todo
        var aimPoint = T();
        
        barrelPivot.transform.LookAt(aimPoint, Vector3.up);

        if (Input.GetKeyDown(KeyCode.Space)  && _previousFire + reloadingTimeSec < Time.time)
        {
            _previousFire = Time.time;
            var bullet = Instantiate(_bulletController, bulletSpawn.transform.position, Quaternion.identity);

            particleSystem.Play();
            bullet.Fire((bulletSpawn.forward) * gunPower, false);
        }
    }

    private Vector3 T()
    {
        return camera.forward * 100f;
    }
}
