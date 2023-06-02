using System.Collections;
using System.Collections.Generic;
using Services.Cannon;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletController : MonoBehaviour, IBulletController
{
    public Rigidbody rigidbody;
    // [SerializeField] private AudioSource _source;
    // [SerializeField] private AudioClip[] _clips;
    [SerializeField] private GameObject _poofPrefab;
    private bool _isGhost;

    public void Start()
    {
     
    }
    
    public void Fire(Vector3 velocity, bool isGhost) {
        _isGhost = isGhost;
        rigidbody.AddForce(velocity, ForceMode.Impulse);
    }
        
        
    public void OnCollisionEnter(Collision col) {
        if (_isGhost && col.gameObject.tag.Contains("Obstacles"))
        {
            Instantiate(_poofPrefab, col.contacts[0].point, Quaternion.Euler(col.contacts[0].normal));
            Debug.Log("it will hit");
        }
            
        return;
        //_source.clip = _clips[Random.Range(0, _clips.Length)];
        //_source.Play();
    }
}
