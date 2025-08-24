using UnityEngine;

public class DestructableWalls : MonoBehaviour
{
    public void HitDestructableWall()
    {
        Destroy(gameObject);
    }
}
