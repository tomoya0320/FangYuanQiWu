using COSXML;
using COSXML.Auth;
using COSXML.Model.Object;
using COSXML.Utils;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Wzb;

public class UserFeedback : MonoBehaviour
{
    private CosXmlConfig config;
    private readonly string secretId = "AKIDPdcXDevnZ6NdfVw3OZWEk1WorgKkikW4";
    private readonly string secretKey = "TjkW3KRJwKSM3WkpfHfoiX9WkkY7RXTA";

    private void Start()
    {
        config = new CosXmlConfig.Builder()
        .SetConnectionTimeoutMs(60000)  //设置连接超时时间，单位毫秒，默认45000ms
        .SetReadWriteTimeoutMs(40000)  //设置读写超时时间，单位毫秒，默认45000ms
        .IsHttps(true)  //设置默认 HTTPS 请求
        .SetAppid("1300228367") //设置腾讯云账户的账户标识 APPID
        .SetRegion("ap-shanghai") //设置一个默认的存储桶地域
        .Build();
    }
    public void SendFeedback(InputField content)
    {
        if (string.IsNullOrEmpty(content.text))
        {
            return;
        }
        long durationSecond = 600;          //每次请求签名有效时长，单位为秒
        QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(secretId,
          secretKey, durationSecond);

        CosXml cosXml = new CosXmlServer(config, qCloudCredentialProvider);
        try
        {
            string bucket = "fangyuanqiwu-1300228367"; //存储桶，格式：BucketName-APPID
            string key = "UserFeedbacks/" + DateTime.Now.ToString().Replace("/","-") + ".txt"; //对象在存储桶中的位置，即称对象键
            string srcPath =Application.persistentDataPath + "feedback.txt";//本地文件绝对路径
            File.WriteAllText(srcPath, content.text);
            PutObjectRequest request = new PutObjectRequest(bucket, key, srcPath);
            //设置签名有效时长
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
            //设置进度回调
            request.SetCosProgressCallback(delegate (long completed, long total)
            {
                if(completed>= total)
                {
                    MessageCenter.AddMessage(new Message(MessageStyle.ColorFade, "感谢您的反馈！"));
                }
            });
            //执行请求
            PutObjectResult result = cosXml.PutObject(request);
            //对象的 eTag
            string eTag = result.eTag;
        }
        catch (COSXML.CosException.CosClientException clientEx)
        {
            //请求失败
            MessageCenter.AddMessage(new Message(MessageStyle.ColorFade, "发送失败，错误信息：" + clientEx));
        }
        catch (COSXML.CosException.CosServerException serverEx)
        {
            //请求失败
            MessageCenter.AddMessage(new Message(MessageStyle.ColorFade, "发送失败，错误信息：" + serverEx.GetInfo()));
        }
    }
}
