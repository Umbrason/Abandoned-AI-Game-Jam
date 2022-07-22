using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DeckVisualization : MonoBehaviour
{
    private Deck<CardBase> deck;
    [SerializeField] private GameObject cardTemplate;
    private readonly Queue<GameObject> cardInstances = new Queue<GameObject>();
    [SerializeField] private int seed = 0;

    void OnEnable()
    {
#if UNITY_EDITOR
        SpawnCards(10);
#endif
        if (deck == null)
            return;
        deck.OnSectionChanged += OnDeckSectionChanged;
    }

    void OnDisable()
    {
#if UNITY_EDITOR
        while (cardInstances.Count > 0)
            DestroyImmediate(cardInstances.Dequeue());
#endif
        if (deck == null)
            return;
        deck.OnSectionChanged -= OnDeckSectionChanged;
    }



    private void OnDeckSectionChanged(Deck<CardBase>.Section section, IEnumerable<CardBase> content)
    {
        if (section != Deck<CardBase>.Section.DrawPile)
            return;
        ClearCards();
        SpawnCards(deck.DrawPile.Length);
    }

    private void SpawnCardOnTop()
    {
        var renderer = cardTemplate.GetComponentInChildren<Renderer>();        
        var cardHeight = renderer.bounds.extents.y * 2;
        var cardCenter = renderer.bounds.center;
        var pos = cardCenter + Vector3.up * cardHeight * cardInstances.Count;
        var angle = Random.Range(-2.5f, 2.5f);
        var instance = Instantiate(cardTemplate, pos, transform.rotation * Quaternion.Euler(0, angle, 0), transform);
        cardInstances.Enqueue(instance);
    }

    public void SpawnCards(int amount)
    {
        Random.InitState(seed);
        for (int i = 0; i < amount; i++)
            SpawnCardOnTop();
    }

    private void ClearCards()
    {
        while (cardInstances.Count > 0)
            Destroy(cardInstances.Dequeue());
    }
}
