using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using UnityEngine;

public class Deck
{
    private readonly Dictionary<Section, List<CardBase>> deckSections;
    public CardBase[] AllCards { get { return deckSections.Values.SelectMany(x => x).ToArray(); } }

    public event Action<Section, IEnumerable<CardBase>> OnSectionChanged;

    public enum Section { DrawPile, Hand, DiscardPile }
    private IEnumerable<CardBase> this[Section index]
    {
        get { return (deckSections[index] ??= new List<CardBase>()).ToArray(); }
        set
        {
            deckSections[index].Clear();
            deckSections[index].AddRange(value);
            OnSectionChanged?.Invoke(index, this[index]);
        }
    }
    public CardBase[] DrawPile
    {
        get { return this[Section.DrawPile].ToArray(); }
        set { this[Section.DrawPile] = value; }
    }
    public CardBase[] Hand
    {
        get { return this[Section.Hand].ToArray(); }
        set { this[Section.Hand] = value; }
    }
    public CardBase[] DiscardPile
    {
        get { return this[Section.DiscardPile].ToArray(); }
        set { this[Section.DiscardPile] = value; }
    }



    public Deck(params CardBase[] cards)
    {
        var sectionEnumValues = Enum.GetValues(typeof(Section));
        deckSections = new Dictionary<Section, List<CardBase>>();
        foreach (Section section in Enum.GetValues(typeof(Section)))
            deckSections.Add(section, new List<CardBase>());
    }

    public void ShuffleNewCardsInto(Section targetSection, params CardBase[] cards)
    {
        var rand = new System.Random();
        this[targetSection] = this[targetSection].Concat(cards).OrderBy(x => rand.Next()).ToArray();
        OnSectionChanged?.Invoke(targetSection, this[targetSection]);
    }

    public void ShuffleIntoFromSections(Section targetSection, params Section[] sourceSections)
    {        
        var cards = sourceSections.Where(s => s != targetSection).Distinct().SelectMany(section => this[section]);
        var rand = new System.Random();
        this[targetSection] = this[targetSection].Concat(cards).OrderBy(x => rand.Next());
        foreach (var sec in sourceSections.Where(s => s != targetSection))
        {
            this[sec] = new CardBase[0];
            OnSectionChanged?.Invoke(sec, this[sec]);
        }
        OnSectionChanged?.Invoke(targetSection, this[targetSection]);
    }

    public void MoveInto(CardBase card, Section destination)
    {
        Section sourceSection = default;
        bool foundSource = false;
        foreach (Section section in Enum.GetValues(typeof(Section)))
        {
            sourceSection = section;
            if (this[section].Contains(card))
            {
                foundSource = true;
                break;
            }
        }
        if (!foundSource)
            return;
        this[sourceSection] = this[sourceSection].Where(x => x != card);
        OnSectionChanged?.Invoke(sourceSection, this[sourceSection]);
        this[destination] = this[destination].Concat(new CardBase[] { card });
        OnSectionChanged?.Invoke(destination, this[destination]);
    }

    public CardBase[] DrawCards(int count = 1, bool autoRefreshFromDP = false)
    {
        var dp = new Queue<CardBase>(DrawPile);
        var drawnCards = new List<CardBase>();
        var toDraw = Mathf.Min(dp.Count, count);
        for (int i = 0; i < toDraw; i++)
            drawnCards.Add(dp.Dequeue());
        DrawPile = dp.ToArray();
        Hand = Hand.Concat(drawnCards).ToArray();
        if (drawnCards.Count < count && autoRefreshFromDP)
        {
            ShuffleIntoFromSections(Section.DrawPile, Section.DiscardPile);
            DrawCards(count - drawnCards.Count, false);
        }        
        return drawnCards.ToArray();
    }

    public void DiscardCards(params CardBase[] cards)
    {
        var hand = new List<CardBase>(Hand);
        foreach (var card in cards)
            hand.Remove(card);
        Hand = hand.ToArray();
        DiscardPile = DiscardPile.Concat(cards).ToArray();
    }
    public CardBase[] DiscardEntireHand()
    {
        var cards = Hand;
        DiscardCards(cards);
        return cards;
    }

    public void RemoveCardsFromDeck(params CardBase[] cards)
    {
        foreach (Section section in Enum.GetValues(typeof(Section)))
        {
            var containsAny = this[section].Any(card => !cards.Contains(card));
            if (!containsAny)
                continue;
            this[section] = this[section].Where(card => !cards.Contains(card));
            OnSectionChanged?.Invoke(section, this[section]);
        }
    }

    public void PlayCard(CardBase card)
    {
        if (!card.CanPlay())
            return;
        card.OnPlay();
        MoveInto(card, Section.DiscardPile);
    }
}