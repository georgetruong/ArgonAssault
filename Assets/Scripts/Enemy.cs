using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject deathVFX;
    [SerializeField] Transform parent;
    [SerializeField] int scorePerHit = 100;

    Scoreboard scoreBoard;

    void Start() {
        // FindObjectOfType can be resource heavy. Okay in Start(), 
        // but shouldn't use in Update() when it runs each frame.
        scoreBoard = FindObjectOfType<Scoreboard>();    
    }

    void OnParticleCollision(GameObject other) {
        ProcessHit();
        KillEnemy();
    }

    void KillEnemy() {
        GameObject vfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parent;
        Destroy(this.gameObject);
    }

    void ProcessHit() {
        scoreBoard.IncreaseScore(scorePerHit);
    }
}
