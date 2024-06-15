# 简历分析算法部分

关于简历分析项目算法部分的详细概述，主要介绍一下功能概述、涉及的文件目录结构以及代码的工作流程。

## 功能概述

主要包括以下功能：

- 将Word、PDF和图片格式的简历转换为文本文件。
- 使用百度的简历分析功能提取简历的基本信息，并且把json格式的文件保存到指定文件夹
- 使用预定义的提示（Prompt）作为输入，引导GPT模型进行分析。
- 提取GPT模型返回的结果中的JSON格式数据。
- 将分析结果保存到指定的文件夹中。

## 文件目录结构

项目中涉及的文件目录结构如下所示：

```
简历分析项目
│
├── 原始简历文件夹（Original_Resumes）
│
├── 文本转换文件夹（Text_Conversions）
│   └── [转换后的TXT文件]
│
└── 解析结果文件夹（Analysis_Results）
    ├── 百度解析结果子文件夹（Baidu_Analysis）
    ├── GPT解析人才画像子文件夹（GPT_Talent_Portraits）
    ├── GPT解析人岗匹配程度子文件夹（GPT_Job_Matching）
    └── Prompt文本文件子文件夹（Prompt_Texts）
        ├── 人才画像Prompt（Talent_Portrait_Guide.txt）
        └── 人岗匹配程度Prompt（Job_Matching_Guide.txt）
 
```

## 代码流程

代码的工作流程分为以下几个步骤：

1. **转换简历：** 根据简历的原始格式（Word、PDF、图片），使用相应的转换函数将简历内容转换为文本格式，并保存在`Text_Conversions`文件夹中。

2. **提取基本信息：**使用百度的简历解析接口，对简历文件进行解析，提取基本信息，存入相应的json文件

3. **准备Prompt：** 根据分析需求，从`Prompt_Texts`文件夹中读取相应的提示文本。

4. **调用GPT模型：** 使用转换后的简历文本和读取的提示文本作为输入，请求OpenAI的API，并获取分析结果。

5. **提取并保存JSON结果：** 从GPT模型返回的内容中提取JSON格式的分析结果，并且将提取出的JSON数据保存在`Analysis_Results`文件夹的相应子文件夹中


