
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public float damage = 1f;
    public float range = 100f;
    public float fireRate = 15f;
    private float nextTimeToFire = 0f;
    
    public Camera playerCam;

    public bool isSemiAuto = false;
    public bool isShotGun = false;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, range))
        {
            TargetScript target =hit.transform.GetComponent<TargetScript>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }
}
