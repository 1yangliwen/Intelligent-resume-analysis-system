using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using resume.Models;
using resume.open;
using resume.Others;
using resume.ResultModels;
using resume.Service;
using resume.Services;
using resume.WebSentModel;

namespace resume.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ApplicantService _applicantService;
        private readonly ResumeService _resumeService;
        private readonly CompanyService _companyService;
        public TestController(ApplicantService applicantService, ResumeService resumeService, CompanyService companyService)
        {
            _applicantService = applicantService;
            _resumeService = resumeService;
            _companyService = companyService;
        }
        [HttpPost]
        [Route("AnalysisTest")]
        public FirstAddResumeModelClass AnalysisTest(string dataFilePath, string filePath_1, int jobId)
        {
            int UserId = 1;
            //int jobId = 6;
            //string filePath = @"D:\PythonCode\openai\txt";
            //string filePath_1 = @"D:\Test";
            //string dataFilePath = @"D:\visualStudio workspace\SCB_ClassAssignment_backend\enample\110.json"; // 数据文件路径
            Dictionary<string, object> resumeInfo = null;
            // 从文件中读取数据
            string resumeData = System.IO.File.ReadAllText(dataFilePath);
            resumeInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(resumeData);
            // 传入参数：filepath 返回：FirstAddResumeModelClass 并实现将该路径存入数据库
            var storedApplicantId = _applicantService.CreateApplicantFromDictionary(resumeInfo).Result;

            int resumeID = _resumeService.AddResumePath(dataFilePath, filePath_1, storedApplicantId, UserId, jobId);
            var detailedResume = _resumeService.GetResumeById(resumeID);
            var result = new FirstAddResumeModelClass()
            {
                Code = 20000,
                DetailedResume = detailedResume
            };
            return result;
        }

        [HttpPost]
        [Route("Analysis")]
        public void RunAnalysisTests(int startFileNumber, int endFileNumber, int jobId)
        {
            for (int i = startFileNumber; i <= endFileNumber; i++)
            {
                try
                {
                    string dataFilePath = $@"D:\visualStudio workspace\SCB_ClassAssignment_backend\727\{i}.json";
                    string filePath_1 = $@"D:\visualStudio workspace\SCB_ClassAssignment_backend\image\{i}.jpg";
                    AnalysisTest(dataFilePath, filePath_1, jobId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred when processing file number {i}: {ex.Message}");
                    // If you want to stop the loop when an error occurs, you can uncomment the following line
                    // break;
                }
            }
        }

        [HttpPost]
        [Route("Initialize")]
        public IActionResult SystemInitialize()
        {
            RegisterSentModel registerSentModel = new RegisterSentModel()
            {
                Name = "admin",
                Account = "admin",
                Email = "test",
                Password = "123456"
            };
            var result = _companyService.CreateNewAccount(registerSentModel);
            return Ok(result);
        }
    }
}
