using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    
    public TextMeshProUGUI AmmoText;
    public TextMeshProUGUI ScoreText;
    
    private int currentScore = 0;
    private int maxAmmo = 0;
    private int currentAmmo = 0;

    private void Start()
    {
        EventScript.current.onUpdateScore += AddScore;
        EventScript.current.onUpdateMaxAmmo += UpdateMaxAmmo;
        EventScript.current.onUpdateAmmo += UpdateCurrentAmmo;
        UpdateScore();
    }
    private void OnDestroy()
    {
        EventScript.current.onUpdateScore -= AddScore;
        EventScript.current.onUpdateMaxAmmo -= UpdateMaxAmmo;
        EventScript.current.onUpdateAmmo -= UpdateCurrentAmmo;
    }

    private void AddScore(int scoreAdd)
    {
        currentScore += scoreAdd;
        UpdateScore();
    }
    private void UpdateScore()
    {
        ScoreText.text = "Score: " + currentScore.ToString();
    }
    private void UpdateMaxAmmo(int newMax)
    {
        maxAmmo = newMax;
        UpdateAmmoText();
    }
    private void UpdateCurrentAmmo(int current)
    {
        currentAmmo = current;
        UpdateAmmoText();
    }
    private void UpdateAmmoText()
    {
        AmmoText.text = "Ammo: "+currentAmmo.ToString()+"/"+maxAmmo.ToString();
    }
}
