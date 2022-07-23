using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

[ExecuteInEditMode]
public class DeckVisualization : MonoBehaviour
{
    private Deck deck;
    [SerializeField] private GameObject cardTemplate;
    [SerializeField] private TooltipTrigger tooltipTrigger;
    private readonly Queue<GameObject> cardInstances = new Queue<GameObject>();
    [SerializeField] private int seed = 0;


    void OnEnable()
    {
        if (deck == null)
            return;
        deck.OnSectionChanged += OnDeckSectionChanged;
    }

    void OnDisable()
    {
        if (deck == null)
            return;
        deck.OnSectionChanged -= OnDeckSectionChanged;
    }

    public void SetDeck(Deck deck)
    {
        if (this.deck != null)
            deck.OnSectionChanged -= OnDeckSectionChanged;
        this.deck = deck;
        this.deck.OnSectionChanged += OnDeckSectionChanged;
        OnDeckSectionChanged(Deck.Section.DrawPile, deck.DrawPile);
    }

    private void OnDeckSectionChanged(Deck.Section section, IEnumerable<CardBase> content)
    {
        if (section != Deck.Section.DrawPile)
            return;
        tooltipTrigger.tooltipHeader = $"{content.Count()} Cards";
        ClearCards();
        SpawnCards(deck.DrawPile.Length);
    }

    private void SpawnCardOnCardBaseop()
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
            SpawnCardOnCardBaseop();
    }

    private void ClearCards()
    {
        while (cardInstances.Count > 0)
            Destroy(cardInstances.Dequeue());
    }
}
