using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("General")]
    public float StartSpeed = 10f;
    public float StartHealth = 100;
    public float health;
    public int worth = 50;
    public int exp = 20;
    public int kills = 1;
    public GameObject DeathEffect;

    [HideInInspector]
    public float speed = 10f;

    [Header("OnFire")]
    public bool isBurning = false;
    public float BurningDamage = 1f;
    public ParticleSystem BurningEffect;
    private float BurningCountDown = 4;

    [Header("Unity Stuff")]
    public Image HealthBar;
    public GameObject FloatingMoneyPrefab;
    public GameObject FloatingExpPrefab;

    private bool IsDead = false;

    public void Start()
    {
        speed = StartSpeed;
        health = StartHealth;     
    }

    void Update()
    {
        BurningCountDown -= Time.deltaTime;

        if (BurningCountDown <= 0)
        {
            isBurning = false;
            BurningEffect.Stop();
        } 
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        HealthBar.fillAmount = health / StartHealth;


        if (BurningCountDown >= 0)
        {
            isBurning = true;
            BurningEffect.Play();
        }

        if (health <= 0 && !IsDead)
        {
            Die();
        }
    }

    public void Slow(float pct)
    {
        speed = StartSpeed * (1f - pct);
    }

    public void ShowFloatingText()
    {
        //adjust rotation and postion
        Quaternion adjRot = Quaternion.Euler(90, transform.rotation.y, transform.rotation.z);
        Vector3 adjPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);

        //Instantiate money and exp
        GameObject FloatingMoneyText = Instantiate(FloatingMoneyPrefab, transform.position, adjRot);
        GameObject FloatingExpText = Instantiate(FloatingExpPrefab, adjPos, adjRot);

        FloatingMoneyText.GetComponentInChildren<TextMesh>().text = "+" + worth.ToString();
        FloatingExpText.GetComponentInChildren<TextMesh>().text = "+" + exp.ToString();
    }

    void Die()
    {
        IsDead = true;

        PlayerStats.Money += worth;
        PlayerStats.Exp += exp;
        PlayerStats.Kills += kills;

        //Death effect
        GameObject Effect = Instantiate(DeathEffect, transform.position, Quaternion.identity);
        Destroy(Effect, 5f);

        WaveSpawner.EnemiesAlive--;

        ShowFloatingText();

        Destroy(gameObject);
    }

}
