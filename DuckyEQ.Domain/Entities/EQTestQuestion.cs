using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Domain.Entities;

public class EQTestQuestion
{
    public Guid Id { get; private set; }
    public string QuestionText { get; private set; } = null!;
    public string OptionA { get; private set; } = null!;
    public string OptionB { get; private set; } = null!;
    public string OptionC { get; private set; } = null!;
    public string OptionD { get; private set; } = null!;
    public EQTestOption CorrectOption { get; private set; }
    public string Explanation { get; private set; } = null!;
    public Pillar? PillarId { get; private set; }           // null = general EQ, not pillar-specific

    private EQTestQuestion() { }

    public static EQTestQuestion Create(
        string questionText,
        string optionA,
        string optionB,
        string optionC,
        string optionD,
        EQTestOption correctOption,
        string explanation,
        Pillar? pillarId = null)
    {
        return new EQTestQuestion
        {
            Id = Guid.NewGuid(),
            QuestionText = questionText,
            OptionA = optionA,
            OptionB = optionB,
            OptionC = optionC,
            OptionD = optionD,
            CorrectOption = correctOption,
            Explanation = explanation,
            PillarId = pillarId
        };
    }
}
