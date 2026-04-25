namespace DuckyEQ.Services.Exceptions;

public abstract class DuckyEQException : Exception
{
    protected DuckyEQException(string message) : base(message) { }
}

public class NotFoundException : DuckyEQException
{
    public NotFoundException(string message = "Resource not found.") : base(message) { }
}

public class InvalidCredentialsException : DuckyEQException
{
    public InvalidCredentialsException() : base("Invalid email or password.") { }
}

public class EmailAlreadyExistsException : DuckyEQException
{
    public EmailAlreadyExistsException() : base("An account with this email already exists.") { }
}

public class AlreadyCheckedInException : DuckyEQException
{
    public AlreadyCheckedInException() : base("You have already checked in today.") { }
}

public class QuackLimitExceededException : DuckyEQException
{
    public QuackLimitExceededException() : base("You have already sent a Quack to this friend today.") { }
}

public class NotFriendsException : DuckyEQException
{
    public NotFriendsException() : base("Sender and recipient must be accepted friends.") { }
}

public class InsufficientCoinsException : DuckyEQException
{
    public InsufficientCoinsException() : base("Insufficient QuackCoins to complete this purchase.") { }
}

public class AlreadyOwnedException : DuckyEQException
{
    public AlreadyOwnedException() : base("You already own this item.") { }
}

public class FriendRequestConflictException : DuckyEQException
{
    public FriendRequestConflictException(string message = "A friendship or request already exists.") : base(message) { }
}

public class CooldownActiveException : DuckyEQException
{
    public CooldownActiveException() : base("This pillar is in cooldown. Please wait before starting a new lesson.") { }
}
