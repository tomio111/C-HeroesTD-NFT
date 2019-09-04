﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
  public float range;
  public int attack;
  public float attackSpeed;
  public Transform missilePrefab;
  public GameObject currentTarget;
  private HeroStats heroStats;
  private float nextAttackTime;

    void Start(){
        heroStats = GetComponent<HeroStats>();
        attack = heroStats.getAttack();
        attackSpeed = Utils.calculateAttackRate(heroStats.getUnitType(), heroStats.getAgility());
        range = heroStats.getRange();

        nextAttackTime = 0.0f;
    }

    void FixedUpdate(){
        if(Time.time > nextAttackTime){
            acquireAndAttackTarget();
            nextAttackTime = Time.time + attackSpeed;
        }
    }

    private void acquireAndAttackTarget(){
        if(isCurrentTargetInRange()){
            fireMissile();
        } else {
            //Debug.Log("No target. Attempting to acquire new target.");
            acquireNewTarget();
            fireMissile();
        }

    }

    private void acquireNewTarget(){
        List<GameObject> enemiesInRange = Utils.getEnemiesInRange(transform.position, range);
        //Debug.Log("Targets in range:"+enemiesInRange.Count);
        if(enemiesInRange.Count > 0){
            //Debug.Log("Acquiring new target! + " + enemiesInRange[0].transform.name);
            currentTarget = enemiesInRange[0];
        } else {
            currentTarget = null;
        }
    }

    private void fireMissile(){
        if(currentTarget != null){            
            Stats enemyStats = currentTarget.transform.GetComponent<Stats>();
            int damage = Utils.calculateDamageDealt(enemyStats, heroStats.getAttack(), heroStats.getMagic());
            
            Transform missile = Instantiate(missilePrefab, transform.position, transform.rotation);
            missile.GetComponent<Missile>().setTarget(currentTarget.transform);
            missile.GetComponent<Missile>().setDamage(damage);
        }
    }

    private bool isCurrentTargetInRange(){
        if(currentTarget != null){
            return Vector3.Distance(currentTarget.transform.position, transform.position) < range;
        } else {
            return false;
        }
    }
}
