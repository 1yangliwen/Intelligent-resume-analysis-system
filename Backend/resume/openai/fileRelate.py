
import os
import openai
import re  # 导入正则表达式模块
import sys
import time
import json
import docx2txt
import sys
import codecs

import shutil

def docx_to_txt(input_docx_path, output_txt_path):
    # 使用docx2txt库读取docx文件内容
    text = docx2txt.process(input_docx_path)

    # 删除重复行
    lines = text.split('\n')
    unique_lines = []
    for line in lines:
        if line not in unique_lines:
            unique_lines.append(line)
    text = '\n'.join(unique_lines)

    # 将读取到的问题内容写入txt文件
    with open(output_txt_path, 'w', encoding='utf-8') as file:
        file.write(text)




# 导入所需的库
from pdfminer.pdfparser import PDFParser, PDFDocument
from pdfminer.pdfdevice import PDFDevice
from pdfminer.pdfinterp import PDFResourceManager, PDFPageInterpreter
from pdfminer.converter import PDFPageAggregator
from pdfminer.layout import LTTextBoxHorizontal, LAParams
from pdfminer.pdfinterp import PDFTextExtractionNotAllowed



# 定义函数，接受输入的PDF文件路径和输出的txt文件路径
def pdf_to_txt(input_pdf_path, output_txt_path):

    open(output_txt_path, 'w').close()  # 清空txt文件

    # 定义内部函数parse，处理PDF文件
    def parse(input_pdf_file, output_txt_file):
        
        # 用文件对象创建一个PDF文档分析器
        parser = PDFParser(input_pdf_file)
        # 创建一个PDF文档
        doc = PDFDocument()
        # 分析器和文档相互连接
        parser.set_document(doc)
        doc.set_parser(parser)
        # 提供初始化密码，没有默认为空
        doc.initialize()

        # 检查文档是否可以转成TXT，如果不可以就忽略
        if not doc.is_extractable:
            raise PDFTextExtractionNotAllowed
        else:
            # 创建PDF资源管理器，来管理共享资源
            rsrcmgr = PDFResourceManager()
            # 创建一个PDF设备对象
            laparams = LAParams()
            # 将资源管理器和设备对象聚合
            device = PDFPageAggregator(rsrcmgr, laparams=laparams)
            # 创建一个PDF解释器对象
            interpreter = PDFPageInterpreter(rsrcmgr, device)

            # 循环遍历列表，每次处理一个page内容
            for page in doc.get_pages():
                interpreter.process_page(page)
                # 接收该页面的LTPage对象
                layout = device.get_result()
                
                for x in layout:
                    try:
                        if isinstance(x, LTTextBoxHorizontal):
                            with open(output_txt_file, 'a', encoding='utf-8-sig') as f:
                                result = x.get_text()
                                # 删除任何前导/尾随的空格
                                result = result.strip()
                                # 如果行不为空，则写入文件
                                if result != '':
                                    f.write(result + "\n")
                    except:
                        print("Failed")

    # 打开并处理PDF文件
    with open(input_pdf_path, 'rb') as pdf_file:
        parse(pdf_file, output_txt_path)







from ecloud import CMSSEcloudOcrClient
import json

accesskey = '4863f884aef84ea4a4af9895285b75ec' 
secretkey = '249b66cddeaa453f8c3689761476b08a'
url = 'https://api-wuxi-1.cmecloud.cn:8443'


def img_to_txt(input_img_path, output_txt_path):
    print("正在从图片转化为txt")
    print(input_img_path)
    print(output_txt_path)
    requesturl = '/api/ocr/v1/webimage'
    try:
        ocr_client = CMSSEcloudOcrClient(accesskey, secretkey, url)
        response = ocr_client.request_ocr_service_file(requestpath=requesturl, imagepath=input_img_path)

        response_json = json.loads(response.text)  # 解析JSON
        words_info = response_json['body']['content']['prism_wordsInfo']  # 取出所有识别出的文字的信息

        with open(output_txt_path, 'w', encoding='utf-8') as file:
            for word_info in words_info:
                file.write(word_info['word'] + '\n')  # 将识别出的文字写入到文件中
    except ValueError as e:
        print(e)
    return output_txt_path


import os
import win32com.client as win32
from docx import Document

import os
import tempfile

def doc_to_txt(input_doc_path, output_txt_path):
    # 生成临时的图片路径
    output_image_path = tempfile.mktemp(suffix='.png')

    # 将.doc文档转为图片
    convert_doc_to_image(input_doc_path, output_image_path)

    # 将图片转为.txt文件
    img_to_txt(output_image_path, output_txt_path)

    # 删除临时的图片文件
    os.remove(output_image_path)

    return output_txt_path








import os
import tempfile
from docx2pdf import convert
from PIL import Image
import fitz
import time

import shutil

def convert_docx_to_image(word_path, image_name):

    # 创建一个临时副本
    temp_word_path = tempfile.mktemp(suffix='.docx')
    shutil.copy(word_path, temp_word_path)

    # 将 Word 文档转换为 PDF
    pdf_path = tempfile.mktemp(suffix='.pdf')
    convert(temp_word_path, pdf_path)

    # 将 PDF 转换为图像
    pdfDoc = fitz.open(pdf_path)
    images = []
    for pg in range(pdfDoc.page_count):
        page = pdfDoc[pg]
        zoom_x = 1.33333333
        zoom_y = 1.33333333
        mat = fitz.Matrix(zoom_x, zoom_y)
        pix = page.get_pixmap(matrix=mat, alpha=False)
        mode = "RGBA" if pix.alpha else "RGB"
        img = Image.frombytes(mode, [pix.width, pix.height], pix.samples)
        images.append(img)

    # 关闭 PyMuPDF 对 PDF 文件的引用
    pdfDoc.close()

    # 清理生成的 PDF 文件和临时 Word 副本
    os.remove(pdf_path)
    os.remove(temp_word_path)

    # 合并所有的图像
    widths, heights = zip(*(i.size for i in images))
    total_width = max(widths)
    total_height = sum(heights)

    new_img = Image.new('RGB', (total_width, total_height))

    y_offset = 0
    for img in images:
        new_img.paste(img, (0, y_offset))
        y_offset += img.height

    # 保存图像
    new_img.save(image_name)





import os
from PIL import Image
import fitz

def convert_pdf_to_image(pdf_path, image_name):

    # 打开 PDF 文件
    pdfDoc = fitz.open(pdf_path)

    # 将 PDF 转换为图像
    images = []
    for pg in range(pdfDoc.page_count):
        page = pdfDoc[pg]
        zoom_x = 1.33333333
        zoom_y = 1.33333333
        mat = fitz.Matrix(zoom_x, zoom_y)
        pix = page.get_pixmap(matrix=mat, alpha=False)
        mode = "RGBA" if pix.alpha else "RGB"
        img = Image.frombytes(mode, [pix.width, pix.height], pix.samples)
        images.append(img)

    # 关闭 PyMuPDF 对 PDF 文件的引用
    pdfDoc.close()

    # 合并所有的图像
    widths, heights = zip(*(i.size for i in images))
    total_width = max(widths)
    total_height = sum(heights)

    new_img = Image.new('RGB', (total_width, total_height))

    y_offset = 0
    for img in images:
        new_img.paste(img, (0, y_offset))
        y_offset += img.height

    # 保存图像
    new_img.save(image_name)






import os
import tempfile
import win32com.client as win32
from docx import Document
from docx2pdf import convert
from PIL import Image
import fitz

def convert_doc_to_image(input_doc_path, output_image_path):
    # 初始化 Word 对象
    word = win32.gencache.EnsureDispatch('Word.Application')

    # 隐藏 Word 程序窗口
    word.Visible = False

    # 打开 .doc 文件
    doc = word.Documents.Open(input_doc_path)

    # 将 .doc 文件另存为 .docx 文件
    docx_path = os.path.splitext(input_doc_path)[0] + ".docx"
    doc.SaveAs(docx_path, FileFormat=16)  # 16 表示 .docx 文件格式

    # 关闭原始 .doc 文件
    doc.Close()

    # 将 .docx 文件转换为 PDF
    pdf_path = tempfile.mktemp(suffix='.pdf')
    convert(docx_path, pdf_path)

    # 将 PDF 转换为图像
    pdfDoc = fitz.open(pdf_path)
    images = []
    for pg in range(pdfDoc.page_count):
        page = pdfDoc[pg]
        zoom_x = 1.33333333
        zoom_y = 1.33333333
        mat = fitz.Matrix(zoom_x, zoom_y)
        pix = page.get_pixmap(matrix=mat, alpha=False)
        mode = "RGBA" if pix.alpha else "RGB"
        img = Image.frombytes(mode, [pix.width, pix.height], pix.samples)
        images.append(img)

    # 关闭 PyMuPDF 对 PDF 文件的引用
    pdfDoc.close()

    # 清理生成的 PDF 文件
    os.remove(pdf_path)

    # 合并所有的图像
    widths, heights = zip(*(i.size for i in images))
    total_width = max(widths)
    total_height = sum(heights)

    new_img = Image.new('RGB', (total_width, total_height))

    y_offset = 0
    for img in images:
        new_img.paste(img, (0, y_offset))
        y_offset += img.height

    # 保存图像
    new_img.save(output_image_path)








# import sys
# import os
# import shutil

# def main():
#     sys.stdout.flush("进入file_relate文件")
#     filepath = sys.argv[1].strip()
#     sys.stdout.write(filepath + "\n")
#     sys.stdout.flush()

#     r_id = sys.argv[2].strip()
#     sys.stdout.write(r_id + "\n")
#     sys.stdout.flush()

#     extension = sys.argv[3].replace(' ', '')
#     sys.stdout.write(extension + "\n")
#     sys.stdout.flush()

#     n = sys.argv[4].strip()
#     sys.stdout.write(n + "\n")
#     sys.stdout.flush()

#     key = sys.argv[5].strip()
#     sys.stdout.write(key + "\n")
#     sys.stdout.flush()

#     # filepath = "E:\\softbei\\code\\end\\word"
#     # r_id = "95"
#     # extension = ".docx"
#     # n = "95"
#     # key = " "

#     # 使用 os.path.dirname 和 os.path.join 构建新的路径
    
#     base_dir = os.path.dirname(filepath)
#     sys.stdout.write(base_dir + "\n")

#     #sys.stdout.write(os.path.join(filepath, f'{r_id}{extension}') + "\n")
#     sys.stdout.write("这是当前文件路径"+os.path.join(filepath, f'{r_id}{extension}') + "\n")
#     sys.stdout.flush()
    

#     if extension == ".docx":
#         docx_to_txt(os.path.join(filepath, f'{r_id}{extension}'), os.path.join(base_dir, "txt", f'{r_id}.txt'))
#         sys.stdout.write( "docx路径"+os.path.join(filepath, f'{r_id}{extension}')+ "\n")
#         sys.stdout.write( "txt路径"+os.path.join(base_dir, "txt", f'{r_id}.txt')+ "\n")
#         convert_docx_to_image(os.path.join(filepath, f'{r_id}{extension}'), os.path.join(base_dir, "image", f'{r_id}.jpg'))
#         sys.stdout.write("成功转为txt和图片" + "\n")
#         sys.stdout.flush()
#     elif extension == ".doc":
#         doc_to_txt(os.path.join(filepath, f'{r_id}{extension}'), os.path.join(base_dir, "txt", f'{r_id}.txt'))
#         convert_doc_to_image(os.path.join(filepath, f'{r_id}{extension}'), os.path.join(base_dir, "image", f'{r_id}.jpg'))
#     elif extension == ".pdf":
#         pdf_to_txt(os.path.join(filepath, f'{r_id}{extension}'), os.path.join(base_dir, "txt", f'{r_id}.txt'))
#         convert_pdf_to_image(os.path.join(filepath, f'{r_id}{extension}'), os.path.join(base_dir, "image", f'{r_id}.jpg'))
#     elif extension in [".img", ".jpg", ".png"]:
#         print("正在从图片转化为txt")
#         print(os.path.join(filepath, f'{r_id}{extension}'))
#         print(os.path.join(base_dir, "txt", f'{r_id}.txt'))
#         img_to_txt(os.path.join(filepath, f'{r_id}{extension}'), os.path.join(base_dir, "txt", f'{r_id}.txt'))
#         shutil.copy2(os.path.join(filepath, f'{r_id}{extension}'), os.path.join(base_dir, "image", f'{r_id}.jpg'))
#     else:
#         print("文件格式不支持")
    
#     sys.stdout.write("成功转为txt和图片" + "\n")
#     sys.stdout.flush()

# if __name__ == "__main__":
#     sys.stdout = codecs.getwriter("utf-8")(sys.stdout.detach())

#     # 试用try except捕获异常
#     try:
#         main()
#     except Exception as e:
#         sys.stdout.write("出现异常" + "\n")
#         sys.stdout.write(str(e) + "\n")
#         sys.stdout.flush()
#         sys.exit(1)
    


import os
import sys
import time
import shutil
import codecs

def main():
    sys.stdout.write("进入file_relate文件\n")
    sys.stdout.flush()

    filepath = sys.argv[1].strip()
    sys.stdout.write(filepath + "\n")
    sys.stdout.flush()

    r_id = sys.argv[2].strip()
    sys.stdout.write(r_id + "\n")
    sys.stdout.flush()

    extension = sys.argv[3].replace(' ', '')
    sys.stdout.write(extension + "\n")
    sys.stdout.flush()

    n = sys.argv[4].strip()
    sys.stdout.write(n + "\n")
    sys.stdout.flush()

    key = sys.argv[5].strip()
    sys.stdout.write(key + "\n")
    sys.stdout.flush()

    base_dir = os.path.dirname(filepath)
    sys.stdout.write(base_dir + "\n")
    sys.stdout.flush()

    sys.stdout.write("这是当前文件路径: " + os.path.join(filepath, f'{r_id}{extension}') + "\n")
    sys.stdout.flush()

    txt_output_dir = os.path.join(base_dir, "txt")
    image_output_dir = os.path.join(base_dir, "image")

    if not os.path.exists(txt_output_dir):
        os.makedirs(txt_output_dir)

    if not os.path.exists(image_output_dir):
        os.makedirs(image_output_dir)

    txt_output_path = os.path.join(txt_output_dir, f'{r_id}.txt')

    if extension == ".docx":
        docx_to_txt(os.path.join(filepath, f'{r_id}{extension}'), txt_output_path)
        sys.stdout.write("docx路径: " + os.path.join(filepath, f'{r_id}{extension}') + "\n")
        sys.stdout.write("txt路径: " + txt_output_path + "\n")
        sys.stdout.flush()
        convert_docx_to_image(os.path.join(filepath, f'{r_id}{extension}'), os.path.join(image_output_dir, f'{r_id}.jpg'))
        sys.stdout.write("成功转为txt和图片\n")
        sys.stdout.flush()
    elif extension == ".doc":
        doc_to_txt(os.path.join(filepath, f'{r_id}{extension}'), txt_output_path)
        convert_doc_to_image(os.path.join(filepath, f'{r_id}{extension}'), os.path.join(image_output_dir, f'{r_id}.jpg'))
    elif extension == ".pdf":
        pdf_to_txt(os.path.join(filepath, f'{r_id}{extension}'), txt_output_path)
        convert_pdf_to_image(os.path.join(filepath, f'{r_id}{extension}'), os.path.join(image_output_dir, f'{r_id}.jpg'))
    elif extension in [".img", ".jpg", ".png"]:
        sys.stdout.write("正在从图片转化为txt\n")
        sys.stdout.write(os.path.join(filepath, f'{r_id}{extension}') + "\n")
        sys.stdout.write(txt_output_path + "\n")
        sys.stdout.flush()
        img_to_txt(os.path.join(filepath, f'{r_id}{extension}'), txt_output_path)
        shutil.copy2(os.path.join(filepath, f'{r_id}{extension}'), os.path.join(image_output_dir, f'{r_id}.jpg'))
    else:
        sys.stdout.write("文件格式不支持\n")
        sys.stdout.flush()

    sys.stdout.write("成功转为txt和图片\n")
    sys.stdout.flush()

if __name__ == "__main__":
    sys.stdout = codecs.getwriter("utf-8")(sys.stdout.detach())

    try:
        main()
    except Exception as e:
        sys.stdout.write("出现异常\n")
        sys.stdout.write(str(e) + "\n")
        sys.stdout.flush()
        sys.exit(1)
