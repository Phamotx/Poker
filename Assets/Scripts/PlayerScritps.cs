using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerScritps : MonoBehaviour
{
    [SerializeField] private Image timer;
    [SerializeField] private float animationTime;
    [SerializeField] private Button bet_Btn;
    [SerializeField] private Image[] cards;
    [SerializeField] private List<int> cardNumbers;
    public Button BET_BTN
    {
        get
        {
            return bet_Btn;
        }
        set
        {
            bet_Btn = value;
        }
    }
    public Image[] Cards
    {
        get
        {
            return cards;
        }
        set
        {
            cards = value;
        }
    }
    public List<int> CardNumber
    {
        get
        {
            return cardNumbers;
        }
        set
        {
            cardNumbers = value;
        }
    }
    private void Awake()
    {
        for(int i = 0; i < cards.Length; i++)
        {
            cards[i].gameObject.SetActive(false);
        }
        cardNumbers = new List<int>();
        bet_Btn.gameObject.SetActive(false);
        bet_Btn.onClick.AddListener(() => TimerAnimation());
    }
    private void OnEnable()
    {
        EventManager.Instance.RegisterEvent(EventManager.eGameEvents.Show_Card, ShowCard);
    }
    private void OnDisable()
    {
        EventManager.Instance.DeRegisterEvent(EventManager.eGameEvents.Show_Card, ShowCard);
    }

    void TimerAnimation()
    {
        timer.DOFillAmount(1, animationTime).OnComplete(()=>AfterAnimationEnd());
    }
    void AfterAnimationEnd()
    {
        timer.gameObject.SetActive(false);
        bet_Btn.gameObject.SetActive(false);
        EventManager.Instance.TriggerEvent(EventManager.eGameEvents.PlayerAnimation_Done);
    }
     void ShowCard(params object[] args) 
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].sprite = GameManager.Manager.Cards[cardNumbers[i]];
        }
    }
}
