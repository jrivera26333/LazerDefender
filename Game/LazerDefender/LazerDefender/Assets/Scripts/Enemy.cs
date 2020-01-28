using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] int scoreValue = 150;
    [SerializeField] float health = 100;

    [Header("Shooting")]
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = .2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject enemyBulletPrefab;
    [SerializeField] float projectileSpeed = 10f;

    [Header("Sound Effects")]
    [SerializeField] GameObject particleExplosion;
    [SerializeField] float durationOfExplosion = .5f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0,1)] float deathSoundVolume = 0.7f; //We are capping this variable
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f; //We are capping this variable



    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        var bullet = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
    }

    private void OnTriggerEnter2D(Collider2D collision) //We don't need physics or coliisions when a lazer hits an object so we are using a trigger in that sense
    {
        DamagerDealer damageDealer = collision.gameObject.GetComponent<DamagerDealer>(); //We are grabbing the DamageDealer component thats on the object that collided with us and assigned it to damageDealer. This is smart because if we have different prefabs we can add different amounts of damage to the bullets or weapons we create.

        if (!damageDealer) //If we don't have a damage dealer return
        {
            return;
        }

        DamageDealer(damageDealer);
    }

    private void DamageDealer(DamagerDealer damageDealer)
    {
        health -= damageDealer.GetDamage(); //We got the damage from the lazer and are subtracting to the health of this class
        damageDealer.Hit();

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        GameObject explosion = Instantiate(particleExplosion, transform.position, Quaternion.identity);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }
}
