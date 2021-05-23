using UnityEngine;
using TMPro;

public class GunScript : MonoBehaviour
{

    public GameObject bullet;

    public float shootForce;

    public float shootingRate;
    public float spread;
    public float reloadTime;
    public float range;
    public float timeBetweenShoting;
    public int magazineSize;
    public int bulletsPerTap;
    public bool allowButtonHold;

    private int bulletsLeft;
    private int bulletsShot;

    private bool shooting;
    private bool readyToShoot;
    private bool reloading;

    public Camera playerCam;
    public Transform attackPoint;

    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunationDisplay;

    public bool allowInvoke = true;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    
    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        if (ammunationDisplay != null)
        {
            ammunationDisplay.SetText(bulletsLeft/bulletsPerTap + " / " + magazineSize/bulletsPerTap);
        }
    }

    private void PlayerInput()
    {
        if (allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft<magazineSize && !reloading)
        {
            Reload();
        }

        if (readyToShoot && shooting && !reloading && bulletsLeft<=0)
        {
            Reload();
        }

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray,out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(range);
        }

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
        
        
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);

        // if (muzzleFlash != null)
       //     Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
       
       bulletsLeft--;
       bulletsShot++;

       if (allowInvoke)
       {
           Invoke("ResetShot",timeBetweenShoting);
           allowInvoke = false;
       }

       if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
       {
           Invoke("Shoot", shootingRate);
       }
       
    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime); 
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
