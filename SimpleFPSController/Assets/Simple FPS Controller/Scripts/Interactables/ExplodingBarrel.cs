using System;
using UnityEngine;

using CommonGames.Utilities.Extensions;

using SimpleFPSController.PlayerSystems.Movement;

using Physics = UnityEngine.Physics;

[RequireComponent(typeof(Rigidbody))]
public class ExplodingBarrel : PlayerBehaviour
{
    public float 
        explosionRange = 15, 
        explosionForce = 20000;
    
    private Collider[] _colliders = new Collider[50];
    
    public void Explode()
    {
        Vector3 explosionPos = transform.position + Vector3.up;
        
        ParticleSystem spawnedExplosion = Instantiate(Weapons.ExplosionFX, explosionPos, Quaternion.Euler(Vector3.zero)).GetComponent<ParticleSystem>();
        Destroy(spawnedExplosion.gameObject, spawnedExplosion.main.duration);

        //Collider[] cols = Physics.OverlapSphere(explosionPos, explosionRange);
        Array.Clear(_colliders, 0, _colliders.Length);
        
        int __colliderCount = Physics.OverlapSphereNonAlloc(explosionPos, explosionRange, results: _colliders);

        float __explosionPlayerDistanceSquared = explosionPos.DistanceSquared(Player.PlayerMotor.transform.position);
        
        Vector3 __explosionVector = (Player.PlayerMotor.transform.position - explosionPos).normalized *
                                    (1.0f / __explosionPlayerDistanceSquared).Clamp(min: 0, max: 0.5f) * explosionForce;
        
        Player.explosionVelocity += Vector3.ClampMagnitude(__explosionVector, maxLength: 50);

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