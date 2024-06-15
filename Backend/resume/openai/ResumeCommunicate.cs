using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq; // 添加引用
using Google.Protobuf.WellKnownTypes;
using Microsoft.OpenApi.Models;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Crmf;

namespace resume.open
{
    internal class ResumeCommunicate
    {
        static List<string> history = new List<string>(); // 存储对话历史

        public static void Main(string[] args)
        {
            test_main(args);
        }

        public static void test_main(string[] args)
        {
            try
            {
                // 测试简历对话初始化
                Console.WriteLine("******************");
                Console.WriteLine("测试简历对话初始化");
                string resumeFilePath = "E:\\study\\software_engineering\\term_project\\old_code\\resume\\resume\\resume\\Resumes\\2024\\5\\txt\\5ED52233AFA5FDE4438D9469397A8591D757C10889CC03DF34887B41277F8BB1.txt"; // 请将此路径替换为实际文件路径
                string resumeTxt = File.ReadAllText(resumeFilePath);

                // 处理简历内容，去除换行符并转义双引号
                resumeTxt = EscapeJsonString(resumeTxt);

                // 调用 initiateDialogue() 方法
                var initResponse = InitiateDialogue(resumeTxt);

                // 解析 JSON 响应
                JObject initJsonObject = JObject.Parse(initResponse.Content);
                string initContent = (string)initJsonObject["choices"][0]["message"]["content"];
                Console.WriteLine("初始化对话: \n" + initContent);

                // 记录初始内容到对话历史
                history.Add("系统: " + initContent);

                // 测试查询和接收响应功能
                Console.WriteLine("\n******************");
                Console.WriteLine("测试查询和接收响应功能");

                string query = "请描述候选人的市场营销技能如何";
                int queryNumber = 1; // 初始化查询次数

                var queryResponse = SendQueryReceiveResponse(query, resumeTxt, history, queryNumber);
                Console.WriteLine("查询响应: " + queryResponse.Content); // 打印响应内容

                // 解析 JSON 响应
                JObject queryJsonObject = JObject.Parse(queryResponse.Content);
                string queryContent = (string)queryJsonObject["choices"][0]["message"]["content"];
                Console.WriteLine("提问的问题是：\n");
                Console.WriteLine(query);
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("提取的内容: \n" + queryContent);

                // 记录用户问题和系统回答到对话历史
                history.Add("用户: " + query);
                history.Add("系统: " + queryContent);

                // 增加更多的查询和响应记录
                query = "请描述候选人是否符合市场营销的职位";
                queryNumber++;
                queryResponse = SendQueryReceiveResponse(query, resumeTxt, history, queryNumber);
                Console.WriteLine("查询响应: " + queryResponse.Content);

                // 解析 JSON 响应
                queryJsonObject = JObject.Parse(queryResponse.Content);
                queryContent = (string)queryJsonObject["choices"][0]["message"]["content"];
                Console.WriteLine("*****************************************************************");
                Console.WriteLine("提问的问题是：\n");
                Console.WriteLine(query);
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("提取的内容: \n");
                Console.WriteLine(queryContent);
                Console.WriteLine("*****************************************************************");
                // 记录用户问题和系统回答到对话历史
                history.Add("用户: " + query);
                history.Add("系统: " + queryContent);

                // 或者使用 for 循环输出列表的每个元素
                Console.WriteLine("#########################################");
                // 输出对话历史
                for (int i = 0; i < history.Count / 2 + 1; i++)
                {
                    Console.WriteLine($"第{i + 1}次问答的内容：");
                    Console.WriteLine(history[i * 2 + 1]);
                    Console.WriteLine(history[i * 2 + 2]);
                    Console.WriteLine("--------------------------");
                }

                Console.WriteLine("#########################################");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static string EscapeJsonString(string value)
        {
            return value.Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
                        .Replace("\n", "\\n")
                        .Replace("\r", "\\r");
        }

        static RestResponse InitiateDialogue(string resumeTxt)
        {
            var client = new RestClient("https://cn2us02.opapi.win");
            var request = new RestRequest("/v1/chat/completions", Method.Post);

            request.AddHeader("User-Agent", "Apifox/1.0.0 (https://apifox.com)");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer sk-75CC2r7WA85ADd342A9bT3BlbKFJAe31b62A86e84970b333"); // 添加你的 API 密钥

            var body = $@"{{
                ""model"": ""gpt-3.5-turbo"",
                ""messages"": [
                    {{""role"": ""system"", ""content"": ""你是一个智能简历解析助手，接下里我将询问你一些有关于这个简历的内容，请你专业具体的回答""}},
                    {{""role"": ""user"", ""content"": ""
            {resumeTxt}""}}
                ]
            }}
            ";

            Console.WriteLine("请求体: " + body); // 打印请求体

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            var response = client.Execute(request);
            return response;
        }

        static RestResponse SendQueryReceiveResponse(string query, string resumeTxt, List<string> history, int queryNumber)
        {
            var client = new RestClient("https://cn2us02.opapi.win");
            var request = new RestRequest("/v1/chat/completions", Method.Post);

            request.AddHeader("User-Agent", "Apifox/1.0.0 (https://apifox.com)");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer sk-75CC2r7WA85ADd342A9bT3BlbKFJAe31b62A86e84970b333"); // 添加你的 API 密钥

            // 构建历史对话内容
            string historyContent = string.Join("\\n", history.ToArray());
            if (history.Count > 5)
            {
                historyContent = string.Join("\\n", history.GetRange(history.Count - 5, 5).ToArray());
            }

            // 融合 resumeTxt、historyContent 和 query
            string combinedContent = $"简历内容: {resumeTxt}\\n对话历史记录: {historyContent}\\n用户问题: {query}";
            combinedContent = EscapeJsonString(combinedContent);

            var body = $@"{{
                ""model"": ""gpt-3.5-turbo"",
                ""messages"": [
                    {{""role"": ""system"", ""content"": ""你是一个智能简历解析助手，接下来我将询问你一些有关于这个简历的内容，请你专业具体的回答""}},
                    {{""role"": ""user"", ""content"": ""{combinedContent}""}}
                ]
            }}";

            Console.WriteLine("请求体: " + body); // 打印请求体

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            var response = client.Execute(request);
            return response;
        }
    }
}
