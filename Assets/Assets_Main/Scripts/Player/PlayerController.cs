using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Scaling Game Design")]
    public float maxExperienceScaling; // the scaling of the max experience of the player
    public int maxNumberOfUpgrades; //a characteristic can only be augmented x times
    public float fireRateScaling; // the scaling of the fire rate

    [Header("In-Game Use")]
    [SerializeField] float cannonRadius;

    [Header("References")]
    //Gameobjects and prefabs
    public GameObject ballPrefab;
    public GameObject collectiblePrefab;
    public GameObject experienceBar;
    public GameObject healthBar;
    public GameObject mimicPrefab;
    public GameObject cannonPrefab;
    [SerializeField] private FixedJoystick _joystick;

    //TMP
    public TMP_Text experienceText;
    public TMP_Text healthText;
    public TMP_Text statsText;
    public TMP_Text levelText;

    //other
    public ScriptableObjectStats playerStatsSO;
    private Image experienceProgress;
    private Image healthProgress;
    public Transform cannonsParentTransform;

    //Movement
    private Rigidbody rb3D;

    //Weapons & Upgrades
    List<int> availableStats = new List<int>();
    private int numberOfChoices;
    ObjectPooling objectPooling;

    //Stats
    Stats playerStats;
    private float fireRateTimer;



    #region Game Design
    /* Rules 
    ---Special---
    - On "special", drop a mimic of the current player weapon that cannot move
    - The player's weapon level resets to 1
    - The player gains health = current level. If max health is achieved, max health is augmented
    - The player's level is set back to 1
    - (not done yet) the player has to choose for another "first" weapon
    */
    #endregion

    private void Awake()
    {
        //Getcomponents
        rb3D = GetComponent<Rigidbody>();
        experienceProgress = experienceBar.GetComponent<Image>();
        healthProgress = healthBar.GetComponent<Image>();
        playerStats = GetComponent<Stats>();
        objectPooling = GameManager.Instance.GetComponent<ObjectPooling>();

    }
    void Start()
    {
        ResetPlayerStatsFromSO();

        // Initialize values
        fireRateTimer = playerStats.fireRate;

        //health
        playerStats.playerHealth = playerStatsSO.playerStartingHealth;
        playerStats.playerHealthMax = playerStats.playerHealth;

        //XP
        playerStats.playerExperience = 0;
        playerStats.playerExperienceMax = playerStatsSO.playerExperienceMax;
        playerStats.currentLevel = 1;

        //Movement
        playerStats.movementSpeed = playerStatsSO.movementSpeed;

        for (int i = 0; i < numberOfChoices; i++)
        {
            availableStats.Add(i);
        }
        RefreshUI();

        PlaceCannons(playerStats, cannonsParentTransform);
    }


    void Update()
    {
        //Automatically shoots
        AutomaticShooting();

        //Automatically gathers
        GatherCollectibles(playerStats.collectionRadius, LayerMask.GetMask(LayerMask.LayerToName(collectiblePrefab.layer)));
    }
    public void SpawnMimic()
    {
        if (playerStats.currentLevel > 1)
        {
            CreateMimic();
            resetPlayerLevel();
            PlaceCannons(playerStats, cannonsParentTransform);
        }

    }

    private void FixedUpdate()
    {
        rb3D.velocity = new Vector3(_joystick.Horizontal * playerStats.movementSpeed, rb3D.velocity.y, _joystick.Vertical * playerStats.movementSpeed );
    }
    #region ResetPlayerLevel
    void resetPlayerLevel()
    {
        ResetPlayerStatsFromSO();
        IncreaseHPFromLevel();
        SetPlayerLevelTo1();
        RefreshUI();
    }

    void ResetPlayerStatsFromSO()
    {
        // Reset player stats
        playerStats.numberOfProjectiles = playerStatsSO.numberOfProjectiles;
        playerStats.fireRate = playerStatsSO.fireRate;
        playerStats.movementSpeed = playerStatsSO.movementSpeed;
        playerStats.projectileSpeed = playerStatsSO.projectileSpeed;
        playerStats.projectilePiercing = playerStatsSO.projectilePiercing;
        playerStats.collectionRadius = playerStatsSO.collectionRadius;
        playerStats.projectileRange = playerStatsSO.projectileRange;
    }
    void IncreaseHPFromLevel()
    {
        playerStats.playerHealth += playerStats.currentLevel;
        if(playerStats.playerHealthMax < playerStats.playerHealth)
        {
            playerStats.playerHealthMax = playerStats.playerHealth;
        }
    }
    void SetPlayerLevelTo1()
    {
        playerStats.currentLevel = 1;
        playerStats.playerExperienceMax = playerStatsSO.playerExperienceMax;
        RefreshUILevel();
    }
    #endregion

    #region Shooting
    private void ShootInMultipleDirections()
    {
        for (int i = 0; i< playerStats.numberOfProjectiles; i++)
        {
            // Instantiating a ball in the right direction
            Vector3 direction = Quaternion.Euler(0, i * (360/ playerStats.numberOfProjectiles), 0) * Vector3.forward;
            //Debug.Log(" i = " + i + "   " + direction);

            // Add the player position
            Vector3 ballPosition = transform.position + direction.normalized;

            // Instantiate the ball
            GameObject newBall = objectPooling.GetPooledObject(objectPooling.projectilePooledObjects, objectPooling.projectilePrefab,ballPosition,null);
            newBall.SetActive(true);

            //Give it its stats
            newBall.GetComponent<ProjectileController>().projectilePiercing = playerStats.projectilePiercing;
            newBall.GetComponent<ProjectileController>().projectileRange = playerStats.projectileRange;

            // Giving her the right direction and speed
            Rigidbody rb = newBall.GetComponent<Rigidbody>();
            rb.velocity = direction * playerStats.projectileSpeed;

            //Giving it the timebefore death
        }
    }
    private void AutomaticShooting()
    {
        if (fireRateTimer >= 0)
        {
            fireRateTimer -= Time.deltaTime;
        }
        else
        {
            ShootInMultipleDirections();
            fireRateTimer = playerStats.fireRate;
        }
    }
    #endregion
    public void IncreaseFireRate()
    {
        playerStats.fireRate /= fireRateScaling;
    }
    public void IncreaseProjectilePiercing()
    {
        playerStats.projectilePiercing++;
    }
    public void IncreaseProjectileNumber()
    {
        playerStats.numberOfProjectiles++;
    }
    public void GatherCollectibles(float radius, LayerMask layerMask)
    {
        //Find all the collectibles in a radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius,layerMask,QueryTriggerInteraction.Collide);

        //Check what collectible it is
        foreach (Collider collider in hitColliders)
        {
            if (collider.GetComponent<Collectible>()!=null)
            {
                if (collider.GetComponent<Collectible>().collectibleType == CollectibleType.XpSmallCollectible || collider.GetComponent<Collectible>().collectibleType == CollectibleType.XpBigCollectible)
                {
                    collider.GetComponent<Collectible>().GetAttracted(transform);
                }
            }
        }
    }
    public void AddExperience(CollectibleType collectibleType)
    {
        //collects small collectible
        if(collectibleType == CollectibleType.XpSmallCollectible)
        {
            playerStats.playerExperience += GameManager.Instance.XpSmallCollectibleAmount;
        }

        //collects big collectible
        if (collectibleType == CollectibleType.XpBigCollectible)
        {
            playerStats.playerExperience += GameManager.Instance.XpBigCollectibleAmount;
        }

        //Check level up
        if (playerStats.playerExperience >= playerStats.playerExperienceMax)
        {
            LevelUp();
        }
        RefreshUI();
    }

    public void AfterLevelUp()
    {
        //destroy all cannons then rebuild cannons
        PlaceCannons(playerStats, cannonsParentTransform);
    }

    void PlaceCannons(Stats p_playerStats, Transform p_parent)
    {
        if (p_parent.name == "Cannons")
        {
            foreach (Transform child in p_parent)
            {
                    Destroy(child.gameObject);
            }
        }
        float angleStep = 360f / p_playerStats.numberOfProjectiles;

        for (int i = 0; i < p_playerStats.numberOfProjectiles; i++)
        {
            float angle = i * angleStep;
            Vector3 spawnPosition = CalculateCannonSpawnPosition(angle);
            Quaternion spawnRotation = Quaternion.Euler(0, angle, 0);

            Instantiate(cannonPrefab, spawnPosition, spawnRotation, p_parent);
        }
    }
    Vector3 CalculateCannonSpawnPosition(float angle)
    {
        float x = Mathf.Sin(Mathf.Deg2Rad * angle) * cannonRadius;
        float z = Mathf.Cos(Mathf.Deg2Rad * angle) * cannonRadius;

        return new Vector3(x, 0, z) + transform.position;
    }

    public void ReducePlayerHealth (int p_playerHealth)
    {
        if(playerStats.playerHealth - p_playerHealth <= 0)
        {
            playerStats.playerHealth = 0;
        }
        else
        {
            playerStats.playerHealth -= p_playerHealth;
        }
        RefreshUIHealth();
        if(playerStats.playerHealth <=0)
        {
            GameManager.Instance.PlayerLost();
        }
    }
    void LevelUp()
    {
        playerStats.playerExperience = -(playerStats.playerExperienceMax - playerStats.playerExperience);
        playerStats.playerExperienceMax = Mathf.FloorToInt(playerStats.playerExperienceMax * maxExperienceScaling);
        playerStats.currentLevel++;
        GameManager.Instance.OnLevelUp();

    }
    void CreateMimic()
    {
        GameObject mimic = Instantiate(mimicPrefab, transform.position, Quaternion.identity);

        Stats mimicStats = mimic.GetComponent<Stats>();

        mimicStats.numberOfProjectiles = playerStats.numberOfProjectiles;
        mimicStats.fireRate = playerStats.fireRate;
        mimicStats.movementSpeed = playerStats.movementSpeed;
        mimicStats.projectilePiercing = playerStats.projectilePiercing;
        mimicStats.projectileSpeed = playerStats.projectileSpeed;
        mimicStats.projectileRange = playerStats.projectileRange;
        mimicStats.collectionRadius = playerStats.collectionRadius;
        mimicStats.playerHealth = playerStats.playerHealth;

        //place his cannons
        PlaceCannons(mimicStats, mimic.transform);
}
    #region Refresh UI
    public void RefreshUI()
    {
        RefreshUILevel();
        RefreshUIXp();
        RefreshUIStats();
        RefreshUIHealth();
    }
    void RefreshUIXp()
    {
        experienceProgress.fillAmount = (float)playerStats.playerExperience / playerStats.playerExperienceMax;
        experienceText.text = playerStats.playerExperience + "/" + playerStats.playerExperienceMax;
    }
    void RefreshUIStats()
    {
        statsText.text = "Bullets: " + playerStats.numberOfProjectiles + "<br>";
        statsText.text += "Fire rate: " +  Mathf.Round(100/playerStats.fireRate)/100 + "<br>";
        statsText.text += "Movement speed: " + playerStats.movementSpeed + "<br>";
        statsText.text += "Projectile speed: " + playerStats.projectileSpeed + "<br>";
        statsText.text += "Projectile piercing: " + playerStats.projectilePiercing + "<br>";
        statsText.text += "Projectile range: " + playerStats.projectileRange + "<br>";
        statsText.text += "Collection radius: " + playerStats.collectionRadius + "<br>";
    }
    void RefreshUIHealth()
    {
        healthProgress.fillAmount = (float)playerStats.playerHealth / playerStats.playerHealthMax;
        healthText.text = playerStats.playerHealth + "/" + playerStats.playerHealthMax;
    }
    void RefreshUILevel()
    {
        levelText.text = "Level " + playerStats.currentLevel;
    }
    #endregion
}
