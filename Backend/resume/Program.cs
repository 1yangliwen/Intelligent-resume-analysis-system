using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using resume.Models;
using resume.open;
using resume.Service;
using resume.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure database context
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 30))));

// �����ķ���
builder.Services.AddTransient<CompanyService>();
builder.Services.AddTransient<ApplicantService>();
builder.Services.AddTransient<ResumeService>();
builder.Services.AddTransient<JobService>();
builder.Services.AddTransient<EmailService>();
builder.Services.AddTransient<EmailResumeProcessor>();
builder.Services.AddTransient<ConversationService>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(cfg =>
{
    cfg.AllowAnyOrigin(); //��Ӧ��������ĵ�ַ
    cfg.AllowAnyMethod(); //��Ӧ���󷽷���Method
    cfg.AllowAnyHeader(); //��Ӧ���󷽷���Headers
                          //cfg.AllowCredentials(); //��Ӧ�����withCredentials ֵ
});

app.UseAuthorization();

app.MapControllers();

app.Run();
