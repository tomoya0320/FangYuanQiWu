using System.Collections.Generic;
using System;
using UnityEngine;
using Wzb;

[Serializable]
public class UserData
{
    public int lv;
    public int maxAP;
    public float exp;
    public int goldNum;
    public int currentAP;
    public int maxEnergy;
    public int crystalNum;
    public int currentEnergy;
    public int activityValue;

    public string account;
    public string nickname;
    public string signature;
    public int currentTitleId;
    public int currentPlayerId;
    public DateTime lastLoginTime;
    //list都是按获得的顺序排列的，id和index不一定对应
    public List<Prop> ownedPropsInBag;
    public List<Prop> ownedPropsInWarehouse;
    public List<MainMap> ownedMainMaps;
    public List<Personality> ownedPersonalities;
    //解锁情况
    public HashSet<int> unlockedProps;
    public HashSet<int> unlockedTitles;
    public HashSet<int> unlockedMainMaps;
    public HashSet<int> unlockedPersonalities;
    public HashSet<int> unlockedNPCs;
    //活跃度礼物获取情况
    public HashSet<int> receivedActivityGifts;
    //邮件和主线信件
    public List<List<Mail>> mails;
    public List<Letter> unreadMainStoryLetters;
    //任务数据
    public List<List<Task>> tasks;
    //NPC任务进度
    public Dictionary<int, int> npcDialogueProcesses;

    public UserData()
    {
        lv = 1;
        exp = 0;
        maxAP = 30;
        currentAP = 30;
        maxEnergy = 100;
        activityValue = 0;
        currentEnergy = 100;
        currentTitleId = -1;
        lastLoginTime = DateTime.Today;
        account = SystemInfo.deviceUniqueIdentifier;

        tasks = new List<List<Task>>();
        for (int i = 0; i < Enum.GetValues(typeof(TaskType)).Length; i++)
        {
            List<Task> temp = new List<Task>();
            foreach (JSONObject json in DataBase.GetAllDefaultTaskInfos()[i].list)
            {
                temp.Add(JsonUtility.FromJson<Task>(json.ToString()));
            }
            tasks.Add(temp);
        }

        ownedPropsInBag = new List<Prop>();
        ownedPropsInWarehouse = new List<Prop>();
        ownedMainMaps = new List<MainMap>();
        ownedPersonalities = new List<Personality>();

        unlockedNPCs = new HashSet<int>();
        unlockedProps = new HashSet<int>();
        unlockedTitles = new HashSet<int>();
        unlockedMainMaps = new HashSet<int>();
        unlockedPersonalities = new HashSet<int>();

        receivedActivityGifts = new HashSet<int>();

        unreadMainStoryLetters = new List<Letter>();

        mails = new List<List<Mail>>();
        for (int i = 0; i < Enum.GetValues(typeof(MailType)).Length; i++)
        {
            mails.Add(new List<Mail>());
        }

        npcDialogueProcesses = new Dictionary<int, int>();
        for (int i = 0; i < DataBase.GetNPCNum(); i++)
        {
            npcDialogueProcesses.Add(i, 0);
        }
        //初始角色
        Personality defaultPlayer = DataBase.GetDefaultPersonalityByIndex(0);
        unlockedPersonalities.Add(defaultPlayer.id);
        ownedPersonalities.Add(defaultPlayer);
        currentPlayerId = defaultPlayer.id;
        //初始地图
        unlockedMainMaps.Add(0);
        ownedMainMaps.Add(new MainMap(0));
        //初始主线信件
        string[] letterContents = DataBase.GetMainStoryLetterContentsById(0);
        unreadMainStoryLetters.Add(new Letter(letterContents[0], letterContents[1], letterContents[2]));
    }
}
public enum PropPos
{
    Bag,
    Warehouse,
}
[Serializable]
public class Prop
{
    public int id;
    public int num;

    public Prop(int id,int num)
    {
        this.id = id;
        this.num = num;
    }
}
[Serializable]
public class MainMap
{
    public int id;
    public float degreeOfExploration;
    public MainMap(int id)
    {
        this.id = id;
        degreeOfExploration = 0;
    }
}
[Serializable]
public class Personality : BasicPropertyValue,ICloneable
{
    public int rarity;
    public bool jumpable;
    public float jumpSpeed;
    public float dashSpeedRate;
    public bool dashable;
    public float currentJumpSpeed;
    public float gravity;
    public int illustration2Index;

    public Personality()
    {
        dashSpeedRate = 3.0f;
        gravity = 0.3f;       
        jumpable = true;
        dashable = true;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}
[Serializable]
public class Monster : BasicPropertyValue,ICloneable
{
    public float attackDist;
    public float followDist;
    public float attackDelta;
    public float maxMoveDist;
    public int killExp;

    public object Clone()
    {
        return MemberwiseClone();
    }
}
[Serializable]
public class BasicPropertyValue
{
    public int id;
    public string name;
    public float currentHP;
    public float maxHP;
    public float attack;
    public float defence; 
    public float moveSpeed;
    public int attackMode;
    public float steeringSpeed;
    public float animSpeed;
    public bool interruptable;
    public bool movable;
    public bool attackable;
    public AnimState animState;

    public BasicPropertyValue()
    {
        animSpeed = 1.5f;
        steeringSpeed = 12.0f;
        attackMode = -1;
        movable = true;
        attackable = true; 
        interruptable = true;
        animState = AnimState.Idle;
    }
}