using System.Text;
using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Contracts.Interfaces.Services;
using DuckyEQ.Infrastructure.Data;
using DuckyEQ.Infrastructure.Repositories;
using DuckyEQ.Services.Behaviors;
using DuckyEQ.Services.Services;
using DuckyEQ.Services.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DuckyEQ
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ── Database ──────────────────────────────────────────────────
            builder.Services.AddDbContext<DuckyEQDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddMemoryCache();

            // ── JWT Authentication ────────────────────────────────────────
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                    };
                });

            // ── Repositories (all Scoped) ─────────────────────────────────
            builder.Services.AddScoped<IUserRepository, SqlUserRepository>();
            builder.Services.AddScoped<IPillarProgressRepository, SqlPillarProgressRepository>();
            builder.Services.AddScoped<IUserLessonProgressRepository, SqlUserLessonProgressRepository>();
            builder.Services.AddScoped<ILessonRepository, SqlLessonRepository>();
            builder.Services.AddScoped<IEQTestQuestionRepository, SqlEQTestQuestionRepository>();
            builder.Services.AddScoped<IUserEQTestResultRepository, SqlUserEQTestResultRepository>();
            builder.Services.AddScoped<ICoinRepository, SqlCoinRepository>();
            builder.Services.AddScoped<IShopItemRepository, SqlShopItemRepository>();
            builder.Services.AddScoped<IUserInventoryRepository, SqlUserInventoryRepository>();
            builder.Services.AddScoped<IGratitudeRepository, SqlGratitudeRepository>();
            builder.Services.AddScoped<IDailyCheckInRepository, SqlDailyCheckInRepository>();
            builder.Services.AddScoped<IFriendshipRepository, SqlFriendshipRepository>();
            builder.Services.AddScoped<IQuackRepository, SqlQuackRepository>();

            // ── Services ──────────────────────────────────────────────────
            builder.Services.AddSingleton<IUsernameGenerator, UsernameGenerator>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IScoringService, ScoringService>();
            builder.Services.AddScoped<IEQTestScoringService, EQTestScoringService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<ICooldownService, CooldownService>();
            builder.Services.AddScoped<ICoinService, CoinService>();
            builder.Services.AddScoped<IShopService, ShopService>();
            builder.Services.AddScoped<IGratitudeService, GratitudeService>();
            builder.Services.AddScoped<ILessonService, LessonService>();
            builder.Services.AddScoped<IEQTestService, EQTestService>();
            builder.Services.AddScoped<ICheckInService, CheckInService>();
            builder.Services.AddScoped<IFriendshipService, FriendshipService>();
            builder.Services.AddScoped<IQuackService, QuackService>();

            // ── Behaviors (all Scoped) ────────────────────────────────────
            builder.Services.AddScoped<AuthBehavior>();
            builder.Services.AddScoped<CheckInBehavior>();
            builder.Services.AddScoped<FriendsBehavior>();
            builder.Services.AddScoped<QuackBehavior>();
            builder.Services.AddScoped<LessonBehavior>();

            // ── API ───────────────────────────────────────────────────────
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
