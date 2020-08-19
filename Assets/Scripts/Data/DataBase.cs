using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Wzb;

public class DataBase
{
    //不同稀有度称号的颜色
    private readonly static Color[] titleColors =
    {
        new Color(0.5764706f,0.7333333f,0.7725491f),
        new Color(0.682353f,0.5333334f,0.7254902f),
        new Color(0.8980393f,0.764706f,0.4745098f)
    };
    private static string userDataPath;
    private static UserData userData;
    //下面的数据id和index一一对应
    private static List<JSONObject> propInfos;
    private static List<JSONObject> monsterInfos;
    private static List<JSONObject> personalityInfos;
    private static List<JSONObject> mapInfos;
    private static List<JSONObject> titleInfos;
    private static List<JSONObject> mainStoryLetterInfos;
    private static List<JSONObject> mainStoryMailInfos;
    private static List<JSONObject> activityGiftInfos;
    private static List<JSONObject> npcInfos;
    private static List<JSONObject> taskInfos;
    private static List<JSONObject> buffInfos;

    //一些游戏数据的初始化操作
    public static async void LoadGameData()
    {
        WeatherController.timer = Time.realtimeSinceStartup;

        userDataPath = Application.persistentDataPath + "/GameData.bin";
        TextAsset textAsset;
        JSONObject jsonInfo;

        textAsset = await Addressables.LoadAssetAsync<TextAsset>("props").Task;
        jsonInfo = new JSONObject(textAsset.text);
        propInfos = jsonInfo["props"].list;

        textAsset = await Addressables.LoadAssetAsync<TextAsset>("monsters").Task;
        jsonInfo = new JSONObject(textAsset.text);
        monsterInfos = jsonInfo["monsters"].list;

        textAsset = await Addressables.LoadAssetAsync<TextAsset>("personalities").Task;
        jsonInfo = new JSONObject(textAsset.text);
        personalityInfos = jsonInfo["personalities"].list;

        textAsset = await Addressables.LoadAssetAsync<TextAsset>("titles").Task;
        jsonInfo = new JSONObject(textAsset.text);
        titleInfos = jsonInfo["titles"].list;   
        
        textAsset = await Addressables.LoadAssetAsync<TextAsset>("maps").Task;
        jsonInfo = new JSONObject(textAsset.text);
        mapInfos = jsonInfo["maps"].list;

        textAsset = await Addressables.LoadAssetAsync<TextAsset>("mainStoryLetters").Task;
        jsonInfo = new JSONObject(textAsset.text);
        mainStoryLetterInfos = jsonInfo["mainStoryLetters"].list;

        textAsset = await Addressables.LoadAssetAsync<TextAsset>("mainStoryMails").Task;
        jsonInfo = new JSONObject(textAsset.text);
        mainStoryMailInfos = jsonInfo["mainStoryMails"].list;

        textAsset = await Addressables.LoadAssetAsync<TextAsset>("activityGifts").Task;
        jsonInfo = new JSONObject(textAsset.text);
        activityGiftInfos = jsonInfo["activityGifts"].list;

        textAsset = await Addressables.LoadAssetAsync<TextAsset>("npcs").Task;
        jsonInfo = new JSONObject(textAsset.text);
        npcInfos = jsonInfo["npcs"].list;

        textAsset = await Addressables.LoadAssetAsync<TextAsset>("tasks").Task;
        jsonInfo = new JSONObject(textAsset.text);
        taskInfos = jsonInfo["tasks"].list;

        textAsset = await Addressables.LoadAssetAsync<TextAsset>("buffs").Task;
        jsonInfo = new JSONObject(textAsset.text);
        buffInfos = jsonInfo["buffs"].list;
    }
    public static void SaveUserData()
    {
        if (userData != null)
        {
            string filePath = Application.persistentDataPath + "/GameData.bin";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(filePath, FileMode.Create);
            //序列化     
            bf.Serialize(fs, userData);
            fs.Close();
        }
    }
    public static void ReadUserData()
    {
        if (IsUserDataExists())
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(userDataPath, FileMode.Open);
            userData = (UserData)bf.Deserialize(fs);
            TimeSpan timeSpan = DateTime.Today - userData.lastLoginTime;
            if(timeSpan.Days >= 1)
            {      
                ResetTask(TaskType.DailyTask);
                ResetTask(TaskType.SideTask);
                ResetActivity();
            }

            userData.lastLoginTime = DateTime.Today;
            TaskCenter.Init();
            fs.Close();
    
            SaveUserData();
        }
        else
        {
            CreateUserData();
        }
    }
    public static void CreateUserData()
    {
        userData = new UserData();
        //新手补给
        AddMail(MailType.System, new Mail("新手补给", "欢迎来到 方圆·齐物 ！", "管理员", 
        new Prop(-2, 100), new Prop(-1, 1000)));
        TaskCenter.Init();
    }
    public static bool IsUserDataExists()
    {
        return File.Exists(userDataPath);
    }
    public static void ResetTask(TaskType type)
    {
        userData.tasks[(int)type].Clear();
        foreach (JSONObject json in GetAllDefaultTaskInfos()[(int)type].list)
        {
            userData.tasks[(int)type].Add(JsonUtility.FromJson<Task>(json.ToString()));
        }
    }
    public static void ResetActivity()
    {
        userData.activityValue = 0;
        userData.receivedActivityGifts.Clear();
    }
    //获取初始的角色和怪物属性、道具和地图的详细描述信息
    public static int GetNPCNum()
    {
        return npcInfos.Count;
    }
    public static Buff GetBuffByIndex(int index)
    {
        if (index >= buffInfos.Count)
        {
            return null;
        }
        return JsonUtility.FromJson<Buff>(buffInfos[index].ToString());
    }
    public static Monster GetDefaultMonsterByIndex(int index)
    {
        Monster monster = JsonUtility.FromJson<Monster>(monsterInfos[index].ToString());
        return monster;
    }
    public static Personality GetDefaultPersonalityByIndex(int index)
    {
        Personality player = JsonUtility.FromJson<Personality>(personalityInfos[index].ToString());
        return player;
    }
    public static JSONObject GetPropInfoById(int id)
    {
        return propInfos.Find(propInfo => propInfo["id"].n == id);
    }
    public static JSONObject GetMapInfoByIndex(int index)
    {
        if (index >= mapInfos.Count)
        {
            return null;
        }
        return mapInfos[index];
    }
    public static JSONObject GetNPCInfoByIndex(int index)
    {
        if (index >= npcInfos.Count)
        {
            return null;
        }
        return npcInfos[index];
    }
    public static JSONObject GetTitleInfoByIndex(int index)
    {
        if (index >= titleInfos.Count)
        {
            return null;
        }
        return titleInfos[index];
    }
    public static JSONObject GetPersonalityInfoByIndex(int index)
    {
        if (index >= personalityInfos.Count)
        {
            return null;
        }
        return personalityInfos[index];
    }
    public static JSONObject GetActivityGiftInfoByIndex(int index)
    {
        if (index >= activityGiftInfos.Count)
        {
            return null;
        }
        return activityGiftInfos[index];
    }
    public static List<JSONObject> GetAllDefaultTaskInfos()
    {
        return taskInfos;
    }
    //获取玩家拥有的道具、人格等信息
    /*这里的道具获取不包含金币、钻石等信息*/
    public static Prop GetOwnedPropById(int id,PropPos propPos = PropPos.Bag)
    {
        List<Prop> propList = null;
        switch (propPos)
        {
            case PropPos.Bag:
                {
                    propList = userData.ownedPropsInBag;
                    break;
                }
            case PropPos.Warehouse:
                {
                    propList = userData.ownedPropsInWarehouse;
                    break;
                }
        }
        return propList.Find(p => p.id == id);
    }
    public static Prop GetOwnedPropByIndex(int index, PropPos propPos = PropPos.Bag)
    {
        List<Prop> propList = null;
        switch (propPos)
        {
            case PropPos.Bag:
                {
                    propList = userData.ownedPropsInBag;
                    break;
                }
            case PropPos.Warehouse:
                {
                    propList = userData.ownedPropsInWarehouse;
                    break;
                }
        }
        if (index >= propList.Count)
        {
            return null;
        }
        return propList[index];
    }
    public static Prop GetOwnedPropByIndex(int index, List<int> classTags, PropPos propPos = PropPos.Bag)
    {
        List<Prop> propList = null;
        switch (propPos)
        {
            case PropPos.Bag:
                {
                    propList = userData.ownedPropsInBag;
                    break;
                }  
            case PropPos.Warehouse:
                {
                    propList = userData.ownedPropsInWarehouse;
                    break;
                }
        }
        propList = propList.FindAll(prop => classTags.Contains((int)GetPropInfoById(prop.id)["class"].n));
        if(index < propList.Count)
        {
            return propList[index];
        }
        return null;
    }
    public static List<Prop> GetOwnedPropListByClassTag(int classTag, PropPos propPos = PropPos.Bag)
    {
        List<Prop> propList = null;
        switch (propPos)
        {
            case PropPos.Bag:
                {
                    propList = userData.ownedPropsInBag;
                    break;
                }
            case PropPos.Warehouse:
                {
                    propList = userData.ownedPropsInWarehouse;
                    break;
                }
        }
        return propList.FindAll(prop => (int)GetPropInfoById(prop.id)["class"].n == classTag);
    }
    /**************************************/
    public static Personality GetOwnedPersonalityByIndex(int index)
    {
        if (index >= userData.ownedPersonalities.Count)
        {
            return null;
        }
        return userData.ownedPersonalities[index];
    }
    public static Personality GetOwnedPersonalityById(int id)
    {
        return userData.ownedPersonalities.Find(p => p.id == id);
    }
    public static List<Personality> GetOwnedPersonalities()
    {
        return userData.ownedPersonalities;
    }
    public static MainMap GetOwnedMainMapByIndex(int index)
    {
        if (index >= userData.ownedMainMaps.Count)
        {
            return null;
        }
        return userData.ownedMainMaps[index];
    }
    //判断玩家是否解锁某个道具、人格等
    public static bool IsExistUnreceivedMail()
    {
        for (int i = 0; i < userData.mails.Count; i++)
        {
            if(userData.mails[i].FindIndex(mail => !mail.isReceived) >= 0)
            {
                return true;
            }
        }
        return false;
    }
    public static bool IsExistUnclaimedTask()
    {
        for (int i = 0; i < userData.tasks.Count; i++)
        {
            int index = userData.tasks[i].FindIndex(task => task.state == TaskState.Completed);
            if(index >= 0 && !userData.tasks[i][index].isIncludeDialogue)
            {
                return true;
            }
        }
        return (userData.activityValue / 25) > userData.receivedActivityGifts.Count;
    }
    public static bool IsUnlockedNPCById(int id)
    {
        return userData.unlockedNPCs.Contains(id);
    }
    public static bool IsUnlockedPropById(int id)
    {
        return userData.unlockedProps.Contains(id);
    }
    public static bool IsUnlockedTitleById(int id)
    {
        return userData.unlockedTitles.Contains(id);
    }
    public static bool IsUnlockedMainMapById(int id)
    {
        return userData.unlockedMainMaps.Contains(id);
    }
    public static bool IsUnlockedPersonalityById(int id)
    {
        return userData.unlockedPersonalities.Contains(id);
    }
    public static bool IsGotActivityGiftByIndex(int index)
    {
        return userData.receivedActivityGifts.Contains(index);
    }
    //获取玩家解锁地图、人格等的数量   
    public static int GetUnlockedTitleNum()
    {
        return userData.unlockedTitles.Count;
    }
    public static int GetUnlockedMainMapNum()
    {
        return userData.unlockedMainMaps.Count;
    }
    public static int GetUnlockedPersonalityNum()
    {
        return userData.unlockedPersonalities.Count;
    }
    //获取玩家当前的一些信息（例如体力值、经验值、金币数等，有当前数值和最大数值的用“/”分割）
    public static int GetLevel()
    {
        return userData.lv;
    }
    public static float GetUserExp()
    {
        return userData.exp;
    }
    public static string GetAPInfo()
    {
        string info = userData.currentAP + "/" + userData.maxAP;
        return info;
    }
    public static int GetCurrentAP()
    {
        return userData.currentAP;
    }
    public static string GetAccount()
    {
        return userData.account;
    }
    public static string GetNickname()
    {
        return userData.nickname;
    }
    public static string GetSignature()
    {
        return userData.signature;
    }
    public static int GetActivityValue()
    {
        return userData.activityValue;
    }
    public static string GetEnergyInfo()
    {
        string info = userData.currentEnergy + "/" + userData.maxEnergy;
        return info;
    }
    public static int GetCurrentTitleId()
    {
        return userData.currentTitleId;
    }
    public static int GetPropNumById(int id, PropPos propPos = PropPos.Bag)
    {
        switch (id)
        {
            case -1:
                {
                    return userData.goldNum;
                }
            case -2:
                {
                    return userData.crystalNum;
                }
        }
        List<Prop> propList = null;
        switch (propPos)
        {
            case PropPos.Bag:
                {
                    propList = userData.ownedPropsInBag;
                    break;
                }
            case PropPos.Warehouse:
                {
                    propList = userData.ownedPropsInWarehouse;
                    break;
                }
        }
        Prop prop = propList.Find(p => p.id == id);
        return prop == null ? 0 : prop.num;
    }
    public static string GetCurrentTitleName()
    {
        if (userData.unlockedTitles.Contains(userData.currentTitleId))
        {
            return titleInfos[userData.currentTitleId]["name"].str;
        }
        else
        {
            return "无";
        }
    }
    public static Color GetCurrentTitleColor()
    {
        if (userData.unlockedTitles.Contains(userData.currentTitleId))
        {
            return titleColors[(int)titleInfos[userData.currentTitleId]["rarity"].n];
        }
        else
        {
            return new Color(0.5843138f, 0.5843138f, 0.5843138f);
        }
    }
    public static int GetCurrentPersonalityId()
    {
        return userData.currentPlayerId;
    }
    public static List<List<Task>> GetAllTaskList()
    {
        return userData.tasks;
    }
    public static int GetNPCDialogueIndexById(int id)
    {
        if (userData.npcDialogueProcesses.ContainsKey(id))
        {
            return userData.npcDialogueProcesses[id];
        }
        return 0;
    }
    public static Personality GetCurrentPersonality()
    {
        return userData.ownedPersonalities[GetCurrentPersonalityId()];
    }
    public static List<Mail> GetMailListByType(MailType mailType)
    {
        return userData.mails[(int)mailType];
    }
    public static List<Task> GetTaskListByType(TaskType taskType)
    {
        return userData.tasks[(int)taskType];
    }
    public static Task GetTask(TaskType taskType, int taskId)
    {
        return userData.tasks[(int)taskType].Find(task => task.id == taskId);
    }
    public static void SetNPCDialogueIndexById(int id, int index)
    {
        if (userData.npcDialogueProcesses.ContainsKey(id))
        {
            userData.npcDialogueProcesses[id] = index;
            SaveUserData();
        }
    }
    //设置玩家当前的一些信息（例如昵称、签名等）
    public static void SetTitleById(int id)
    {
        userData.currentTitleId = id;
        EventCenter.Broadcast(Wzb.EventType.UpdateUserInfo);
        SaveUserData();
    }
    public static void SetNickname(string nickname)
    {
        userData.nickname = nickname;
        EventCenter.Broadcast(Wzb.EventType.UpdateUserInfo);
        SaveUserData();
    }
    public static void SetAPVariation(int variation)
    {
        userData.currentAP += variation;
        SaveUserData();
    }
    public static void SetSignature(string signature)
    {
        userData.signature = signature;
        EventCenter.Broadcast(Wzb.EventType.UpdateUserInfo);
        SaveUserData();
    }
    public static void SetEnergyVariation(int variation)
    {
        userData.currentEnergy = Mathf.Clamp(userData.currentEnergy + variation, 0, userData.maxEnergy);
        SaveUserData();
    }
    public static void SetCurrentPersonalityById(int id)
    {
        if (userData.unlockedPersonalities.Contains(id))
        {
            userData.currentPlayerId = id;
            EventCenter.Broadcast(Wzb.EventType.UpdateLive2D);
            EventCenter.Broadcast(Wzb.EventType.UpdateUserInfo);
            SaveUserData();
        }
    }
    //通过id获得主线信件的具体内容（格式：标题、正文、署名）
    public static string[] GetMainStoryLetterContentsById(int id)
    {
        string[] content = {mainStoryLetterInfos[id]["title"].str,
                            mainStoryLetterInfos[id]["text"].str ,
                            mainStoryLetterInfos[id]["signature"].str };
        return content;
    }
    //获取未读的主线信件列表
    public static List<Letter> GetUnreadMainStoryLetterList()
    {
        return userData.unreadMainStoryLetters;
    }
    //增加或减少玩家的一些拥有物品（这里的物品包括解锁的地图、主线信件等）
    public static void AddPropById(int id, int num)
    {  
        //增加的是金币或钻石的情况
        if (id < 0)
        {
            switch (id)
            {
                case -1:
                    {
                        userData.goldNum += num;
                        break;
                    }
                case -2:
                    {
                        userData.crystalNum += num;
                        break;
                    }
            }
            EventCenter.Broadcast(Wzb.EventType.UpdateUserInfo);
            TaskCenter.UpdateTaskState(RequirementType.PropNum.ToString(), id, num);
            SaveUserData();
            return;
        }
        //增加的是道具的情况
        int index = userData.ownedPropsInBag.FindIndex(prop => prop.id == id);
        if (index >= 0)
        {
            userData.ownedPropsInBag[index].num += num;
            EventCenter.Broadcast(Wzb.EventType.UpdatePropInfo);
            TaskCenter.UpdateTaskState(RequirementType.PropNum.ToString(), id, num);
            SaveUserData();
            return;
        }
        //新增道具
        Prop newProp = new Prop(id, num);
        userData.ownedPropsInBag.Add(newProp);
        userData.unlockedProps.Add(id);
        EventCenter.Broadcast(Wzb.EventType.UpdatePropInfo);
        EventCenter.Broadcast(Wzb.EventType.UpdatePropHandbook);
        TaskCenter.UpdateTaskState(RequirementType.PropNum.ToString(), id, num);
        //更新消耗道具快捷栏
        if (GetPropInfoById(id)["class"].n == 0)
        {
            EventCenter.Broadcast(Wzb.EventType.AddPropToKit, newProp);
        }
        SaveUserData();
    }
    public static void AddExp(int exp)
    {
        userData.exp += exp;
        MessageCenter.Instance.AddMessage(new Message(MessageStyle.ColorFade, string.Format("获得经验值{0}", exp)));
        TaskCenter.UpdateTaskState(RequirementType.Exp.ToString(), 0, exp);
        int oLevel = userData.lv;
        while (userData.exp >= Utils.CalNeededExp(userData.lv))
        {
            userData.exp -= Utils.CalNeededExp(userData.lv);
            userData.lv++;
        }
        if (userData.lv > oLevel)
        {
            TaskCenter.UpdateTaskState(RequirementType.Lv.ToString(), 0, userData.lv - oLevel);
            string tip = string.Format("等级提升至Lv{0}", userData.lv);
            MessageCenter.Instance.AddMessage(new Message(MessageStyle.ColorFade, tip));
        }
        EventCenter.Broadcast(Wzb.EventType.UpdateUserInfo);
        SaveUserData();
    }
    public static void AddNPCById(int id)
    {
        userData.unlockedNPCs.Add(id);
        EventCenter.Broadcast(Wzb.EventType.UpdateNPCHandbook);
        SaveUserData();
    }
    public static void AddMainMapById(int id)
    {
        if (userData.ownedMainMaps.FindIndex(mainMap => mainMap.id == id) < 0)
        {
            userData.unlockedMainMaps.Add(id);
            userData.ownedMainMaps.Add(new MainMap(id));
            EventCenter.Broadcast(Wzb.EventType.UpdateMainMapHandbook);
            SaveUserData();
        }
    }
    public static void AddTitleById(int id)
    {
        if (!IsUnlockedTitleById(id))
        {
            userData.unlockedTitles.Add(id);
            MessageCenter.Instance.AddMessage(new Message(MessageStyle.ColorFade,
            "获得成就：" + titleInfos[id]["name"].str));
            EventCenter.Broadcast(Wzb.EventType.UpdateTitleInfo);
            SaveUserData();
        }
    }
    public static void AddPersonalityById(int id)
    {
        if (userData.ownedPersonalities.FindIndex(p => p.id == id) >= 0)
        {
            Prop repeatedProp = JsonUtility.FromJson<Prop>(GetPersonalityInfoByIndex(id)["repeatedProp"].ToString());
            AddPropById(repeatedProp.id, repeatedProp.num);
            MessageCenter.Instance.AddMessage(new Message(MessageStyle.ColorFade,
            string.Format("获得：{0}*{1}", GetPropInfoById(repeatedProp.id)["name"].str, repeatedProp.num)));
            return;
        }

        userData.unlockedPersonalities.Add(id);
        userData.ownedPersonalities.Add(GetDefaultPersonalityByIndex(id));
        EventCenter.Broadcast(Wzb.EventType.UpdatePersonalityHandbook);
        EventCenter.Broadcast(Wzb.EventType.UpdatePersonalityInfo);
  
        //这个提示之后再换了
        MessageCenter.Instance.AddMessage(new Message(MessageStyle.ColorFade,
        "获得人格：" + GetPersonalityInfoByIndex(id)["name"].str));

        TaskCenter.UpdateTaskState(RequirementType.PersonalityNum.ToString(), 0, 1);
        SaveUserData();
    }
    public static void TransferProp(int id, int num, PropPos from, PropPos to)
    {
        List<Prop> fromList = null;
        List<Prop> toList = null;

        if (from == PropPos.Bag && to == PropPos.Warehouse)
        {
            fromList = userData.ownedPropsInBag;
            toList = userData.ownedPropsInWarehouse;
        }
        else if (from == PropPos.Warehouse && to == PropPos.Bag)
        {
            fromList = userData.ownedPropsInWarehouse;
            toList = userData.ownedPropsInBag;
        }
        Prop fromProp = fromList.Find(p => p.id == id);
        Prop toProp = toList.Find(p => p.id == id);

        if(fromProp.num > num)
        {
            fromProp.num -= num;          
        }
        else
        {
            fromList.Remove(fromProp);
        }
        if (toProp == null)
        {
            toList.Add(new Prop(id, num));
        }
        else
        {
            toProp.num += num;
        }

        EventCenter.Broadcast(Wzb.EventType.UpdatePropInfo);
        SaveUserData();
    }
    ///<param name="flag">flag = -2表示交任务消耗，flag = -1表示使用，flag = 0表示丢弃，flag = 1表示出售</param>
    public static bool ConsumePropById(int flag, int id, int num)
    {
        //消耗的是金币或钻石的情况
        if (id < 0)
        {
            switch (id)
            {
                case -1:
                    {
                        if(num > userData.goldNum)
                        {
                            return false;
                        }
                        userData.goldNum -= num;
                        break;
                    }
                case -2:
                    {
                        if (num > userData.crystalNum)
                        {
                            return false;
                        }
                        userData.crystalNum -= num;
                        break;
                    }
            }
            EventCenter.Broadcast(Wzb.EventType.UpdateUserInfo);
            TaskCenter.UpdateTaskState(RequirementType.PropNum.ToString(), id, num * (flag == -1 ? -1 : 0));
            SaveUserData();
            return true;
        }
        //消耗的是道具的情况
        int index = userData.ownedPropsInBag.FindIndex(prop => prop.id == id && prop.num >= num);
        if (index >= 0)
        {
            userData.ownedPropsInBag[index].num -= num;
            if (userData.ownedPropsInBag[index].num == 0)
            {
                userData.ownedPropsInBag.RemoveAt(index);
            }
            EventCenter.Broadcast(Wzb.EventType.UpdatePropInfo);
            TaskCenter.UpdateTaskState(RequirementType.PropNum.ToString(), id, num * (flag == -1 ? -1 : 0));
            SaveUserData();
            return true;
        }
        return false;
    }
    public static void UpPersonalityStar(Personality personality)
    {
        //暂时测试用
        if(personality.rarity < 5)
        {
            personality.rarity++;
            personality.maxHP *= 1.5f; 
            personality.currentHP *= 1.5f;
            personality.defence *= 1.5f;
            personality.attack *= 1.5f;
            personality.moveSpeed *= 1.2f;
            SaveUserData();
        }
    }
    //任务、活跃度等相关操作
    public static void CompleteTask(Task task)
    {
        if (task.state == TaskState.Completed)
        {
            task.Complete();
            TaskCenter.UpdateTaskState(task.type.ToString(), task.id, 1);
            EventCenter.Broadcast(Wzb.EventType.UpdateNewContentTip, IsExistUnclaimedTask(), NewContentType.Task);
            SaveUserData();
        }
    }
    public static void AddActivityValue(int value)
    {
        userData.activityValue += value;
        EventCenter.Broadcast(Wzb.EventType.UpdateActivityValue);
        SaveUserData();
    }
    public static void ActiveTask(TaskType type, int id)
    {
        int index = userData.tasks[(int)type].FindIndex(task => task.id == id && task.state == TaskState.Inactive);
        if (index >= 0)
        {
            userData.tasks[(int)type][index].state = TaskState.InProcess;
            TaskCenter.InitTask(userData.tasks[(int)type][index]);
            EventCenter.Broadcast(Wzb.EventType.UpdateTaskView);
            EventCenter.Broadcast(Wzb.EventType.AddTaskToTrackInMap, userData.tasks[(int)type][index]);
            SaveUserData();
        }
    }
    public static void CompleteTask(TaskType type, int id)
    {
        int index = userData.tasks[(int)type].FindIndex(task => task.id == id && task.state == TaskState.Completed);
        if (index >= 0)
        {
            userData.tasks[(int)type][index].Complete();
            TaskCenter.UpdateTaskState(userData.tasks[(int)type][index].type.ToString(), userData.tasks[(int)type][index].id, 1);
            EventCenter.Broadcast(Wzb.EventType.UpdateNewContentTip, IsExistUnclaimedTask(), NewContentType.Task);
            SaveUserData();
        }
    }
    public static void AddReceivedActivityGiftByIndex(int index)
    {
        userData.receivedActivityGifts.Add(index);
        SaveUserData();
    }
    //信件、邮件相关操作
    public static void ReadMail(Mail mail)
    {
        mail.isRead = true;
        SaveUserData();
    }
    public static List<Prop> GetMailAttachments(Mail mail)
    {
        mail.isReceived = true;  
        foreach (Prop ownedPropData in mail.attachments)
        {
            AddPropById(ownedPropData.id, ownedPropData.num);
        }
        List<Prop> acquiredPropList = new List<Prop>(mail.attachments);
        EventCenter.Broadcast(Wzb.EventType.UpdateNewContentTip,
        GetUnreadMainStoryLetterList().Count == 0 && IsExistUnreceivedMail(), NewContentType.Mail);
        return acquiredPropList;
    }
    public static void AddMainStoryLetterById(int id)
    {
        string[] letterContents = GetMainStoryLetterContentsById(id);
        Letter letter = new Letter(letterContents[0], letterContents[1], letterContents[2]);
        userData.unreadMainStoryLetters.Add(letter);
        SaveUserData();
    }
    public static void AddMail(MailType mailType, Mail mail)
    {
        userData.mails[(int)mailType].Add(mail);
        switch (mailType)
        {
            case MailType.MainStory:
                {
                    EventCenter.Broadcast(Wzb.EventType.AddMainStoryMail);
                    break;
                }
            case MailType.System:
                {
                    EventCenter.Broadcast(Wzb.EventType.AddSystemMail);
                    break;
                }
        }
        EventCenter.Broadcast(Wzb.EventType.UpdateNewContentTip,
        GetUnreadMainStoryLetterList().Count == 0 && IsExistUnreceivedMail(), NewContentType.Mail);
        SaveUserData();
    }
    public static bool DeleteMail(MailType mailType, Mail mail)
    {
        if (!userData.mails[(int)mailType].Contains(mail))
        {
            return false;
        }
        userData.mails[(int)mailType].Remove(mail);
        SaveUserData();
        return true;
    }
}