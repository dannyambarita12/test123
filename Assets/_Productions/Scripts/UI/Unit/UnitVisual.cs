using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class UnitVisual : MonoBehaviour
{
    [Title("Renderer")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private SortingGroup sortingGroup;

    [SerializeField] 
    private ParticleSystem unitParticleSystem;

    private void Start()
    {
        unitParticleSystem.Pause();
        unitParticleSystem.Clear();
    }

    public void DarkenUnit()
    {
        spriteRenderer.color = Color.gray;    
        sortingGroup.sortingOrder = 0;
    }

    public void HighlightUnit()
    {
        spriteRenderer.color = Color.white;
        sortingGroup.sortingOrder = 1;
    }

    public void TurnUnit(bool toLeft)
    {
        var sc = transform.localScale;
        sc.x = toLeft ? 1 : -1;
        transform.localScale = sc;
    }

    private IEnumerator ShowParticleSystemVisualOnly()
    {
        unitParticleSystem.Clear();
        unitParticleSystem.Play();
        yield return new WaitForSeconds(1f);
        unitParticleSystem.Stop();
        yield return new WaitForSeconds(.5f);        
    }
}
