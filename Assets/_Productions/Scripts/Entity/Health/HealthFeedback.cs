using DamageNumbersPro;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class HealthFeedback : MonoBehaviour
{
    [Required("Required Health Component")]
    [SerializeField]
    private Health health;

    [SerializeField]
    private DamageNumberMesh damageNumberMeshPrefab;

    [Title("HEALTH BAR UI")]
    [SerializeField]
    private MMProgressBar healthBar;
    [SerializeField]
    private TextMeshProUGUI healthText;


    private void Awake()
    {
        if (health == null)
        {
            Debug.LogWarning("HealthFeedback: Health component not found in parent.");
        }

        health.OnHit.AddListener(OnHit);
        health.OnChange.AddListener(OnChange);
    }

    private void OnChange(float current, float max)
    {
        healthBar.SetBar(current, 0, max);
        healthText.text = $"{current}";
    }

    private void OnHit(HitData hitData)
    {
        if (hitData.IsInvulnerable)
        {
            damageNumberMeshPrefab.Spawn(transform.position, $"{hitData.InvulnerableType}");
        }
        else
        {
            damageNumberMeshPrefab.Spawn(transform.position, hitData.Amount);
        }                
    }
}
