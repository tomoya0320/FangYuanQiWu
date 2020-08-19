using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Wzb
{
    public enum TaskState
    {
        Inactive,
        InProcess,
        Completed,
        Final,
    }
    public enum TaskType
    {
        DailyTask,
        MainStoryTask,
        AchievementTask,
        SideTask,
    }
    public enum RequirementType
    {
        PropNum,
        NPCDialogue,
        MonsterKill,
        DailyTask,
        MainStoryTask,
        AchievementTask,
        Exp,
        Lv,
        PersonalityNum,
        ActiveSideTaskNum,
        SurvivalTimeInMap,
        ShopInTrolley,
        EnterMap,
        OpenTreasureBoxInMap,
    }
    [Serializable]
    public class TaskRequirement
    {
        public int flag;//flag=-1表示使用多少某某物品，flag=1表示获得多少某某物品（包括杀怪数、完成任务数）,flag=0表示拥有某种东西
        public bool isConsume;//完成任务后是否消耗所需物品
        public int requirementId;
        public bool isIgnoreId;
        public string description;
        public RequirementType type;
        public int requiredNum;
        public int currentNum;

        public bool Check()
        {
            return currentNum >= requiredNum;
        }
    }
    [Serializable]
    public class TaskReward
    {
        public int exp; 
        public int titleId;
        public int activityValue;
        public List<Prop> props;
    }
    [Serializable]
    public class Task
    {
        public int id; 
        public string title;
        public TaskType type;
        public TaskState state; 
        public int activeTaskId; 
        public string description; 
        public TaskReward taskRewards;
        public bool isIncludeDialogue;
        public bool isAutoComplete;
        public TaskType activeTaskType;
        public List<TaskRequirement> taskRequirements;

        public void Update(string type,int id,int num)
        {
            foreach (TaskRequirement requirement in taskRequirements)
            {
                if (requirement.type.ToString() == type && (requirement.requirementId == id || requirement.isIgnoreId))
                {
                    switch (requirement.flag)
                    {
                        case -1:
                            {
                                requirement.currentNum -= num < 0 ? num : 0;
                                break;
                            }
                        case 0:
                            {
                                SyncTaskRequirement(requirement);
                                break;
                            }
                        case 1:
                            {
                                requirement.currentNum += num > 0 ? num : 0;
                                break;
                            }
                    }
                }         
            }
            Check();
        }
        //同步当前已完成的数量，只适用于诸如拥有xxx类型的任务需求
        public void SyncTaskRequirement(TaskRequirement requirement)
        {
            switch (requirement.type)
            {
                case RequirementType.PropNum:
                    {
                        requirement.currentNum = DataBase.GetPropNumById(requirement.requirementId);
                        break;
                    }
                case RequirementType.Exp:
                    {
                        requirement.currentNum = (int)DataBase.UserExp;
                        break;
                    }
                case RequirementType.Lv:
                    {
                        requirement.currentNum = DataBase.Level;
                        break;
                    }
                case RequirementType.PersonalityNum:
                    {
                        requirement.currentNum = DataBase.UnlockedPersonalityNum;
                        break;
                    }      
                case RequirementType.SurvivalTimeInMap:
                    {
                        requirement.currentNum = PlayerStateManager.currentLiveTime;
                        break;
                    }
            }
        }
        public void Check()
        {
            if (state == TaskState.InProcess || state == TaskState.Completed) {
                bool completed = true;
                foreach (TaskRequirement requirement in taskRequirements)
                {
                    completed = completed && requirement.Check();
                }
                if (completed && state == TaskState.InProcess)
                {
                    state = TaskState.Completed;
                    MessageCenter.Instance.AddMessage(new Message(MessageStyle.ColorFade, string.Format("任务：{0} 已完成！", title)));
                    if (isAutoComplete)
                    {
                        DataBase.CompleteTask(this);
                    }
                }
                else if (!completed && state == TaskState.Completed)
                {
                    state = TaskState.InProcess;
                }
            }
        }
        public async void Complete()
        {
            state = TaskState.Final;
            if (taskRewards.exp > 0)
            {
                DataBase.AddExp(taskRewards.exp);
            }
            if(taskRewards.titleId >= 0)
            {
                DataBase.AddTitleById(taskRewards.titleId);
            }
            if (taskRewards.activityValue > 0)
            {
                DataBase.AddActivityValue(taskRewards.activityValue);
            }
            foreach (Prop prop in taskRewards.props)
            {
                DataBase.AddPropById(prop.id, prop.num);
                JSONObject propInfo = DataBase.GetPropInfoById(prop.id);
                Sprite propIcon = await Addressables.LoadAssetAsync<Sprite>(propInfo["iconFileName"].str).Task;
                MessageCenter.Instance.AddMessage(new Message(MessageStyle.ColorFade,string.Format("获得{0}*{1}", propInfo["name"].str, prop.num), propIcon));
            }
            if (activeTaskId >= 0)
            {
                DataBase.ActiveTask(activeTaskType, activeTaskId);
            }
            foreach(TaskRequirement taskRequirement in taskRequirements)
            {
                if (taskRequirement.isConsume && taskRequirement.type == RequirementType.PropNum)
                {
                    DataBase.ConsumePropById(-2, taskRequirement.requirementId, taskRequirement.requiredNum);
                }
            }
        }
    }
}