using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Manager;
    private int playerIndex = -1;
    private int cardDistributionIndex= 0;
    private int numberOfCard;
    [SerializeField]private PlayerScritps[] players;
    [SerializeField]private RectTransform card_Rect;
    [SerializeField]private float animationTime;
    [SerializeField]private Sprite defaultImage;
    [SerializeField]private List<Sprite> cards;
    [SerializeField]private GameObject gameOverPanel;
    [SerializeField] private Button reset_Btn;
    private List<int> temp_Card_Ids;
    private Vector2 temp;
    private int card_id;
    public List<Sprite> Cards
    {
        get
        {
            return cards;
        }
    }
    private void OnEnable()
    {
        EventManager.Instance.RegisterEvent(EventManager.eGameEvents.PlayerAnimation_Done, TriggerPlayer);
    }
    private void OnDisable()
    {
        EventManager.Instance.DeRegisterEvent(EventManager.eGameEvents.PlayerAnimation_Done, TriggerPlayer);
    }
    private void Awake()
    {
        Manager = this;
    }
    private void Start()
    {
        reset_Btn.onClick.AddListener(() => ResetGame());
        temp_Card_Ids = new List<int>();
        temp = card_Rect.transform.localPosition;
        CardDistrbutionAnimation();
    }

    void CardDistrbutionAnimation()
    {
        card_Rect.gameObject.SetActive(true);
        Vector2 endpos = players[cardDistributionIndex].transform.localPosition;
        Debug.Log(endpos);
        card_Rect.DOAnchorPos(endpos, animationTime).OnComplete(()=>AfterEachCardDistribution());

    }
    void AfterEachCardDistribution()
    {
        numberOfCard++;
        if (numberOfCard >= 2)
        {
            numberOfCard = 0;
            cardDistributionIndex++;
            if (cardDistributionIndex <= 4)
            {
                card_Rect.gameObject.SetActive(false);
                card_Rect.gameObject.transform.localPosition = temp;
                players[cardDistributionIndex - 1].Cards[numberOfCard + 1].sprite = defaultImage;
                players[cardDistributionIndex - 1].Cards[numberOfCard + 1].gameObject.SetActive(true);
                CardDistribution(cardDistributionIndex-1);
                CardDistrbutionAnimation();
            }
            else
            {
                card_Rect.gameObject.SetActive(false);
                players[cardDistributionIndex - 1].Cards[numberOfCard + 1].sprite = defaultImage;
                players[cardDistributionIndex - 1].Cards[numberOfCard + 1].gameObject.SetActive(true);
                CardDistribution(cardDistributionIndex-1);
                Debug.Log("Start Basic Animation");
                TriggerPlayer();
            }
        }
        else
        {
            players[cardDistributionIndex].Cards[numberOfCard - 1].sprite = defaultImage;
            players[cardDistributionIndex].Cards[numberOfCard - 1].gameObject.SetActive(true);
            CardDistribution(cardDistributionIndex);
            card_Rect.gameObject.SetActive(false);
            card_Rect.gameObject.transform.localPosition = temp;
            CardDistrbutionAnimation();
        }
    }
    void CardDistribution(int index)
    {
        do {
            card_id = Random.Range(0, cards.Count);
        }
        while (temp_Card_Ids.Contains(card_id));
        temp_Card_Ids.Add(card_id);
        players[index].CardNumber.Add(card_id);
        
    }
    void TriggerPlayer(params object[] args)
    {
        playerIndex++;
        if (playerIndex <= 4)
        {
            players[playerIndex].BET_BTN.gameObject.SetActive(true);
        }
        else
        {
            //Show Cards.
            EventManager.Instance.TriggerEvent(EventManager.eGameEvents.Show_Card);
            Invoke("ShowGameOverPanel", 1f);
        }
    }

    void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        gameOverPanel.transform.DOScale(Vector3.one, animationTime / 1.25f).SetEase(Ease.InOutElastic);
    }

    void ResetGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
