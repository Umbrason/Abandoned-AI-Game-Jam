using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandVisualization : MonoBehaviour
{
    private Deck<CardBase> deck;
    private GameObject cardTemplate;
    private readonly Queue<GameObject> cardInstances = new Queue<GameObject>();    

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
        if (section != Deck<CardBase>.Section.Hand)
            return;
        ClearCards();
        SpawnCards(deck.DrawPile.Length);
    }

    private void SpawnCard()
    {        
        var instance = Instantiate(cardTemplate, transform);
        cardInstances.Enqueue(instance);
    }

    public void SpawnCards(int amount)
    {
        for (int i = 0; i < amount; i++)
            SpawnCard();
    }

    private void ClearCards()
    {
        while (cardInstances.Count > 0)
            Destroy(cardInstances.Dequeue());
    }
}
