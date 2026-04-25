namespace DuckyEQ.Domain.Entities;

public class LessonConnectQuestion
{
    public Guid Id { get; private set; }
    public Guid LessonId { get; private set; }
    public string QuestionText { get; private set; } = null!;
    public int DisplayOrder { get; private set; }

    private LessonConnectQuestion() { }

    public static LessonConnectQuestion Create(Guid lessonId, string questionText, int displayOrder)
    {
        return new LessonConnectQuestion
        {
            Id = Guid.NewGuid(),
            LessonId = lessonId,
            QuestionText = questionText,
            DisplayOrder = displayOrder
        };
    }
}
