using System.Collections.Generic;

namespace Wzb
{
    public class TaskCenter
    {
        private static List<List<Task>> tasks;

        public static void Init()
        {
            tasks = DataBase.AllTaskList;
            foreach (List<Task> taskList in tasks)
            {
                foreach (Task task in taskList)
                {
                    if (task.state == TaskState.InProcess)
                    {
                        InitTask(task);
                    }
                }
            } 
        }
        public static void InitTask(Task task)
        {
            foreach (TaskRequirement requirement in task.taskRequirements)
            {
                if (requirement.flag == 0)
                {
                    task.SyncTaskRequirement(requirement);
                    task.Check();
                }
            }
        }
        public static void UpdateTaskState(string type, int id, int num)
        {
            foreach (List<Task> taskList in tasks)
            {
                foreach (Task task in taskList)
                {
                    if (task.state == TaskState.InProcess || task.state == TaskState.Completed)
                    {
                        task.Update(type, id, num);                     
                    }
                }
            }
            EventCenter.Broadcast(EventType.UpdateTaskView);
            EventCenter.Broadcast(EventType.UpdateNewContentTip, 
            DataBase.IsExistUnclaimedTask(), NewContentType.Task);
        }
    }
}
