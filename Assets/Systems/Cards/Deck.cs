using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

public class Deck<T> where T : CardBase
{
    private readonly Dictionary<Section, List<T>> deckSections;
    public T[] AllCards { get { return deckSections.Values.SelectMany(x => x).ToArray(); } }

    public event Action<Section, IEnumerable<T>> OnSectionChanged;

    public enum Section { DrawPile, Hand, DiscardPile }
    private IEnumerable<T> this[Section index]
    {
        get { return (deckSections[index] ??= new List<T>()).ToArray(); }
        set { deckSections[index].Clear(); deckSections[index].AddRange(value); }
    }
    public T[] DrawPile
    {
        get { return this[Section.DrawPile].ToArray(); }
        set { this[Section.DrawPile] = value; }
    }
    public T[] Hand
    {
        get { return this[Section.Hand].ToArray(); }
        set { this[Section.Hand] = value; }
    }
    public T[] DiscardPile
    {
        get { return this[Section.DiscardPile].ToArray(); }
        set { this[Section.DiscardPile] = value; }
    }



    public Deck(params T[] cards)
    {
        var sectionEnumValues = Enum.GetValues(typeof(Section));
        deckSections = new Dictionary<Section, List<T>>();
    }

    public void ShuffleCardsInto(Section targetSection, params T[] cards)
    {
        var rand = new Random();
        this[targetSection] = this[targetSection].Concat(cards).OrderBy(x => rand.Next()).ToArray();
        OnSectionChanged?.Invoke(targetSection, this[targetSection]);
    }

    public void ShuffleSectionsInto(Section[] sourceSections, Section targetSection)
    {
        var cards = sourceSections.Distinct().SelectMany(section => this[section]);
        var rand = new Random();
        this[targetSection] = cards.OrderBy(x => rand.Next());
        OnSectionChanged?.Invoke(targetSection, this[targetSection]);
        foreach (var sec in sourceSections)
            OnSectionChanged?.Invoke(sec, this[sec]);
    }

    public void MoveInto(T card, Section destination)
    {
        Section sourceSection;
        foreach (Section section in Enum.GetValues(typeof(Section)))
        {
            sourceSection = section;
            if (this[section].Contains(card))
                break;
        }
    }

    public T[] DrawCards(int count = 1)
    {
        var dp = new Queue<T>(DrawPile);
        var cards = new List<T>();
        for (int i = 0; i < count; i++)
            cards.Add(dp.Dequeue());
        DrawPile = dp.ToArray();
        Hand = Hand.Concat(cards).ToArray();
        return cards.ToArray();
    }

    public void DiscardCards(params T[] cards)
    {
        var hand = new List<T>(Hand);
        foreach (var card in cards)
            hand.Remove(card);
        Hand = hand.ToArray();
        DiscardPile = DiscardPile.Concat(cards).ToArray();
    }
    public T[] DiscardEntireHand()
    {
        var cards = Hand;
        DiscardCards(cards);
        return cards;
    }

    public void RemoveCardsFromDeck(params T[] cards)
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

}