using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StartingStats", order = 1)]
public class ScriptableObjectStats : ScriptableObject
{
    public int numberOfProjectiles; // how many bullets you shoot
    public float fireRate; // shoots a bullet every "fireRate" seconds
    public float movementSpeed; // your move speed
    public float projectileSpeed; // your bullets speed
    public int projectilePiercing; // your bullets piercing
    public float collectionRadius; // increase your collection radius
    public float projectileRange; // increase your collection radius
    public int playerStartingHealth; // starting health of the player
    public int playerExperienceMax; //starting experience of the player
}