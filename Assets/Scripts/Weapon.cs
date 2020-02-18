using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bullet;
    public Transform FirePoint;
    public Rigidbody2D rb;
    private GameManager gameManager;
    private AudioManager audioManager;
    private float counter = -1f;
    private float reloadTime = .25f;
    private string shotSoundName = "Shot";

    void Start()
    {
        audioManager = AudioManager.instance;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    public void Shoot()
    {
        if ( counter + reloadTime <= Time.time)
        {
            counter = Time.time;
            Instantiate(bullet, FirePoint.position, FirePoint.rotation);
            audioManager.PlaySound(shotSoundName);
            //rb.AddForce(transform.up * -strengh);
        }
    }
}
