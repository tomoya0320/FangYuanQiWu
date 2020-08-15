using UnityEngine;
using System.Collections.Generic;

public static class DialogJsonRead {

    public static int rootIndex;                //根目录下标
    public static int secondIndex;              //二级目录下标
    public static bool accept;                  //是否接受任务
    public static JSONObject currentObject;     //当前数据
    public static bool secondDirectory;         //是否处于二级目录
    public static JSONObject JsonObject;
    public static List<JSONObject> JsonList;

    //加载数据
    public static void LoadJson(TextAsset json) {
        JsonObject = new JSONObject(json.text);
        JsonList = JsonObject["content"].list;
        rootIndex = secondIndex = 0;
        secondDirectory = false;
    }

    //读取下一次对话框数据
    public static JSONObject ReadJson() {
        JSONObject data1;
        if (!secondDirectory) {  //根目录
            if (rootIndex < JsonList.Count) {  //遍历未结束
                currentObject = JsonList[rootIndex];
                if (currentObject["category"].str == "dialog") {    //对白
                    rootIndex++;
                }
                else if (currentObject["category"].str == "task") {    //接任务
                    secondDirectory = true;
                }
                data1 = currentObject;
                return data1;
            }
            else {  //读取完毕
                return null;
            }
        }
        else {  //二级目录
            if (accept) {    //接受了任务
                if (secondIndex < JsonList[rootIndex]["accept"].Count) {  //二级遍历未结束
                    currentObject = JsonList[rootIndex]["accept"][secondIndex];
                    secondIndex++;
                    data1 = currentObject;
                    return data1;
                }
                else {  //二级遍历结束
                    secondDirectory = false;
                    rootIndex++;
                    secondIndex = 0;
                    return ReadJson();
                }
            }
            else {  //拒绝了任务
                if (secondIndex < JsonList[rootIndex]["refuse"].Count) {  //二级遍历未结束
                    currentObject = JsonList[rootIndex]["refuse"][secondIndex];
                    secondIndex++;
                    data1 = currentObject;
                    return data1;
                }
                else {  //二级遍历结束
                    secondDirectory = false;
                    rootIndex++;
                    secondIndex = 0;
                    return ReadJson();
                }
            }
        }
    }

    //任务处理
    public static void AcceptTask(bool a) {
        accept = a;
    }

}