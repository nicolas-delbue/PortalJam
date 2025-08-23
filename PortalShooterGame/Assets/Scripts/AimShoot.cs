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

    private LineRenderer line;

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
    void Start()
    {
        m_Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        currentAmmo = maxAmmo; //This needs to change so different levels are different max ammo
        UnityEngine.Debug.Log(currentAmmo);
    }
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

            Debug.Log("Fired");
            GameObject lineObject = Instantiate(bulletLine, barrelTransform.position, Quaternion.identity);
            line = lineObject.GetComponent<LineRenderer>();

            RayHitAndShoot(barrelTransform.position, shootDirection, 0);

            //Old Code so I can look back on it
            #region
            //RaycastFired(barrelTransform.position, shootDirection, 3, 0);

            //RaycastHit2D hit = Physics2D.Raycast(barrelTransform.position, shootDirection, float.MaxValue, Mask);
            ////Shoots raycast and sees everything it has hit
            //RaycastHit2D[] hits = Physics2D.RaycastAll(barrelTransform.position, shootDirection, float.MaxValue, Mask);
            ////Looks through all hits and sorts from a to b
            //System.Array.Sort(hits, (a, b) => (a.distance.CompareTo(b.distance)));

            ////Updates a line at the barrel's position and prepares to add the next line.
            //UpdateLine(barrelTransform.position, 0);
            //AddLine();
            ////bool didHitaWall = false;
            //ParseHits(hits, barrelTransform.position);

            //if (hit)
            //{
            //    Debug.Log("Hit");
            //    UnityEngine.Debug.DrawLine(barrelTransform.position, hit.point, Color.magenta, 10.0f);
            //    //line.SetPosition(1, hit.point);
            //    UpdateLine(hit.point, 1);

            //    hit.collider.gameObject.GetComponent<testhit>().wasHit();

            //    Vector2 barrelPos = new Vector2(barrelTransform.position.x, barrelTransform.position.y);
            //    Vector2 direction = (hit.point - barrelPos).normalized;
            //    Vector2 bounceDirection = Vector2.Reflect(direction, hit.normal);

            //    RaycastHit2D hitbounce = Physics2D.Raycast(hit.point, bounceDirection, float.MaxValue, Mask);

            //    //didHitaWall = ParseHits(hits, hit.point);

            //    if (hitbounce)
            //    {
            //        //line.positionCount += 1;
            //        AddLine();
            //        Debug.Log("Hit2");
            //        UnityEngine.Debug.DrawLine(hit.point, hitbounce.point, Color.magenta, 10.0f);
            //        //line.SetPosition(2, hitbounce.point);
            //        UpdateLine(hitbounce.point, 2);
            //        hitbounce.collider.gameObject.GetComponent<testhit>().wasHit();
            //        //Vector2 barrelPos2 = new Vector2(barrelTransform.position.x, barrelTransform.position.y);
            //        Vector2 direction2 = (hitbounce.point - hit.point).normalized;
            //        Vector2 bounceDirection2 = Vector2.Reflect(direction2, hitbounce.normal);

            //        RaycastHit2D hitbounce2 = Physics2D.Raycast(hitbounce.point, bounceDirection2, float.MaxValue, Mask);
            //        if (hitbounce2)
            //        {
            //            //line.positionCount += 1;
            //            AddLine();
            //            Debug.Log("Hit2");
            //            UnityEngine.Debug.DrawLine(hitbounce.point, hitbounce2.point, Color.magenta, 10.0f);
            //            UpdateLine(hitbounce2.point, 3);
            //            hitbounce2.collider.gameObject.GetComponent<testhit>().wasHit();
            //        }
            //        else
            //        {
            //            Debug.Log("Miss");
            //            UnityEngine.Debug.DrawLine(hitbounce.point, bounceDirection2 * 100, Color.magenta, 10.0f);
            //            AddLine();
            //            UpdateLine(bounceDirection2 * 100, 3);
            //        }

            //    }
            //    else
            //    {
            //        Debug.Log("Miss");
            //        UnityEngine.Debug.DrawLine(hit.point, bounceDirection * 100, Color.magenta, 10.0f);
            //        AddLine();
            //        UpdateLine(bounceDirection * 100, 2);
            //    }

            //    //StartCoroutine(SpawnTrail(trail,hit.point, hit.normal, bounceDistance,true));
            //    //Destroy(trail, trail.time);
            //}
            //else
            //{
            //    //StartCoroutine(SpawnTrail(trail, shootDirection*100, Vector2.zero, bounceDistance, false));
            //    Debug.Log("Miss");
            //    Vector2 direction = (shootDirection - barrelTransform.position).normalized;
            //    UnityEngine.Debug.DrawLine(barrelTransform.position, direction * 10000, Color.magenta, 10.0f);
            //    line.SetPosition(1, direction * 10000);
            //}
            #endregion

            canFire = false;
            currentAmmo--;
            Destroy(line, 5f);
            Destroy(lineObject, 5f);
        }
    }
    void RayHitAndShoot(Vector2 OriginPosition, Vector2 direction, int currentLinePosition)
    {
        int shotHit;
        RaycastHit2D[] hits = Physics2D.RaycastAll(OriginPosition, direction, float.MaxValue, Mask);
        System.Array.Sort(hits, (a, b) => (a.distance.CompareTo(b.distance)));
        UpdateLine(OriginPosition, currentLinePosition);
        AddLine();
        currentLinePosition++;

        shotHit = ParseHits(hits, OriginPosition);

        if (shotHit >= 0)
        {
            Debug.Log("Hit");
            RaycastHit2D hit = hits[shotHit];
            UnityEngine.Debug.DrawLine(OriginPosition, hit.point, Color.magenta, 10.0f);
            UpdateLine(hit.point, currentLinePosition);

            Vector2 normalDirectionHit = (hit.point - OriginPosition).normalized;
            Vector2 bounceDirection = Vector2.Reflect(direction, hit.normal);

            RayHitAndShoot(hit.point, bounceDirection, currentLinePosition);
        }
        else
        {
            Vector2 normalDirectionMiss = (direction - OriginPosition).normalized;
            UnityEngine.Debug.DrawLine(OriginPosition, normalDirectionMiss * 10000, Color.magenta, 10.0f);
            UpdateLine(direction * 10000, currentLinePosition);
            line.SetPosition(currentLinePosition, normalDirectionMiss * 10000);
        }
    }
    int ParseHits(RaycastHit2D[] hits, Vector2 originalPosition)
    {
        bool WallHit = false;
        int wallIndex = -1;

        for(int i=0; i < hits.Length; i++)
        {
            RaycastHit2D hit = hits[i];
            switch (hit.collider.tag)
            {
                //Breakable Wall breaks
                case "BreakableWall":
                    //hit.collider.GetComponent<BreakableWall>().BreakWall();
                //All Walls bounce shots.
                case "Bounce":
                    WallHit = true;
                    break;
                //Target breaks but shot continues no matter what.
                case "Target":
                    //hit.collider.GetComponent<Target>().TargetBreak();
                    break;
                //Shot goes through 
                case "Portal":
                    //hit.collider.GetComponent<Portal>().PortalRay();
                    break;
                //This is if it doesnt have tag.
                default:
                    Debug.Assert(false, "Ray hit object with: "+hit.collider.tag+" Please Fix Tag or Add Tag");
                    break;
            }
            if(WallHit)
            {
                wallIndex = i;
                break;
            }
        }
        if (WallHit)
        {
            return wallIndex;
        }
        else
        {
            return -1;
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
}
