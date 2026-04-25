**🦆 DuckyEQ**

**Post-Day 5 Project State Reference**

*Complete file inventory · namespace map · architectural decisions ·
what was built through Day 5*

  ---------------- --------------------------------------------------
  **Status**       **Days 1--5 complete. Zero compile errors. Ready
                   for Day 6.**

  **Days           1--5 (Backend Foundation phase)
  complete**       

  **Days           6--45
  remaining**      

  **Next session** Day 6 --- EF Core DbContext + all repositories +
                   DI registration

  **Compile        Clean build, zero errors as of end of Day 5
  state**          

  **Projects**     5 (Api, Domain, Contracts, Infrastructure,
                   Services)

  **Interfaces**   13 repository + 14 service = 27 total

  **DTOs +         18 DTOs + 19 request/response models = 37 total
  Models**         

  **Domain         14 entities + 9 enums
  entities**       
  ---------------- --------------------------------------------------

> 💡 *Attach this doc alongside DuckyEQ_Day5_Interfaces_DTOs.docx for
> every future build session. This doc covers what exists and where. The
> Day 5 doc covers the method signatures and design rationale.*

**1. Solution Structure**

Five projects. Each compiles independently. Dependency direction is
enforced at compile time --- if you accidentally reference a project
that\'s not in the project references list, it won\'t build.

  ------------------------ ----------------------------------------------
  **Project**              **Purpose + Dependencies**

  DuckyEQ.Api              Entry point. Controllers only. References:
                           Contracts, Services, Infrastructure
                           (Program.cs only)

  DuckyEQ.Domain           Entities + enums. Zero dependencies --- pure
                           C# classes.

  DuckyEQ.Contracts        Interfaces + DTOs + request/response models.
                           References: Domain only.

  DuckyEQ.Infrastructure   DbContext + repository implementations +
                           migrations. References: Domain, Contracts.

  DuckyEQ.Services         Behavior + service implementations +
                           utilities. References: Domain, Contracts.
  ------------------------ ----------------------------------------------

> ⚠️ *Api → Infrastructure is allowed ONLY in Program.cs for DI
> registration. A controller must never instantiate or call a repository
> directly.*

**2. DuckyEQ.Domain --- Complete File Inventory**

**Namespace root: DuckyEQ.Domain**

  -----------------------------------------------------------------
  **2.1 Enums (DuckyEQ.Domain/Enums/)**

  -----------------------------------------------------------------

+-----------------------------------------------------------------+
| **DuckyEQ.Domain/Enums/**                                       |
|                                                                 |
| ├── Pillar.cs *SelfAwareness=1, SelfManagement=2,               |
| SocialAwareness=3, RelationshipSkills=4,                        |
| ResponsibleDecisionMaking=5*                                    |
|                                                                 |
| ├── DuckCharacter.cs *Quack, Lola*                              |
|                                                                 |
| ├── ShopCategory.cs *Hat, Accessory, Glow, Color*               |
|                                                                 |
| ├── GratitudeCategory.cs *School, Work, Family, Friends,        |
| Health, Self, Other*                                            |
|                                                                 |
| ├── Emotion.cs *Happy, Sad, Angry, Anxious, Meh, Excited*       |
|                                                                 |
| ├── QuackType.cs *Hug, Smile, HighFive, ThinkingOfYou, Cheer*   |
|                                                                 |
| ├── FriendshipStatus.cs *Pending, Accepted, Declined*           |
|                                                                 |
| ├── LessonEngageFormat.cs *game format identifiers*             |
|                                                                 |
| └── EQTestOption.cs *A, B, C, D*                                |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **2.2 Entities (DuckyEQ.Domain/Entities/)**

  -----------------------------------------------------------------

All entities have private setters and static factory methods (e.g.
User.Create(\...)). No public constructors.

+-----------------------------------------------------------------+
| **DuckyEQ.Domain/Entities/**                                    |
|                                                                 |
| ├── User.cs *Id(Guid), Email, PasswordHash, Username(immutable  |
| PascalCase), KnownAs(max 10), DuckCharacter, OverallXP,         |
| OverallLevel, StreakDays, LastActiveDate, CreatedAt,            |
| EmailVerified*                                                  |
|                                                                 |
| ├── PillarProgress.cs *Id, UserId(FK), PillarId(enum),          |
| CurrentLevel, XP, LastNewLessonCompletedAt*                     |
|                                                                 |
| ├── UserLessonProgress.cs *Id, UserId(FK), LessonId(FK),        |
| BestScore(0-300), BestStars(1-3), TotalAttempts,                |
| FirstCompletedAt(nullable), LastAttemptedAt*                    |
|                                                                 |
| ├── Lesson.cs *Id, PillarId, Level, Title, CoreMessage,         |
| Objective, JokeSetup, JokePunchline, JokeSetupExpr,             |
| JokePunchlineExpr, DefineConcept, DefineFlashcardsJson,         |
| EngageGameType, EngageConfigJson, RewardTier, DuckArcJson,      |
| ShareCardConfigJson, CreatedAt, UpdatedAt*                      |
|                                                                 |
| ├── EQTestQuestion.cs *Id, QuestionText, OptionA-D,             |
| CorrectOption(EQTestOption), Explanation, PillarId(nullable)*   |
|                                                                 |
| ├── UserEQTestResult.cs *Id, UserId(FK), Score, Stars(0-3),     |
| CorrectAnswers, AttemptedAt*                                    |
|                                                                 |
| ├── QuackCoins.cs *Id, UserId(FK), Balance, TotalEarned,        |
| LastEarnedAt*                                                   |
|                                                                 |
| ├── ShopItem.cs *Id, Name, Category(enum), CoinPrice,           |
| QuackImageUrl, LolaImageUrl, IsDefault, IsActive, IsWeeklyItem, |
| WeekNumber, WeeklyAvailableFrom, WeeklyAvailableTo,             |
| Description, Rarity*                                            |
|                                                                 |
| ├── UserInventory.cs *Id, UserId(FK), ShopItemId(FK),           |
| Category(denormalized), IsEquipped, PurchasedAt*                |
|                                                                 |
| ├── GratitudeEntry.cs *Id, UserId(FK), Text,                    |
| Category(GratitudeCategory), CreatedAt*                         |
|                                                                 |
| ├── DailyCheckIn.cs *Id, UserId(FK), CheckInDate(DATE),         |
| EmotionIds(JSON array), Phrase(nvarchar 120, nullable),         |
| CreatedAt ★ social*                                             |
|                                                                 |
| ├── Friendship.cs *Id, RequesterId(FK→User),                    |
| AddresseeId(FK→User), Status(FriendshipStatus), CreatedAt,      |
| UpdatedAt ★ social*                                             |
|                                                                 |
| ├── Quack.cs *Id, SenderId(FK→User), RecipientId(FK→User),      |
| QuackType(enum), SentAt, SeenAt(nullable) ★ social*             |
|                                                                 |
| └── LessonConnectQuestion.cs / LessonReflectQuestion.cs *Id,    |
| LessonId, QuestionText, DisplayOrder*                           |
+-----------------------------------------------------------------+

**3. DuckyEQ.Contracts --- Complete File Inventory**

**Namespace root: DuckyEQ.Contracts**

Zero NuGet dependencies. Only references DuckyEQ.Domain. This is the
stable contract layer --- both Infrastructure and Services depend on it,
never the reverse.

  -----------------------------------------------------------------
  **3.1 Repository Interfaces
  (Contracts/Interfaces/Repositories/)**

  -----------------------------------------------------------------

**Namespace: DuckyEQ.Contracts.Interfaces.Repositories**

All usings: using DuckyEQ.Domain.Entities; --- plus DuckyEQ.Domain.Enums
where a Pillar/ShopCategory/FriendshipStatus param is used, plus
DuckyEQ.Contracts.DTOs for the two JOIN-result interfaces.

+-----------------------------------------------------------------+
| **Contracts/Interfaces/Repositories/**                          |
|                                                                 |
| ├── IUserRepository.cs *GetByIdAsync, GetByEmailAsync,          |
| GetByUsernameAsync, IsUsernameTakenAsync, CreateAsync,          |
| UpdateAsync, UpdateKnownAsAsync*                                |
|                                                                 |
| ├── IPillarProgressRepository.cs *GetByUserAndPillarAsync,      |
| GetAllByUserAsync, CreateAsync, UpdateAsync,                    |
| EnsureAllPillarsExistAsync*                                     |
|                                                                 |
| ├── IUserLessonProgressRepository.cs *GetByUserAndLessonAsync,  |
| GetByUserAndPillarAsync, UpsertAsync,                           |
| GetLastNewLessonCompletedAtAsync*                               |
|                                                                 |
| ├── ILessonRepository.cs *GetByPillarAsync, GetByIdAsync,       |
| GetByPillarAndLevelAsync*                                       |
|                                                                 |
| ├── IEQTestQuestionRepository.cs *GetRandomSetAsync,            |
| GetAllAsync*                                                    |
|                                                                 |
| ├── IUserEQTestResultRepository.cs *CreateAsync,                |
| GetBestByUserAsync*                                             |
|                                                                 |
| ├── ICoinRepository.cs *GetByUserAsync, EnsureExistsAsync,      |
| AwardAsync, DeductAsync*                                        |
|                                                                 |
| ├── IShopItemRepository.cs *GetActiveItemsAsync,                |
| GetWeeklyItemsForCurrentWeekAsync, GetByTypeAsync,              |
| GetByIdAsync*                                                   |
|                                                                 |
| ├── IUserInventoryRepository.cs *GetByUserAsync,                |
| GetByUserAndItemAsync, UserOwnsItemAsync, CreateAsync,          |
| UnequipAllInCategoryAsync, EquipAsync, GetEquippedItemsAsync    |
| \[needs DTOs\]*                                                 |
|                                                                 |
| ├── IGratitudeRepository.cs *CreateAsync, GetByUserPagedAsync,  |
| GetTodayByUserAsync, GetRandomByUserAsync,                      |
| GetCurrentStreakAsync, GetLongestStreakAsync*                   |
|                                                                 |
| ├── IDailyCheckInRepository.cs *GetTodayAsync(returns           |
| DailyCheckIn? --- null=204), CreateAsync ★ social*              |
|                                                                 |
| ├── IFriendshipRepository.cs *CreateAsync,                      |
| GetFriendsWithCheckInAsync, GetPendingIncomingAsync,            |
| GetByIdAsync, UpdateStatusAsync, GetBetweenUsersAsync,          |
| GetFriendDetailAsync ★ social \[needs DTOs\]*                   |
|                                                                 |
| └── IQuackRepository.cs *CreateAsync,                           |
| GetUnseenByRecipientAsync, MarkSeenAsync, HasSentTodayAsync ★   |
| social*                                                         |
+-----------------------------------------------------------------+

> 💡 *Purple entries = those two files also need using
> DuckyEQ.Contracts.DTOs; because their return types are pre-shaped JOIN
> DTOs, not raw entities.*

  -----------------------------------------------------------------
  **3.2 Service Interfaces (Contracts/Interfaces/Services/)**

  -----------------------------------------------------------------

**Namespace: DuckyEQ.Contracts.Interfaces.Services**

Services return DTOs, never entities. Most need using
DuckyEQ.Contracts.DTOs; --- some also need using
DuckyEQ.Contracts.Models; for request payloads. Four need no usings at
all (primitive in/out only).

+-----------------------------------------------------------------+
| **Contracts/Interfaces/Services/**                              |
|                                                                 |
| ├── IUsernameGenerator.cs *GenerateUniqueAsync() →              |
| Task\<string\> \[no usings needed\]*                            |
|                                                                 |
| ├── IScoringService.cs *CalculateScore, GetStars,               |
| BaseCoinsForStars \[no usings needed\]*                         |
|                                                                 |
| ├── IEQTestScoringService.cs *CalculateScore, GetStars \[no     |
| usings needed\]*                                                |
|                                                                 |
| ├── ICoinService.cs *GetBalanceAsync, AwardAsync,               |
| TryDeductAsync \[no usings needed\]*                            |
|                                                                 |
| ├── IAuthService.cs *RegisterAsync, LoginAsync,                 |
| VerifyEmailAsync, UpdateKnownAsAsync \[needs DTOs + Models\]*   |
|                                                                 |
| ├── ISessionService.cs *CreateSession, GetSession,              |
| RemoveSession \[needs Models --- LessonSession\]*               |
|                                                                 |
| ├── ICheckInService.cs *GetTodayAsync, CheckInAsync ★ social    |
| \[needs DTOs + Models\]*                                        |
|                                                                 |
| ├── IQuackService.cs *SendQuackAsync, GetUnseenAsync,           |
| MarkSeenAsync ★ social \[needs DTOs + Models\]*                 |
|                                                                 |
| ├── IFriendshipService.cs *SendRequestAsync,                    |
| GetPendingIncomingAsync, AcceptRequestAsync,                    |
| DeclineRequestAsync, GetFriendsWithCheckInAsync,                |
| GetFriendDetailAsync ★ social \[needs DTOs + Models\]*          |
|                                                                 |
| ├── IGratitudeService.cs *AddEntryAsync, GetAllPagedAsync,      |
| GetRandomAsync, GetStreakAsync \[needs DTOs + Models\]*         |
|                                                                 |
| ├── IShopService.cs *GetActiveItemsAsync, PurchaseAsync,        |
| EquipAsync, GetInventoryAsync \[needs DTOs + Models\]*          |
|                                                                 |
| ├── IEQTestService.cs *GetQuestionsAsync, SubmitAsync,          |
| GetBestScoreAsync \[needs DTOs + Models\]*                      |
|                                                                 |
| ├── ICooldownService.cs *GetPillarStatusAsync,                  |
| CanStartNewLessonAsync \[needs Models + Domain.Enums ---        |
| Pillar\]*                                                       |
|                                                                 |
| └── ILessonService.cs *GetAllPillarProgressAsync,               |
| GetLessonsForPillarAsync, GetLessonContentAsync,                |
| StartLessonAsync, CompleteLessonAsync \[needs DTOs + Models +   |
| Domain.Enums --- Pillar\]*                                      |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **3.3 DTOs (Contracts/DTOs/)**

  -----------------------------------------------------------------

**Namespace: DuckyEQ.Contracts.DTOs**

All are C# records. DTOs flow outward from services to controllers to
API responses. They never expose domain internals like PasswordHash.

+-----------------------------------------------------------------+
| **Contracts/DTOs/**                                             |
|                                                                 |
| ├── UserProfileDto.cs *Username, KnownAs, DuckCharacter,        |
| OverallXP, OverallLevel, StreakDays, EmailVerified,             |
| EquippedItems*                                                  |
|                                                                 |
| ├── EquippedItems.cs *Hat?, Accessory?, Glow?, Color? --- all   |
| ShopItemDto? --- used everywhere a duck is rendered*            |
|                                                                 |
| ├── ShopItemDto.cs *Id, Name, Category, CoinPrice,              |
| QuackImageUrl?, LolaImageUrl?, IsOwned, IsEquipped, Rarity?,    |
| Description?*                                                   |
|                                                                 |
| ├── UserInventoryDto.cs *Id, ShopItemId, ItemName, Category,    |
| IsEquipped, PurchasedAt*                                        |
|                                                                 |
| ├── PillarProgressDto.cs *Pillar, Name, CurrentLevel, XP,       |
| IsUnlocked*                                                     |
|                                                                 |
| ├── LessonWithProgressDto.cs *Id, Pillar, Level, Title,         |
| Objective, BestScore?, BestStars?, FirstCompletedAt?,           |
| IsUnlocked*                                                     |
|                                                                 |
| ├── LessonContentDto.cs *Id, Pillar, Level, Title, ContentJson* |
|                                                                 |
| ├── GratitudeEntryDto.cs *Id, Text, Category, CreatedAt*        |
|                                                                 |
| ├── GratitudeStreakDto.cs *CurrentStreak, LongestStreak*        |
|                                                                 |
| ├── EQTestQuestionDto.cs *Id, QuestionText, OptionA/B/C/D ---   |
| CorrectOption intentionally excluded*                           |
|                                                                 |
| ├── EQTestResultDto.cs *Score, Stars, CorrectAnswers,           |
| IsNewBest*                                                      |
|                                                                 |
| ├── FriendshipDto.cs *Id, RequesterId, AddresseeId, Status,     |
| CreatedAt ★ social*                                             |
|                                                                 |
| ├── CheckInDto.cs *Id, UserId, CheckInDate,                     |
| EmotionIds(List\<string\>), Phrase?, CreatedAt ★ social*        |
|                                                                 |
| ├── FriendWithCheckInDto.cs *FriendshipId, UserId, KnownAs,     |
| Username, OverallLevel, DuckCharacter, EquippedItems,           |
| HasCheckedInToday, EmotionIds?, Phrase? ★ social --- populated  |
| by LEFT JOIN*                                                   |
|                                                                 |
| ├── FriendDetailDto.cs *UserId, KnownAs, Username,              |
| OverallLevel, OverallXP, DuckCharacter, EquippedItems,          |
| HasCheckedInToday, TodayEmotionIds?, TodayPhrase?, FriendshipId |
| ★ social*                                                       |
|                                                                 |
| ├── FriendRequestDto.cs *FriendshipId, RequesterId,             |
| RequesterKnownAs, RequesterUsername, RequesterDuckCharacter,    |
| RequesterEquippedItems, CreatedAt ★ social*                     |
|                                                                 |
| ├── QuackDto.cs *Id, SenderId, SenderKnownAs, SenderUsername,   |
| SenderDuckCharacter, RecipientId, QuackType, SentAt, SeenAt? ★  |
| social*                                                         |
|                                                                 |
| └── UserSearchResultDto.cs *UserId, Username, KnownAs,          |
| DuckCharacter, EquippedItems,                                   |
| ExistingRelationship(FriendshipStatus?) ★ social*               |
+-----------------------------------------------------------------+

  -----------------------------------------------------------------
  **3.4 Request / Response Models (Contracts/DTOs/Models/)**

  -----------------------------------------------------------------

**Namespace: DuckyEQ.Contracts.Models (flat --- all subfolders share the
same namespace)**

All are C# records. Request models are inbound JSON payloads. Response
models are structured service return types. Keep using
DuckyEQ.Contracts.Models; alongside using DuckyEQ.Contracts.DTOs; in
service interfaces.

+-----------------------------------------------------------------+
| **Contracts/DTOs/Models/**                                      |
|                                                                 |
| ├── **Auth/**                                                   |
|                                                                 |
| │ ├── RegisterRequest.cs *Email, Password, DuckCharacter,       |
| KnownAs*                                                        |
|                                                                 |
| │ ├── LoginRequest.cs *Email, Password*                         |
|                                                                 |
| │ ├── UpdateKnownAsRequest.cs *KnownAs*                         |
|                                                                 |
| │ └── AuthResult.cs *Token, Username, KnownAs, UserId*          |
|                                                                 |
| ├── **CheckIn/**                                                |
|                                                                 |
| │ └── CheckInRequest.cs *EmotionIds(List\<string\>), Phrase?*   |
|                                                                 |
| ├── **Friendship/**                                             |
|                                                                 |
| │ └── SendFriendRequestPayload.cs *TargetUsername*              |
|                                                                 |
| ├── **Quack/**                                                  |
|                                                                 |
| │ └── SendQuackRequest.cs *RecipientId, QuackType*              |
|                                                                 |
| ├── **Lesson/**                                                 |
|                                                                 |
| │ ├── StartLessonResult.cs *SessionToken, ExpiresAt*            |
|                                                                 |
| │ ├── CompleteLessonRequest.cs *SessionToken, CorrectAnswers,   |
| TotalQuestions*                                                 |
|                                                                 |
| │ ├── LessonCompleteResult.cs *Score, Stars, IsNewBest,         |
| IsFirstCompletion, CoinsAwarded*                                |
|                                                                 |
| │ ├── CooldownStatus.cs *IsLocked, NextAvailableAt?*            |
|                                                                 |
| │ └── LessonSession.cs *UserId, LessonId, StartedAt ---         |
| in-memory only, never persisted*                                |
|                                                                 |
| ├── **EQTest/**                                                 |
|                                                                 |
| │ ├── SubmitEQTestRequest.cs *List\<EQTestAnswer\>*             |
|                                                                 |
| │ └── EQTestAnswer.cs *QuestionId,                              |
| SelectedOption(EQTestOption)*                                   |
|                                                                 |
| ├── **Shop/**                                                   |
|                                                                 |
| │ ├── PurchaseRequest.cs *ShopItemId*                           |
|                                                                 |
| │ ├── EquipRequest.cs *ShopItemId*                              |
|                                                                 |
| │ └── PurchaseResponse.cs *Success, NewBalance, EquippedItems*  |
|                                                                 |
| └── **Gratitude/**                                              |
|                                                                 |
| ├── AddGratitudeRequest.cs *Text, Category(GratitudeCategory)*  |
|                                                                 |
| └── GratitudeResponse.cs *Id, CoinsAwarded*                     |
+-----------------------------------------------------------------+

**4. DuckyEQ.Infrastructure --- What Gets Built Day 6**

**Namespace root: DuckyEQ.Infrastructure**

Nothing exists here yet --- Day 5 was pure interfaces and contracts. Day
6 builds everything below.

+-----------------------------------------------------------------+
| **DuckyEQ.Infrastructure/**                                     |
|                                                                 |
| ├── **Data/**                                                   |
|                                                                 |
| │ └── DuckyEQDbContext.cs *DbSets for all 14 entities. Fluent   |
| API config. Restrict cascade on Friendship + Quack FKs.*        |
|                                                                 |
| ├── **Repositories/**                                           |
|                                                                 |
| │ ├── SqlUserRepository.cs *implements IUserRepository*         |
|                                                                 |
| │ ├── SqlPillarProgressRepository.cs *implements                |
| IPillarProgressRepository*                                      |
|                                                                 |
| │ ├── SqlUserLessonProgressRepository.cs *implements            |
| IUserLessonProgressRepository*                                  |
|                                                                 |
| │ ├── SqlLessonRepository.cs *implements ILessonRepository*     |
|                                                                 |
| │ ├── SqlEQTestQuestionRepository.cs *implements                |
| IEQTestQuestionRepository*                                      |
|                                                                 |
| │ ├── SqlUserEQTestResultRepository.cs *implements              |
| IUserEQTestResultRepository*                                    |
|                                                                 |
| │ ├── SqlCoinRepository.cs *implements ICoinRepository*         |
|                                                                 |
| │ ├── SqlShopItemRepository.cs *implements IShopItemRepository* |
|                                                                 |
| │ ├── SqlUserInventoryRepository.cs *implements                 |
| IUserInventoryRepository*                                       |
|                                                                 |
| │ ├── SqlGratitudeRepository.cs *implements                     |
| IGratitudeRepository*                                           |
|                                                                 |
| │ ├── SqlDailyCheckInRepository.cs *implements                  |
| IDailyCheckInRepository ★ social*                               |
|                                                                 |
| │ ├── SqlFriendshipRepository.cs *implements                    |
| IFriendshipRepository --- contains the LEFT JOIN query ★        |
| social*                                                         |
|                                                                 |
| │ └── SqlQuackRepository.cs *implements IQuackRepository ---    |
| HasSentTodayAsync date comparison ★ social*                     |
|                                                                 |
| └── **Migrations/**                                             |
|                                                                 |
| └── (generated by EF Core) *Add-Migration InitialCreate →       |
| Update-Database*                                                |
+-----------------------------------------------------------------+

**5. DuckyEQ.Services --- What Gets Built Day 6 Onward**

**Namespace root: DuckyEQ.Services**

+-----------------------------------------------------------------+
| **DuckyEQ.Services/**                                           |
|                                                                 |
| ├── **Behaviors/ (orchestration --- calls multiple services)**  |
|                                                                 |
| │ ├── AuthBehavior.cs *register + login + email verify flow*    |
|                                                                 |
| │ ├── LessonBehavior.cs *start + complete + reflect             |
| orchestration*                                                  |
|                                                                 |
| │ ├── CheckInBehavior.cs *check-in + friends panel fetch ★      |
| social*                                                         |
|                                                                 |
| │ ├── FriendsBehavior.cs *request + accept + decline + list ★   |
| social*                                                         |
|                                                                 |
| │ └── QuackBehavior.cs *send + unseen + mark seen ★ social*     |
|                                                                 |
| ├── **Services/ (single-responsibility business rules)**        |
|                                                                 |
| │ ├── AuthService.cs *implements IAuthService*                  |
|                                                                 |
| │ ├── ScoringService.cs *implements IScoringService*            |
|                                                                 |
| │ ├── EQTestScoringService.cs *implements                       |
| IEQTestScoringService*                                          |
|                                                                 |
| │ ├── SessionService.cs *implements ISessionService --- wraps   |
| IMemoryCache*                                                   |
|                                                                 |
| │ ├── CooldownService.cs *implements ICooldownService*          |
|                                                                 |
| │ ├── CoinService.cs *implements ICoinService*                  |
|                                                                 |
| │ ├── ShopService.cs *implements IShopService*                  |
|                                                                 |
| │ ├── GratitudeService.cs *implements IGratitudeService*        |
|                                                                 |
| │ ├── LessonService.cs *implements ILessonService*              |
|                                                                 |
| │ ├── EQTestService.cs *implements IEQTestService*              |
|                                                                 |
| │ ├── CheckInService.cs *implements ICheckInService ★ social*   |
|                                                                 |
| │ ├── FriendshipService.cs *implements IFriendshipService ★     |
| social*                                                         |
|                                                                 |
| │ └── QuackService.cs *implements IQuackService ★ social*       |
|                                                                 |
| └── **Utilities/**                                              |
|                                                                 |
| └── UsernameGenerator.cs *implements IUsernameGenerator ---     |
| registered as AddSingleton*                                     |
+-----------------------------------------------------------------+

**6. DuckyEQ.Api --- Controller Map**

**Namespace root: DuckyEQ.Api**

Controllers call Behaviors (not Services directly). Behaviors
orchestrate Services. Services call Repositories. No layer skips.

  --------------------- ----------------------------------------------
  **Controller**        **Endpoints handled**

  AuthController        POST /api/auth/register, /api/auth/login,
                        /api/auth/verify-email

  UserController        GET /api/user/profile, PUT
                        /api/user/duck-character, PUT
                        /api/user/known-as

  PillarController      GET /api/pillars/progress, GET
                        /api/pillars/{id}/lessons, GET
                        /api/pillars/{id}/cooldown-status

  LessonController      GET /api/lessons/{pillarId}/{level}, POST
                        /api/lessons/{id}/start, POST
                        /api/lessons/{id}/complete

  CoinController        GET /api/coins/balance

  EQTestController      GET /api/eq-test/questions, POST
                        /api/eq-test/submit, GET
                        /api/eq-test/best-score

  ShopController        GET /api/shop/items, POST /api/shop/purchase,
                        POST /api/shop/equip, GET /api/shop/inventory

  GratitudeController   POST /api/gratitude, GET /api/gratitude, GET
                        /api/gratitude/today, GET
                        /api/gratitude/random, GET
                        /api/gratitude/streak

  CheckInController ★   GET /api/checkin/today, POST /api/checkin

  FriendsController ★   GET /api/friends, GET /api/friends/{id}, POST
                        /api/friends/request, GET
                        /api/friends/requests/pending, PUT
                        /api/friends/requests/{id}/accept, PUT
                        /api/friends/requests/{id}/decline, GET
                        /api/users/search

  QuacksController ★    POST /api/quacks, GET /api/quacks/unseen,
                        PATCH /api/quacks/{id}/seen
  --------------------- ----------------------------------------------

**7. Namespace Quick Reference**

Copy-paste cheat sheet for using statements. Every project and folder
has exactly one namespace.

  ---------------------- -------------------------------------------
  **Folder / Purpose**   **Namespace**

  Domain entities        DuckyEQ.Domain.Entities

  Domain enums           DuckyEQ.Domain.Enums

  Repository interfaces  DuckyEQ.Contracts.Interfaces.Repositories

  Service interfaces     DuckyEQ.Contracts.Interfaces.Services

  DTOs                   DuckyEQ.Contracts.DTOs

  Request/response       DuckyEQ.Contracts.Models
  models                 

  DbContext              DuckyEQ.Infrastructure.Data

  Repository             DuckyEQ.Infrastructure.Repositories
  implementations        

  Service                DuckyEQ.Services.Services
  implementations        

  Behavior               DuckyEQ.Services.Behaviors
  implementations        

  Utilities (UsernameGen DuckyEQ.Services.Utilities
  etc)                   

  Controllers            DuckyEQ.Api.Controllers
  ---------------------- -------------------------------------------

**8. Architectural Rules --- Never Break These**

  -----------------------------------------------------------------
  **8.1 Layer Call Direction**

  -----------------------------------------------------------------

  -----------------------------------------------------------------
  Controller → Behavior → Service → Repository

  ✓ Controller calls Behavior

  ✓ Behavior calls multiple Services

  ✓ Service calls Repository

  ✗ Controller must never call Repository directly

  ✗ Service must never call another Service\'s Repository

  ✗ Repository must never contain business logic
  -----------------------------------------------------------------

  -----------------------------------------------------------------
  **8.2 What Each Layer Returns**

  -----------------------------------------------------------------

  ------------------- ----------------------------------------------
  **Layer**           **Returns**

  Repository          Domain entities (User, Lesson, etc.) ---
                      except JOIN queries which return DTOs directly

  Service             DTOs and Model records --- never domain
                      entities

  Behavior            DTOs and Model records --- orchestrates
                      services, adds no new data logic

  Controller          IActionResult wrapping DTOs --- maps
                      exceptions to HTTP status codes
  ------------------- ----------------------------------------------

  -----------------------------------------------------------------
  **8.3 Critical EF Core Rules for Day 6**

  -----------------------------------------------------------------

- Friendship has TWO FKs to User (RequesterId + AddresseeId) → use
  DeleteBehavior.Restrict on BOTH or the migration will fail with a
  cascade cycle error

- Quack also has two FKs to User (SenderId + RecipientId) → same
  Restrict treatment

- Unique indexes required: users.Username, (UserId, CheckInDate) on
  DailyCheckIns, (RequesterId, AddresseeId) on Friendships

- UserInventory.Category must be denormalized from ShopItem --- needed
  for fast UnequipAllInCategoryAsync without a JOIN

- LessonSession is NOT a DbSet --- it lives only in IMemoryCache with
  30-min TTL

  -----------------------------------------------------------------
  **8.4 Security Rules (always enforce)**

  -----------------------------------------------------------------

- Server always determines coin amounts --- client never sends a coin
  value, only an action

- CorrectOption is never included in EQTestQuestionDto --- validated
  server-side only on submit

- JWT carries UserId, Username, KnownAs claims --- extract UserId from
  token in every authenticated endpoint

- PUT /api/user/known-as reissues a full new JWT --- client must replace
  stored token immediately

- Quack rate limit: one per (SenderId, RecipientId) per calendar day ---
  429 if exceeded

- Daily check-in: unique constraint (UserId, CheckInDate) enforced at DB
  AND service layer

  -----------------------------------------------------------------
  **8.5 Shop Equip Sequence --- Always in This Order**

  -----------------------------------------------------------------

  -----------------------------------------------------------------
  1\. Load ShopItem to get its Category

  2\. UnequipAllInCategoryAsync(userId, category) ← clears the slot

  3\. EquipAsync(userId, shopItemId) ← sets the new item

  4\. Return GetEquippedItemsAsync(userId) ← full EquippedItems
  record

  Steps 2+3 run inside a DB transaction.

  Never equip without unequipping first --- one active item per
  category slot.
  -----------------------------------------------------------------

  -----------------------------------------------------------------
  **8.6 Weekly Shop Rotation --- No Background Job Needed**

  -----------------------------------------------------------------

Weekly items are NOT rotated by a timer or cron job. The query does it:

  -----------------------------------------------------------------
  WHERE IsWeeklyItem = 1

  AND WeeklyAvailableFrom \<= GETUTCDATE()

  AND WeeklyAvailableTo \>= GETUTCDATE()

  Pre-seed 8+ weeks of weekly items with date ranges at deploy
  time.

  Users keep purchased items after rotation --- ownership is
  permanent.
  -----------------------------------------------------------------

**9. Social System --- Key Rules**

  -----------------------------------------------------------------
  **9.1 FriendWithCheckInDto --- Always a JOIN, Never N+1**

  -----------------------------------------------------------------

The Friends Feeling Panel on Home needs profile + today\'s emotions for
every friend. This MUST be one LEFT JOIN query --- never fetch friends
then loop for each check-in.

  -----------------------------------------------------------------
  SELECT friendships + users + daily_checkins

  LEFT JOIN daily_checkins ON UserId = friend.Id

  AND CheckInDate = CAST(GETUTCDATE() AS DATE)

  LEFT JOIN = friends who haven\'t checked in still appear
  (HasCheckedInToday = false)

  INNER JOIN = friends without a check-in are silently excluded ---
  wrong
  -----------------------------------------------------------------

  -----------------------------------------------------------------
  **9.2 DailyCheckIn --- Null Semantics**

  -----------------------------------------------------------------

  -----------------------------------------------------------------
  IDailyCheckInRepository.GetTodayAsync() returns DailyCheckIn?

  null → user has not checked in today → controller returns 204 No
  Content

  204 → React Native shows the check-in modal

  Do not throw NotFoundException for null --- 204 is the designed
  signal.
  -----------------------------------------------------------------

  -----------------------------------------------------------------
  **9.3 Quack Rate Limit**

  -----------------------------------------------------------------

  -----------------------------------------------------------------
  IQuackRepository.HasSentTodayAsync(senderId, recipientId)

  Query: WHERE SenderId = \@s AND RecipientId = \@r

  AND CAST(SentAt AS DATE) = CAST(GETUTCDATE() AS DATE)

  true → service throws QuackLimitExceededException

  → controller maps to 429 Too Many Requests

  → Retry-After header = midnight UTC

  Also validate: sender and recipient must be accepted friends (403
  if not).
  -----------------------------------------------------------------

  -----------------------------------------------------------------
  **9.4 Phrase Field --- Null vs Empty String**

  -----------------------------------------------------------------

  -----------------------------------------------------------------
  DailyCheckIn.Phrase is nullable --- null means \'user chose not
  to share\'

  Empty string is INVALID --- factory method rejects it and
  converts to null

  CheckInRequest.Phrase arrives as string? from client

  Service: if (string.IsNullOrWhiteSpace(request.Phrase)) phrase =
  null;

  Never store empty string --- you can\'t tell if it was
  intentional or accidental.
  -----------------------------------------------------------------

**10. Day 6 Build Checklist**

Everything needed to complete Day 6: EF Core DbContext + all
repositories + DI registration.

  -----------------------------------------------------------------
  **DbContext**

  -----------------------------------------------------------------

- Create DuckyEQDbContext : DbContext in DuckyEQ.Infrastructure/Data/

- Add DbSet\<T\> for all 13 persisted entities (LessonSession is NOT a
  DbSet)

- Fluent API: Friendship --- DeleteBehavior.Restrict on RequesterId FK
  and AddresseeId FK

- Fluent API: Quack --- DeleteBehavior.Restrict on SenderId FK and
  RecipientId FK

- Unique index: .HasIndex(u =\> u.Username).IsUnique() on User

- Unique index: .HasIndex(c =\> new { c.UserId, c.CheckInDate
  }).IsUnique() on DailyCheckIn

- Unique index: .HasIndex(f =\> new { f.RequesterId, f.AddresseeId
  }).IsUnique() on Friendship

  -----------------------------------------------------------------
  **Migrations**

  -----------------------------------------------------------------

- Run: Add-Migration InitialCreate in Package Manager Console
  (Infrastructure project selected)

- Run: Update-Database

- Verify in Azure Portal Query Editor: all tables created with correct
  columns

  -----------------------------------------------------------------
  **Repository Implementations**

  -----------------------------------------------------------------

- Create Sql\[Name\]Repository.cs for each of the 13 repository
  interfaces

- Each takes DuckyEQDbContext via constructor injection

- SqlFriendshipRepository.GetFriendsWithCheckInAsync: the LEFT JOIN
  query (see Section 9.1)

- SqlQuackRepository.HasSentTodayAsync: CAST date comparison (see
  Section 9.3)

- SqlUserInventoryRepository: UnequipAll + Equip must run in a
  transaction

  -----------------------------------------------------------------
  **DI Registration in Program.cs**

  -----------------------------------------------------------------

  ------------------------------------------------------------------------------------------
  builder.Services.AddDbContext\<DuckyEQDbContext\>(options =\>

  options.UseSqlServer(builder.Configuration.GetConnectionString(\"DefaultConnection\")));

  builder.Services.AddMemoryCache();

  // Repositories --- all AddScoped

  builder.Services.AddScoped\<IUserRepository, SqlUserRepository\>();

  // \... repeat for all 13 repositories

  // Services --- all AddScoped except UsernameGenerator

  builder.Services.AddSingleton\<IUsernameGenerator, UsernameGenerator\>();

  builder.Services.AddScoped\<IAuthService, AuthService\>();

  // \... repeat for all remaining services
  ------------------------------------------------------------------------------------------

*🦆 DuckyEQ · Post-Day 5 Reference · duckyeq.com*
