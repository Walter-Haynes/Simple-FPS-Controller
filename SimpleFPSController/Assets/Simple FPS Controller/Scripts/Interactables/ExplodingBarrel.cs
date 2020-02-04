using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ExplodingBarrel : MonoBehaviour
{
    public float 
        explosionRange = 15, 
        explosionForce = 20000;
    
    private Collider[] _colliders = new Collider[50];
    
    public void Explode()
    {
        Vector3 explosionPos = transform.position + Vector3.up;

        // Spawning an explosion
        ParticleSystem spawnedExplosion = Instantiate(Weapons.ExplosionFX, explosionPos, Quaternion.Euler(Vector3.zero)).GetComponent<ParticleSystem>();

        // Destroying the explosion after its duration
        Destroy(spawnedExplosion.gameObject, spawnedExplosion.main.duration);

        //Collider[] cols = Physics.OverlapSphere(explosionPos, explosionRange);
        Array.Clear(_colliders, 0, _colliders.Length);
        
        int __colliderCount = Physics.OverlapSphereNonAlloc(explosionPos, explosionRange, results: _colliders);

        GrapplingHook.pvm.explosionVelocity += Vector3.ClampMagnitude(
            (PlayerMovement.characterController.transform.position - explosionPos).normalized *
            Mathf.Clamp(value: 1.0f / explosionPos.DistanceSquared(PlayerMovement.characterController.transform.position), 0, .5f) * 
            explosionForce / 80.0f, 
            50);
        
        //GrapplingHook.pvm.gravityVector += (PlayerMovement.characterController.transform.position - explosionPos).normalized * Mathf.Clamp(1.0f / GrapplingHook.DistanceSquared(explosionPos, PlayerMovement.characterController.transform.position), 0, .35f) * explosionForce / 80.0f;
        //PlayerMovement.characterController.Move((PlayerMovement.characterController.transform.position - explosionPos).normalized * (1.0f / GrapplingHook.DistanceSquared(explosionPos, PlayerMovement.characterController.transform.position)) * explosionForce / 50.0f);

        for(int __index = 0; __index < __colliderCount; __index++)
        {
            Debug.DrawLine(transform.position, _colliders[__index].transform.position, Color.red);

            if(_colliders[__index].TryGetComponent(out Rigidbody __rigidbody))
            {
                __rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRange);
            }
        }

        Destroy(this.gameObject);
    }
}