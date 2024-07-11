using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float movementSpeed = 1f;
    
    [SerializeField] private float sqrStartAttackDistance = 25;
    [SerializeField] private float sqrAttackDistance = 36;

    [SerializeField] private int amountOfProjectilesForAttackPattern = 3;
    [SerializeField] private float timeBetweenProjectiles = 0.5f;

    [SerializeField] private float timeBetweenAttackPatterns;

    [SerializeField] private Projectile projectilePrf;
    [SerializeField] private Transform spawnProjectilePosition;
    [SerializeField] private float spreadOffset = 1;
    [SerializeField] private float spreadRotation = 10;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() => StartCoroutine(AttackPatter());
    

    private IEnumerator MoveTowardsPlayer() {
        while (!IsPlayerWithinStartAttackDistance()) {
            transform.LookAt(playerTransform);
            transform.Translate(Vector3.forward * movementSpeed);
            yield return null;
        }
    }

    private IEnumerator AttackPatter()
    {
        WaitUntil waitForStartAttackDistance = new WaitUntil(IsPlayerWithinStartAttackDistance);
        WaitForSeconds attackCooldown = new WaitForSeconds(timeBetweenProjectiles);
        
        for (int i = 0; i < amountOfProjectilesForAttackPattern; i++) {
            if (!IsPlayerWithinAttackDistance()) {
                StartCoroutine(MoveTowardsPlayer());
                yield return waitForStartAttackDistance;
            }
            ShootTowardsPlayer();
            yield return attackCooldown;
        }
        
        if (IsPlayerWithinAttackDistance()) {
            StartCoroutine(MoveTowardsPlayer());
            yield return waitForStartAttackDistance;
        }
        SpreadShooting();
        yield return new WaitForSeconds(timeBetweenAttackPatterns);
        StartCoroutine(AttackPatter());
    }

    private void ShootTowardsPlayer()
    {
        transform.LookAt(playerTransform);
        Instantiate(projectilePrf, spawnProjectilePosition.position, Quaternion.identity).Shoot(spawnProjectilePosition.forward);
    }

    private void SpreadShooting() {
        Vector3 rightDirection = spawnProjectilePosition.right;
        
        Instantiate(
                projectilePrf, 
                spawnProjectilePosition.position+(rightDirection*spreadOffset), 
                Quaternion.identity
            )
            .Shoot(spawnProjectilePosition.forward+rightDirection*spreadRotation);
        
        Instantiate(
                projectilePrf, 
                spawnProjectilePosition.position, 
                Quaternion.identity
                )
            .Shoot(spawnProjectilePosition.forward);
        
        Instantiate(
                projectilePrf, 
                spawnProjectilePosition.position-(rightDirection*spreadOffset), 
                Quaternion.identity
            )
            .Shoot(spawnProjectilePosition.forward-rightDirection*spreadRotation);;
    }

    private bool IsPlayerWithinStartAttackDistance() =>
        (playerTransform.position - transform.position).sqrMagnitude < sqrStartAttackDistance;

    private bool IsPlayerWithinAttackDistance() =>
        (playerTransform.position - transform.position).sqrMagnitude < sqrAttackDistance;
}
