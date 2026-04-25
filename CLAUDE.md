# 🦆 DuckyEQ — Claude Code Reference

> **This file is the single source of truth for every Claude Code session.**
> Read it fully before writing any code. When in doubt, refer back here.
> Supporting docs live in `/docs/` — links are in each section below.

---

## Project Overview

**DuckyEQ** is a social-emotional learning (SEL) mobile app built with React Native (Expo) + ASP.NET Core. It teaches the five CASEL-aligned EQ competencies through a duck mascot companion system, board game lesson progression, daily emotion check-ins, and a friends social layer.

- **Domain**: duckyeq.com
- **API subdomain**: api.duckyeq.com
- **Deep link scheme**: `duckyeq://`
- **Bundle ID**: com.duckyeq.app
- **Solo build** — one developer (Diego)

---

## Current Build Status

| Field | Value |
|---|---|
| **Days complete** | 1–5 (Backend Foundation phase) |
| **Next session** | Day 6 — EF Core DbContext + all repositories + DI registration |
| **Compile state** | Clean build, zero errors as of end of Day 5 |
| **Solution projects** | 5 (Api, Domain, Contracts, Infrastructure, Services) |
| **Interfaces** | 13 repository + 14 service = 27 total |
| **DTOs + Models** | 18 DTOs + 19 request/response models = 37 total |
| **Domain entities** | 14 entities + 9 enums |

---

## ⚠️ Critical Overrides — Read First

### Duck Character Names
The two duck companions are **Ducky** (boy) and **Daisy** (girl). Older docs may say "Quack" and "Lola" — **those are wrong**. Always use:

| | Correct | Wrong (old) |
|---|---|---|
| Boy duck | `Ducky` | ~~Quack~~ |
| Girl duck | `Daisy` | ~~Lola~~ |
| Enum | `DuckCharacter.Ducky` | ~~DuckCharacter.Quack~~ |

**Asset folder convention:**
```
/src/assets/characters/ducky/   ← boy duck
/src/assets/characters/daisy/   ← girl duck
```

---

## Architecture Pattern

**Strict layered architecture. Never break the dependency rules.**

```
Controller → Behavior → Service → Repository → DbContext
```

| Layer | Project | Rule |
|---|---|---|
| Controllers | `DuckyEQ.Api` | Call services only. Never touch repositories directly. |
| Behaviors | `DuckyEQ.Services` | Cross-cutting concerns (validation, auth checks) that wrap service calls. |
| Services | `DuckyEQ.Services` | Business logic. Return DTOs, never entities. |
| Repositories | `DuckyEQ.Infrastructure` | Data access only. Return domain entities, never DTOs. |
| Interfaces + DTOs | `DuckyEQ.Contracts` | Shared contract layer. Both Services and Infrastructure depend on this. |
| Entities + Enums | `DuckyEQ.Domain` | Zero dependencies — pure C# classes with private setters and static factory methods. |

**Dependency direction (enforced at compile time):**
```
Api → Contracts, Services
Services → Contracts, Domain
Infrastructure → Contracts, Domain
Contracts → Domain
Domain → (nothing)
```

> ⚠️ `Api → Infrastructure` is ONLY allowed in `Program.cs` for DI registration. A controller must never instantiate or call a repository directly.

---

## Solution Structure

```
duckyeq-api/
├── DuckyEQ.Api/                  # Entry point — controllers only
├── DuckyEQ.Domain/               # Entities + enums — zero dependencies
├── DuckyEQ.Contracts/            # Interfaces + DTOs + request/response models
├── DuckyEQ.Infrastructure/       # DbContext + repo implementations + migrations
└── DuckyEQ.Services/             # Behavior + service implementations + utilities
```

---

## Domain Layer — Entities & Enums

### Enums (`DuckyEQ.Domain/Enums/`)

| File | Values |
|---|---|
| `Pillar.cs` | `SelfAwareness=1, SelfManagement=2, SocialAwareness=3, RelationshipSkills=4, ResponsibleDecisionMaking=5` |
| `DuckCharacter.cs` | `Ducky, Daisy` |
| `ShopCategory.cs` | `Hat, Accessory, Glow, Color` |
| `GratitudeCategory.cs` | `School, Work, Family, Friends, Health, Self, Other` |
| `Emotion.cs` | `Happy, Sad, Angry, Anxious, Meh, Excited` |
| `QuackType.cs` | `Hug, Smile, HighFive, ThinkingOfYou, Cheer` |
| `FriendshipStatus.cs` | `Pending, Accepted, Declined` |
| `LessonEngageFormat.cs` | Game format identifiers |
| `EQTestOption.cs` | `A, B, C, D` |

### Entities (`DuckyEQ.Domain/Entities/`)

All entities use **private setters** and **static factory methods** (e.g. `User.Create(...)`). No public constructors.

| Entity | Key Fields |
|---|---|
| `User` | Id(Guid), Email, PasswordHash, Username(immutable PascalCase), KnownAs(max 10), DuckCharacter, OverallXP, OverallLevel, StreakDays, LastActiveDate, CreatedAt, EmailVerified |
| `PillarProgress` | Id, UserId(FK), PillarId(enum), CurrentLevel, XP, LastNewLessonCompletedAt |
| `UserLessonProgress` | Id, UserId(FK), LessonId(FK), BestScore(0-300), BestStars(1-3), TotalAttempts, FirstCompletedAt(nullable), LastAttemptedAt |
| `Lesson` | Id, PillarId, Level, Title, CoreMessage, Objective, JokeSetup, JokePunchline, JokeSetupExpr, JokePunchlineExpr, DefineConcept, DefineFlashcardsJson, EngageGameType, EngageConfigJson, RewardTier, DuckArcJson, ShareCardConfigJson, CreatedAt, UpdatedAt |
| `EQTestQuestion` | Id, QuestionText, OptionA/B/C/D, CorrectOption(EQTestOption), Explanation, PillarId(nullable) |
| `UserEQTestResult` | Id, UserId(FK), Score, Stars(0-3), CorrectAnswers, AttemptedAt |
| `QuackCoins` | Id, UserId(FK), Balance, TotalEarned, LastEarnedAt |
| `ShopItem` | Id, Name, Category(enum), CoinPrice, DuckyImageUrl, DaisyImageUrl, IsDefault, IsActive, IsWeeklyItem, WeekNumber, WeeklyAvailableFrom, WeeklyAvailableTo, Description, Rarity |
| `UserInventory` | Id, UserId(FK), ShopItemId(FK), Category(denormalized), IsEquipped, PurchasedAt |
| `GratitudeEntry` | Id, UserId(FK), Text, Category(GratitudeCategory), CreatedAt |
| `DailyCheckIn` ★ | Id, UserId(FK), CheckInDate(DATE), EmotionIds(JSON array), Phrase(nvarchar 120, nullable), CreatedAt |
| `Friendship` ★ | Id, RequesterId(FK→User), AddresseeId(FK→User), Status(FriendshipStatus), CreatedAt, UpdatedAt |
| `Quack` ★ | Id, SenderId(FK→User), RecipientId(FK→User), QuackType(enum), SentAt, SeenAt(nullable) |
| `LessonConnectQuestion` | Id, LessonId, QuestionText, DisplayOrder |
| `LessonReflectQuestion` | Id, LessonId, QuestionText, DisplayOrder |

★ = social entities

---

## Contracts Layer

### Repository Interfaces (`DuckyEQ.Contracts/Interfaces/Repositories/`)
Namespace: `DuckyEQ.Contracts.Interfaces.Repositories`

All methods return `Task<T>`. Repositories return domain entities — no DTOs.

| Interface | Key Methods |
|---|---|
| `IUserRepository` | GetByIdAsync, GetByEmailAsync, GetByUsernameAsync, IsUsernameTakenAsync, CreateAsync, UpdateAsync, UpdateKnownAsAsync |
| `IPillarProgressRepository` | GetByUserAndPillarAsync, GetAllByUserAsync, CreateAsync, UpdateAsync, EnsureAllPillarsExistAsync |
| `IUserLessonProgressRepository` | GetByUserAndLessonAsync, GetByUserAndPillarAsync, UpsertAsync, GetLastNewLessonCompletedAtAsync |
| `ILessonRepository` | GetByPillarAsync, GetByIdAsync, GetByPillarAndLevelAsync |
| `IEQTestQuestionRepository` | GetRandomSetAsync, GetAllAsync |
| `IUserEQTestResultRepository` | CreateAsync, GetBestByUserAsync |
| `ICoinRepository` | GetByUserAsync, EnsureExistsAsync, AwardAsync, DeductAsync |
| `IShopItemRepository` | GetActiveItemsAsync, GetWeeklyItemsForCurrentWeekAsync, GetByTypeAsync, GetByIdAsync |
| `IUserInventoryRepository` | GetByUserAsync, GetByUserAndItemAsync, UserOwnsItemAsync, CreateAsync, UnequipAllInCategoryAsync, EquipAsync, GetEquippedItemsAsync |
| `IGratitudeRepository` | CreateAsync, GetByUserPagedAsync, GetTodayByUserAsync, GetRandomByUserAsync, GetCurrentStreakAsync, GetLongestStreakAsync |
| `IDailyCheckInRepository` ★ | GetTodayAsync (returns `DailyCheckIn?` — null = 204), CreateAsync |
| `IFriendshipRepository` ★ | CreateAsync, GetFriendsWithCheckInAsync, GetPendingIncomingAsync, GetByIdAsync, UpdateStatusAsync, GetBetweenUsersAsync, GetFriendDetailAsync |
| `IQuackRepository` ★ | CreateAsync, GetUnseenByRecipientAsync, MarkSeenAsync, HasSentTodayAsync |

### Service Interfaces (`DuckyEQ.Contracts/Interfaces/Services/`)
Namespace: `DuckyEQ.Contracts.Interfaces.Services`

Services return DTOs, never entities.

| Interface | Notes |
|---|---|
| `IUsernameGenerator` | GenerateUniqueAsync() → `Task<string>` — registered Singleton |
| `IScoringService` | CalculateScore, GetStars, BaseCoinsForStars |
| `IEQTestScoringService` | CalculateScore, GetStars |
| `ICoinService` | GetBalanceAsync, AwardAsync, TryDeductAsync |
| `IAuthService` | RegisterAsync, LoginAsync, VerifyEmailAsync, UpdateKnownAsAsync |
| `ISessionService` | CreateSession, GetSession, RemoveSession |
| `ICheckInService` ★ | GetTodayAsync, CheckInAsync |
| `IQuackService` ★ | SendQuackAsync, GetUnseenAsync, MarkSeenAsync |
| `IFriendshipService` ★ | SendRequestAsync, GetPendingIncomingAsync, AcceptRequestAsync, DeclineRequestAsync, GetFriendsWithCheckInAsync, GetFriendDetailAsync |
| `IGratitudeService` | AddEntryAsync, GetAllPagedAsync, GetRandomAsync, GetStreakAsync |
| `IShopService` | GetActiveItemsAsync, PurchaseAsync, EquipAsync, GetInventoryAsync |
| `IEQTestService` | GetQuestionsAsync, SubmitAsync, GetBestScoreAsync |
| `ICooldownService` | GetPillarStatusAsync, CanStartNewLessonAsync |
| `ILessonService` | GetAllPillarProgressAsync, GetLessonsForPillarAsync, GetLessonContentAsync, StartLessonAsync, CompleteLessonAsync |

### DTOs (`DuckyEQ.Contracts/DTOs/`)
Namespace: `DuckyEQ.Contracts.DTOs` — all C# records, flow outward from services → controllers → API.

| DTO | Purpose |
|---|---|
| `UserProfileDto` | Username, KnownAs, DuckCharacter, OverallXP, OverallLevel, StreakDays, EmailVerified, EquippedItems |
| `EquippedItems` | Hat?, Accessory?, Glow?, Color? — all `ShopItemDto?` — used everywhere a duck is rendered |
| `ShopItemDto` | Id, Name, Category, CoinPrice, DuckyImageUrl?, DaisyImageUrl?, IsOwned, IsEquipped, Rarity?, Description? |
| `UserInventoryDto` | Id, ShopItemId, ItemName, Category, IsEquipped, PurchasedAt |
| `PillarProgressDto` | Pillar, Name, CurrentLevel, XP, IsUnlocked |
| `LessonWithProgressDto` | Id, Pillar, Level, Title, Objective, BestScore?, BestStars?, FirstCompletedAt?, IsUnlocked |
| `LessonContentDto` | Id, Pillar, Level, Title, ContentJson |
| `GratitudeEntryDto` | Id, Text, Category, CreatedAt |
| `GratitudeStreakDto` | CurrentStreak, LongestStreak |
| `EQTestQuestionDto` | Id, QuestionText, OptionA/B/C/D — CorrectOption intentionally excluded from DTO |
| `EQTestResultDto` | Score, Stars, CorrectAnswers, IsNewBest |
| `FriendshipDto` ★ | Id, RequesterId, AddresseeId, Status, CreatedAt |
| `CheckInDto` ★ | Id, UserId, CheckInDate, EmotionIds(List\<string\>), Phrase?, CreatedAt |
| `FriendWithCheckInDto` ★ | FriendshipId, UserId, KnownAs, Username, OverallLevel, DuckCharacter, EquippedItems, HasCheckedInToday, EmotionIds?, Phrase? |
| `FriendDetailDto` ★ | UserId, KnownAs, Username, OverallLevel, OverallXP, DuckCharacter, EquippedItems, HasCheckedInToday, TodayEmotionIds?, TodayPhrase?, FriendshipId |
| `FriendRequestDto` ★ | FriendshipId, RequesterId, RequesterKnownAs, RequesterUsername, RequesterDuckCharacter, RequesterEquippedItems, CreatedAt |
| `QuackDto` ★ | Id, SenderId, SenderKnownAs, SenderUsername, SenderDuckCharacter, RecipientId, QuackType, SentAt, SeenAt? |
| `UserSearchResultDto` ★ | UserId, Username, KnownAs, DuckCharacter, EquippedItems, ExistingRelationship(FriendshipStatus?) |

---

## Key Design Decisions

### Username System
- Format: `PascalCase AdjectiveNounXXXX` — e.g. `SunnyDuck4832`, `CozyDabbler9034`
- 30 adjectives × 30 nouns × 10,000 digits = 9M possible usernames
- Permanently assigned at registration — client never sends a username
- Unique DB index on `users.Username`, retry up to 15 times on collision
- `UsernameGenerator` registered as **Singleton**

### Known As (Display Name)
- Max 10 chars, min 2 chars — mutable from Profile screen
- Prompt: "What should your friends call you?"
- Included in JWT claims — client reads it without an API call
- `PUT /api/user/known-as` reissues JWT

### Daily Check-In Null Pattern
```
IDailyCheckInRepository.GetTodayAsync() returns DailyCheckIn?
null → user has not checked in today → controller returns 204 No Content
204 → React Native shows the check-in modal
```
Do NOT throw `NotFoundException` for null — 204 is the designed signal.

### Quack Rate Limit
- `IQuackRepository.HasSentTodayAsync(senderId, recipientId)` — one Quack per friend pair per day
- `true` → service throws `QuackLimitExceededException` → controller maps to 429 with `Retry-After: midnight UTC`
- Sender and recipient must be accepted friends (403 if not)

### Phrase Field — Null vs Empty String
- `DailyCheckIn.Phrase` is nullable — null means "user chose not to share"
- Empty string is INVALID — factory method converts to null
- Never store empty string

### Shop — Weekly Rotation
- Implemented via **date-range queries** — no background job needed for MVP
- `GetWeeklyItemsForCurrentWeekAsync` queries `WeeklyAvailableFrom` / `WeeklyAvailableTo`
- Admin adds items via SQL INSERT or admin API endpoint

### Equip Logic
- One active item per `ShopCategory` slot at a time
- `UnequipAllInCategoryAsync` + `EquipAsync` must run in a **transaction**

### EF Core DbContext Notes
- `Friendship`: `DeleteBehavior.Restrict` on both `RequesterId` FK and `AddresseeId` FK
- `Quack`: `DeleteBehavior.Restrict` on both `SenderId` FK and `RecipientId` FK
- Unique indexes: `User.Username`, `DailyCheckIn(UserId + CheckInDate)`, `Friendship(RequesterId + AddresseeId)`

---

## DI Registration Pattern (Program.cs)

```csharp
builder.Services.AddDbContext<DuckyEQDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMemoryCache();

// Repositories — all AddScoped
builder.Services.AddScoped<IUserRepository, SqlUserRepository>();
// ... repeat for all 13 repositories

// Services — all AddScoped except UsernameGenerator
builder.Services.AddSingleton<IUsernameGenerator, UsernameGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();
// ... repeat for all remaining services
```

---

## Tech Stack

### Backend
| Tool | Detail |
|---|---|
| ASP.NET Core Web API | C# — Visual Studio |
| Entity Framework Core | ORM + migrations |
| Azure SQL Database | Production database |
| Azure App Service | API hosting |
| Azure Communication Services | Email (verification) |
| Azure Blob Storage | Asset hosting |
| AspNetCoreRateLimit | Rate limiting |
| FluentValidation | Request validation |
| BCrypt | Password hashing |
| JWT | Auth tokens (KnownAs + username in claims) |

### Frontend
| Tool | Detail |
|---|---|
| React Native + TypeScript | Mobile app |
| Expo + EAS Build | Build and deploy |
| Cursor | Primary RN editor (AI co-pilot) |
| RevenueCat | Subscriptions (Phase 2) |

### Infrastructure
| Tool | Detail |
|---|---|
| GitHub | Two repos: `duckyeq-api` and `duckyeq-app` |
| Azure Portal | App Service, SQL, Blob, ACS Email |
| Postman | API testing — run full collection after each phase |

---

## MVP Screen Plan

Three primary screens + profile bar:

| Screen | Description |
|---|---|
| **Home** | Daily check-in modal, Quack banner, Friends Panel, profile bar |
| **Learn** | Duolingo-style board game. Duck moves node-by-node through EQ pillar "worlds" |
| **Shop** | Weekly rotating shop (6 items/week). Equip one item per category slot |

**Deferred to Phase 2:** Arena, Ponds (SignalR), full Profile screen, friend system push notifications

---

## Lesson Content

- **5 pillars × 10 levels × 5 lessons = 250 lessons total** (CDER model per lesson)
- Content is **universal** — no age-group filtering. Clear language works across all ages.
- Each lesson: Define → Connect → Engage (mini-game) → Reflect
- 4 mini-game types for Engage phase
- 60-second "Test Your EQ" mode with 15 random questions from the question pool

---

## 45-Day Phase Plan

| Phase | Days | Milestone |
|---|---|---|
| 1 — Backend Foundation | 1–7 | Enterprise backend, all domain models, interfaces, DI, EF migration |
| 2 — API + Content + Azure | 8–17 | All 40+ endpoints in Postman, 250 lessons seeded, API live on Azure |
| 3 — RN + Home Screen | 18–25 | Expo app on phone, onboarding, check-in modal, Home screen |
| 4 — Learn + Friends Screen | 26–33 | Friends screen, board game path, CDER lessons, Test Your EQ |
| 5 — Gratitude + Shop + Integration | 34–40 | Gratitude Garden, Shop, full integration test, coin economy |
| 6 — App Store Submission | 41–45 | Content QA, duckyeq.com live, privacy policy, EAS build, App Store Connect |

---

## Supporting Docs (`/docs/`)

Convert these from `/project/` docx files using `pandoc` and place in `/docs/`:

| File | Contents |
|---|---|
| `PostDay5Reference.md` | Complete file inventory, namespace map, Day 6 checklist |
| `Day5InterfacesDTOs.md` | All method signatures and design rationale for Contracts layer |
| `45DayPlan.md` | Full day-by-day build plan with patterns + learning takeaways |
| `SecurityReference.md` | Auth, rate limiting, BCrypt, JWT, FluentValidation patterns |
| `DuckShopSystem.md` | Shop item schema, weekly rotation, equip logic, coin economy |
| `FriendsSocialMVP.md` | Full social system spec: check-ins, friendships, Quacks |
| `LessonArchitecture.md` | CDER model, lesson JSON schema, board game progression |
| `EQTestQuestionPool.md` | All seeded EQ test questions by pillar |
| `ProjectCorrections.md` | ⚠️ Overrides — always check here first (Ducky/Daisy names, etc.) |

---

*DuckyEQ · duckyeq.com · Build the EQ. Grow the Garden. Connect with Friends. Ship the Duck. 🦆*
