using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class AimShoot : MonoBehaviour
{
    private Camera m_Camera;
    private Vector3 mousePos;
    public Transform barrelTransform;
    public bool canFire;
    public int maxAmmo;
    private int currentAmmo = 0;
    private float timer = 0;
    public float timeBetweenFiring;

    //[SerializeField]
    //private TrailRenderer bulletTrail;
    [SerializeField]
    private GameObject bulletLine;
    [SerializeField]
    private float bulletSpeed = 100;
    [SerializeField]
    private LayerMask Mask;
    [SerializeField]
    private bool BouncingBullets;
    [SerializeField]
    private float bounceDistance = 10f;

    private LineRenderer line;
    void Start()
    {
        m_Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        currentAmmo = maxAmmo; //This needs to change so different levels are different max ammo
        UnityEngine.Debug.Log(currentAmmo);
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.Debug.Log(canFire);
        mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition); //updates mousePos to where mouse is on screen
    
        Vector3 rotation = mousePos - transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0,0,rotZ);

        if(!canFire)
        {
            timer += Time.deltaTime;
            if(timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }

        if(currentAmmo <= 0)
        {
            UnityEngine.Debug.Log("Checkup");
            canFire = false;
            //Let level manager know ammo ran out.
        }


        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("AMMO LEFT: " + currentAmmo);
            Shoot();
        }

    }

    private void Shoot()
    {
        if(canFire)
        {
            Vector3 shootDirection = mousePos;
            //TrailRenderer trail = Instantiate(bulletTrail,barrelTransform.position, Quaternion.identity);
            //trail.transform.position = new Vector3(trail.transform.position.x, trail.transform.position.y, 0);

            //Will change so i use raycast all get all hit and then sort based on distance
            //Then check hit tag, if bounce then run coroutine with bounce
            //if hit target break target then continue
            //if hit breakable wall, break wall, stop raycast there
            //if hit portal find other portal raycast from there (through spawn trail)
            Debug.Log("Fired");
            GameObject lineObject = Instantiate(bulletLine, barrelTransform.position, Quaternion.identity);
            line = lineObject.GetComponent<LineRenderer>();

            //RaycastFired(barrelTransform.position, shootDirection, 3, 0);

            RaycastHit2D hit = Physics2D.Raycast(barrelTransform.position, shootDirection, float.MaxValue, Mask);
            UpdateLine(barrelTransform.position, 0);
            AddLine();

            if (hit)
            {
                Debug.Log("Hit");
                UnityEngine.Debug.DrawLine(barrelTransform.position, hit.point, Color.magenta, 10.0f);
                //line.SetPosition(1, hit.point);
                UpdateLine(hit.point, 1);

                Vector2 barrelPos = new Vector2(barrelTransform.position.x, barrelTransform.position.y);
                Vector2 direction = (hit.point - barrelPos).normalized;
                Vector2 bounceDirection = Vector2.Reflect(direction, hit.normal);
                Vector2 lasthitpoint = hit.point;

                RaycastHit2D hitbounce = Physics2D.Raycast(hit.point, bounceDirection, float.MaxValue, Mask);
                if (hitbounce)
                {
                    //line.positionCount += 1;
                    AddLine();
                    Debug.Log("Hit2");
                    UnityEngine.Debug.DrawLine(lasthitpoint, hitbounce.point, Color.magenta, 10.0f);
                    //line.SetPosition(2, hitbounce.point);
                    UpdateLine(hitbounce.point, 2);

                    Vector2 barrelPos2 = new Vector2(barrelTransform.position.x, barrelTransform.position.y);
                    Vector2 direction2 = (hitbounce.point - barrelPos2).normalized;
                    Vector2 bounceDirection2 = Vector2.Reflect(direction2, hitbounce.normal);

                    RaycastHit2D hitbounce2 = Physics2D.Raycast(hitbounce.point, bounceDirection2, float.MaxValue, Mask);
                    if (hitbounce2)
                    {
                        //line.positionCount += 1;
                        AddLine();
                        Debug.Log("Hit2");
                        UnityEngine.Debug.DrawLine(hitbounce.point, hitbounce2.point, Color.magenta, 10.0f);
                        UpdateLine(hitbounce2.point, 3);
                    }
                    else
                    {
                        Debug.Log("Miss");
                        UnityEngine.Debug.DrawLine(hitbounce.point, bounceDirection * 100, Color.magenta, 10.0f);
                        AddLine();
                        UpdateLine(bounceDirection * 100, 3);
                    }

                }
                else
                {
                    Debug.Log("Miss");
                    UnityEngine.Debug.DrawLine(lasthitpoint, bounceDirection * 100, Color.magenta, 10.0f);
                    AddLine();
                    line.SetPosition(2, bounceDirection * 100);
                    UpdateLine(bounceDirection * 100, 2);
                }

                //StartCoroutine(SpawnTrail(trail,hit.point, hit.normal, bounceDistance,true));
                //Destroy(trail, trail.time);
            }
            else
            {
                //StartCoroutine(SpawnTrail(trail, shootDirection*100, Vector2.zero, bounceDistance, false));
                Debug.Log("Miss");
                Vector2 direction = (shootDirection - barrelTransform.position).normalized;
                UnityEngine.Debug.DrawLine(barrelTransform.position, direction * 10000, Color.magenta, 10.0f);
                line.SetPosition(1, direction * 10000);
            }

            canFire = false;
            currentAmmo--;
            Destroy(line, 5f);
            Destroy(lineObject, 5f);
        }
    }

    void UpdateLine(Vector2 origin, int position)
    {
        line.SetPosition(position, origin);
    }
    void AddLine()
    {
        line.positionCount++;
    }
    //    Vector2 barrelPos = new Vector2(barrelTransform.position.x, barrelTransform.position.y);
    //    Vector2 direction = (hit.point - barrelPos).normalized;
    //    Debug.Log("Direction: " + direction);
    //    Debug.Log("Hit Point: " + hit.point);
    //    Vector2 bounceDirection = Vector2.Reflect(direction, hit.normal);
    //    Vector2 lasthitpoint = hit.point;

    //    RaycastHit2D hitbounce = Physics2D.Raycast(hit.point, bounceDirection, float.MaxValue, Mask);
    //    if (hitbounce)
    //    {
    //        //line.positionCount += 1;
    //        AddLine();
    //        Debug.Log("Hit2");
    //        UnityEngine.Debug.DrawLine(lasthitpoint, hitbounce.point, Color.magenta, 10.0f);
    //        line.SetPosition(2, hitbounce.point);
    //        UpdateLine(hitbounce.point, 2);
    //    }
    void RaycastFired(Vector2 Origin, Vector2 Direction, int bounceLeft, int position)
    {
        //line.SetPosition(0, Origin);
        //line.positionCount++;
        UpdateLine(Origin, position);
        AddLine();
        position++;

        if(bounceLeft > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(Origin, Direction, float.MaxValue, Mask);

            if (hit)
            {
                //line.SetPosition(linePosition,hit.point);
                Vector2 barrelPos = new Vector2(Origin.x, Origin.y);
                Vector2 newDirection = (hit.point - barrelPos).normalized;
                Vector2 bounceDirection = Vector2.Reflect(newDirection, hit.normal);
                bounceLeft--;
                UnityEngine.Debug.DrawLine(Origin, hit.point, Color.magenta, 10.0f);
                RaycastFired(hit.point, bounceDirection, bounceLeft, position);
            }
            else
            {
                Vector2 finalDirection = (Direction - Origin).normalized;
                UnityEngine.Debug.DrawLine(Origin, finalDirection * 10000, Color.magenta, 10.0f);
                UpdateLine(finalDirection, position);
            }
        }
    }
    private IEnumerator BulletShot(TrailRenderer Trail, Vector2 HitPoint, Vector2 HitNormal, float BounceDistance, bool MadeImpact)
    {
        Vector2 barrelPos = new Vector2(barrelTransform.position.x, barrelTransform.position.y);
        Vector2 direction = (HitPoint - barrelPos).normalized;
        Vector2 bounceDirection = Vector2.Reflect(direction, HitNormal);
        //RaycastHit2D hitbounce = Physics2D.Raycast(barrelTransform.position, shootDirection, float.MaxValue, Mask);

        yield return null;
    }
}
