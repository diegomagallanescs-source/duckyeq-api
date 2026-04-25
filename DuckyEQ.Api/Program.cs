using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Infrastructure.Data;
using DuckyEQ.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

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

            // ── Services (added Day 7+) ───────────────────────────────────
            // builder.Services.AddSingleton<IUsernameGenerator, UsernameGenerator>();
            // builder.Services.AddScoped<IAuthService, AuthService>();
            // builder.Services.AddScoped<IScoringService, ScoringService>();
            // builder.Services.AddScoped<IEQTestScoringService, EQTestScoringService>();
            // builder.Services.AddScoped<ISessionService, SessionService>();
            // builder.Services.AddScoped<ICooldownService, CooldownService>();
            // builder.Services.AddScoped<ICoinService, CoinService>();
            // builder.Services.AddScoped<IShopService, ShopService>();
            // builder.Services.AddScoped<IGratitudeService, GratitudeService>();
            // builder.Services.AddScoped<ILessonService, LessonService>();
            // builder.Services.AddScoped<IEQTestService, EQTestService>();
            // builder.Services.AddScoped<ICheckInService, CheckInService>();
            // builder.Services.AddScoped<IFriendshipService, FriendshipService>();
            // builder.Services.AddScoped<IQuackService, QuackService>();
            // builder.Services.AddScoped<IAuthService, AuthService>();

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
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
