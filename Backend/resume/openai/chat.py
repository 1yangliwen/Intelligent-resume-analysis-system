import os
import sys
import time
import json
import docx2txt
import requests

def request_openai(prompt, resume_text):
    url = "https://cn2us02.opapi.win/v1/chat/completions"
    api_key = "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"

    payload = json.dumps({
       "model": "gpt-3.5-turbo",
       "messages": [
          {
             "role": "system",
             "content": prompt
          },
          {
             "role": "user",
             "content": f"我这里有一份简历，我需要获取其中的一些信息。简历如下：{resume_text}"
          }
       ]
    })
    headers = {
        'User-Agent': 'Apifox/1.0.0 (https://apifox.com)',
        'Content-Type': 'application/json',
        'Authorization': f'Bearer {api_key}'
    }


    response = requests.request("POST", url, headers=headers, data=payload)

    response_data = response.json()

    # 提取最终content的内容
    content = response_data['choices'][0]['message']['content']
    return content


def main():
    start_time = time.time()
    print(start_time)

    try:
        # 读取命令行参数
        filepath = sys.argv[1].strip()
        print(filepath)
        r_id = sys.argv[2].strip()
        print(r_id)
        extention = sys.argv[3].replace(' ', '')
        print(extention)
        n = sys.argv[4].strip()
        print(n)

        # 使用 os.path.dirname 和 os.path.join 构建新的路径
        base_dir = os.path.dirname(filepath)
        prompt_txt = os.path.join(base_dir, f'{n}.txt')

        print(os.path.join(filepath, f'{r_id}{extention}'))
        print(os.path.join(base_dir, "txt", f'{r_id}.txt'))

        # 现在，resume_txt将指向新生成的txt文件
        resume_txt_path = os.path.join(base_dir, f'txt\\{r_id}.txt')
        print(resume_txt_path)

        # 打开文件并读取内容
        with open(resume_txt_path, 'r', encoding='utf-8') as file:
            resume_text = file.read()

        # 读取txt的prompt文件
        with open(prompt_txt, 'r', encoding='utf-8') as file:
            prompt = file.read().replace('\n', '')

        # 获取OpenAI响应
        answer = request_openai(prompt, resume_text)



        print(answer)

    except Exception as e:
        print("出现异常")
        print(str(e))
        sys.exit(1)

    end_time = time.time()
    print(end_time)
    print('Total execution time: ' + str(end_time - start_time) + ' seconds')

if __name__ == "__main__":
    main()



