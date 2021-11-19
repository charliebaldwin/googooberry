using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCrystal : MonoBehaviour
{
    public CrystalCores crystalData;
    public bool enemiesDefeated = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") && enemiesDefeated)
        {
            Debug.Log("crystal core hit");
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        crystalData.Health -= 1;
        if (crystalData.Health <= 0)
        {
            Destroyed();
        }
    }

    private void Destroyed()
    {
        crystalData.isMined = 1;
        gameObject.SetActive(false);
    }
}
