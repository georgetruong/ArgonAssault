using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject deathVFX;
    [SerializeField] GameObject hitVFX;

    [SerializeField] int hitPoints = 4;
    [SerializeField] int scorePerHit = 20;
    [SerializeField] int damagePerHit = 1;

    Scoreboard scoreBoard;
    GameObject parentGameObject;

    void Start()
    {
        // FindObjectOfType can be resource heavy. Okay in Start(), 
        // but shouldn't use in Update() when it runs each frame.
        scoreBoard = FindObjectOfType<Scoreboard>();
        parentGameObject = GameObject.FindWithTag("SpawnAtRuntime");

        AddRigidbody();
    }

    private void AddRigidbody()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void OnParticleCollision(GameObject other) {
        ProcessHit();
        if (hitPoints <= 0) {
            KillEnemy();
        }
    }

    void KillEnemy() {
        GameObject vfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parentGameObject.transform;
        Destroy(this.gameObject);
    }

    void ProcessHit() {
        GameObject vfx = Instantiate(hitVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parentGameObject.transform;
        scoreBoard.IncreaseScore(scorePerHit);
        hitPoints -= damagePerHit;
    }
}
