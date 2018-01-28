using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomerDetailsUI : CustomerUI {
    public TextMeshProUGUI m_customerStartDate;
    public Image m_activityImg;
    public TextMeshProUGUI m_activityName;
    public Image m_locationImg;
    public TextMeshProUGUI m_locationName;

    public SpeedIndicatorUI m_speedIndicator;

    public float m_timeBetweenAnimationFrames = 0.5f;

    public Sprite[] m_activitySprites;
    public Sprite[] m_activityOffSprites;

    public Sprite m_emptyLocationSprite;
    public Sprite[] m_locationSprites;

    public Image m_head;
    public Image m_face;
    public Image m_hair;
    public CharacterData[] m_characterDatas;

    public GameObject m_reactionTextArea;
    public TextMeshProUGUI m_reactionText;

    public string[] m_boostReactions;
    public string[] m_throttleReactions;
    public string[] m_disconnectReactions;

    protected float m_animationTimer = 0.0f;
    protected int m_frame = 0;

    protected Dictionary<CustomerData.Race, CharacterData> m_characterDataMapping = new Dictionary<CustomerData.Race, CharacterData>();
    protected int m_seed = 0;

    protected void Awake()
    {
        m_reactionTextArea.transform.localScale = Vector3.zero;
        for (int i = 0; i < m_characterDatas.Length; ++i)
        {
            m_characterDataMapping.Add(m_characterDatas[i].m_race, m_characterDatas[i]);
        }
    }

    public override void Init(CustomerData data)
    {
        base.Init(data);
        m_dataUsage.text = string.Format("{0:N2} GB used", data.m_dataUsage);

        System.DateTime dateStarted = TitleBarUI.GameDate.AddDays(-1 * data.m_daysActive);
        m_customerStartDate.text = string.Format("{0}/{1}/{2}", dateStarted.Month, dateStarted.Day, dateStarted.Year);

        m_activityName.text = data.m_activity.m_name;
        m_locationName.text = CustomerData.LOCATION_NAMES[(int)data.m_location];

        m_reactionTextArea.transform.localScale = Vector3.zero;
    }

    public override void UpdateProfileImg()
    {
        CharacterData characterData;
        if(m_characterDataMapping.TryGetValue(m_data.m_race, out characterData))
        {
            m_seed = (int)(Random.value * 10000.0f);
            characterData.GenerateCharacter(m_seed, m_profileImg, m_head, m_face, m_hair);
        }
    }

    protected void Update()
    {
        m_animationTimer += Time.deltaTime;
        float animationTime = m_timeBetweenAnimationFrames;
        switch (m_speedIndicator.m_currentState)
        {
            case SpeedIndicatorUI.SpeedState.Boosted:
                animationTime *= 0.25f;
                break;
            case SpeedIndicatorUI.SpeedState.Throttled:
                animationTime *= 3.0f;
                break;
        }

        if (m_animationTimer > animationTime)
        {
            m_animationTimer = 0.0f;
            m_frame = (m_frame + 1) % 2;
        }

        UpdateAnimation();
    }

    protected void UpdateAnimation()
    {
        if(m_speedIndicator.m_currentState == SpeedIndicatorUI.SpeedState.Disconnected)
        {
            m_locationImg.sprite = m_emptyLocationSprite;
            m_activityImg.sprite = m_activityOffSprites[(int)m_data.m_activity.m_type * 2 + m_frame];
        }
        else
        {
            if (m_frame == 0)
            {
                m_locationImg.sprite = m_emptyLocationSprite;
            }
            else
            {
                m_locationImg.sprite = m_locationSprites[(int)m_data.m_location];
            }
            m_activityImg.sprite = m_activitySprites[(int)m_data.m_activity.m_type * 2 + m_frame];
        }
    }

    public void OnDisconnectChoice()
    {
        CharacterData characterData;
        if (m_characterDataMapping.TryGetValue(m_data.m_race, out characterData))
        {
            m_face.sprite = characterData.GetSprite(characterData.m_negativeFaces, m_seed);
        }

        ShowDialog(m_disconnectReactions[Random.Range(0, m_disconnectReactions.Length)]);
    }

    public void OnThrottleChoice()
    {
        CharacterData characterData;
        if (m_characterDataMapping.TryGetValue(m_data.m_race, out characterData))
        {
            m_face.sprite = characterData.GetSprite(characterData.m_negativeFaces, m_seed);
        }

        ShowDialog(m_throttleReactions[Random.Range(0, m_throttleReactions.Length)]);
    }

    public void OnBoostChoice()
    {
        CharacterData characterData;
        if (m_characterDataMapping.TryGetValue(m_data.m_race, out characterData))
        {
            m_face.sprite = characterData.GetSprite(characterData.m_positiveFaces, m_seed);
        }
        ShowDialog(m_boostReactions[Random.Range(0, m_boostReactions.Length)]);
    }


    public void ShowDialog(string dialog)
    {
        m_reactionText.text = dialog;
        m_reactionTextArea.transform.localScale = Vector3.zero;
        LeanTween.scale(m_reactionTextArea, Vector3.one, 0.33f).setEaseOutBack();//.setOnComplete(HideDialog);
    }

    public void HideDialog()
    {
        LeanTween.scale(m_reactionTextArea, Vector3.zero, 0.33f).setEaseInBack().setDelay(1.0f);
    }
}
