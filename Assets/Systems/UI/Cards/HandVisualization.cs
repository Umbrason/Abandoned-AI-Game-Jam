using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class HandVisualization : MonoBehaviour
{
    private Deck deck;
    [SerializeField] private GameObject cardTemplate;
    private readonly Queue<GameObject> cardInstances = new Queue<GameObject>();

    private RectTransform rectTransform;
    public RectTransform RectTransform { get { return rectTransform ??= GetComponent<RectTransform>(); } }

    private CardLayoutProvider handLayout;
    private CardLayoutProvider HandLayout { get { return handLayout ??= GetComponent<CardLayoutProvider>(); } }


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
        deck.OnSectionChanged += OnDeckSectionChanged;
        OnDeckSectionChanged(Deck.Section.Hand, deck.Hand);
    }

    private void OnDeckSectionChanged(Deck.Section section, IEnumerable<CardBase> content)
    {
        if (section != Deck.Section.Hand)
            return;
        ClearCards();
        SpawnCards(deck.Hand);
    }

    private void SpawnCard(CardBase card)
    {
        var instance = Instantiate(cardTemplate, transform);
        var tooltipTrigger = instance.GetComponentInChildren<TooltipTrigger>();
        if (tooltipTrigger)
        {
            tooltipTrigger.tooltipHeader = card.Name;
            tooltipTrigger.tooltipBody = card.Description;
        }
        var cardworkImage = instance.GetComponentsInChildren<Image>().Where(i => i.name.ToLower().Contains("cardwork")).SingleOrDefault();
        if (cardworkImage)
        {
            cardworkImage.sprite = CardSpriteLibrary.Instance[card.GetType()];
        }
        var uiCardToken = instance.GetComponentInChildren<UICardToken>();
        if (uiCardToken)
        {
            uiCardToken.SetCard(card);
            uiCardToken.OnEndDragCard = (ct) =>
            {
                if (ct.transform.position.y - ct.RectTransform.rect.height / 2f > RectTransform.rect.height)
                    deck.PlayCard(card);
            };
            uiCardToken.OnPointerEnterCard += HighlightCard;
            uiCardToken.OnPointerExitCard += StopHighlightingCard;
            uiCardToken.OnBeginDragCard += SetPlaceholderCard;
            uiCardToken.OnBeginDragCard += StopHighlightingCard;
            uiCardToken.OnDragCard += SetPlaceholderCard;
            uiCardToken.OnEndDragCard += OnDropCard;

        }
        cardInstances.Enqueue(instance);
    }
    private void SetPlaceholderCard(UICardToken cardToken) => HandLayout?.SetPlaceholderCard(GetCardIndexByPosition(cardToken));
    private void SetCardIndex(UICardToken cardToken, int index) => HandLayout?.ChangeCardIndex(GetCardIndex(cardToken), index);

    private void OnDropCard(UICardToken cardToken)
    {
        if (!HandLayout)
            return;
        var placeholderIndex = HandLayout.PlaceholderPosition;
        HandLayout.ResetPlaceholderCard();
        SetCardIndex(cardToken, placeholderIndex);
        HighlightCard(cardToken);
    }

    private void HighlightCard(UICardToken cardToken) => HandLayout?.SetHighlightedCard(GetCardIndex(cardToken));
    private void StopHighlightingCard(UICardToken cardToken) => HandLayout?.ResetHighlightedCard();

    private int GetCardIndexByPosition(UICardToken cardToken) => HandLayout?.CardIndexByPosition(cardToken.RectTransform.anchoredPosition.x) ?? 0;
    private int GetCardIndex(UICardToken cardToken)
    {
        var children = from i in Enumerable.Range(0, transform.childCount) select transform.GetChild(i);
        var activeRectTransforms = children.Where((x) => x.gameObject.activeSelf);
        return activeRectTransforms.ToList().IndexOf(cardToken.transform);
    }

    public void SpawnCards(CardBase[] cards)
    {
        for (int i = 0; i < cards.Length; i++)
            SpawnCard(cards[i]);
    }

    private void ClearCards()
    {
        while (cardInstances.Count > 0)
            Destroy(cardInstances.Dequeue());
    }
}
