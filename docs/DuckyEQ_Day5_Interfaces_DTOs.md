**🦆 DuckyEQ**

**Day 5 --- All Interfaces + DTOs + Request/Response Models**

*DuckyEQ.Contracts layer · April 2026 · duckyeq.com*

  ---------------------- -------------------------------------------------
  **Layer**              **DuckyEQ.Contracts**

  **Project**            DuckyEQ.Contracts (no dependencies --- pure
                         interfaces + DTOs)

  **Repository           13 interfaces in
  Interfaces**           Contracts/Interfaces/Repositories/

  **Service Interfaces** 14 interfaces in Contracts/Interfaces/Services/

  **DTOs**               7 named DTOs + full supporting type library

  **Request/Response**   All request payloads + response models for every
                         endpoint

  **Pattern**            Interface Segregation · async Task everywhere ·
                         nullable returns where required
  ---------------------- -------------------------------------------------

> 💡 *This document is your complete Contracts layer reference. Every
> interface here was designed to match the API endpoint map exactly ---
> one method per server action. File location comments are included
> above each interface block.*

**1. Repository Interfaces**

**Location: DuckyEQ.Contracts/Interfaces/Repositories/**

All methods return Task\<T\>. Repositories deal only with domain
entities --- no business logic, no DTOs. Every nullable return type
signals \'not found\' semantics rather than throwing.

  -----------------------------------------------------------------
  **1.1 IUserRepository --- Core user lookups and mutations**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **IUserRepository**                                             |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Repositories*                     |
+-----------------------------------------------------------------+
| Task\<User?\> GetByIdAsync(Guid id);                            |
|                                                                 |
| Task\<User?\> GetByEmailAsync(string email);                    |
|                                                                 |
| Task\<User?\> GetByUsernameAsync(string username);              |
|                                                                 |
| Task\<bool\> IsUsernameTakenAsync(string username);             |
|                                                                 |
| Task\<User\> CreateAsync(User user);                            |
|                                                                 |
| Task UpdateAsync(User user);                                    |
|                                                                 |
| Task UpdateKnownAsAsync(Guid userId, string knownAs);           |
+-----------------------------------------------------------------+

> 💡 *GetByIdAsync returns User? --- null means not found. The service
> layer throws NotFoundException which the controller maps to 404.
> UpdateKnownAsAsync is a targeted update: it touches only the KnownAs
> column, avoiding a full entity write for a single-field change.*

  -----------------------------------------------------------------
  **1.2 IPillarProgressRepository --- Per-pillar XP tracking**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **IPillarProgressRepository**                                   |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Repositories*                     |
+-----------------------------------------------------------------+
| Task\<PillarProgress?\> GetByUserAndPillarAsync(Guid userId,    |
| Pillar pillar);                                                 |
|                                                                 |
| Task\<IReadOnlyList\<PillarProgress\>\> GetAllByUserAsync(Guid  |
| userId);                                                        |
|                                                                 |
| Task\<PillarProgress\> CreateAsync(PillarProgress progress);    |
|                                                                 |
| Task UpdateAsync(PillarProgress progress);                      |
|                                                                 |
| Task EnsureAllPillarsExistAsync(Guid userId);                   |
+-----------------------------------------------------------------+

> 💡 *EnsureAllPillarsExistAsync creates all 5 PillarProgress rows in a
> single transaction if they don\'t exist. Called once on the first GET
> /api/pillars/progress --- this way controllers never have to worry
> about missing rows.*

  -----------------------------------------------------------------
  **1.3 IUserLessonProgressRepository --- Per-lesson best scores**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **IUserLessonProgressRepository**                               |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Repositories*                     |
+-----------------------------------------------------------------+
| Task\<UserLessonProgress?\> GetByUserAndLessonAsync(Guid        |
| userId, Guid lessonId);                                         |
|                                                                 |
| Task\<IReadOnlyList\<UserLessonProgress\>\>                     |
| GetByUserAndPillarAsync(                                        |
|                                                                 |
| Guid userId, Pillar pillar);                                    |
|                                                                 |
| Task UpsertAsync(Guid userId, Guid lessonId, int score,         |
|                                                                 |
| int stars, bool isFirstCompletion, bool isNewBest);             |
|                                                                 |
| Task\<DateTime?\> GetLastNewLessonCompletedAtAsync(Guid userId, |
| Pillar pillar);                                                 |
+-----------------------------------------------------------------+

> 💡 *UpsertAsync handles both insert and update in one call. The
> repository checks if a row exists and either INSERTs or UPDATEs,
> updating BestScore only when score \> current best.
> GetLastNewLessonCompletedAtAsync powers the cooldown check: if the
> result is within the last 24 hours, the pillar is locked.*

  -----------------------------------------------------------------
  **1.4 ILessonRepository --- Lesson content retrieval**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **ILessonRepository**                                           |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Repositories*                     |
+-----------------------------------------------------------------+
| Task\<IReadOnlyList\<Lesson\>\> GetByPillarAsync(Pillar         |
| pillar);                                                        |
|                                                                 |
| Task\<Lesson?\> GetByIdAsync(Guid id);                          |
|                                                                 |
| Task\<Lesson?\> GetByPillarAndLevelAsync(Pillar pillar, int     |
| level);                                                         |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **1.5 IEQTestQuestionRepository --- Test question pool**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **IEQTestQuestionRepository**                                   |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Repositories*                     |
+-----------------------------------------------------------------+
| Task\<IReadOnlyList\<EQTestQuestion\>\> GetRandomSetAsync(int   |
| count);                                                         |
|                                                                 |
| Task\<IReadOnlyList\<EQTestQuestion\>\> GetAllAsync();          |
+-----------------------------------------------------------------+

> 💡 *GetRandomSetAsync uses ORDER BY NEWID() FETCH NEXT \@count ROWS
> ONLY in EF Core raw SQL, or LINQ OrderBy(q =\> Guid.NewGuid()). The
> count will be 15 for a standard EQ test session. The CorrectOption is
> included in the repository return but stripped from the DTO before the
> API response.*

  -----------------------------------------------------------------
  **1.6 IUserEQTestResultRepository --- EQ test history**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **IUserEQTestResultRepository**                                 |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Repositories*                     |
+-----------------------------------------------------------------+
| Task\<UserEQTestResult\> CreateAsync(UserEQTestResult result);  |
|                                                                 |
| Task\<UserEQTestResult?\> GetBestByUserAsync(Guid userId);      |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **1.7 ICoinRepository --- Coin balance management**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **ICoinRepository**                                             |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Repositories*                     |
+-----------------------------------------------------------------+
| Task\<QuackCoins?\> GetByUserAsync(Guid userId);                |
|                                                                 |
| Task\<QuackCoins\> EnsureExistsAsync(Guid userId);              |
|                                                                 |
| Task AwardAsync(Guid userId, int amount);                       |
|                                                                 |
| Task\<bool\> DeductAsync(Guid userId, int amount);              |
+-----------------------------------------------------------------+

> 💡 *DeductAsync returns false (rather than throwing) when the balance
> is insufficient --- the service layer turns this into a 402 Payment
> Required response. EnsureExistsAsync creates a QuackCoins row with
> Balance=0 on first call, so the service never has to null-check before
> awarding.*

  -----------------------------------------------------------------
  **1.8 IShopItemRepository --- Shop catalogue queries**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **IShopItemRepository**                                         |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Repositories*                     |
+-----------------------------------------------------------------+
| Task\<IReadOnlyList\<ShopItem\>\> GetActiveItemsAsync();        |
|                                                                 |
| Task\<IReadOnlyList\<ShopItem\>\>                               |
| GetWeeklyItemsForCurrentWeekAsync();                            |
|                                                                 |
| Task\<IReadOnlyList\<ShopItem\>\> GetByTypeAsync(bool           |
| isWeekly);                                                      |
|                                                                 |
| Task\<ShopItem?\> GetByIdAsync(Guid id);                        |
+-----------------------------------------------------------------+

> 💡 *GetWeeklyItemsForCurrentWeekAsync queries WHERE IsWeeklyItem = 1
> AND WeeklyAvailableFrom \<= GETUTCDATE() AND WeeklyAvailableTo \>=
> GETUTCDATE(). No background job required --- the date range handles
> rotation automatically. GetByTypeAsync supports the ?type=permanent
> and ?type=weekly query params.*

  -----------------------------------------------------------------
  **1.9 IUserInventoryRepository --- Owned items and equip state**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **IUserInventoryRepository**                                    |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Repositories*                     |
+-----------------------------------------------------------------+
| Task\<IReadOnlyList\<UserInventory\>\> GetByUserAsync(Guid      |
| userId);                                                        |
|                                                                 |
| Task\<UserInventory?\> GetByUserAndItemAsync(Guid userId, Guid  |
| shopItemId);                                                    |
|                                                                 |
| Task\<bool\> UserOwnsItemAsync(Guid userId, Guid shopItemId);   |
|                                                                 |
| Task\<UserInventory\> CreateAsync(UserInventory item);          |
|                                                                 |
| Task UnequipAllInCategoryAsync(Guid userId, ShopCategory        |
| category);                                                      |
|                                                                 |
| Task EquipAsync(Guid userId, Guid shopItemId);                  |
|                                                                 |
| Task\<EquippedItems\> GetEquippedItemsAsync(Guid userId);       |
+-----------------------------------------------------------------+

> 💡 *The equip sequence is always: (1) UnequipAllInCategoryAsync for
> the item\'s category, (2) EquipAsync for the target item. These run in
> a DB transaction in the service layer. GetEquippedItemsAsync returns
> the full EquippedItems record used in UserProfileDto and every duck
> rendering surface.*

  -----------------------------------------------------------------
  **1.10 IGratitudeRepository --- Gratitude Garden entries**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **IGratitudeRepository**                                        |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Repositories*                     |
+-----------------------------------------------------------------+
| Task\<GratitudeEntry\> CreateAsync(GratitudeEntry entry);       |
|                                                                 |
| Task\<IReadOnlyList\<GratitudeEntry\>\> GetByUserPagedAsync(    |
|                                                                 |
| Guid userId, int page, int pageSize);                           |
|                                                                 |
| Task\<IReadOnlyList\<GratitudeEntry\>\>                         |
| GetTodayByUserAsync(Guid userId);                               |
|                                                                 |
| Task\<GratitudeEntry?\> GetRandomByUserAsync(Guid userId);      |
|                                                                 |
| Task\<int\> GetCurrentStreakAsync(Guid userId);                 |
|                                                                 |
| Task\<int\> GetLongestStreakAsync(Guid userId);                 |
+-----------------------------------------------------------------+

> 💡 *GetRandomByUserAsync uses ORDER BY NEWID() FETCH NEXT 1 ROWS ONLY.
> The Pick Me Up feature calls this; if fewer than 3 entries exist, the
> service returns a minimum-entry guard message instead. Streak
> calculation walks backwards from today through CreatedAt dates.*

  -----------------------------------------------------------------
  **1.11 IDailyCheckInRepository --- Social daily check-in**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **IDailyCheckInRepository**                                     |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Repositories*                     |
+-----------------------------------------------------------------+
| // Returns null if user has not checked in today --- signals    |
| 204 No Content at API layer                                     |
|                                                                 |
| Task\<DailyCheckIn?\> GetTodayAsync(Guid userId);               |
|                                                                 |
| Task\<DailyCheckIn\> CreateAsync(DailyCheckIn checkIn);         |
+-----------------------------------------------------------------+

> 💡 *GetTodayAsync queries WHERE UserId = \@userId AND CheckInDate =
> CAST(GETUTCDATE() AS DATE). The nullable return is the contract: null
> means not-yet-checked-in, which the controller turns into 204. The
> client shows the check-in modal when it receives 204 from GET
> /api/checkin/today.*

  -----------------------------------------------------------------
  **1.12 IFriendshipRepository --- Friend graph and pending
  requests**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **IFriendshipRepository**                                       |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Repositories*                     |
+-----------------------------------------------------------------+
| Task\<Friendship\> CreateAsync(Friendship friendship);          |
|                                                                 |
| // JOIN: friendships + users + daily_checkins LEFT JOIN for     |
| today\'s check-in                                               |
|                                                                 |
| Task\<IReadOnlyList\<FriendWithCheckInDto\>\>                   |
| GetFriendsWithCheckInAsync(                                     |
|                                                                 |
| Guid userId);                                                   |
|                                                                 |
| // Incoming requests where AddresseeId = userId AND Status =    |
| Pending                                                         |
|                                                                 |
| Task\<IReadOnlyList\<Friendship\>\>                             |
| GetPendingIncomingAsync(Guid userId);                           |
|                                                                 |
| Task\<Friendship?\> GetByIdAsync(Guid id);                      |
|                                                                 |
| Task UpdateStatusAsync(Guid friendshipId, FriendshipStatus      |
| status);                                                        |
|                                                                 |
| // Used to check if a request already exists before sending     |
| another                                                         |
|                                                                 |
| Task\<Friendship?\> GetBetweenUsersAsync(Guid userId1, Guid     |
| userId2);                                                       |
|                                                                 |
| // Rich JOIN for the Friend Detail screen                       |
|                                                                 |
| Task\<FriendDetailDto?\> GetFriendDetailAsync(                  |
|                                                                 |
| Guid currentUserId, Guid friendUserId);                         |
+-----------------------------------------------------------------+

> ⚠️ *GetFriendsWithCheckInAsync uses a LEFT JOIN on daily_checkins
> WHERE CheckInDate = TODAY. LEFT JOIN is required --- if a friend
> hasn\'t checked in, their check-in columns come back as NULL and the
> DTO sets HasCheckedInToday = false. Using INNER JOIN would silently
> exclude friends who haven\'t checked in yet.*
>
> 💡 *GetBetweenUsersAsync queries WHERE (RequesterId = \@a AND
> AddresseeId = \@b) OR (RequesterId = \@b AND AddresseeId = \@a) ---
> friendships are bidirectional in the DB but stored with a single
> directional row. This query handles both directions.*

  -----------------------------------------------------------------
  **1.13 IQuackRepository --- Quack reactions**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **IQuackRepository**                                            |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Repositories*                     |
+-----------------------------------------------------------------+
| Task\<Quack\> CreateAsync(Quack quack);                         |
|                                                                 |
| // Returns quacks WHERE RecipientId = userId AND SeenAt IS NULL |
|                                                                 |
| // Filtered to last 48 hours to keep banner data fresh          |
|                                                                 |
| Task\<IReadOnlyList\<Quack\>\> GetUnseenByRecipientAsync(Guid   |
| recipientId);                                                   |
|                                                                 |
| // Sets SeenAt = UTC_NOW for the given quack                    |
|                                                                 |
| Task MarkSeenAsync(Guid quackId);                               |
|                                                                 |
| // Checks if sender already sent a quack to recipient today     |
| (calendar day UTC)                                              |
|                                                                 |
| // Used by service layer to enforce one-per-day rate limit      |
|                                                                 |
| Task\<bool\> HasSentTodayAsync(Guid senderId, Guid              |
| recipientId);                                                   |
+-----------------------------------------------------------------+

> 💡 *HasSentTodayAsync query: WHERE SenderId = \@s AND RecipientId =
> \@r AND CAST(SentAt AS DATE) = CAST(GETUTCDATE() AS DATE). The service
> throws QuackLimitExceededException when this returns true, which the
> controller maps to 429 Too Many Requests with a Retry-After header set
> to midnight UTC.*

**2. Service Interfaces**

**Location: DuckyEQ.Contracts/Interfaces/Services/**

Service interfaces define business logic contracts. Services coordinate
repositories, enforce rules, and return DTOs --- never domain entities.
The Behavior layer calls services; services call repositories.

  -----------------------------------------------------------------
  **2.1 IAuthService --- Registration, login, and Known As update**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **IAuthService**                                                |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Services*                         |
+-----------------------------------------------------------------+
| Task\<AuthResult\> RegisterAsync(RegisterRequest request);      |
|                                                                 |
| Task\<AuthResult\> LoginAsync(LoginRequest request);            |
|                                                                 |
| Task\<bool\> VerifyEmailAsync(string token);                    |
|                                                                 |
| // Re-issues JWT with updated KnownAs claim after DB save       |
|                                                                 |
| Task\<AuthResult\> UpdateKnownAsAsync(Guid userId, string       |
| knownAs);                                                       |
+-----------------------------------------------------------------+

> 💡 *UpdateKnownAsAsync saves the KnownAs change to the DB AND returns
> a new JWT in one atomic call. The controller replaces the stored JWT
> on the client so all screens reflect the new display name
> immediately.*

  -----------------------------------------------------------------
  **2.2 IUsernameGenerator --- PascalCase username factory**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **IUsernameGenerator**                                          |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Services*                         |
+-----------------------------------------------------------------+
| // Generates a unique PascalCase username, e.g. SunnyDuck4832   |
|                                                                 |
| // Format: AdjectiveNounXXXX where XXXX is a 4-digit random     |
| number                                                          |
|                                                                 |
| // Retries until a non-taken username is found (max 10          |
| attempts)                                                       |
|                                                                 |
| Task\<string\> GenerateUniqueAsync();                           |
+-----------------------------------------------------------------+

> 💡 *Registered as AddSingleton\<IUsernameGenerator,
> UsernameGenerator\> in Program.cs. The adjective and noun word lists
> are embedded resources. The generator calls
> IUserRepository.IsUsernameTakenAsync in a retry loop --- collision
> probability at 10,000 users is negligible.*

  -----------------------------------------------------------------
  **2.3 IScoringService --- Lesson score and star calculation**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **IScoringService**                                             |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Services*                         |
+-----------------------------------------------------------------+
| // score = round(correctAnswers / totalQuestions \* 300),       |
| clamped 0--300                                                  |
|                                                                 |
| int CalculateScore(int correctAnswers, int totalQuestions);     |
|                                                                 |
| // 1 star: score \<= 100 \| 2 stars: score \<= 200 \| 3 stars:  |
| score \> 200                                                    |
|                                                                 |
| int GetStars(int score);                                        |
|                                                                 |
| // Coin awards: 1 star = 20, 2 stars = 40, 3 stars = 60         |
|                                                                 |
| int BaseCoinsForStars(int stars);                               |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **2.4 IEQTestScoringService --- EQ Test score and star
  calculation**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **IEQTestScoringService**                                       |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Services*                         |
+-----------------------------------------------------------------+
| // score = correctAnswers \* 10 (15 questions max = 150 max     |
| score)                                                          |
|                                                                 |
| int CalculateScore(int correctAnswers);                         |
|                                                                 |
| // 1 star: score \< 60 \| 2 stars: score \< 110 \| 3 stars:     |
| score \>= 110                                                   |
|                                                                 |
| int GetStars(int score);                                        |
+-----------------------------------------------------------------+

> 💡 *EQ Test scoring is separate from lesson scoring --- different
> formula, different star thresholds. Keeping them in separate
> interfaces prevents a single IScoringService from growing a tangle of
> overloads.*

  -----------------------------------------------------------------
  **2.5 ISessionService --- In-memory lesson session tokens**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **ISessionService**                                             |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Services*                         |
+-----------------------------------------------------------------+
| // Creates a GUID token stored in IMemoryCache with 30-minute   |
| TTL                                                             |
|                                                                 |
| // Throws if userId already has an active session for this      |
| lesson                                                          |
|                                                                 |
| string CreateSession(Guid userId, Guid lessonId);               |
|                                                                 |
| // Returns null if token doesn\'t exist or has expired          |
|                                                                 |
| LessonSession? GetSession(string token);                        |
|                                                                 |
| // Called after CompleteLessonAsync to clean up the token       |
|                                                                 |
| void RemoveSession(string token);                               |
+-----------------------------------------------------------------+

> 💡 *LessonSession is a simple value type: record LessonSession(Guid
> UserId, Guid LessonId, DateTime StartedAt). It lives only in
> IMemoryCache --- never in the database. ISessionService is registered
> as AddSingleton because IMemoryCache is singleton-scoped.*

  -----------------------------------------------------------------
  **2.6 ICooldownService --- Pillar lesson cooldown enforcement**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **ICooldownService**                                            |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Services*                         |
+-----------------------------------------------------------------+
| Task\<CooldownStatus\> GetPillarStatusAsync(Guid userId, Pillar |
| pillar);                                                        |
|                                                                 |
| Task\<bool\> CanStartNewLessonAsync(Guid userId, Pillar         |
| pillar);                                                        |
+-----------------------------------------------------------------+

> 💡 *Cooldown only blocks new-lesson starts. Retrying an
> already-completed lesson is never blocked. The cooldown window is 24
> hours from the timestamp of the most recent first completion in that
> pillar.*

  -----------------------------------------------------------------
  **2.7 ICheckInService --- Daily emotion check-in logic**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **ICheckInService**                                             |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Services*                         |
+-----------------------------------------------------------------+
| // Returns null if user has not checked in today (controller →  |
| 204)                                                            |
|                                                                 |
| Task\<CheckInDto?\> GetTodayAsync(Guid userId);                 |
|                                                                 |
| // Creates check-in. Throws AlreadyCheckedInException if        |
| today\'s row exists.                                            |
|                                                                 |
| Task\<CheckInDto\> CheckInAsync(Guid userId, CheckInRequest     |
| request);                                                       |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **2.8 IFriendshipService --- Friend request and friend list
  logic**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **IFriendshipService**                                          |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Services*                         |
+-----------------------------------------------------------------+
| // Looks up target by username. Throws if already friends or    |
| request exists.                                                 |
|                                                                 |
| Task\<FriendshipDto\> SendRequestAsync(Guid requesterId, string |
| targetUsername);                                                |
|                                                                 |
| Task\<IReadOnlyList\<FriendRequestDto\>\>                       |
| GetPendingIncomingAsync(Guid userId);                           |
|                                                                 |
| // Validates that userId == AddresseeId before accepting        |
|                                                                 |
| Task\<FriendshipDto\> AcceptRequestAsync(Guid userId, Guid      |
| friendshipId);                                                  |
|                                                                 |
| Task DeclineRequestAsync(Guid userId, Guid friendshipId);       |
|                                                                 |
| // Returns JOIN result: friend profile + today\'s check-in or   |
| null                                                            |
|                                                                 |
| Task\<IReadOnlyList\<FriendWithCheckInDto\>\>                   |
| GetFriendsWithCheckInAsync(                                     |
|                                                                 |
| Guid userId);                                                   |
|                                                                 |
| Task\<FriendDetailDto\> GetFriendDetailAsync(                   |
|                                                                 |
| Guid currentUserId, Guid friendUserId);                         |
+-----------------------------------------------------------------+

> 💡 *SendRequestAsync uses IUserRepository.GetByUsernameAsync to
> resolve the target, then checks GetBetweenUsersAsync to prevent
> duplicate requests. Throws FriendRequestConflictException (mapped to
> 409) if a relationship already exists in any state.*

  -----------------------------------------------------------------
  **2.9 IQuackService --- Quack reaction sending and retrieval**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **IQuackService**                                               |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Services*                         |
+-----------------------------------------------------------------+
| // Throws QuackLimitExceededException (→ 429) if already sent   |
| today                                                           |
|                                                                 |
| // Throws NotFriendsException (→ 403) if sender and recipient   |
| aren\'t friends                                                 |
|                                                                 |
| Task\<QuackDto\> SendQuackAsync(Guid senderId, SendQuackRequest |
| request);                                                       |
|                                                                 |
| // Returns unseen quacks from last 48 hours for Home banner     |
|                                                                 |
| Task\<IReadOnlyList\<QuackDto\>\> GetUnseenAsync(Guid userId);  |
|                                                                 |
| // Validates that quackId belongs to userId before marking seen |
|                                                                 |
| Task MarkSeenAsync(Guid userId, Guid quackId);                  |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **2.10 ICoinService --- Coin balance operations**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **ICoinService**                                                |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Services*                         |
+-----------------------------------------------------------------+
| Task\<int\> GetBalanceAsync(Guid userId);                       |
|                                                                 |
| Task AwardAsync(Guid userId, int amount);                       |
|                                                                 |
| // Returns false when balance is insufficient --- no throw      |
|                                                                 |
| Task\<bool\> TryDeductAsync(Guid userId, int amount);           |
+-----------------------------------------------------------------+

> 💡 *ICoinService never exposes a raw Earn endpoint. Coins are always
> awarded as side effects: lessons call AwardAsync after
> CompleteLessonAsync, gratitude calls AwardAsync on first daily entry.
> The shop calls TryDeductAsync and converts false to a 402 response.*

  -----------------------------------------------------------------
  **2.11 IShopService --- Shop, purchase, and equip logic**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **IShopService**                                                |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Services*                         |
+-----------------------------------------------------------------+
| Task\<IReadOnlyList\<ShopItemDto\>\> GetActiveItemsAsync();     |
|                                                                 |
| // Validates ownership, deducts coins, creates UserInventory    |
| row                                                             |
|                                                                 |
| // Throws InsufficientCoinsException (→ 402) if balance too low |
|                                                                 |
| // Throws AlreadyOwnedException (→ 409) if user already owns    |
| the item                                                        |
|                                                                 |
| Task\<PurchaseResponse\> PurchaseAsync(Guid userId, Guid        |
| shopItemId);                                                    |
|                                                                 |
| // Runs UnequipAllInCategory + EquipAsync in a transaction      |
|                                                                 |
| // Validates user owns the item before equipping                |
|                                                                 |
| Task\<EquippedItems\> EquipAsync(Guid userId, Guid shopItemId); |
|                                                                 |
| Task\<IReadOnlyList\<UserInventoryDto\>\>                       |
| GetInventoryAsync(Guid userId);                                 |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **2.12 IGratitudeService --- Gratitude Garden entry management**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **IGratitudeService**                                           |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Services*                         |
+-----------------------------------------------------------------+
| // Awards 10 coins on first entry of the day only               |
|                                                                 |
| Task\<GratitudeResponse\> AddEntryAsync(Guid userId,            |
| AddGratitudeRequest request);                                   |
|                                                                 |
| Task\<IReadOnlyList\<GratitudeEntryDto\>\> GetAllPagedAsync(    |
|                                                                 |
| Guid userId, int page, int pageSize);                           |
|                                                                 |
| // Returns null if \< 3 entries exist (Pick Me Up guard)        |
|                                                                 |
| Task\<GratitudeEntryDto?\> GetRandomAsync(Guid userId);         |
|                                                                 |
| Task\<GratitudeStreakDto\> GetStreakAsync(Guid userId);         |
+-----------------------------------------------------------------+

> 💡 *GetRandomAsync returns null (not an exception) when the user has
> fewer than 3 entries. The controller returns a 204 with a reason
> header, and the frontend shows \'Keep adding to your garden --- your
> duck needs more to find for you.\'*

  -----------------------------------------------------------------
  **2.13 ILessonService --- Full lesson lifecycle**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **ILessonService**                                              |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Services*                         |
+-----------------------------------------------------------------+
| // Auto-creates missing PillarProgress rows on first call       |
|                                                                 |
| Task\<IReadOnlyList\<PillarProgressDto\>\>                      |
| GetAllPillarProgressAsync(Guid userId);                         |
|                                                                 |
| // Returns all 50 lessons for the pillar with per-lesson        |
| progress overlay                                                |
|                                                                 |
| Task\<IReadOnlyList\<LessonWithProgressDto\>\>                  |
| GetLessonsForPillarAsync(                                       |
|                                                                 |
| Guid userId, Pillar pillar);                                    |
|                                                                 |
| Task\<LessonContentDto\> GetLessonContentAsync(Pillar pillar,   |
| int level);                                                     |
|                                                                 |
| // Checks cooldown, creates session token, returns token +      |
| expiry                                                          |
|                                                                 |
| Task\<StartLessonResult\> StartLessonAsync(Guid userId, Guid    |
| lessonId);                                                      |
|                                                                 |
| // Validates session, scores, updates progress, awards coins    |
|                                                                 |
| Task\<LessonCompleteResult\> CompleteLessonAsync(               |
|                                                                 |
| Guid userId, CompleteLessonRequest request);                    |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **2.14 IEQTestService --- EQ Test questions and submission**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **IEQTestService**                                              |
|                                                                 |
| *DuckyEQ.Contracts.Interfaces.Services*                         |
+-----------------------------------------------------------------+
| // Returns 15 random questions --- CorrectOption stripped from  |
| DTO                                                             |
|                                                                 |
| Task\<IReadOnlyList\<EQTestQuestionDto\>\> GetQuestionsAsync(); |
|                                                                 |
| // Scores submission, persists UserEQTestResult, returns result |
|                                                                 |
| Task\<EQTestResultDto\> SubmitAsync(                            |
|                                                                 |
| Guid userId, SubmitEQTestRequest request);                      |
|                                                                 |
| // Returns null if user has never taken the test                |
|                                                                 |
| Task\<EQTestResultDto?\> GetBestScoreAsync(Guid userId);        |
+-----------------------------------------------------------------+

**3. Data Transfer Objects (DTOs)**

**Location: DuckyEQ.Contracts/DTOs/**

DTOs flow from the service layer to the controller and into the API
response. They are C# records for immutability. They never expose domain
internals like PasswordHash. They are shaped to match exactly what the
UI needs --- one DTO per screen concern.

  -----------------------------------------------------------------
  **3.1 UserProfileDto --- Current user identity + equipped items**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **UserProfileDto**                                              |
|                                                                 |
| *DuckyEQ.Contracts.DTOs / returned by GET /api/user/profile*    |
+-----------------------------------------------------------------+
| public record UserProfileDto(                                   |
|                                                                 |
| string Username, // PascalCase, immutable, shown in social UI   |
|                                                                 |
| string KnownAs, // display name, max 10 chars, mutable          |
|                                                                 |
| DuckCharacter DuckCharacter, // Quack or Lola                   |
|                                                                 |
| int OverallXP,                                                  |
|                                                                 |
| int OverallLevel,                                               |
|                                                                 |
| int StreakDays,                                                 |
|                                                                 |
| bool EmailVerified,                                             |
|                                                                 |
| EquippedItems EquippedItems // hat + accessory + glow + color   |
|                                                                 |
| );                                                              |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **3.2 FriendWithCheckInDto --- Friend card: profile + today\'s
  feeling**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **FriendWithCheckInDto**                                        |
|                                                                 |
| *DuckyEQ.Contracts.DTOs / returned by GET /api/friends*         |
+-----------------------------------------------------------------+
| public record FriendWithCheckInDto(                             |
|                                                                 |
| Guid FriendshipId,                                              |
|                                                                 |
| Guid UserId,                                                    |
|                                                                 |
| string KnownAs,                                                 |
|                                                                 |
| string Username,                                                |
|                                                                 |
| int OverallLevel,                                               |
|                                                                 |
| DuckCharacter DuckCharacter,                                    |
|                                                                 |
| EquippedItems EquippedItems,                                    |
|                                                                 |
| // Check-in data --- null values when HasCheckedInToday = false |
|                                                                 |
| bool HasCheckedInToday,                                         |
|                                                                 |
| List\<string\>? EmotionIds, // e.g. \[\"Happy\", \"Anxious\"\]  |
|                                                                 |
| string? Phrase // max 120 chars, nullable                       |
|                                                                 |
| );                                                              |
+-----------------------------------------------------------------+

> ⚠️ *This DTO is populated by a single LEFT JOIN query in
> IFriendshipRepository.GetFriendsWithCheckInAsync. Do NOT fetch friends
> and check-ins in separate calls --- that produces N+1 API calls for N
> friends. The JOIN returns everything the Friends Feeling Panel needs
> in one round trip.*

  -----------------------------------------------------------------
  **3.3 FriendDetailDto --- Full friend profile for the detail
  screen**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **FriendDetailDto**                                             |
|                                                                 |
| *DuckyEQ.Contracts.DTOs / returned by GET /api/friends/{id}*    |
+-----------------------------------------------------------------+
| public record FriendDetailDto(                                  |
|                                                                 |
| Guid UserId,                                                    |
|                                                                 |
| string KnownAs,                                                 |
|                                                                 |
| string Username,                                                |
|                                                                 |
| int OverallLevel,                                               |
|                                                                 |
| int OverallXP,                                                  |
|                                                                 |
| DuckCharacter DuckCharacter,                                    |
|                                                                 |
| EquippedItems EquippedItems,                                    |
|                                                                 |
| // Today\'s check-in (nullable --- friend may not have checked  |
| in yet)                                                         |
|                                                                 |
| bool HasCheckedInToday,                                         |
|                                                                 |
| List\<string\>? TodayEmotionIds,                                |
|                                                                 |
| string? TodayPhrase,                                            |
|                                                                 |
| // Relationship context                                         |
|                                                                 |
| Guid FriendshipId                                               |
|                                                                 |
| );                                                              |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **3.4 FriendRequestDto --- Incoming friend request card**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **FriendRequestDto**                                            |
|                                                                 |
| *DuckyEQ.Contracts.DTOs / returned by GET                       |
| /api/friends/requests/pending*                                  |
+-----------------------------------------------------------------+
| public record FriendRequestDto(                                 |
|                                                                 |
| Guid FriendshipId,                                              |
|                                                                 |
| Guid RequesterId,                                               |
|                                                                 |
| string RequesterKnownAs,                                        |
|                                                                 |
| string RequesterUsername,                                       |
|                                                                 |
| DuckCharacter RequesterDuckCharacter,                           |
|                                                                 |
| EquippedItems RequesterEquippedItems,                           |
|                                                                 |
| DateTime CreatedAt                                              |
|                                                                 |
| );                                                              |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **3.5 QuackDto --- Quack reaction data**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **QuackDto**                                                    |
|                                                                 |
| *DuckyEQ.Contracts.DTOs / returned by POST /api/quacks and GET  |
| /api/quacks/unseen*                                             |
+-----------------------------------------------------------------+
| public record QuackDto(                                         |
|                                                                 |
| Guid Id,                                                        |
|                                                                 |
| Guid SenderId,                                                  |
|                                                                 |
| string SenderKnownAs,                                           |
|                                                                 |
| string SenderUsername,                                          |
|                                                                 |
| DuckCharacter SenderDuckCharacter,                              |
|                                                                 |
| Guid RecipientId,                                               |
|                                                                 |
| QuackType QuackType, // Hug \| Smile \| HighFive \|             |
| ThinkingOfYou \| Cheer                                          |
|                                                                 |
| DateTime SentAt,                                                |
|                                                                 |
| DateTime? SeenAt // null = unseen                               |
|                                                                 |
| );                                                              |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **3.6 CheckInDto --- Daily check-in record**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **CheckInDto**                                                  |
|                                                                 |
| *DuckyEQ.Contracts.DTOs / returned by GET /api/checkin/today    |
| and POST /api/checkin*                                          |
+-----------------------------------------------------------------+
| public record CheckInDto(                                       |
|                                                                 |
| Guid Id,                                                        |
|                                                                 |
| Guid UserId,                                                    |
|                                                                 |
| DateOnly CheckInDate, // UTC date only --- no time component    |
|                                                                 |
| List\<string\> EmotionIds, // \[\"Happy\", \"Anxious\"\] ---    |
| multi-select                                                    |
|                                                                 |
| string? Phrase, // null = user chose not to share               |
|                                                                 |
| DateTime CreatedAt                                              |
|                                                                 |
| );                                                              |
+-----------------------------------------------------------------+

> 💡 *EmotionIds is List\<string\> (not List\<Emotion\>) in the DTO to
> avoid requiring the client to map enum values. The JSON stores the
> string names: \[\"Happy\", \"Sad\"\]. The service validates each
> string against the Emotion enum before persisting.*

  -----------------------------------------------------------------
  **3.7 UserSearchResultDto --- Username search result**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **UserSearchResultDto**                                         |
|                                                                 |
| *DuckyEQ.Contracts.DTOs / returned by GET                       |
| /api/users/search?username=x*                                   |
+-----------------------------------------------------------------+
| public record UserSearchResultDto(                              |
|                                                                 |
| Guid UserId,                                                    |
|                                                                 |
| string Username,                                                |
|                                                                 |
| string KnownAs,                                                 |
|                                                                 |
| DuckCharacter DuckCharacter,                                    |
|                                                                 |
| EquippedItems EquippedItems,                                    |
|                                                                 |
| // Existing relationship status (null = no relationship yet)    |
|                                                                 |
| FriendshipStatus? ExistingRelationship                          |
|                                                                 |
| );                                                              |
+-----------------------------------------------------------------+

> 💡 *ExistingRelationship allows the Search screen to show contextual
> buttons: \'Add Friend\' (null), \'Request Sent\' (Pending + requester
> = currentUser), \'Accept Request\' (Pending + addressee =
> currentUser), or \'Already Friends\' (Accepted). The service loads the
> friendship row and maps the status.*

**3.8 Supporting Types --- EquippedItems, ShopItemDto,
PillarProgressDto**

+-----------------------------------------------------------------+
| **EquippedItems (record --- used throughout)**                  |
|                                                                 |
| *DuckyEQ.Contracts.DTOs*                                        |
+-----------------------------------------------------------------+
| public record EquippedItems(                                    |
|                                                                 |
| ShopItemDto? Hat,                                               |
|                                                                 |
| ShopItemDto? Accessory,                                         |
|                                                                 |
| ShopItemDto? Glow,                                              |
|                                                                 |
| ShopItemDto? Color                                              |
|                                                                 |
| );                                                              |
|                                                                 |
| public record ShopItemDto(                                      |
|                                                                 |
| Guid Id,                                                        |
|                                                                 |
| string Name,                                                    |
|                                                                 |
| ShopCategory Category,                                          |
|                                                                 |
| int CoinPrice,                                                  |
|                                                                 |
| string? QuackImageUrl,                                          |
|                                                                 |
| string? LolaImageUrl,                                           |
|                                                                 |
| bool IsOwned,                                                   |
|                                                                 |
| bool IsEquipped,                                                |
|                                                                 |
| string? Rarity,                                                 |
|                                                                 |
| string? Description                                             |
|                                                                 |
| );                                                              |
|                                                                 |
| public record PillarProgressDto(                                |
|                                                                 |
| Pillar Pillar,                                                  |
|                                                                 |
| string Name,                                                    |
|                                                                 |
| int CurrentLevel,                                               |
|                                                                 |
| int XP,                                                         |
|                                                                 |
| bool IsUnlocked                                                 |
|                                                                 |
| );                                                              |
|                                                                 |
| public record LessonWithProgressDto(                            |
|                                                                 |
| Guid Id,                                                        |
|                                                                 |
| Pillar Pillar,                                                  |
|                                                                 |
| int Level,                                                      |
|                                                                 |
| string Title,                                                   |
|                                                                 |
| string Objective,                                               |
|                                                                 |
| int? BestScore,                                                 |
|                                                                 |
| int? BestStars,                                                 |
|                                                                 |
| DateTime? FirstCompletedAt,                                     |
|                                                                 |
| bool IsUnlocked                                                 |
|                                                                 |
| );                                                              |
|                                                                 |
| public record GratitudeEntryDto(                                |
|                                                                 |
| Guid Id,                                                        |
|                                                                 |
| string Text,                                                    |
|                                                                 |
| GratitudeCategory Category,                                     |
|                                                                 |
| DateTime CreatedAt                                              |
|                                                                 |
| );                                                              |
|                                                                 |
| public record EQTestQuestionDto(                                |
|                                                                 |
| Guid Id,                                                        |
|                                                                 |
| string QuestionText,                                            |
|                                                                 |
| string OptionA,                                                 |
|                                                                 |
| string OptionB,                                                 |
|                                                                 |
| string OptionC,                                                 |
|                                                                 |
| string OptionD                                                  |
|                                                                 |
| // CorrectOption intentionally excluded from DTO                |
|                                                                 |
| );                                                              |
|                                                                 |
| public record UserInventoryDto(                                 |
|                                                                 |
| Guid Id,                                                        |
|                                                                 |
| Guid ShopItemId,                                                |
|                                                                 |
| string ItemName,                                                |
|                                                                 |
| ShopCategory Category,                                          |
|                                                                 |
| bool IsEquipped,                                                |
|                                                                 |
| DateTime PurchasedAt                                            |
|                                                                 |
| );                                                              |
+-----------------------------------------------------------------+

> 💡 *EQTestQuestionDto deliberately excludes CorrectOption. The correct
> answer is validated server-side only during POST /api/eq-test/submit
> --- it never travels to the client in the question payload.*

**4. Request / Response Models**

**Location: DuckyEQ.Contracts/Models/ (or grouped with DTOs in
DuckyEQ.Contracts/DTOs/)**

Request models are the inbound JSON payloads validated by
FluentValidation. Response models are structured results returned by the
service to the controller. They are kept separate from DTOs to make
intent clear: DTOs describe data shape, models describe API actions.

  -----------------------------------------------------------------
  **4.1 Auth --- Register, Login, Known As Update**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **Auth Request / Response Models**                              |
|                                                                 |
| *DuckyEQ.Contracts.Models.Auth*                                 |
+-----------------------------------------------------------------+
| // Inbound: POST /api/auth/register                             |
|                                                                 |
| public record RegisterRequest(                                  |
|                                                                 |
| string Email,                                                   |
|                                                                 |
| string Password, // validated 8--128 chars server-side          |
|                                                                 |
| DuckCharacter DuckCharacter, // Quack or Lola                   |
|                                                                 |
| string KnownAs // 2--10 chars, letters/spaces/numbers           |
|                                                                 |
| );                                                              |
|                                                                 |
| // Inbound: POST /api/auth/login                                |
|                                                                 |
| public record LoginRequest(                                     |
|                                                                 |
| string Email,                                                   |
|                                                                 |
| string Password                                                 |
|                                                                 |
| );                                                              |
|                                                                 |
| // Inbound: PUT /api/user/known-as                              |
|                                                                 |
| public record UpdateKnownAsRequest(                             |
|                                                                 |
| string KnownAs // 2--10 chars, letters/spaces/numbers only      |
|                                                                 |
| );                                                              |
|                                                                 |
| // Outbound: register + login + known-as update                 |
|                                                                 |
| public record AuthResult(                                       |
|                                                                 |
| string Token, // JWT bearer token                               |
|                                                                 |
| string Username, // PascalCase auto-generated                   |
|                                                                 |
| string KnownAs,                                                 |
|                                                                 |
| Guid UserId                                                     |
|                                                                 |
| );                                                              |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **4.2 Check-In --- Emotion submission**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **CheckIn Request Model**                                       |
|                                                                 |
| *DuckyEQ.Contracts.Models.CheckIn*                              |
+-----------------------------------------------------------------+
| // Inbound: POST /api/checkin                                   |
|                                                                 |
| public record CheckInRequest(                                   |
|                                                                 |
| List\<string\> EmotionIds, // required, 1--6 values from        |
| Emotion enum                                                    |
|                                                                 |
| string? Phrase // optional, max 120 chars; empty string → null  |
|                                                                 |
| );                                                              |
|                                                                 |
| // Outbound: CheckInDto (see Section 3.6)                       |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **4.3 Friendship --- Send, Accept, Decline**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **Friendship Request / Response Models**                        |
|                                                                 |
| *DuckyEQ.Contracts.Models.Friendship*                           |
+-----------------------------------------------------------------+
| // Inbound: POST /api/friends/request                           |
|                                                                 |
| public record SendFriendRequestPayload(                         |
|                                                                 |
| string TargetUsername // exact username match, min 3 chars      |
|                                                                 |
| );                                                              |
|                                                                 |
| // Outbound: FriendshipDto --- used by POST                     |
| /api/friends/request                                            |
|                                                                 |
| // and PUT /api/friends/requests/{id}/accept                    |
|                                                                 |
| public record FriendshipDto(                                    |
|                                                                 |
| Guid Id,                                                        |
|                                                                 |
| Guid RequesterId,                                               |
|                                                                 |
| Guid AddresseeId,                                               |
|                                                                 |
| FriendshipStatus Status,                                        |
|                                                                 |
| DateTime CreatedAt                                              |
|                                                                 |
| );                                                              |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **4.4 Quack --- Send a Quack reaction**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **Quack Request Model**                                         |
|                                                                 |
| *DuckyEQ.Contracts.Models.Quack*                                |
+-----------------------------------------------------------------+
| // Inbound: POST /api/quacks                                    |
|                                                                 |
| public record SendQuackRequest(                                 |
|                                                                 |
| Guid RecipientId,                                               |
|                                                                 |
| QuackType QuackType // Hug \| Smile \| HighFive \|              |
| ThinkingOfYou \| Cheer                                          |
|                                                                 |
| );                                                              |
|                                                                 |
| // Outbound: QuackDto (see Section 3.5)                         |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **4.5 Lessons --- Start, Complete**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **Lesson Request / Response Models**                            |
|                                                                 |
| *DuckyEQ.Contracts.Models.Lesson*                               |
+-----------------------------------------------------------------+
| // Outbound: POST /api/lessons/{id}/start                       |
|                                                                 |
| public record StartLessonResult(                                |
|                                                                 |
| string SessionToken, // GUID string --- required for complete   |
| call                                                            |
|                                                                 |
| DateTime ExpiresAt // UTC, 30 minutes from start                |
|                                                                 |
| );                                                              |
|                                                                 |
| // Inbound: POST /api/lessons/{id}/complete                     |
|                                                                 |
| public record CompleteLessonRequest(                            |
|                                                                 |
| string SessionToken,                                            |
|                                                                 |
| int CorrectAnswers,                                             |
|                                                                 |
| int TotalQuestions                                              |
|                                                                 |
| );                                                              |
|                                                                 |
| // Outbound: POST /api/lessons/{id}/complete                    |
|                                                                 |
| public record LessonCompleteResult(                             |
|                                                                 |
| int Score,                                                      |
|                                                                 |
| int Stars,                                                      |
|                                                                 |
| bool IsNewBest,                                                 |
|                                                                 |
| bool IsFirstCompletion,                                         |
|                                                                 |
| int CoinsAwarded                                                |
|                                                                 |
| );                                                              |
|                                                                 |
| // Outbound: GET /api/pillars/{id}/cooldown-status              |
|                                                                 |
| public record CooldownStatus(                                   |
|                                                                 |
| bool IsLocked,                                                  |
|                                                                 |
| DateTime? NextAvailableAt // null when IsLocked = false         |
|                                                                 |
| );                                                              |
|                                                                 |
| // Outbound: GET /api/lessons/{pillarId}/{level}                |
|                                                                 |
| public record LessonContentDto(                                 |
|                                                                 |
| Guid Id,                                                        |
|                                                                 |
| Pillar Pillar,                                                  |
|                                                                 |
| int Level,                                                      |
|                                                                 |
| string Title,                                                   |
|                                                                 |
| string ContentJson // full CDER JSON blob                       |
|                                                                 |
| );                                                              |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **4.6 EQ Test --- Submit answers**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **EQ Test Request / Response Models**                           |
|                                                                 |
| *DuckyEQ.Contracts.Models.EQTest*                               |
+-----------------------------------------------------------------+
| // Inbound: POST /api/eq-test/submit                            |
|                                                                 |
| public record SubmitEQTestRequest(                              |
|                                                                 |
| List\<EQTestAnswer\> Answers                                    |
|                                                                 |
| );                                                              |
|                                                                 |
| public record EQTestAnswer(                                     |
|                                                                 |
| Guid QuestionId,                                                |
|                                                                 |
| EQTestOption SelectedOption // A \| B \| C \| D                 |
|                                                                 |
| );                                                              |
|                                                                 |
| // Outbound: POST /api/eq-test/submit + GET                     |
| /api/eq-test/best-score                                         |
|                                                                 |
| public record EQTestResultDto(                                  |
|                                                                 |
| int Score,                                                      |
|                                                                 |
| int Stars,                                                      |
|                                                                 |
| int CorrectAnswers,                                             |
|                                                                 |
| bool IsNewBest                                                  |
|                                                                 |
| );                                                              |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **4.7 Shop --- Purchase and Equip**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **Shop Request / Response Models**                              |
|                                                                 |
| *DuckyEQ.Contracts.Models.Shop*                                 |
+-----------------------------------------------------------------+
| // Inbound: POST /api/shop/purchase                             |
|                                                                 |
| public record PurchaseRequest(                                  |
|                                                                 |
| Guid ShopItemId                                                 |
|                                                                 |
| );                                                              |
|                                                                 |
| // Inbound: POST /api/shop/equip                                |
|                                                                 |
| public record EquipRequest(                                     |
|                                                                 |
| Guid ShopItemId                                                 |
|                                                                 |
| );                                                              |
|                                                                 |
| // Outbound: POST /api/shop/purchase                            |
|                                                                 |
| public record PurchaseResponse(                                 |
|                                                                 |
| bool Success,                                                   |
|                                                                 |
| int NewBalance,                                                 |
|                                                                 |
| EquippedItems EquippedItems // auto-equips on purchase          |
|                                                                 |
| );                                                              |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **4.8 Gratitude --- Entry submission**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **Gratitude Request / Response Models**                         |
|                                                                 |
| *DuckyEQ.Contracts.Models.Gratitude*                            |
+-----------------------------------------------------------------+
| // Inbound: POST /api/gratitude                                 |
|                                                                 |
| public record AddGratitudeRequest(                              |
|                                                                 |
| string Text, // 1--500 chars                                    |
|                                                                 |
| GratitudeCategory Category // School \| Work \| Family \|       |
| Friends \| Self \| Other                                        |
|                                                                 |
| );                                                              |
|                                                                 |
| // Outbound: POST /api/gratitude                                |
|                                                                 |
| public record GratitudeResponse(                                |
|                                                                 |
| Guid Id,                                                        |
|                                                                 |
| int CoinsAwarded // 10 on first entry of day, 0 thereafter      |
|                                                                 |
| );                                                              |
|                                                                 |
| // Outbound: GET /api/gratitude/streak                          |
|                                                                 |
| public record GratitudeStreakDto(                               |
|                                                                 |
| int CurrentStreak,                                              |
|                                                                 |
| int LongestStreak                                               |
|                                                                 |
| );                                                              |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **4.9 LessonSession --- In-memory session value (IMemoryCache)**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **LessonSession (not a DTO --- lives in DuckyEQ.Services or     |
| DuckyEQ.Contracts)**                                            |
|                                                                 |
| *DuckyEQ.Contracts.Models.Lesson (or DuckyEQ.Services.Session)* |
+-----------------------------------------------------------------+
| // Stored in IMemoryCache keyed by session token GUID string    |
|                                                                 |
| // 30-minute sliding expiration --- never persisted to DB       |
|                                                                 |
| public record LessonSession(                                    |
|                                                                 |
| Guid UserId,                                                    |
|                                                                 |
| Guid LessonId,                                                  |
|                                                                 |
| DateTime StartedAt                                              |
|                                                                 |
| );                                                              |
+-----------------------------------------------------------------+

**5. Complete File Layout --- DuckyEQ.Contracts**

The Contracts project has zero dependencies --- no NuGet packages, no
project references. It compiles in isolation. This is what makes it a
stable contract layer.

  -----------------------------------------------------------------
  DuckyEQ.Contracts/

  │

  ├── Interfaces/

  │ ├── Repositories/

  │ │ ├── IUserRepository.cs

  │ │ ├── IPillarProgressRepository.cs

  │ │ ├── IUserLessonProgressRepository.cs

  │ │ ├── ILessonRepository.cs

  │ │ ├── IEQTestQuestionRepository.cs

  │ │ ├── IUserEQTestResultRepository.cs

  │ │ ├── ICoinRepository.cs

  │ │ ├── IShopItemRepository.cs

  │ │ ├── IUserInventoryRepository.cs

  │ │ ├── IGratitudeRepository.cs

  │ │ ├── IDailyCheckInRepository.cs ★ social

  │ │ ├── IFriendshipRepository.cs ★ social

  │ │ └── IQuackRepository.cs ★ social

  │ │

  │ └── Services/

  │ ├── IAuthService.cs

  │ ├── IUsernameGenerator.cs

  │ ├── IScoringService.cs

  │ ├── IEQTestScoringService.cs

  │ ├── ISessionService.cs

  │ ├── ICooldownService.cs

  │ ├── ICheckInService.cs ★ social

  │ ├── IFriendshipService.cs ★ social

  │ ├── IQuackService.cs ★ social

  │ ├── ICoinService.cs

  │ ├── IShopService.cs

  │ ├── IGratitudeService.cs

  │ ├── ILessonService.cs

  │ └── IEQTestService.cs

  │

  └── DTOs/

  ├── UserProfileDto.cs

  ├── FriendWithCheckInDto.cs ★ social

  ├── FriendDetailDto.cs ★ social

  ├── FriendRequestDto.cs ★ social

  ├── QuackDto.cs ★ social

  ├── CheckInDto.cs ★ social

  ├── UserSearchResultDto.cs ★ social

  ├── FriendshipDto.cs ★ social

  ├── EquippedItems.cs (shared, used by many DTOs)

  ├── ShopItemDto.cs

  ├── UserInventoryDto.cs

  ├── PillarProgressDto.cs

  ├── LessonWithProgressDto.cs

  ├── LessonContentDto.cs

  ├── GratitudeEntryDto.cs

  ├── GratitudeStreakDto.cs

  ├── EQTestQuestionDto.cs

  ├── EQTestResultDto.cs

  │

  └── Models/ (request + response payloads)

  ├── Auth/

  │ ├── RegisterRequest.cs

  │ ├── LoginRequest.cs

  │ ├── UpdateKnownAsRequest.cs

  │ └── AuthResult.cs

  ├── CheckIn/

  │ └── CheckInRequest.cs

  ├── Friendship/

  │ ├── SendFriendRequestPayload.cs

  │ └── FriendshipDto.cs

  ├── Quack/

  │ └── SendQuackRequest.cs

  ├── Lesson/

  │ ├── StartLessonResult.cs

  │ ├── CompleteLessonRequest.cs

  │ ├── LessonCompleteResult.cs

  │ └── CooldownStatus.cs

  ├── EQTest/

  │ ├── SubmitEQTestRequest.cs

  │ ├── EQTestAnswer.cs

  │ └── EQTestResultDto.cs

  ├── Shop/

  │ ├── PurchaseRequest.cs

  │ ├── EquipRequest.cs

  │ └── PurchaseResponse.cs

  └── Gratitude/

  ├── AddGratitudeRequest.cs

  ├── GratitudeResponse.cs

  └── LessonSession.cs
  -----------------------------------------------------------------

**6. Interface → API Endpoint Traceability Map**

Every API endpoint maps to a specific service method. This table lets
you verify coverage: every endpoint in the API map should have exactly
one service interface method as its handler.

  ------------------------------------ ------------------------ --------------------------------------------------
  **Endpoint**                         **Service Interface**    **Method**

  POST /api/auth/register              **IAuthService**         RegisterAsync()

  POST /api/auth/login                 **IAuthService**         LoginAsync()

  POST /api/auth/verify-email          **IAuthService**         VerifyEmailAsync()

  GET /api/user/profile                **IUserRepository**      GetByIdAsync() +
                                                                IUserInventoryRepository.GetEquippedItemsAsync()

  PUT /api/user/known-as               **IAuthService**         UpdateKnownAsAsync()

  PUT /api/user/duck-character         **IUserRepository**      UpdateAsync()

  GET /api/pillars/progress            **ILessonService**       GetAllPillarProgressAsync()

  GET /api/pillars/{id}/lessons        **ILessonService**       GetLessonsForPillarAsync()

  GET                                  **ICooldownService**     GetPillarStatusAsync()
  /api/pillars/{id}/cooldown-status                             

  GET /api/lessons/{pillarId}/{level}  **ILessonService**       GetLessonContentAsync()

  POST /api/lessons/{id}/start         **ILessonService**       StartLessonAsync()

  POST /api/lessons/{id}/complete      **ILessonService**       CompleteLessonAsync()

  GET /api/coins/balance               **ICoinService**         GetBalanceAsync()

  GET /api/eq-test/questions           **IEQTestService**       GetQuestionsAsync()

  POST /api/eq-test/submit             **IEQTestService**       SubmitAsync()

  GET /api/eq-test/best-score          **IEQTestService**       GetBestScoreAsync()

  GET /api/shop/items                  **IShopService**         GetActiveItemsAsync()

  POST /api/shop/purchase              **IShopService**         PurchaseAsync()

  POST /api/shop/equip                 **IShopService**         EquipAsync()

  GET /api/shop/inventory              **IShopService**         GetInventoryAsync()

  POST /api/gratitude                  **IGratitudeService**    AddEntryAsync()

  GET /api/gratitude                   **IGratitudeService**    GetAllPagedAsync()

  GET /api/gratitude/random            **IGratitudeService**    GetRandomAsync()

  GET /api/gratitude/streak            **IGratitudeService**    GetStreakAsync()

  GET /api/checkin/today ★             **ICheckInService**      GetTodayAsync()

  POST /api/checkin ★                  **ICheckInService**      CheckInAsync()

  GET /api/users/search ★              **IFriendshipService**   (resolves via IUserRepository)

  POST /api/friends/request ★          **IFriendshipService**   SendRequestAsync()

  GET /api/friends/requests/pending ★  **IFriendshipService**   GetPendingIncomingAsync()

  PUT                                  **IFriendshipService**   AcceptRequestAsync()
  /api/friends/requests/{id}/accept ★                           

  PUT                                  **IFriendshipService**   DeclineRequestAsync()
  /api/friends/requests/{id}/decline ★                          

  GET /api/friends ★                   **IFriendshipService**   GetFriendsWithCheckInAsync()

  GET /api/friends/{id} ★              **IFriendshipService**   GetFriendDetailAsync()

  POST /api/quacks ★                   **IQuackService**        SendQuackAsync()

  GET /api/quacks/unseen ★             **IQuackService**        GetUnseenAsync()

  PATCH /api/quacks/{id}/seen ★        **IQuackService**        MarkSeenAsync()
  ------------------------------------ ------------------------ --------------------------------------------------

**7. Key Patterns + Learning Takeaways**

**7.1 Nullable Returns = \'Not Found\' Semantics, Not Exceptions**

Nullable return types (User?, DailyCheckIn?, Friendship?) are a
deliberate design choice. They signal that \'nothing found\' is a valid,
expected outcome --- not an error. The service layer interprets null and
responds appropriately:

  -----------------------------------------------------------------
  // Repository returns null → Service converts to meaningful
  response

  var checkIn = await \_checkInRepo.GetTodayAsync(userId);

  if (checkIn is null) return null; // Service returns null

  // Controller receives null → Returns 204 No Content

  var result = await \_checkInService.GetTodayAsync(userId);

  if (result is null) return NoContent(); // Frontend shows
  check-in modal
  -----------------------------------------------------------------

**7.2 Why FriendWithCheckInDto Bundles Profile + Check-In**

The Home screen Friends Panel needs, for each friend: KnownAs, username,
duck character, equipped items, today\'s emotions, and today\'s phrase.
If you fetch these in separate calls:

- GET /api/friends → N friend profiles

- GET /api/checkin/today for user_id=X → for each of the N friends

That is N+1 API calls for N friends. With 10 friends that is 11 calls on
every Home screen load. FriendWithCheckInDto solves this with a single
LEFT JOIN query:

  -----------------------------------------------------------------
  SELECT f.Id as FriendshipId,

  u.Id, u.KnownAs, u.Username, u.OverallLevel, u.DuckCharacter,

  ci.EmotionIds, ci.Phrase, ci.CheckInDate

  FROM Friendships f

  JOIN Users u ON (u.Id = f.AddresseeId AND f.RequesterId =
  \@userId)

  OR (u.Id = f.RequesterId AND f.AddresseeId = \@userId)

  LEFT JOIN DailyCheckIns ci ON ci.UserId = u.Id

  AND ci.CheckInDate = CAST(GETUTCDATE() AS DATE)

  WHERE f.Status = \'Accepted\'
  -----------------------------------------------------------------

The LEFT JOIN ensures friends who haven\'t checked in today still appear
--- with null check-in columns. HasCheckedInToday is set by the
repository: !string.IsNullOrEmpty(ci.EmotionIds). One query, one DTO,
one render.

**7.3 Interface Segregation: Each Interface Has One Job**

Every interface in this document covers exactly one domain concern.
IAuthService handles identity. ICoinService handles balance.
IShopService handles inventory and equip. They do not overlap. This
makes them independently testable and independently swappable:

- In tests: mock ICoinService to always return 500 coins without
  touching the DB

- In Phase 2: swap ICooldownService for a more permissive version
  without touching auth

- For debugging: add logging to IQuackService without touching
  IFriendshipService

**7.4 async Task Everywhere --- No Sync DB Calls**

Every repository and service method is async. There are no synchronous
database calls in this codebase. This is non-negotiable:

- Sync DB calls block ASP.NET Core thread pool threads --- the API stops
  handling new requests

- await propagates the async chain cleanly from controller → service →
  repository

- ISessionService is the only exception --- it calls IMemoryCache which
  is synchronous in-memory

**7.5 Contracts Has No Dependencies**

DuckyEQ.Contracts has no NuGet packages and no project references. It
references only the .NET BCL and DuckyEQ.Domain (for enums). This is the
key property that makes it a stable contract layer --- Infrastructure
and Services can both reference it without creating circular
dependencies. The compile-time boundary enforces the architecture:

  -----------------------------------------------------------------
  // Project references (enforced at compile time):

  // DuckyEQ.Api → DuckyEQ.Contracts + DuckyEQ.Services +
  DuckyEQ.Infrastructure

  // DuckyEQ.Services → DuckyEQ.Contracts + DuckyEQ.Domain

  // DuckyEQ.Infrastructure → DuckyEQ.Domain + DuckyEQ.Contracts

  // DuckyEQ.Contracts → DuckyEQ.Domain only

  // DuckyEQ.Domain → (nothing --- pure C# classes)

  // If a controller tries to call a repository directly:

  // error CS0246: The type or namespace name \'SqlUserRepository\'
  could not be found

  // (because DuckyEQ.Api does not reference DuckyEQ.Infrastructure
  directly in code)
  -----------------------------------------------------------------

  -----------------------------------------------------------------
  **Day 6 Preview --- EF Core DbContext + All Repositories + DI
  Registration**

  -----------------------------------------------------------------

**Tomorrow you implement everything you defined today. The flow:**

- Create DuckyEQDbContext with DbSets for all entities

- Fluent API: configure Friendship and Quack with
  DeleteBehavior.Restrict on both FKs to avoid cascade cycle errors

- Unique indexes: users.Username, (UserId, CheckInDate) on
  daily_checkins, (RequesterId, AddresseeId) on friendships

- Run Add-Migration InitialCreate and Update-Database

- Implement all repository classes --- each class implements its
  matching interface from today

- SqlFriendshipRepository.GetFriendsWithCheckInAsync: the LEFT JOIN
  query from Section 7.2 above

- SqlQuackRepository.HasSentTodayAsync: CAST(SentAt AS DATE) =
  CAST(GETUTCDATE() AS DATE)

- Register everything in Program.cs: AddDbContext, AddScoped for all
  repo/service pairs, AddMemoryCache, AddSingleton\<IUsernameGenerator\>

> ⚠️ *Cascade Delete Reminder --- Friendship has two FKs to User
> (RequesterId + AddresseeId). SQL Server will refuse to create the
> migration if both are set to Cascade because it detects a potential
> cascade cycle. Use DeleteBehavior.Restrict on both FKs in Fluent API.
> User deletion does not auto-cascade --- you handle friendship cleanup
> explicitly in the account deletion endpoint.*
>
> 💡 *Compile check before Day 6: the Contracts project should compile
> cleanly with zero warnings. If you see \'missing using directive\'
> errors, the interface references an enum that hasn\'t been imported
> from DuckyEQ.Domain. Add the using statement and verify the project
> reference to Domain is in place.*

*🦆 DuckyEQ · Day 5 Complete · duckyeq.com*
