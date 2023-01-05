using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform parent;
    [SerializeField] GameObject deathVFX;
    [SerializeField] GameObject hitVFX;

    [SerializeField] int hitPoints = 100;
    [SerializeField] int scorePerHit = 20;
    [SerializeField] int damagePerHit = 20;

    Scoreboard scoreBoard;

    void Start() {
        // FindObjectOfType can be resource heavy. Okay in Start(), 
        // but shouldn't use in Update() when it runs each frame.
        scoreBoard = FindObjectOfType<Scoreboard>();    
    }

    void OnParticleCollision(GameObject other) {
        ProcessHit();
        if (hitPoints <= 0) {
            KillEnemy();
        }
    }

    void KillEnemy() {
        GameObject vfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parent;
        Destroy(this.gameObject);
    }

    void ProcessHit() {
        GameObject vfx = Instantiate(hitVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parent;
        scoreBoard.IncreaseScore(scorePerHit);
        hitPoints -= damagePerHit;
    }
}
