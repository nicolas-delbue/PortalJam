using UnityEngine;

public class Target : MonoBehaviour
{
    public int ScoreAmount;
    public void TargetHit()
    {
        EventScript.current.UpdateScore(ScoreAmount);
        Destroy(gameObject);
    }
}
