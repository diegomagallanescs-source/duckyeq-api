namespace DuckyEQ.Domain.Entities;

public class LessonReflectQuestion
{
    public Guid Id { get; private set; }
    public Guid LessonId { get; private set; }
    public string QuestionText { get; private set; } = null!;
    public int DisplayOrder { get; private set; }

    private LessonReflectQuestion() { }

    public static LessonReflectQuestion Create(Guid lessonId, string questionText, int displayOrder)
    {
        return new LessonReflectQuestion
        {
            Id = Guid.NewGuid(),
            LessonId = lessonId,
            QuestionText = questionText,
            DisplayOrder = displayOrder
        };
    }
}
