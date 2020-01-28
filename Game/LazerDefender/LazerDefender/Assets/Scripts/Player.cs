using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")] //This creates a header which is basically a format tool in the Unity Editor
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] int health = 200;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.7f; //We are capping this variable
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f; //We are capping this variable


    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;

    Coroutine firingCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    //A Coroutine is a method which can suspend (otherwise known as a yield) its execution until the yield instructions you gave it are met.

    // TO NOTE: We set the laser to Kinematic so forces, collisions, or joints will not affect the rigidbody. It will be under full control of transform.position movements. Kinematic is based off of energy not gravity.
    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    IEnumerator FireContinuously()
    {
        while (true) //Keep replaying this line of code
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity); //Identity = no rotation
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed); //Add a velocity of 10
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod); //This creates the space between the bullets
        }
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1")) //When we press the mouse button down
        {
            firingCoroutine = StartCoroutine(FireContinuously()); //Call this Coroutine
        }

        if(Input.GetButtonUp("Fire1")) //When the Fire1 button is pushed back up
        {
            StopCoroutine(firingCoroutine); //Since we the Coroutine a global variable we can switch it off
        }

    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed; //Left movement is a -1 and right movement is +1 it'll shoot back to the center which is 0. We do Time.deltaTime because all computer run the a certain amount of frames per second differently so what Time.deltaTime returns is the duration of the last frame.
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBoundaries() //We want to clamp to our camera
    {
        Camera gameCamera = Camera.main; //Assigning our main camera
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding; //We are passing in a vector of 0,0,0 because in camera space thats the starting point so we want are min to be that. Converts the max unit of 1 to world space. 
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding; //We are passing in a vector of 1,0,0 because in camera space thats the max space in the camera. Converts the max unit of 1 to world space. 

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding; //We are passing in a vector of 0,0,0 because in camera space thats the starting point so we want are min to be that. Converts the max unit of 1 to world space. 
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding; //We are passing in a vector of 0,1,0 because in camera space thats the max space in the camera. Converts the max unit of 1 to world space. 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damageDealer = collision.gameObject.GetComponent<DamagerDealer>(); //Our enemy has this script so collision with the enemy will also do damage

        if(!damageDealer) //If we don't have a damage dealer return
        {
            return;
        }
        DamageDealer(damageDealer);
    }

    private void DamageDealer(DamagerDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }

    public int GetHealth()
    {
        return health;
    }
}
