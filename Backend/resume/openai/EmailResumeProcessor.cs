using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using resume.ResultModels;
using resume.Service;
using resume.Services;
using Microsoft.Extensions.DependencyInjection;

namespace resume.open
{
    internal class EmailResumeProcessor
    {
        private string emailAddress;
        private string emailPassword;
        private bool isListening;
        private ImapClient imapClient;
        private string downloadPath;
        private Action<string> resumeReceivedCallback;
        private readonly IServiceScopeFactory _scopeFactory; // 添加 IServiceProvider 字段

        public EmailResumeProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            emailAddress = "";
            emailPassword = "";
            isListening = false;
            imapClient = null;
            downloadPath = Directory.GetCurrentDirectory(); // 默认下载路径为当前目录
            resumeReceivedCallback = null;
        }

        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }

        public string EmailPassword
        {
            get { return emailPassword; }
            set { emailPassword = value; }
        }

        public bool IsListening
        {
            get { return isListening; }
        }

        public string DownloadPath
        {
            get { return downloadPath; }
            set { downloadPath = value; }
        }


        public void StartListening(string emailAddress, string emailPassword)
        {
            this.emailAddress = emailAddress;
            this.emailPassword = emailPassword;

            this.isListening = true;

            Task.Run(async () =>
            {
                ConnectToImapServer();


                while (isListening && imapClient != null && imapClient.IsConnected)
                {
                    await Console.Out.WriteLineAsync("检查一次邮箱");
                    await CheckForNewEmailsAsync();
                    await Task.Delay(10000); // 每10秒检查一次新邮件
                }
                
            });
        }

        private void ConnectToImapServer()
        {
            imapClient = new ImapClient();
            try
            {
                imapClient.Connect("imap.qq.com", 993, SecureSocketOptions.SslOnConnect);
                imapClient.Authenticate(emailAddress, emailPassword);
                Console.WriteLine("Connected and authenticated to IMAP server.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not connect to IMAP server: " + ex.Message);
            }
        }

        private void DisconnectFromImapServer()
        {
            if (imapClient != null && imapClient.IsConnected)
            {
                imapClient.Disconnect(true);
            }
        }

        private async Task CheckForNewEmailsAsync()
        {
            if (!isListening || imapClient == null || !imapClient.IsConnected)
                return;

            var inbox = imapClient.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadWrite);
            var uids = await inbox.SearchAsync(SearchQuery.NotSeen);

            foreach (var uid in uids)
            {
                var message = await inbox.GetMessageAsync(uid);
                await DownloadResumeAttachmentsAsync(message, uid);
                await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
            }
        }

        private async Task DownloadResumeAttachmentsAsync(MimeMessage message, MailKit.UniqueId uid)
        {
            foreach (var attachment in message.Attachments)
            {
                if (attachment is MimePart && IsResumeAttachment(attachment))
                {
                    var part = (MimePart)attachment;
                    var fileName = part.FileName;
                    string staticFileRoot = "Resumes";
                    string fileUrlWithoutFileName = @$"{DateTime.Now.Year}\{DateTime.Now.Month}\{DateTime.Now.Day}";
                    Directory.CreateDirectory($"{staticFileRoot}/{fileUrlWithoutFileName}");

                    byte[] fileBytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        await part.Content.DecodeToAsync(memoryStream);
                        fileBytes = memoryStream.ToArray();
                    }
                    SHA256 hash = SHA256.Create();
                    byte[] hashByte = hash.ComputeHash(fileBytes);
                    string hashedFileName = BitConverter.ToString(hashByte).Replace("-", "");

                    string newFileName = hashedFileName + "." + fileName.Split('.').Last();
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), $@"{staticFileRoot}\{fileUrlWithoutFileName}", newFileName);

                    await File.WriteAllBytesAsync(filePath, fileBytes);
                    Console.WriteLine($"Resume attachment downloaded and saved: {filePath}");

                    var resumeFilePath = filePath;

                    try
                    {
                        Console.WriteLine("开始处理简历文件...");

                        Connect connect = new Connect();
                        Console.WriteLine($"分析简历：{resumeFilePath}");
                        var resumeInfo = connect.analysis(resumeFilePath);

                        Console.WriteLine("获取文件名...");
                        string fileName11 = Path.GetFileNameWithoutExtension(resumeFilePath);
                        Console.WriteLine($"文件名：{fileName}");

                        Console.WriteLine("生成图片路径...");
                        string staticFileRoot11 = "Resumes";
                        string newFileName_1 = fileName11 + ".jpg";
                        string fileUrlWithoutFileName_1 = @$"{DateTime.Now.Year}\{DateTime.Now.Month}\{"image"}";
                        string filePath_1 = Path.Combine(Directory.GetCurrentDirectory(), $@"{staticFileRoot11}\{fileUrlWithoutFileName_1}", newFileName_1);
                        Console.WriteLine($"图片路径：{filePath_1}");
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            Console.WriteLine("存储申请人信息...");
                            var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();
                            var applicantService = scope.ServiceProvider.GetRequiredService<ApplicantService>();
                            var storedApplicantId = await applicantService.CreateApplicantFromDictionary(context, resumeInfo);
                            Console.WriteLine($"申请人ID：{storedApplicantId}");
                            Console.WriteLine("存储简历路径信息...");
                            var resumeService = scope.ServiceProvider.GetRequiredService<ResumeService>();
                            int resumeID = resumeService.AddResumePath(resumeFilePath, filePath_1, storedApplicantId, 1, 1);
                            Console.WriteLine($"简历ID：{resumeID}");
                            var detailedResume = resumeService.GetResumeById(resumeID);
                            Console.WriteLine($"详细简历信息：{detailedResume}");
                            var result = new FirstAddResumeModelClass()
                            {
                                Code = 20000,
                                DetailedResume = detailedResume
                            };

                            Console.WriteLine($"简历处理成功：{result}");
                        }
                                          
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"简历处理失败：{ex.Message}");
                    }
                }
            }
        }

        private bool IsResumeAttachment(MimeEntity attachment)
        {
            var fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;
            return !string.IsNullOrEmpty(fileName) && (fileName.EndsWith(".docx") || fileName.EndsWith(".pdf"));
        }
    }
}
