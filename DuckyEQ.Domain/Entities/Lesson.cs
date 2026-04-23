using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Domain.Entities;

public class Lesson
{
    public Guid Id { get; private set; }
    public PillarId PillarId { get; private set; }
    public int Level { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public LessonFormat LessonFormat { get; private set; }

    private Lesson() { }
}