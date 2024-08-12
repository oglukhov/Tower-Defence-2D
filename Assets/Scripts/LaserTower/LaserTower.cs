using System.Collections.Generic;
using UnityEngine;

public class LaserTower : MonoBehaviour
{
    public GameObject laserPrefab; // Prefab for the laser
    public Transform firePoint; // Fire point of the laser
    public float laserDamage = 1f; // Damage per second of the laser
    public float maxHealth = 20; // Maximum health of the tower
    public float laserDuration = 3f; // Duration the laser is active
    public float cooldownTime = 20f; // Cooldown time before the laser can fire again
    public float laserLength = 5f; // Fixed length of the laser

    private GameObject currentLaser;
    private float currentHealth;
    private List<GameObject> enemiesInRange = new List<GameObject>();
    private bool isOnCooldown = false;
    private bool isTowerClicked = false;
    StartButtonController startButtonController;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startButtonController = GameObject.Find("LevelManager").GetComponent<StartButtonController>();
        currentHealth = maxHealth; // Initialize current health
    }

    void Update()
    {
        if (isTowerClicked && !isOnCooldown && startButtonController.gameStarted)
        {
            StartFiringLaser();
        }

        if (currentLaser != null)
        {
            //UpdateLaserDirection();
            UpdateLaserDirectionWithAccelerometer();
        }
        if(isOnCooldown){
            gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.gray;
        }
        else {
            gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
        if(ToggleButtonImage.musicOn){
            audioSource.volume = 1;
        }
        if(!ToggleButtonImage.musicOn){
            audioSource.volume = 0;
        }
    }

    void StartFiringLaser()
    {
        if (currentLaser == null)
        {
            currentLaser = Instantiate(laserPrefab, firePoint.position, Quaternion.identity);
            currentLaser.transform.SetParent(firePoint);
            currentLaser.transform.localPosition = Vector3.zero; // Ensure laser starts at firePoint
            currentLaser.transform.localScale = new Vector3(laserLength, currentLaser.transform.localScale.y, currentLaser.transform.localScale.z); // Set laser length
            audioSource.Play();    
            Invoke("StopFiringLaser", laserDuration);
            Invoke("ResetCooldown", cooldownTime);
        }
    }

    void StopFiringLaser()
    {
        if (currentLaser != null)
        {
            Destroy(currentLaser);
            currentLaser = null;
            isOnCooldown = true;
        }
    }

    void ResetCooldown()
    {
        isOnCooldown = false;
        isTowerClicked = false; // Reset the tower click state
    }

    // void UpdateLaserDirection()
    // {
    //     Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     Vector3 direction = (mousePosition - firePoint.position).normalized;
    //     float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //     currentLaser.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    // }

    
    void UpdateLaserDirectionWithAccelerometer()
    {
        Vector3 acceleration = Input.acceleration;
        float angle = Mathf.Atan2(acceleration.y, acceleration.x) * Mathf.Rad2Deg;
        currentLaser.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        Debug.Log("Tower health: " + currentHealth); // Debugging current health

        if (currentHealth <= 0)
        {
            DestroyTower();
        }
    }

    void DestroyTower()
    {
        if(VibrationToggle.isVibrationOn){
            Handheld.Vibrate();
        }

    // Handle the tower destruction (e.g., animation, removal from the game, etc.)
    
        Destroy(gameObject, 0.2f);
    }

    void OnMouseDown()
    {
        // Detect if the tower is clicked (for PC or mobile with mouse support)
        isTowerClicked = true;
    }

    void OnMouseUp()
    {
        // Detect if the tower click is released
        isTowerClicked = false;
    }

    // For iOS touch handling
    /*
    void Update()
    {
        if (Input.touchCount > 0 && !isOnCooldown)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    isTowerClicked = true;
                }
            }
        }

        if (isTowerClicked)
        {
            StartFiringLaser();
        }

        if (currentLaser != null)
        {
            UpdateLaserDirection();
        }
    }
    */
}