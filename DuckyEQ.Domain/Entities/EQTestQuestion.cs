using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Domain.Entities;

public class EQTestQuestion
{
    public Guid Id { get; private set; }
    public PillarId PillarId { get; private set; }
    public string QuestionText { get; private set; } = null!;
    public string OptionA { get; private set; } = null!;
    public string OptionB { get; private set; } = null!;
    public string OptionC { get; private set; } = null!;
    public string OptionD { get; private set; } = null!;
    public string CorrectAnswer { get; private set; } = null!;

    private EQTestQuestion() { }
}