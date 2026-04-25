using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Domain.Entities;

public class Lesson
{
    public Guid Id { get; private set; }
    public Pillar PillarId { get; private set; }
    public int Level { get; private set; }
    public string Title { get; private set; } = null!;
    public string CoreMessage { get; private set; } = null!;
    public string Objective { get; private set; } = null!;
    public string JokeSetup { get; private set; } = null!;
    public string JokePunchline { get; private set; } = null!;
    public string JokeSetupExpr { get; private set; } = null!;
    public string JokePunchlineExpr { get; private set; } = null!;
    public string DefineConcept { get; private set; } = null!;
    public string DefineFlashcardsJson { get; private set; } = null!;
    public LessonEngageFormat EngageGameType { get; private set; }
    public string EngageConfigJson { get; private set; } = null!;
    public int RewardTier { get; private set; }
    public string DuckArcJson { get; private set; } = null!;
    public string ShareCardConfigJson { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Lesson() { }

    public static Lesson Create(
        Pillar pillarId,
        int level,
        string title,
        string coreMessage,
        string objective,
        string jokeSetup,
        string jokePunchline,
        string jokeSetupExpr,
        string jokePunchlineExpr,
        string defineConcept,
        string defineFlashcardsJson,
        LessonEngageFormat engageGameType,
        string engageConfigJson,
        int rewardTier,
        string duckArcJson,
        string shareCardConfigJson)
    {
        var now = DateTime.UtcNow;
        return new Lesson
        {
            Id = Guid.NewGuid(),
            PillarId = pillarId,
            Level = level,
            Title = title,
            CoreMessage = coreMessage,
            Objective = objective,
            JokeSetup = jokeSetup,
            JokePunchline = jokePunchline,
            JokeSetupExpr = jokeSetupExpr,
            JokePunchlineExpr = jokePunchlineExpr,
            DefineConcept = defineConcept,
            DefineFlashcardsJson = defineFlashcardsJson,
            EngageGameType = engageGameType,
            EngageConfigJson = engageConfigJson,
            RewardTier = rewardTier,
            DuckArcJson = duckArcJson,
            ShareCardConfigJson = shareCardConfigJson,
            CreatedAt = now,
            UpdatedAt = now
        };
    }
}
