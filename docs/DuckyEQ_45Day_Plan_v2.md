**🦆 DuckyEQ**

**45-Day MVP Build Plan**

Friends & Social Edition · April 2026

duckyeq.com · 1 Hour/Day · Enterprise Patterns · Full Stack · App Store

This is the complete updated DuckyEQ 45-day MVP build guide. It
incorporates all original features --- 250 CDER lessons, board game
progression, Test Your EQ, Gratitude Garden, Shop --- and adds the
Friends & Social system as a core MVP feature. Friends is a primary
differentiator: daily emotion check-ins, friend connections, and Quack
reactions create the social habit loop that drives daily return visits
without requiring real-time infrastructure in Phase 1.

Username format is now PascalCase AdjectiveNounXXXX (e.g. SunnyDuck4832,
CozyDabbler9034, FluffyPaddler5590) --- no underscores, no snake_case.
Users also have a Known As display name, max 10 characters, mutable,
shown alongside their username throughout the social UI.

  ------------- ---------- --------------------------------------------------
  **Phase**     **Days**   **Milestone / Outcome**

  1 --- Backend 1--7       Duck mascot commissioned. Enterprise backend with
  Foundation               ALL domain models including Friendship, Quack,
                           DailyCheckIn. Interfaces, services, behaviors, and
                           full DI registered. EF Core migration applied.

  2 --- API +   8--17      All 40+ endpoints working in Postman including
  Content +                social (CheckIn, Friends, Quacks). 250 lessons
  Azure                    seeded. API live on Azure. Two-user social Postman
                           flow passes.

  3 --- RN +    18--25     Expo app on phone. Onboarding with Known As +
  Home Screen              PascalCase reveal. Daily check-in modal. Home with
                           Quack banner + Friends Panel. Profile with Known
                           As editor.

  4 --- Learn + 26--33     Friends screen (3 tabs). Board game path with duck
  Friends                  on node. CDER lessons, 4 mini-games. Test Your EQ
  Screen                   60-second timer. Social integration verified.

  5 ---         34--40     Gratitude Garden, Pick Me Up, Shop. Full
  Gratitude +              integration test including social. Coin economy
  Shop +                   verified. Monitoring configured.
  Integration              

  6 --- App     41--45     Content QA. duckyeq.com live. Privacy policy with
  Store                    social data disclosures. 6 screenshots. EAS build
  Submission               succeeds. App Store Connect submitted.
  ------------- ---------- --------------------------------------------------

**Development Environment**

Visual Studio for C#/ASP.NET Core --- your home turf. Cursor for React
Native with inline AI suggestions. Claude for architecture decisions,
debugging, and seeding scripts. Postman for endpoint verification after
every phase.

  --------------- ------------------- -----------------------------
  **Tool**        **What It Is**      **Setup / Use**

  Visual Studio   C# / ASP.NET Core   Use exclusively for
                  backend             duckyeq-api.

  Cursor          AI editor for React cursor.so --- frontend only,
                  Native              inline suggestions.

  Node.js v20+    JavaScript runtime  nodejs.org LTS --- includes
                                      npm.

  Expo CLI + EAS  Build + App Store   npm install -g expo-cli
  CLI             deploy              eas-cli

  Expo Go         Test on real phone  App Store → scan QR from
  (iPhone)                            terminal.

  Xcode           iOS simulator +     Mac App Store → install
                  build tools         Command Line Tools.

  Postman         API testing --- 40+ postman.com --- run full
                  endpoints           collection after each phase.

  Git + GitHub    Version control     2 repos: duckyeq-api and
                                      duckyeq-app.

  Azure Portal    Cloud               portal.azure.com --- App
                  infrastructure      Service, SQL, Blob, ACS
                                      Email.

  Figma (free)    App icon +          figma.com
                  screenshot overlays 
  --------------- ------------------- -----------------------------

**Username Generation System**

Every user receives a permanently assigned, uniquely generated username
at signup. PascalCase: a duck-themed adjective followed by a duck-themed
noun, followed by exactly 4 random digits. No underscores, no
separators, no spaces.

**Format**

  ------------------ ----------------------------------------------
  **Format**         AdjectiveNounXXXX --- both words capitalize
                     their first letter, then 4 digits appended

  **Examples**       SunnyDuck4832 · BoldQuack7201 ·
                     CozyDabbler9034 · FluffyPaddler5590 ·
                     ZippyRipple1123

  **Characters**     Letters and digits only --- no underscores,
                     hyphens, or spaces

  **Uniqueness**     Unique DB index on users.Username --- retry up
                     to 15 times on collision

  **Mutability**     Permanent --- cannot be changed by the user at
                     any time

  **Combinations**   30 adjectives × 30 nouns × 10,000 digits =
                     9,000,000 possible usernames
  ------------------ ----------------------------------------------

**Word Pools**

Adjective pool (30, duck-themed, all ages): Sunny, Bold, Calm, Swift,
Brave, Plucky, Fuzzy, Pudgy, Cozy, Zippy, Sleek, Fluffy, Cheeky, Jolly,
Perky, Snappy, Dandy, Merry, Bouncy, Floaty, Dreamy, Bubbly, Gentle,
Lucky, Gleamy, Breezy, Misty, Dapper, Peppy, Waddly. Noun pool (30):
Duck, Quack, Waddle, Paddle, Splash, Feather, Beak, Diver, Glider,
Floater, Pebble, Ripple, Dabbler, Swimmer, Drifter, Raindrop, Napper,
Hopper, Skipper, Nestling, Duckling, Flapper, Crest, Brook, Wader,
Tufter, Puddle, Flier, Plover, Drifter.

**Generation Logic**

UsernameGenerator: IUsernameGenerator / UsernameGenerator, registered as
scoped service. Two private string arrays. Generate(): pick random
adjective (already capitalized in array), pick random noun (already
capitalized), generate 4-digit padded random string (0000--9999),
concatenate as \$\"{adj}{noun}{num:D4}\". Call
IUserRepository.IsUsernameTakenAsync(candidate). Retry up to 15 times.
Username is generated server-side at registration --- the client never
sends a username field.

**Known As --- Mutable Display Name**

  ------------------ ----------------------------------------------
  **Prompt           \"What should your friends call you?\" ---
  (onboarding)**     step 2 of onboarding, after duck selection

  **Max length**     10 characters

  **Min length**     2 characters

  **Mutability**     Fully editable from Profile screen at any time
                     --- PUT /api/user/known-as reissues JWT

  **JWT claim**      KnownAs included in JWT claims --- client
                     reads it without an API call on every launch

  **DB column**      users.KnownAs (nvarchar 10, NOT NULL)

  **Display**        Shown alongside \@username on every friend
                     card and in the Friends screen
  ------------------ ----------------------------------------------

**Friends & Social System**

The Friends & Social system is a core MVP differentiator --- not a Phase
2 add-on. It creates a social habit loop without requiring real-time
infrastructure. Full technical detail is in
DuckyEQ_Friends_Social_MVP.docx in project context.

**Daily Emotion Check-In**

- Full-screen modal shown on FIRST app open each calendar day --- cannot
  be dismissed by tapping outside

- Six emotions as duck expression photo cards (multi-select): Happy,
  Sad, Angry, Anxious, Meh, Excited

- Step 2 --- reason chips: School, Work, Family, Friends, Health,
  Weather, Just Because, Don\'t really want to talk about it

- Optional free-text phrase: max 120 chars, nullable --- null handled
  gracefully throughout the UI

- One per day: unique constraint (UserId, CheckInDate) + client-side
  AsyncStorage gate

**Friends**

- Search by username (min 3 chars) → send request → accept/decline flow

- Friends screen (3 tabs): Friends List, Add Friends, Pending Requests
  (with badge count on pending tab)

- Friend Detail: Known As, \@username, level, today\'s emotions +
  phrase, Send a Quack CTA

- Friends icon in Home screen header (not a tab) --- navigates to
  Friends modal stack

**Quack Reactions**

- 5 types: 🤗 Hug · 😄 Smile · ✋ High Five · 💛 Thinking of You · 🎉
  Cheer

- One Quack per sender-recipient pair per calendar day --- 429 if rate
  limit hit

- Incoming Quacks shown as animated banner on Home screen on next app
  open (useFocusEffect)

- SeenAt tracked --- banner clears after all Quacks marked seen via
  PATCH /api/quacks/{id}/seen

**Home Screen Social Panels**

- Quack Banner: animated duck bounce at top of Home when unseen Quacks
  exist --- tapping marks seen + opens Friends screen

- Friends Feeling Panel: horizontal ScrollView --- Known As +
  \@username + emotion emoji(s) + phrase snippet

- Ghost cards for friends who haven\'t checked in today: \'Hasn\'t
  checked in yet\'

- Empty state when no friends yet: \'Add friends to see how they\'re
  feeling 🐥\'

**Complete Domain Model Map**

All entities for the full MVP including social. ★ = new this version. †
= updated.

  -------------------- -----------------------------------------------
  **Entity**           **Fields (★ = new this version / † = updated)**

  User †               Id (Guid), Email, PasswordHash, Username
                       (PascalCase auto-generated, unique, immutable),
                       KnownAs (string max 10, mutable) ★,
                       DuckCharacter (enum), OverallXP, OverallLevel,
                       StreakDays, LastActiveDate, CreatedAt,
                       EmailVerified (bool)

  PillarProgress       Id, UserId (FK), PillarId (enum), CurrentLevel,
                       XP, LastNewLessonCompletedAt

  UserLessonProgress   Id, UserId (FK), LessonId (FK), BestScore
                       (0--300), BestStars (1--3), TotalAttempts,
                       FirstCompletedAt (nullable), LastAttemptedAt.
                       Unique index (UserId, LessonId).

  LessonSession        In-memory only (IMemoryCache). SessionToken
                       (Guid), UserId, LessonId, StartedAt. 30-min
                       TTL. Not persisted to DB.

  Lesson               Id, PillarId (enum), LessonNumber (1--50),
                       Title, Objective (sentence), ContentJson (full
                       CDER JSON string)

  EQTestQuestion       Id, QuestionText, OptionA--D, CorrectOption
                       (enum A/B/C/D), Explanation, PillarId
                       (nullable)

  UserEQTestResult     Id, UserId (FK), Score (int), Stars (0--3),
                       CorrectAnswers (int), AttemptedAt

  QuackCoins           Id, UserId (FK), Balance (int), TotalEarned
                       (int), LastEarnedAt

  ShopItem             Id, Name, Category (enum), CoinPrice, ImageUrl,
                       IsDefault, IsWeeklyItem, WeekNumber, IsActive

  UserInventory        Id, UserId (FK), ShopItemId (FK), IsEquipped
                       (bool), PurchasedAt

  GratitudeEntry       Id, UserId (FK), Text, Category (enum),
                       CreatedAt

  DailyCheckIn ★       Id, UserId (FK), CheckInDate (DATE), EmotionIds
                       (JSON array e.g. \[\"Happy\",\"Anxious\"\]),
                       Phrase (nvarchar 120, nullable), CreatedAt.
                       Unique index (UserId, CheckInDate).

  Friendship ★         Id, RequesterId (FK→User), AddresseeId
                       (FK→User), Status (enum:
                       Pending/Accepted/Declined), CreatedAt,
                       UpdatedAt. Unique index (RequesterId,
                       AddresseeId). DeleteBehavior.Restrict on both
                       FKs.

  Quack ★              Id, SenderId (FK→User), RecipientId (FK→User),
                       QuackType (enum:
                       Hug/Smile/HighFive/ThinkingOfYou/Cheer),
                       SentAt, SeenAt (nullable). Service-layer rate
                       limit: one quack per (SenderId, RecipientId)
                       per calendar day → 429 if exceeded.
  -------------------- -----------------------------------------------

New enums: Emotion (Happy, Sad, Angry, Anxious, Meh, Excited) ·
QuackType (Hug, Smile, HighFive, ThinkingOfYou, Cheer) ·
FriendshipStatus (Pending, Accepted, Declined). All existing enums
unchanged: Pillar, DuckCharacter, ShopCategory, GratitudeCategory,
LessonEngageFormat, EQTestOption.

**Complete API Endpoint Map**

All endpoints for the full MVP. All follow Controller → Behavior →
Service → Repository. ★ = new social endpoints.

+-------------------+------------------------------------+----------------------+---------------------------+
| **Method**        | **Endpoint**                       | **Purpose**          | **Returns**               |
+-------------------+------------------------------------+----------------------+---------------------------+
| **Authentication & User**                                                                                 |
+-------------------+------------------------------------+----------------------+---------------------------+
| POST              | /api/auth/register                 | Create account +     | 201 + JWT + username +    |
|                   |                                    | PascalCase           | knownAs                   |
|                   |                                    | username + Known As  |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| POST              | /api/auth/login                    | Login                | 200 + JWT                 |
+-------------------+------------------------------------+----------------------+---------------------------+
| POST              | /api/auth/verify-email             | Email verification   | 200                       |
|                   |                                    | via deep link        |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| GET               | /api/user/profile                  | Current user profile | 200 + UserProfileDto      |
|                   |                                    | with equipped items  |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| PUT               | /api/user/duck-character           | Switch active duck   | 200                       |
|                   |                                    | (Quack/Lola)         |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| PUT ★             | /api/user/known-as                 | Update Known As (max | 200 + new JWT             |
|                   |                                    | 10 chars) ---        |                           |
|                   |                                    | reissues JWT         |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| **Pillars, Lessons & Coins**                                                                              |
+-------------------+------------------------------------+----------------------+---------------------------+
| GET               | /api/pillars/progress              | All 5 pillar         | 200 +                     |
|                   |                                    | progress summaries   | PillarProgressDto\[\]     |
+-------------------+------------------------------------+----------------------+---------------------------+
| GET               | /api/pillars/{id}/lessons          | 50 lessons +         | 200 +                     |
|                   |                                    | per-lesson progress  | LessonWithProgressDto\[\] |
|                   |                                    | (board game data)    |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| GET               | /api/lessons/{pillarId}/{level}    | Get single CDER      | 200 + LessonContentDto    |
|                   |                                    | lesson content       |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| POST              | /api/lessons/{id}/start            | Begin lesson, get    | 200 + { sessionToken,     |
|                   |                                    | session token        | expiresAt }               |
+-------------------+------------------------------------+----------------------+---------------------------+
| POST              | /api/lessons/{id}/complete         | Submit score, update | 200 +                     |
|                   |                                    | stars, award coins   | LessonCompleteResult      |
+-------------------+------------------------------------+----------------------+---------------------------+
| GET               | /api/coins/balance                 | Current coin balance | 200 + { balance }         |
+-------------------+------------------------------------+----------------------+---------------------------+
| **Test Your EQ**                                                                                          |
+-------------------+------------------------------------+----------------------+---------------------------+
| GET               | /api/eq-test/questions             | Random shuffled EQ   | 200 + EQTestQuestion\[\]  |
|                   |                                    | question pool        |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| POST              | /api/eq-test/submit                | Submit test score    | 201 + { score, stars,     |
|                   |                                    | (correctAnswers ×    | isNewBest }               |
|                   |                                    | 10)                  |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| GET               | /api/eq-test/best-score            | User\'s all-time     | 200 + { score, stars }    |
|                   |                                    | best EQ score        |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| **Shop & Gratitude**                                                                                      |
+-------------------+------------------------------------+----------------------+---------------------------+
| GET               | /api/shop/items                    | Active weekly shop   | 200 + ShopItemDto\[\]     |
|                   |                                    | items by category    |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| POST              | /api/shop/purchase                 | Buy a shop item      | 200 + PurchaseResponse    |
+-------------------+------------------------------------+----------------------+---------------------------+
| POST              | /api/shop/equip                    | Equip owned item     | 200                       |
|                   |                                    | (unequips same       |                           |
|                   |                                    | category)            |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| GET               | /api/shop/inventory                | All owned items with | 200 +                     |
|                   |                                    | equip status         | UserInventoryDto\[\]      |
+-------------------+------------------------------------+----------------------+---------------------------+
| POST              | /api/gratitude                     | Add gratitude entry  | 201 + GratitudeResponse   |
+-------------------+------------------------------------+----------------------+---------------------------+
| GET               | /api/gratitude                     | All entries          | 200 + GratitudeEntry\[\]  |
|                   |                                    | (paginated)          |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| GET               | /api/gratitude/today               | Today\'s entries     | 200 + GratitudeEntry\[\]  |
+-------------------+------------------------------------+----------------------+---------------------------+
| GET               | /api/gratitude/random              | Random entry for     | 200 + GratitudeEntry      |
|                   |                                    | Pick Me Up           |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| GET               | /api/gratitude/streak              | Streak count +       | 200 + { streak, longest } |
|                   |                                    | longest streak       |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| **★ Social --- Daily Check-In, Friends & Quacks (New in this version)**                                   |
+-------------------+------------------------------------+----------------------+---------------------------+
| GET ★             | /api/checkin/today                 | Check if checked in  | 200 + CheckInDto or 204   |
|                   |                                    | today --- 204 = not  |                           |
|                   |                                    | yet                  |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| POST ★            | /api/checkin                       | Submit emotions      | 201 + CheckInDto          |
|                   |                                    | array + optional     |                           |
|                   |                                    | phrase (max 120      |                           |
|                   |                                    | chars)               |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| GET ★             | /api/users/search?username=x       | Find user by         | 200 + UserSearchResultDto |
|                   |                                    | username prefix (min |                           |
|                   |                                    | 3 chars)             |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| POST ★            | /api/friends/request               | Send a friend        | 201 + FriendshipDto       |
|                   |                                    | request              |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| GET ★             | /api/friends/requests/pending      | Incoming pending     | 200 +                     |
|                   |                                    | friend requests      | FriendRequestDto\[\]      |
+-------------------+------------------------------------+----------------------+---------------------------+
| PUT ★             | /api/friends/requests/{id}/accept  | Accept a friend      | 200 + FriendshipDto       |
|                   |                                    | request              |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| PUT ★             | /api/friends/requests/{id}/decline | Decline a friend     | 200                       |
|                   |                                    | request              |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| GET ★             | /api/friends                       | Friends list +       | 200 +                     |
|                   |                                    | today\'s check-in    | FriendWithCheckInDto\[\]  |
|                   |                                    | (JOIN query)         |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| GET ★             | /api/friends/{id}                  | Friend detail ---    | 200 + FriendDetailDto     |
|                   |                                    | level, achievements, |                           |
|                   |                                    | feeling              |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| POST ★            | /api/quacks                        | Send a Quack (1 per  | 201 + QuackDto or 429     |
|                   |                                    | sender-recipient per |                           |
|                   |                                    | day)                 |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| GET ★             | /api/quacks/unseen                 | Unseen Quacks for    | 200 + QuackDto\[\]        |
|                   |                                    | Home banner (last 48 |                           |
|                   |                                    | hrs)                 |                           |
+-------------------+------------------------------------+----------------------+---------------------------+
| PATCH ★           | /api/quacks/{id}/seen              | Mark Quack as seen   | 200                       |
|                   |                                    | (SeenAt = UtcNow)    |                           |
+-------------------+------------------------------------+----------------------+---------------------------+

**Project Structure**

**Backend (Visual Studio --- duckyeq-api)**

- DuckyEQ.Api/ --- Controllers: Auth, User, Pillar, Lesson, Coin, Shop,
  Gratitude, EQTest, CheckIn, Friends, Quacks

- DuckyEQ.Domain/ --- Entities (User, DailyCheckIn, Friendship, Quack,
  ...) + Enums (including Emotion, QuackType, FriendshipStatus)

- DuckyEQ.Contracts/ --- Interfaces/Repositories/ +
  Interfaces/Services/ + DTOs (FriendWithCheckInDto, QuackDto,
  CheckInDto, UserSearchResultDto, FriendDetailDto)

- DuckyEQ.Infrastructure/ --- DuckyEQDbContext (with Restrict cascade on
  Friendship FKs) + all repositories + migrations + seed data

- DuckyEQ.Services/ --- Behaviors: Auth, Lesson, Friends, Quacks,
  CheckIn. Services: UsernameGenerator, FriendshipService, QuackService,
  CheckInService, ScoringService, CoinService

**Frontend (Cursor / VS Code --- duckyeq-app)**

- /src/features/home/ --- HomeScreen.tsx, ProfileBar, QuackBanner,
  FriendsPanel, CheckInModal (emotion grid + reason chips + phrase
  input)

- /src/features/friends/ --- FriendsScreen.tsx, FriendsList, AddFriends,
  PendingRequests, FriendDetail, SendQuackModal (bottom sheet)

- /src/features/learn/ --- LearnScreen.tsx, PillarCards, BoardGamePath,
  LessonPreviewModal, LessonPlayer, CDERFlow

<!-- -->

- /learn/eq-test/ --- EQTestScreen.tsx, TimerBar, CooldownOverlay,
  EQTestReward

- /learn/games/ --- MultipleChoiceGame, TrueFalseGame, ScenarioGame,
  FlashcardFlipGame

<!-- -->

- /src/features/{gratitude,shop,profile}/ --- standard feature folders

- /src/services/ --- apiClient.ts, authService.ts, friendsService.ts,
  checkInService.ts, quackService.ts

- /src/navigation/ --- TabNavigator.tsx, OnboardingStack.tsx,
  FriendsStack.tsx (modal), ProfileStack.tsx (modal)

- /src/context/ --- InventoryContext.tsx (shared equip state across
  Shop, Profile, Home, Learn)

- /src/constants/ --- EMOTIONS.ts, CHECKIN_CHIPS.ts, QUACK_TYPES.ts
  (static --- never fetched from DB)

**MVP Scope Boundaries**

  -------------------------------- --------------------------------
  **✅ MVP --- Ships in 45 Days**  **⏳ Phase 2 --- After Launch**

  Daily emotion check-in (6        Push notifications (Quacks,
  emotions, multi-select)          streaks, shop refresh)

  Quick-tap reason chips +         Real-time Quack delivery via
  120-char phrase (nullable)       Azure SignalR

  PascalCase username              Friend activity feed (level-ups,
  (AdjectiveNounXXXX, immutable)   completions)

  Known As display name (10 chars, Blocking / reporting users
  mutable)                         

  Send / accept / decline friend   Friend leaderboards
  requests                         

  Friends screen (3 tabs: list,    Group features or messaging
  add, pending)                    

  Friends Feeling Panel on Home    Arena --- async EQ battles with
  screen                           AI opponents

  Quack reactions (5 types)        Ponds --- real-time duck social
                                   presence

  Quack banner on Home screen      Full Profile screen with mood
                                   history chart

  Friend Detail (level,            Admin content API for lesson
  achievements, today\'s feeling)  management

  250 CDER lessons + board game    B2B school/district licensing +
  path                             RevenueCat

  Test Your EQ 60-second challenge 

  Gratitude Garden + Pick Me Up    

  Shop with weekly rotating items  
  -------------------------------- --------------------------------

**PHASE 1 Character Design + Backend Foundation**

**Goal:** Commission duck mascot on Fiverr. Scaffold enterprise
solution. Build ALL domain models including social entities
(DailyCheckIn, Friendship, Quack). Define every interface. Register all
DI. Social system designed from Day 1.

**Milestone:** *Character commissioned. Backend compiles with all
entities --- social included --- all repositories, all interfaces
registered. EF Core migration created and applied.*

**Day 1** (Mon) **Commission duck mascot + social media accounts**

**Build**

Go to fiverr.com and search \"3D character design mascot\" or \"cute
mascot illustration.\" Find a designer whose style is cute, friendly,
and Pixar-adjacent. Send the commission brief: two duck characters
(Quack --- boy, Lola --- girl), ten expression sheets per character
(happy, curious, thoughtful, excited, calm, playful, proud, sympathetic,
encouraging, surprised), 10--15 pose sheets per character, Blender/PSD
source files, 5--8 sample accessories, transparent PNG at minimum
2048×2048, 7-day delivery. While waiting: create social media accounts
--- \@duckyeq on TikTok, Instagram, YouTube, and X. Bio: \"Build your
EQ. Grow your gratitude. Connect with friends. 🦆 Coming soon to the App
Store.\" Note: 6 of the 10 duck expressions map directly to the 6
check-in emotion cards the user taps each morning.

**Patterns Practiced**

Brand asset planning, freelancer brief writing, platform handle
reservation, expression-to-UI mapping
(happy/sad/angry/anxious/calm/excited expressions are the actual photo
cards in the check-in modal)

**Learning Takeaway**

*Why you commission art on Day 1 --- the check-in modal uses duck
expression photos as tappable cards. The board game path has the duck
standing on the current node. The CDER lesson flow uses all 10
expressions. Build screens first and you redesign everything when assets
arrive in Week 3. Commission on Day 1, build around the character from
Phase 3 onward.*

**Day 2** (Tue) **Character feedback + content calendar planning**

**Build**

Check Fiverr messages --- provide feedback emphasizing warmth and
expressiveness. Confirm all ten expressions by name; they are
non-negotiable because the check-in modal requires the duck\'s
expression photos as the actual UI elements users tap (happy for Happy,
sad for Sad, angry for Angry, calm for Meh, anxious for Anxious, excited
for Excited). Plan your first 10 social media posts using the duck
holding-a-sign pose with EQ tip overlays. Sketch the app icon concept:
duck face (front view, happy expression) centered on a gold-to-orange
gradient, 1024×1024.

**Patterns Practiced**

Creative direction and feedback iteration, expression-to-UI mapping
confirmation, content strategy planning, brand identity consistency
across platforms

**Learning Takeaway**

*Why confirming expressions before Week 3 matters --- the check-in
emotion cards are photos, not emoji. The card layout is determined by
the image dimensions. Inconsistent framing or aspect ratios break the
3×2 emotion grid. Confirming with the designer now means images arrive
ready to drop straight into the modal layout.*

**Day 3** (Wed) **Backend solution scaffold + GitHub repos**

**Build**

Open Visual Studio. Create ASP.NET Core Web API solution named DuckyEQ
with five projects: DuckyEQ.Api (startup), DuckyEQ.Domain (entities +
enums), DuckyEQ.Contracts (interfaces + DTOs --- no dependencies),
DuckyEQ.Infrastructure (DbContext + repositories + migrations),
DuckyEQ.Services (behaviors + services + utilities). Set references: Api
→ Contracts + Services. Services → Contracts + Domain. Infrastructure →
Domain + Contracts. Api → Infrastructure only in Program.cs. Create
folder structure: Contracts gets /Interfaces/Repositories/ and
/Interfaces/Services/ as separate subfolders --- with 12+ repository
interfaces and 10+ service interfaces, a flat /Interfaces/ folder
becomes hard to navigate. Initialize GitHub repos: duckyeq-api and
duckyeq-app. Push scaffold with .NET .gitignore.

**Patterns Practiced**

Solution architecture, compile-time dependency direction (Api never
references Domain directly), project reference rules, Dependency
Inversion Principle, separate Interfaces/Repositories and
Interfaces/Services subfolders

**Learning Takeaway**

*Why enterprise codebases split into multiple projects --- each compiles
independently and enforces dependency boundaries at compile time. If you
accidentally call a repository directly from a controller, the compiler
rejects it because the project reference does not exist. This is the
same architecture you practiced on the Intune Compliance team and it
scales from MVP to enterprise without refactoring.*

**Day 4** (Thu) **All domain models including social entities**

**Build**

In DuckyEQ.Domain/Entities/, build every entity with private setters and
factory methods. User: Id (Guid), Email, PasswordHash, Username
(immutable, auto-generated PascalCase), KnownAs (string max 10,
mutable), DuckCharacter (enum), OverallXP, OverallLevel, StreakDays,
LastActiveDate, CreatedAt, EmailVerified. Factory: User.Create(email,
passwordHash, username, knownAs, duckCharacter). Social entities:
DailyCheckIn.Create(userId, checkinDate, emotionIds, phrase) --- phrase
nullable, empty string rejected at factory and converted to null.
Friendship.Create(requesterId, addresseeId) --- Status defaults to
Pending. Quack.Create(senderId, recipientId, quackType). All other
entities: PillarProgress, UserLessonProgress, Lesson, EQTestQuestion,
UserEQTestResult, QuackCoins, ShopItem, UserInventory, GratitudeEntry.
New enums: Emotion (Happy, Sad, Angry, Anxious, Meh, Excited), QuackType
(Hug, Smile, HighFive, ThinkingOfYou, Cheer), FriendshipStatus (Pending,
Accepted, Declined).

**Patterns Practiced**

Encapsulation with private setters, factory methods, guard clauses,
nullable vs empty string (phrase: null = intentionally not shared, empty
string = invalid --- factory rejects and nullifies), enums for all fixed
value sets

**Learning Takeaway**

*Why phrase is nullable and empty string is rejected --- null means the
user chose not to share. An empty string is ambiguous: did they skip, or
submit nothing by accident? Storing empty strings creates data you
cannot interpret. Null is semantically clear: intentionally absent. The
factory enforces this invariant at the domain boundary so no empty
strings ever reach the database.*

**Day 5** (Fri) **All interfaces + DTOs + request/response models**

**Build**

In DuckyEQ.Contracts/Interfaces/Repositories/: IUserRepository
(GetByIdAsync, GetByEmailAsync, GetByUsernameAsync,
IsUsernameTakenAsync, CreateAsync, UpdateAsync, UpdateKnownAsAsync),
IPillarProgressRepository, IUserLessonProgressRepository,
ILessonRepository, IEQTestQuestionRepository,
IUserEQTestResultRepository, ICoinRepository, IShopItemRepository,
IUserInventoryRepository, IGratitudeRepository. Social:
IDailyCheckInRepository (GetTodayAsync, CreateAsync),
IFriendshipRepository (CreateAsync, GetFriendsWithCheckInAsync,
GetPendingIncomingAsync, GetByIdAsync, UpdateStatusAsync,
GetBetweenUsersAsync, GetFriendDetailAsync), IQuackRepository
(CreateAsync, GetUnseenByRecipientAsync, MarkSeenAsync,
HasSentTodayAsync). Service interfaces: IAuthService,
IUsernameGenerator, IScoringService, IEQTestScoringService,
ISessionService, ICooldownService, ICheckInService, IFriendshipService,
IQuackService, ICoinService, IShopService, IGratitudeService,
ILessonService, IEQTestService. DTOs: UserProfileDto (knownAs +
username + equippedItems), FriendWithCheckInDto (bundles friend
profile + today\'s emotions in one object), FriendDetailDto,
FriendRequestDto, QuackDto, CheckInDto, UserSearchResultDto.

**Patterns Practiced**

Interface segregation, repository pattern, DTO design for social
features, async Task everywhere, nullable return types (GetTodayAsync
returns DailyCheckIn? --- null if not checked in yet)

**Learning Takeaway**

*Why FriendWithCheckInDto bundles friend profile and today\'s check-in
in a single DTO --- the Home screen Friends Panel needs both: Known As,
username, and today\'s emotions for each friend card. Fetching
separately means N+1 API calls for N friends. The repository JOIN
returns everything in one query. The DTO shape mirrors exactly what the
UI needs --- one request, one render.*

**Day 6** (Sat) **EF Core DbContext + all repositories + DI
registration**

**Build**

Create DuckyEQDbContext with DbSets for all entities. Fluent API:
configure Friendship\'s two FKs to User explicitly --- use
DeleteBehavior.Restrict on both to avoid cascade delete cycle errors
(\'Introducing FOREIGN KEY constraint may cause cycles or multiple
cascade paths\'). Same for Quack. Unique indexes: users.Username,
(UserId, CheckInDate) on daily_checkins, (RequesterId, AddresseeId) on
friendships. Run Add-Migration InitialCreate and Update-Database.
Implement all repository classes.
SqlFriendshipRepository.GetFriendsWithCheckInAsync: JOIN friendships +
users + daily_checkins WHERE CheckInDate = TODAY using a LEFT JOIN
(returns null check-in data if friend hasn\'t checked in).
SqlQuackRepository.HasSentTodayAsync: DATE(SentAt) = DATE(GETUTCDATE())
comparison. Register all in Program.cs: AddDbContext, AddScoped for all
12+ repo and service pairs, AddMemoryCache,
AddSingleton\<IUsernameGenerator\>.

**Patterns Practiced**

EF Core Fluent API, bidirectional FK with explicit navigation property
configuration, DeleteBehavior.Restrict for self-referential
relationships, LEFT JOIN for optional check-in data, date-only
comparison in SQL

**Learning Takeaway**

*Why Friendship uses DeleteBehavior.Restrict --- SQL Server raises an
error when two cascade paths lead to the same table. Friendship has two
FKs to User (Requester + Addressee), so cascading a User delete would
try to cascade through both paths simultaneously. Restrict means user
deletion doesn\'t auto-cascade --- you handle friendship cleanup
explicitly. The compiler would not catch this; the migration would apply
but the first cascade delete would fail at runtime.*

**Day 7** (Sun) **Service implementations + behavior layer + Week 1
review**

**Build**

Implement all services. UsernameGenerator: Adjectives array (30
duck-themed, capitalized: Sunny, Bold, Calm, Swift, Brave, Plucky,
Fuzzy, Pudgy, Cozy, Zippy, Sleek, Fluffy, Cheeky, Jolly, Perky, Snappy,
Dandy, Merry, Bouncy, Floaty, Dreamy, Bubbly, Gentle, Lucky, Gleamy,
Breezy, Misty, Dapper, Peppy, Waddly). Nouns array (30 duck-themed,
capitalized: Duck, Quack, Waddle, Paddle, Splash, Feather, Beak, Diver,
Glider, Floater, Pebble, Ripple, Dabbler, Swimmer, Drifter, Raindrop,
Napper, Hopper, Skipper, Nestling, Duckling, Flapper, Crest, Brook,
Wader, Tufter, Puddle, Flier, Plover, Dabbler). Generate: pick random
adj + noun (both already capitalized), generate 4-digit padded random
(0000--9999), concatenate. Retry up to 15 times. AuthService: BCrypt,
JWT with UserId + Username + KnownAs in claims. CheckInService,
FriendshipService (self-request guard, duplicate guard), QuackService
(friendship validation + HasSentTodayAsync rate check → 429 exception).
Behaviors: AuthBehavior (Auth + UsernameGenerator + 50 starter coins),
FriendsBehavior, QuacksBehavior, CheckInBehavior. ScoringService,
EQTestScoringService, SessionService, CoinService. Push to GitHub with
\'phase-1-complete\' tag.

**Patterns Practiced**

Single-responsibility services, behavior orchestration, BCrypt, JWT with
social claims, PascalCase username generation (both words
pre-capitalized in arrays), friendship validation guards, QuackService
429 pattern

**Learning Takeaway**

*Why KnownAs is in the JWT claims alongside UserId and Username --- the
Profile Bar, the Friends screen, and the Known As editor all display
KnownAs instantly without an API call. If it were not in the token,
every cold launch would need GET /user/profile before the user sees
their own name. JWT claims are for display identity. On KnownAs update,
a new JWT is issued and replaces the old one --- one atomic change,
everywhere correct.*

**PHASE 2 Full API + Content Seeding + Azure Deployment**

**Goal:** All controllers including social (CheckIn, Friends, Quacks)
working in Postman. 250 lessons seeded. EQ test pool and shop seeded.
API live on Azure at a public URL.

**Milestone:** *All 40+ endpoints return correct responses against live
Azure URL. Two-user social Postman flow passes. 250 real lessons in the
database.*

**Day 8** (Mon) **Auth + User controllers**

**Build**

Build AuthController: POST /api/auth/register (accepts email, password,
duckCharacter, knownAs --- FluentValidation: knownAs 2--10 chars,
letters/spaces/numbers only --- generates PascalCase username via
UsernameGenerator --- returns 201 with JWT containing UserId +
Username + KnownAs in claims, plus username and knownAs in body). POST
/api/auth/login. POST /api/auth/verify-email. Build UserController: GET
/api/user/profile (UserProfileDto: username, knownAs, duckCharacter, XP,
level, streakDays, equippedItems), PUT /api/user/duck-character, PUT
/api/user/known-as (validates 2--10 chars → saves → reissues JWT with
updated KnownAs → returns new JWT). Test in Postman: register → verify
response includes PascalCase username like SunnyDuck4832. Decode JWT at
jwt.io and confirm KnownAs is in the claims payload. Test known-as
update → verify new JWT has updated KnownAs claim.

**Patterns Practiced**

JWT bearer config, FluentValidation on KnownAs, JWT reissue on KnownAs
update (atomic: DB save + new token in one response), 201 vs 200,
CreatedAtAction for POST

**Learning Takeaway**

*Why PUT /api/user/known-as reissues a JWT --- the JWT carries KnownAs
so the app displays it without API calls. Updating the DB but returning
the old token means every screen shows the old name until the token
expires. Reissuing atomically keeps the token and database in sync. The
client stores the new JWT, and all screens update immediately on next
render.*

**Day 9** (Tue) **Pillar + Lesson + Coin controllers**

**Build**

Build PillarController: GET /api/pillars/progress (auto-creates all 5
PillarProgress rows on first call if missing), GET
/api/pillars/{id}/lessons (returns all 50 LessonWithProgressDto ---
title, objective, lessonNumber, bestScore, bestStars, firstCompletedAt
--- this is the board game path dataset). Build LessonController: GET
/api/lessons/{pillarId}/{level}, POST /api/lessons/{id}/start
(IMemoryCache session token, 30-min TTL), POST
/api/lessons/{id}/complete (validate token, score = round(correct/total
× 300), stars: ≤100=1, ≤200=2, else 3, UpsertAsync UserLessonProgress,
award coins on first completion or new best). Build CoinController: GET
/api/coins/balance. Test: GET pillar lessons → 50 with null progress →
POST start → POST complete {sessionToken, correctAnswers:8,
totalQuestions:10} → score=240, stars=3 → GET pillar lessons → lesson 1
shows 240/3 stars.

**Patterns Practiced**

IMemoryCache session lifecycle (create on start, validate and destroy on
complete), UpsertAsync pattern, scoring formula (8/10 × 300 = 240), star
threshold calculation, coin award on first completion vs new best

**Learning Takeaway**

*Why GET /api/pillars/{id}/lessons returns all 50 at once --- the board
game path renders 50 nodes simultaneously. Fetching one at a time means
50 API calls. All 50 LessonWithProgressDto is \~5KB, one round trip, one
DB query. Pay the serialization cost once, save 49 network calls.*

**Day 10** (Wed) **Shop + Gratitude + EQ Test controllers**

**Build**

Build ShopController: GET /api/shop/items, POST /api/shop/purchase
(validate coins sufficient + not already owned → deduct → add
UserInventory → auto-equip in same transaction), POST /api/shop/equip
(unequip same category, equip selected), GET /api/shop/inventory. Build
GratitudeController: all 5 endpoints. GET /api/gratitude/random weighted
toward entries older than 7 days for Pick Me Up mode. Build
EQTestController: GET /api/eq-test/questions?count=50, POST
/api/eq-test/submit ({correctAnswers} → score = correctAnswers × 10 →
stars: 0=0, 1--30=1, 31--60=2, 61+=3 → save → isNewBest detection), GET
/api/eq-test/best-score. Test all in Postman.

**Patterns Practiced**

Weekly shop filter (IsActive = true), category-based equip constraint
(one per category), weighted random SQL for older gratitude entries, EQ
test scoring (correctAnswers × 10), isNewBest via MAX query

**Learning Takeaway**

*Why EQ test doesn\'t validate totalQuestions --- the EQ test runs for
exactly 60 seconds. How many questions a user completes depends on speed
and wrong-answer cooldowns. The only meaningful validation is
correctAnswers \>= 0. Score is auditable: every correct answer = 10
points. Simple, verifiable, impossible to spoof in a way that produces
unreasonable scores.*

**Day 11** (Thu) **Social controllers: CheckIn + Friends + Quacks**

**Build**

Build CheckInController: GET /api/checkin/today (GetTodayAsync → 204 if
null, 200 + CheckInDto if found). POST /api/checkin ({emotionIds:
string\[\], phrase?: string} --- FluentValidation: emotionIds non-empty
array of valid Emotion enum values, phrase max 120 chars --- phrase null
if absent --- 409 Conflict if already checked in today). Build
FriendsController: GET /api/users/search?username=x (prefix LIKE search,
min 3 chars, filter self, include friendshipStatus relative to caller),
POST /api/friends/request ({addresseeId} --- guard: not self, not
already friends, no existing pending request), GET
/api/friends/requests/pending, PUT /api/friends/requests/{id}/accept,
PUT /api/friends/requests/{id}/decline, GET /api/friends (JOIN
friendships + users + daily_checkins LEFT JOIN for today\'s check-in →
FriendWithCheckInDto\[\]), GET /api/friends/{id}. Build
QuacksController: POST /api/quacks ({recipientId, quackType} ---
validate friendship → HasSentTodayAsync → 429 if rate limited → create),
GET /api/quacks/unseen (SeenAt IS NULL AND SentAt \>= 48 hours ago),
PATCH /api/quacks/{id}/seen (set SeenAt = UtcNow). Test with two Postman
accounts: full social flow end-to-end.

**Patterns Practiced**

204 vs 200 for check-in gate, 409 on duplicate check-in, 429 on quack
rate limit, bidirectional friendship query, LEFT JOIN for optional
check-in data (null fields if friend hasn\'t checked in), prefix search
with LIKE \'%username%\' or StartsWith

**Learning Takeaway**

*Why GET /api/friends uses a LEFT JOIN rather than two queries --- if
you fetch friendships then loop to fetch each friend\'s check-in
separately you make N+1 DB calls for N friends. A single LEFT JOIN on
(UserId = FriendId AND CheckInDate = TODAY) returns every friend\'s
profile and today\'s check-in in one query. Friends who haven\'t checked
in have null emotion fields --- handled gracefully in the DTO mapper.*

**Day 12** (Fri) **Seeding strategy + EQ test questions + shop items**

**Build**

Design SeedData.cs in DuckyEQ.Infrastructure/Seeding/. Static
BuildLesson() helper accepts (pillarId, lessonNumber, title, objective,
contentJson). ContentJson schema: quackOfTheDay (setup + punchline +
expressions), connect (5 questions), define (6--8 flashcards +
expressionArc), engage (gameType + questions), reflect (5 questions),
reward (expression + closingLine). Seed 50 EQ test questions across all
5 pillars (10 per pillar) --- each with four options, CorrectOption
enum, 1--2 sentence explanation. Seed shop items: 6 permanent base items
(2 hats, 1 color, 1 glow, 2 accessories) plus 6 Week 1 weekly items.
Apply via HasData() in OnModelCreating. Run Update-Database. Verify:
EQTestQuestions = 50 rows, ShopItems = 12 rows.

**Patterns Practiced**

HasData() seeding in EF Core, BuildLesson() builder helper, verbatim C#
strings (@\"\") for JSON, JSON schema validation before committing
(paste into jsonlint.com), seed data as code vs SQL scripts

**Learning Takeaway**

*Why seed data lives in C# rather than SQL INSERT scripts --- C# is
type-checked at compile time. Misspell a property name and the build
fails. SQL scripts fail silently at runtime. HasData() in EF Core means
migrations manage seed data automatically --- re-running migrations
without manually clearing tables.*

**Day 13** (Sat) **Seed Pillars 1 and 2 --- Self-Awareness +
Self-Management (100 lessons)**

**Build**

Open the project context documents:
DuckyEQ_SA_Lessons_1to25_Redesigned.docx,
DuckyEQ_SA_Lessons_26to50_Redesigned.docx,
DuckyEQ_P2_SelfMgmt_Lessons_1to25.docx,
DuckyEQ_P2_SelfMgmt_Lessons_26to50.docx. Use Claude in a separate chat
with the lesson doc attached to convert batches of 10--15 lessons at a
time into the ContentJson schema from Day 12. Review every output for
tone and accuracy before committing. Add all 100 Lesson objects to
SeedData.cs as verbatim C# strings. Run Update-Database. Verify: Lessons
table has 100 rows with PillarId 1 and 2.

**Patterns Practiced**

Batch content conversion with Claude, verbatim C# string escaping,
pillar ID mapping (SelfAwareness=1, SelfManagement=2), row count
verification via Azure Portal Query Editor

**Learning Takeaway**

*Why you use Claude to convert lesson docs to ContentJson --- each
lesson has 20--25 pieces of content. For 100 lessons that is
2,000--2,500 strings. Typing manually takes days and introduces
transcription errors. Claude with the lesson doc attached converts each
lesson in under a minute. Your job is reviewing output quality, not
producing it.*

**Day 14** (Sun) **Seed Pillars 3, 4, 5 --- Social Awareness +
Relationship Skills + Decision-Making (150 lessons)**

**Build**

Same process as Day 13. Source docs:
DuckyEQ_P3_SocialAwareness_Lessons_1to25.docx,
DuckyEQ_P3_SA_Lessons_26to50.docx,
DuckyEQ_P4_RelSkills_Lessons_1to25.docx,
DuckyEQ_P4_RelSkills_Lessons_26to50.docx,
DuckyEQ_P5_DecisionMaking_Lessons_1to25.docx,
DuckyEQ_P5_DecisionMaking_Lessons_26to50.docx. Convert in batches,
review cross-pillar tone consistency, commit to SeedData.cs. Run
Update-Database. Verify: 250 total rows, 50 per pillar, PillarIds 1--5.
Create /src/constants/CHECKIN_CHIPS.ts in the React Native project as a
static file (never fetched from DB): the 8 reason chips. By end of day,
the database has every lesson needed for MVP.

**Patterns Practiced**

Cross-pillar tone consistency review, database total row verification,
static constants file for UI data that never changes (chips are fixed UX
text, not user-generated content)

**Learning Takeaway**

*Why the reason chips are a static constants file and not a database
table --- they are fixed UX elements the app team controls, not content
that changes between deployments. A DB table for them adds a migration,
an API endpoint, a repository query, and a DTO for data that changes
approximately never. Static constants are the right tool. Databases are
for data that grows or changes.*

**Day 15** (Mon) **Deploy API to Azure + receive character assets**

**Build**

Go to portal.azure.com. Create Resource Group duckyeq-rg. Provision
Azure SQL Database (Basic tier, \~\$5/month, name duckyeq-db). Provision
Azure App Service (Basic tier, .NET 8, name duckyeq-api). Update
appsettings.Production.json with Azure SQL connection string and JWT
signing key. Run: dotnet ef database update \--connection
\"\<azure-connection-string\>\". Publish from Visual Studio: right-click
Api → Publish → Azure → App Service. Configure Application Settings in
Azure Portal for connection string and JWT key. Test all endpoints
against the live URL in Postman --- including two-user social flow. If
character assets arrived from Fiverr: download, organize into /quack/
and /lola/ folders, batch-resize to 1×/2×/3× for all 10 expressions,
upload originals to Azure Blob Storage.

**Patterns Practiced**

Azure resource provisioning, EF Core remote migration (dotnet ef with
\--connection flag), App Service publish from Visual Studio, Application
Settings for secrets (never in appsettings.json committed to Git),
character asset organization (10 expressions × 2 characters × 3
resolutions = 60 files)

**Learning Takeaway**

*Why you deploy before building the frontend --- Expo Go on your phone
cannot reach localhost on your development machine (different network
contexts). Deploying today means every frontend day in Phase 3--6 works
against a live API at a real URL. No port forwarding, no ngrok tunnels.
The API is the ground truth and it is live from Day 15 onward.*

**Day 16** (Tue) **Full API integration test --- two-user social flow**

**Build**

Create Postman collection \'DuckyEQ Full Flow v2\' against the live
Azure URL. Two environments: User A token, User B token. Run: (1)
Register User A --- verify PascalCase username SunnyDuck4832, knownAs
\'Diego\' in response and JWT claims. (2) Register User B --- knownAs
\'Lola\'. Social: (3) User A: GET /checkin/today → 204. (4) POST
/checkin {emotionIds:\[\"Happy\",\"Anxious\"\], phrase:\"Big day\"} →
201. (5) GET /checkin/today → 200 with data. (6) POST /checkin again →
409 Conflict. (7) GET /users/search?username=Bold → User B appears. (8)
POST /friends/request from A to B. (9) User B: GET
/friends/requests/pending → User A appears. (10) PUT
/friends/requests/{id}/accept. (11) User A: GET /friends → User B with
null check-in. (12) User B: POST /checkin {emotionIds:\[\"Excited\"\]}.
(13) User A: GET /friends → User B shows Excited. (14) POST /quacks
{recipientId:B, quackType:\"Hug\"} → 201. (15) POST /quacks again → 429.
(16) User B: GET /quacks/unseen → Hug quack. (17) PATCH
/quacks/{id}/seen. (18) GET /quacks/unseen → empty. Lesson flow:
(19--21) GET pillars → POST start → POST complete, score 240, stars 3.
Fix all bugs. Push \'phase-2-complete\'.

**Patterns Practiced**

Two-account Postman test (two separate environments with separate JWT
tokens), 409/429/204 status verification, social lifecycle
(request→accept→checkin→visible→quack→seen), test oracle (8/10 × 300 =
240 = 3 stars)

**Learning Takeaway**

*Why the social integration test requires two separate Postman
environments --- social interactions are asymmetric. User A sends to
User B and User B sees and responds. Testing with one account can only
verify half the flow. With separate A and B environments you verify the
full lifecycle and catch the most common social bugs: FK violations,
missing JOIN conditions, authorization logic that lets users see
non-friend data.*

**Day 17** (Wed) **Edge case coverage + Postman collection export + Week
2 review**

**Build**

Run targeted edge case tests against live Azure. (1) Search own username
--- must not appear. (2) Send friend request to someone who already sent
you one --- return existing pending request, not a duplicate. (3) Friend
request to existing friend --- 409. (4) GET /friends before any
check-ins --- phrase field returned as null, not omitted from DTO. (5)
Update Known As to exactly 10 chars --- 200 success. (6) Update Known As
to 11 chars --- 400 validation error. (7) Send quack to non-friend ---
403. (8) Accept already-accepted request --- idempotent 200. Fix all
edge cases. Export the full Postman collection as JSON and commit to
duckyeq-api/postman/ in GitHub. This collection is your regression suite
for every subsequent backend change.

**Patterns Practiced**

Edge case enumeration, self-filter in search, duplicate request
prevention, null field consistency (phrase always returned, never
omitted), KnownAs boundary validation, idempotent accept behavior, 403
for social authorization violations

**Learning Takeaway**

*Why the phrase field must always be returned (never omitted) even when
null --- a frontend that checks \'if (response.phrase)\' works the same
whether phrase is null or absent. But a frontend that uses optional
chaining \'response.phrase?.trim()\' behaves differently if phrase is
undefined vs null. Consistent DTO shapes prevent a class of frontend
bugs where a field that was sometimes absent causes a runtime crash.
Always return the field. Let it be null. Never omit it.*

**PHASE 3 React Native Foundation + Home Screen + Profile**

**Goal:** Expo project running on phone via Expo Go. Tab navigation,
Friends modal stack, Profile modal stack. API client with JWT.
Onboarding with duck selector, Known As input, and PascalCase username
reveal. Daily check-in modal. Home screen with Quack banner, Friends
Feeling Panel, and Profile Bar. Profile screen with Known As editor.

**Milestone:** *App runs on real phone. Onboarding creates a user in
Azure with a PascalCase username. Check-in modal appears on first daily
open. Home screen shows Quack banner and Friends Panel. Profile screen
lets users update Known As and equip items.*

**Day 18** (Thu) **Expo project + all dependencies + folder structure**

**Build**

Open Cursor. Run: npx create-expo-app duckyeq-app \--template
blank-typescript. Install: npx expo install react-native-screens
react-native-safe-area-context \@react-navigation/native
\@react-navigation/bottom-tabs \@react-navigation/native-stack
react-native-reanimated react-native-gesture-handler axios
\@react-native-async-storage/async-storage expo-linear-gradient
expo-constants. npm install jwt-decode. Create full folder structure:
/src/features/{home,friends,learn,gratitude,shop,profile}/,
/src/services/, /src/navigation/, /src/hooks/, /src/context/,
/src/types/, /src/constants/, /src/assets/characters/{quack,lola}/.
Create constants: EMOTIONS.ts (6 entries: id + label + duckExpression),
CHECKIN_CHIPS.ts (8 chips), QUACK_TYPES.ts (5 types with emoji + label).
If character assets ready, copy PNGs into character folders. Run npx
expo start, scan with Expo Go, verify template loads. Push to GitHub.

**Patterns Practiced**

Expo project creation, TypeScript template, jwt-decode for synchronous
JWT claims reading (username + knownAs without API call on app load),
constants files for static UI data (emotions, chips, quack types never
fetched from DB)

**Learning Takeaway**

*Why jwt-decode is installed on Day 1 of frontend --- the Profile Bar
needs username and KnownAs on every screen. If these were fetched via
GET /user/profile on every cold launch, you\'d have a loading flash for
the user\'s own name on every app open. jwt-decode reads the stored JWT
synchronously. No network, no async, no loading state for display
identity. The JWT is the local source of truth; jwt-decode is the tool
to read it.*

**Day 19** (Fri) **Tab navigation + Friends modal stack + Profile modal
stack**

**Build**

Build TabNavigator.tsx: createBottomTabNavigator() with 4 tabs (Home,
Learn, Gratitude, Shop). Tab bar: white background, active tint duck
gold (#F5A623), inactive gray (#9E9E9E). Build FriendsStack.tsx: a
modal-style stack navigator for the Friends screen --- triggered from
the friends icon in the Home screen header, not a tab. Build
ProfileStack.tsx: modal pattern for the Profile screen --- triggered
from the duck avatar in the Home screen header. Build
OnboardingStack.tsx: Screen 1 (DuckSelector + KnownAs input), Screen 2
(UsernameReveal). Build AppNavigator.tsx: reads isAuthenticated from
AsyncStorage → renders OnboardingStack or TabNavigator in
NavigationContainer. Verify: 4 tabs navigate, Home header has a friends
icon and a duck avatar as two separate tappable elements, each opening
their respective modal stack.

**Patterns Practiced**

Bottom tab navigation, modal stacks for Friends and Profile (not tabs
--- they overlay from the header), conditional navigation based on auth
state, header icon placement (friends icon + duck avatar in Home screen
header bar)

**Learning Takeaway**

*Why Friends is a modal stack from the header rather than a fifth tab
--- DuckyEQ has four primary engagement loops (Home, Learn, Gratitude,
Shop). Friends is the social layer across all of them, not a primary
destination. A header icon communicates \'always available from
anywhere\' without adding a fifth tab. A fifth tab would crowd the tab
bar and imply Friends is a daily-schedule destination rather than an
ambient social layer you access when you want it.*

**Day 20** (Sat) **API client + auth service + all social service
hooks**

**Build**

Build /src/services/apiClient.ts: Axios instance with baseURL from
expo-constants.expoConfig.extra. Request interceptor reads JWT from
AsyncStorage and attaches Authorization: Bearer. 401 interceptor clears
token and navigates to onboarding. Build authService.ts: register(email,
password, duckCharacter, knownAs) → POST /auth/register → store JWT,
username, knownAs in AsyncStorage → return {username, knownAs, userId}.
login(). logout(). updateKnownAs(newKnownAs) → PUT /user/known-as →
replace stored JWT with new one. Build checkInService.ts:
getTodayStatus() → 200 or 204. submitCheckIn(emotionIds, phrase?). Build
friendsService.ts: getFriends(), searchUser(username),
sendRequest(addresseeId), acceptRequest(id), declineRequest(id),
getFriendDetail(id). Build quackService.ts: sendQuack(recipientId,
quackType), getUnseen(), markSeen(id). Custom hooks: useAuth (jwt-decode
on stored JWT for synchronous username + knownAs), useCheckIn,
useFriends, useQuacks --- each with {data, isLoading, error, refetch}.

**Patterns Practiced**

Axios interceptors, jwt-decode for synchronous claims reading,
service-per-feature pattern (one service file per feature area), custom
hooks with loading/error/data/refetch pattern

**Learning Takeaway**

*Why each feature gets its own service file --- a single file for all
API calls would be hundreds of lines with no organization. Separate
service files mirror the controller structure on the backend. When you
debug a Quack issue, you open quackService.ts. When you debug a check-in
issue, you open checkInService.ts. The file name tells you where to
look. The modularity that makes the backend maintainable applies equally
to the frontend.*

**Day 21** (Sun) **Onboarding: duck selector + Known As input +
PascalCase username reveal**

**Build**

Build two-screen OnboardingStack. Screen 1 (DuckSelector + KnownAs):
warm gradient background. Header: \'Choose your companion.\' Quack and
Lola as large photo cards side by side using the front-facing happy
expression PNGs. Tapping a card highlights it with Animated glow border
(Animated.loop on border opacity 0.4→1). Below duck cards: text input
\'What should your friends call you?\' --- max 10 chars, character
counter shown live (e.g. 6/10). \'Let\'s go!\' button slides in
email/password fields. On \'Create My Account\': call
authService.register(email, password, duckCharacter, knownAs) with
FluentValidation enforcement (2--10 chars enforced client-side too).
Screen 2 (UsernameReveal): celebration screen. Duck in excited
expression, large. Text \'Meet your duck! Your username is\...\' --- 0.8
second delay --- then PascalCase username animates in with
Animated.spring (scale 0.6→1, opacity 0→1) in large gold text. Below:
\'@SunnyDuck4832 --- friends can find you by searching this name.\'
KnownAs in smaller text: \'Your friends will call you Diego.\' \'Start
my EQ journey!\' navigates to TabNavigator.

**Patterns Practiced**

Two-screen onboarding, Animated glow border on duck selection, 10-char
Known As input with live character counter, PascalCase username
Animated.spring reveal, 0.8s anticipation delay before reveal animation
starts

**Learning Takeaway**

*Why Known As and duck selection are on the same screen --- onboarding
friction is the enemy of activation. Every additional screen increases
drop-off. Duck selection and Known As are the two identity decisions.
One screen, one moment: \'This is who you are in DuckyEQ.\' The reveal
is its own screen because it is a celebration, not a decision. Two
identity choices, one screen. One reveal, one screen.*

**Day 22** (Mon) **Daily emotion check-in modal**

**Build**

Build CheckInModal in /src/features/home/. In HomeScreen on mount: call
useCheckIn.getTodayStatus(). If 204 (not yet checked in), show
CheckInModal as a React Native Modal with animationType=\'slide\'. The
modal cannot be dismissed by tapping outside --- only via explicit user
actions. Step 1 --- Emotion Grid: \'How are you feeling today?\'
headline. Six cards in a 3×2 grid, each: duck expression photo PNG
(happy.png for Happy, sad.png for Sad, etc.) with emotion label below.
Multi-select: tapping adds to selectedEmotions string\[\] and applies a
teal glow border style. Tapping again deselects. Continue button
disabled until at least one emotion selected. Step 2 --- Reason Chips:
\'Want to share why?\' Horizontal ScrollView of 8 chips from
CHECKIN_CHIPS constant. Toggle-selectable. Below: optional TextInput max
120 chars with character counter \'45/120\'. A \'Skip\' text link sets
phrase to null. \'Done\' button: calls
checkInService.submitCheckIn(selectedEmotions, phrase). On 201: dismiss
modal, update check-in state so it doesn\'t reappear today.

**Patterns Practiced**

Full-screen non-dismissable Modal (animationType=\'slide\'),
multi-select string\[\] state, reason chip toggle (add/remove from
selected array), 120-char TextInput with character counter, null phrase
on skip, client-side last_checkin_date stored in AsyncStorage as
secondary gate

**Learning Takeaway**

*Why the check-in modal cannot be dismissed by tapping the backdrop ---
the daily check-in is a ritual, not a notification. If users can tap
outside to dismiss, they do it reflexively without engaging. The two
explicit exit paths --- Done (with at least one emotion) and Skip all
--- both require intentional interaction. The modal is 30 seconds of
self-reflection and should feel deliberate.*

**Day 23** (Tue) **Home screen: Quack banner + Friends Feeling Panel +
Profile Bar**

**Build**

Build the full HomeScreen. Profile Bar at top: left = DuckyEQ wordmark.
Right = friends icon (people icon --- tappable →
FriendsStack.navigate) + duck avatar (user\'s character, happy
expression, small circle --- tappable → ProfileStack.navigate).
\@username in gray below avatar, KnownAs above it (both read from
jwt-decode on stored JWT). Streak flame + count. Coin balance. Quack
Banner: below Profile Bar, conditionally rendered when useQuacks.unseen
has items (fetched via useFocusEffect on every Home tab focus). Animated
duck head with Animated.loop bounce (scale 1.0→1.05, duration 900ms).
Single quack: \'🎉 CozyDabbler9034 sent you a Hug!\' Multiple: \'You
have 3 Quacks waiting 🐥\'. Tapping: mark all seen via
quackService.markSeen for each, close banner with Animated.timing
fade-out, navigate to FriendsStack. Friends Feeling Panel: horizontal
ScrollView. Header row: \'How your friends are feeling today\' + \'See
All →\' button. Friend cards: duck avatar, KnownAs + \@username, emotion
emoji(s), phrase snippet (max 30 chars + \...). No check-in today → gray
ghost card \'Hasn\'t checked in yet\'. No friends → empty state. Below
panel: Motivational Quote Card, XP bar, Quick Actions (Go Learn,
Gratitude Garden).

**Patterns Practiced**

useFocusEffect for Quack banner re-fetch on tab return, Animated.loop
bounce, conditional banner render, FlatList horizontal for friends
panel, emotion-to-emoji mapping (Happy→😊, Sad→😢, Angry→😠, Anxious→😰,
Meh→😐, Excited→🤩), ghost card pattern for null check-in

**Learning Takeaway**

*Why the Quack banner uses useFocusEffect instead of useEffect on mount
--- the user might navigate away from Home, receive a Quack while on
Learn or Shop, then return. Mount-only fetch would show a stale empty
banner. useFocusEffect refetches unseen Quacks every time Home tab
becomes active. Same for the Friends Panel --- friend check-ins can
happen while you are on a different tab. useFocusEffect makes both
panels always reflect the latest state.*

**Day 24** (Wed) **Profile screen: Known As editor + inventory + equip +
character toggle**

**Build**

Build ProfileScreen (ProfileStack modal via duck avatar). Top: large
duck avatar (current character, happy expression). Username in large
gold text. KnownAs with a pencil icon beside it --- tapping opens inline
TextInput (pre-filled, max 10 chars, counter). \'Save\' calls
authService.updateKnownAs(newKnownAs) → receives new JWT → replaces
stored JWT → all screens update via useAuth re-read. \'Cancel\' restores
original. Character switcher: Quack and Lola thumbnails below. Tapping
calls PUT /user/duck-character and updates active character everywhere.
Inventory grid below: organized by category (Hats, Colors, Glows,
Accessories). Each item: thumbnail, name, \'Equipped\' badge if active.
Tapping any owned item calls POST /shop/equip → updates equipped state
in InventoryContext → propagates to Home Profile Bar duck, Shop preview,
board game path duck avatar. X button at top right dismisses modal.

**Patterns Practiced**

Inline Known As editor (pencil → input → save/cancel), JWT replacement
on KnownAs update, React Context for shared equipped state
(InventoryContext shared across Profile, Shop, Home, Learn), character
switcher with immediate visual update

**Learning Takeaway**

*Why the Known As editor replaces the stored JWT rather than just
refreshing the UI --- the JWT carries KnownAs in its claims. If you
update KnownAs in the DB and update local React state but keep the old
JWT, the next cold launch reads the old JWT from AsyncStorage and shows
the old name. Replacing the JWT atomically means the app shows the
correct KnownAs from any entry point: fresh launch, tab switch, or app
background/foreground.*

**Day 25** (Thu) **Friends screen: 3 tabs --- Friends List, Add Friends,
Pending Requests**

**Build**

Build FriendsScreen (FriendsStack modal via friends icon). Header:
\'Friends\' + X close button. Three sub-tabs: Friends List, Add Friends,
Pending (badge count = pending requests count from GET
/friends/requests/pending). Friends List: FlatList of accepted friends.
Each row: avatar, KnownAs + \@username, today\'s emotion emoji(s),
phrase snippet if not null. \'Hasn\'t checked in yet\' in gray if no
check-in. Tapping a row navigates to FriendDetail screen (stack push in
FriendsStack): KnownAs, \@username, level, XP bar, today\'s feelings +
phrase, \'Send a Quack 🐥\' button. Quack button opens a bottom-sheet
picker (Animated.spring slide-up) with 5 quack type cards. Tapping one:
POST /quacks → success → duck animation + toast \'Quack sent!\' → 429 →
\'You already sent a Quack to \[KnownAs\] today.\' Add Friends tab:
search bar (min 3 chars, debounced 400ms, calls GET /users/search).
Results: KnownAs + \@username + status button (Send Request / Already
Friends / Request Pending). Pending tab: incoming requests with Accept
and Decline buttons. Accepting immediately moves friend to Friends List.
Badge count updates on accept.

**Patterns Practiced**

Custom horizontal sub-tabs within a modal screen, Friend Detail as stack
push within FriendsStack, Quack bottom sheet picker, 400ms debounce on
search, 429 rate limit UX (friendly message, not raw status code), badge
count on Pending tab

**Learning Takeaway**

*Why the Quack picker is a bottom sheet not a full screen --- Quacks are
gestures, not features. They should take 2 taps: tap friend, tap quack
type. A full-screen push for a 5-option picker is disproportionate. A
bottom sheet slides up, user taps their type, sheet closes with a
confirmation animation, back to Friend Detail in 3 seconds. The
interaction cost matches the lightweight nature of a Quack.*

**PHASE 4 Learn Screen + Test Your EQ + CDER Lessons**

**Goal:** Full Learn screen with Test Your EQ entry card and 5 pillar
cards. 50-node board game path per pillar with duck standing on the
current node. Full CDER lesson player with 4 mini-game types, 0--300
scoring, 1--3 stars, and coin rewards. 60-second Test Your EQ with
wrong-answer cooldowns.

**Milestone:** *User can open Learn, play Test Your EQ with a live
60-second timer, navigate the 50-node board game path, start and
complete a CDER lesson with a real score and animated reward screen.*

**Day 26** (Fri) **Learn screen: pillar cards + Test Your EQ entry
card**

**Build**

Build LearnScreen. At the very top, above the pillar cards, a \'Test
Your EQ\' card --- duck gold accent (#F5A623), lightning bolt icon,
energetic visual distinct from pillars. Shows best score and stars from
GET /api/eq-test/best-score (or \'Not yet played\' if null). Tapping
navigates to EQTestScreen. Below: ScrollView of 5 pillar cards. Each
card: pillar icon, pillar name, pillar color stripe (5 distinct colors
--- one per pillar), current level (lesson number the user is on), XP
progress bar, cooldown status. Data from GET /api/pillars/progress.
Tapping a pillar navigates to BoardGamePath with pillarId param. First
time user (all null progress): empty state prompt \'Choose a pillar to
start your EQ journey 🐥\'. Use useFocusEffect to re-fetch after
returning from a lesson --- stars and level must update on return.

**Patterns Practiced**

ScrollView with featured card above the list, pillar color theming,
cooldown display, navigation params (pillarId), empty state for new
users, useFocusEffect for post-lesson refresh

**Learning Takeaway**

*Why the Test Your EQ card sits above the pillar cards --- it
communicates that the EQ test is a quick always-available activity
separate from the structured lesson progression. Pillar cards represent
a journey (Lesson 1 to 50). The Test Your EQ card is a game you can play
any time. Positioning it above the pillar section makes this difference
clear without any text explanation. The visual hierarchy does the
communication.*

**Day 27** (Sat) **Board game path: 50 nodes + duck on current node +
preview modal**

**Build**

Build BoardGamePath screen. On mount: GET
/api/pillars/{pillarId}/lessons for all 50 LessonWithProgressDto. Render
a vertical ScrollView with 50 nodes in a zigzag (nodes alternate
positions using a modulo pattern or left-center-right). Node states:
Completed = filled pillar color, 1--3 star icons below circle. Current =
Animated.loop pulse (scale 1.0→1.08, opacity 1.0→0.85, duration 1200ms)
with duck avatar PNG (active character, excited expression, \~48px)
positioned absolutely above the circle using negative top offset ---
duck appears to stand on the node. Locked = gray circle + lock icon.
Auto-scroll to current node on mount using a scrollTo ref. Tapping
completed or current node: Animated.spring slide-up bottom sheet modal
showing lesson number, title, objective, current stars or \'Not yet
played\', primary button (\'Start Lesson\' or \'Replay Lesson\' +
\'Perfect score!\' badge if 3 stars). Tapping Start: navigate to
LessonPlayer with lessonId. After lesson completion (on focus return),
duck hops from completed node to new current node with
Animated.sequence.

**Patterns Practiced**

Zigzag layout with modulo-based positioning, absolute positioning for
duck-on-node (negative top offset), Animated.loop for current node
pulse, scrollTo ref for auto-centering, Animated.spring bottom sheet
modal, duck hop animation with Animated.sequence on lesson completion

**Learning Takeaway**

*Why the duck is positioned ON the node --- in physical board games,
your piece sits on the space. Any other position --- beside, above the
row, floating in a corner --- requires the user to mentally trace a line
from the duck to their current node. On-node positioning needs zero
interpretation. One glance, and the user knows exactly where they are in
50 lessons.*

**Day 28** (Sun) **CDER lesson framework + Connect stage**

**Build**

Build LessonPlayer screen. On mount: POST /api/lessons/{id}/start →
store session token in local state. Build CDERFlow component managing
stage state: CoreMessage → QuackOfTheDay → Connect → Define → Engage →
Reflect → Reward. CoreMessage: lesson title + duck in thoughtful
expression (large) + objective. Auto-advances after 5 seconds or on tap.
QuackOfTheDay: joke setup with duck in curious expression, tap to reveal
punchline with duck switching to the punchline expression from
ContentJson. Connect: 5 questions from ContentJson.connect.questions,
one at a time. Text input per question. After each answer, brief duck
reaction line chosen from a local array (\'Love that.\', \'Same,
honestly.\', \'Interesting.\') then advance. Connect answers stored in
LOCAL STATE ONLY --- never submitted to the server. After question 5,
advance to Define.

**Patterns Practiced**

Stage-based flow controller (currentStage enum), duck expression
switching per stage (imported from EMOTIONS constant), session token
stored in local state for the complete call later, local-only Connect
answers

**Learning Takeaway**

*Why Connect answers are local-only and never sent to the server ---
this is an ethical design decision, not just a performance one. Connect
questions ask personal things: \'What emotion have you felt most
strongly this week?\' Transmitting those answers to your server collects
intimate personal data. Users who know their reflections are stored will
self-censor. Keeping answers on-device means users can be fully honest.
Honest Connect answers make the stage actually work.*

**Day 29** (Mon) **Define stage + Engage stage router**

**Build**

Build Define component: 6--8 flashcard cards from
ContentJson.define.flashcards. Each is a full-screen card with text
centered and the duck shrunk to a small corner avatar (\~20% of screen
width). Duck expression changes per card to match
ContentJson.define.expressionArc. User taps card or Next to advance.
After all flashcards, advance to Engage. Build Engage stage router:
reads ContentJson.engage.gameType and renders the matching game
component. Four game types: \'MultipleChoice\' → MultipleChoiceGame,
\'TrueFalse\' → TrueFalseGame, \'Scenario\' → ScenarioGame,
\'FlashcardFlip\' → FlashcardFlipGame. Each game receives {questions:
\[\], onGameComplete(correct, total)}. The Engage router stores correct
and total from the callback in CDERFlow state for the Reward screen POST
call, then advances to Reflect.

**Patterns Practiced**

Format router pattern (switch on gameType, render matching component ---
same interface for all 4), flashcard carousel with expression arc, duck
size transition (large in Connect, small corner avatar in Define), game
component interface contract

**Learning Takeaway**

*Why all four game components share the same props interface {questions,
onGameComplete} --- the Engage router doesn\'t need to know which game
is running. It passes the same props and receives the same callback.
This is the Strategy pattern: behavior is swappable, interface is fixed.
Adding a fifth game type in Phase 2 means adding one component and one
case in the router switch --- the router itself never changes.*

**Day 30** (Tue) **Mini-game implementations: 4 game types**

**Build**

Build the four game components. MultipleChoiceGame: question + 4 option
buttons. Correct tap → green flash + duck excited expression + score
increment + next question. Wrong tap → red flash on selected + green on
correct + duck encouraging expression + 2-second explanation text
below + next question. TrueFalseGame: rapid-fire --- question +
True/False buttons. Correct → immediate next (no delay, keeps pace).
Wrong → brief correct flash then next. ScenarioGame: scenario paragraph
(2--3 sentences) + 2--4 response options. Tapping reveals outcome +
explanation. FlashcardFlipGame: term on front (tap to flip) → definition
on back. \'Got It\' retires the card. \'Review Again\' cycles it back.
Score = Got It count / total. All four track correctAnswers and
totalQuestions and call onGameComplete(correct, total).

**Patterns Practiced**

Game component pattern (same interface, different implementation ---
Strategy pattern), duck expression on correct vs wrong (excited vs
encouraging --- never shame or punishment), review-loop for
FlashcardFlip, score tracking internal to each component

**Learning Takeaway**

*Why Engage mini-games have no timer --- the Test Your EQ feature is the
timed experience. Lessons are designed to be thoughtful and un-rushed.
Adding a timer to Engage would create anxiety during a learning moment.
Users would rush answers, skip reading question text, and feel penalized
for thinking. Test Your EQ is for fast, scored challenge. Lesson Engage
is for deliberate application. Two features, two mechanics, zero
overlap.*

**Day 31** (Wed) **Reflect stage + Reward screen + lesson complete API
call**

**Build**

Build Reflect component: 5 questions from ContentJson.reflect.questions
one at a time. Duck returns to medium size (\~40% of screen) in
sympathetic or curious expression. Each answer saved to local state
only. After question 5: closing line from ContentJson.reward.closingLine
appears as a stylized quote. \'See My Results\' button navigates to
Reward. Reward screen: immediately on enter, POST
/api/lessons/{id}/complete with {sessionToken, correctAnswers,
totalQuestions}. While call is in-flight: score counts up from 0 to
final over 1.5 seconds (Animated.timing), stars pop in one at a time
(Animated.spring staggered 400ms apart), coin shower animates upward
(staggered Animated.timing). Duck expression from
ContentJson.reward.duckExpression (usually \'proud\'). If isNewBest =
true: \'New Best! 🌟\' badge. \'Back to Learn\' navigates back; board
game re-fetches on focus and advances duck to next node.

**Patterns Practiced**

API call during animation sequence (call at animation start, response
arrives during animation --- animations serve as loading buffer),
staggered Animated.spring star pop-in, Animated.timing score count-up,
isNewBest conditional badge

**Learning Takeaway**

*Why you POST /api/lessons/complete at the start of the Reward animation
rather than after --- the API call takes 200--800ms. The animations take
2--3 seconds. Starting the call at animation start means the response
arrives during the animation. If you wait until animations finish, users
see a freeze after the animation stops while the call completes. Start
call early → animations = loading buffer → data confirmed by the time
the animation ends.*

**Day 32** (Thu) **Test Your EQ screen: timer + questions + cooldown +
reward**

**Build**

Build EQTestScreen. On mount: GET /api/eq-test/questions?count=50,
Fisher-Yates shuffle client-side. Show \'Ready?\' screen: rules summary,
current best score, \'Start!\' button. Session state: totalSeconds=60,
currentQuestionIndex=0, correctAnswers=0, isOnCooldown=false. Timer bar:
full-width at top, shrinks 100%→0% as seconds tick. Color interpolation:
green above 30s, orange 15--30s, red below 15s (Animated.interpolate).
useInterval hook (setInterval with cleanup). On correct answer:
increment correctAnswers, load next question immediately. On wrong
answer: set isOnCooldown=true, highlight correct green + wrong red, show
explanation text, setTimeout 5000ms → set isOnCooldown=false + load next
question + resume timer. Timer pauses during cooldown by stopping the
interval. When timer reaches 0: navigate to EQTestReward screen and POST
/api/eq-test/submit {correctAnswers}. EQTestReward: animated score
reveal, stars pop-in, duck expression (proud for 3 stars, encouraging
for 1--2), \'Try Again\' button resets session.

**Patterns Practiced**

useInterval hook (setInterval with cleanup on unmount), Fisher-Yates
shuffle, timer pause/resume during cooldown (stop/restart setInterval),
Animated.interpolate for color shifting timer bar, isOnCooldown as input
gate

**Learning Takeaway**

*Why the timer pauses during wrong-answer cooldown --- if the timer kept
running during the 5-second explanation, the user is penalized twice:
wrong answer time lost PLUS forced to read explanation while the clock
runs. The cooldown is a learning moment, not a punishment. Pausing makes
the experience fair. Users only lose time when actively answering. This
also makes users actually read the explanation instead of frantically
tapping to skip it.*

**Day 33** (Fri) **Cross-feature social integration check**

**Build**

Day 33 is a targeted integration check: social features working within
the Learn context. Verify: (1) After completing a lesson and earning 3
stars, does the coin balance update in the Profile Bar without a page
refresh? (2) Does the friends icon in the Home header still appear
correctly after navigating deep into a lesson flow? (3) User A and User
B are friends. User A checks in as Excited. User B opens Friends screen
--- User A shows Excited. User A completes Pillar 1 Lesson 1. User B
views User A\'s Friend Detail --- level and XP reflect the lesson
completion on the next GET. (4) User B sends User A a Quack (Cheer).
User A navigates back to Home --- Quack banner appears via
useFocusEffect. Tapping banner marks seen, navigates to Friends screen,
banner disappears. (5) User C has no check-in today --- their card in
the Friends Panel shows \'Hasn\'t checked in yet\' without null crashes
anywhere (phrase, emotionIds all handled gracefully). Fix all issues
found. Push \'phase-4-complete\' tag.

**Patterns Practiced**

Cross-screen state consistency (coins update post-lesson visible in
header), social data freshness (friend level updates after lesson
completion), null-safety audit (null phrase + null emotionIds never
crash the UI), useFocusEffect quack banner refresh

**Learning Takeaway**

*Why cross-feature integration tests happen at Day 33 rather than Day 40
--- catching a cross-screen state bug now requires fixing one hook or
one context value. Catching the same bug after Gratitude and Shop are
also built means debugging across 5+ screens without knowing which layer
introduced the regression. Every phase-boundary test reduces the
debugging surface area. Smaller surfaces, clearer bugs, faster fixes.*

**PHASE 5 Gratitude Garden + Shop + Full Integration**

**Goal:** Gratitude Garden with animated visualization and Pick Me Up
mode. Shop with weekly items, purchase flow, live duck preview, and
cross-screen equipped state via React Context. Full integration test
across all features --- Home, Learn, Gratitude, Shop, Friends, Check-In,
Quacks --- with verified coin economy.

**Milestone:** *All screens complete and working together. Full user
journey verified end-to-end including social. Coin economy manually
confirmed. Monitoring configured and live.*

**Day 34** (Sat) **Gratitude Garden: entry input + category selector +
streak badge + wellness support button**

**Build**

Build GratitudeScreen. expo-linear-gradient background: soft teal
(#E0F2F1) at top, warm gold (#FFF3E0) at bottom. Top: streak badge
(flame icon + count + \'day streak\' from GET /api/gratitude/streak).
Input section: \'What are you grateful for today?\' prompt, TextInput
max 200 chars with character counter, placeholder \'Something big or
small\...\'. Five category buttons in a horizontal row: People (🌸),
Moments (☀️), Things (🌿), Self (⭐), Other (💧). \'Add to Garden\'
button with duck in meditating pose beside it. On submit: POST
/api/gratitude. Optimistic UI --- add entry to local state immediately
before API confirms. If isFirstToday flag in response: animate +10 coins
floating upward. New entry grows into garden below with Animated.spring
scale 0→1. Wellness Support Button: in the bottom-right corner of the
screen, place a small soft icon --- a gentle heart or a subtle cloud
emoji --- with tiny text beneath it: \'feeling anxious?\' in a very
light gray, small enough that it does not demand attention but is always
there. This button must never feel clinical or alarming; it should feel
like a quiet hand on the shoulder. Tapping it opens a full-screen modal
(WellnessModal) that the user can close at any time via an X in the top
right. The modal uses the same warm gradient as the garden. Inside: a
meditating duck (medium size, calm expression). A short block of warm
affirmations in large-ish, readable text --- no paragraphs, no walls of
text: \'You matter. You are important. People love you.\' Three lines,
centered, spaced generously. Below the affirmations, two quick-action
chips: \'Deep Breathing 🌬️\' and \'I want to talk to someone 💬\'. Deep
Breathing chip starts a guided breathing exercise inline --- a simple
animated circle that expands over 4 seconds (inhale) and contracts over
4 seconds (exhale) with the text \'Breathe in\...\' and \'Breathe
out\...\' appearing underneath. Runs for 3 cycles then gently fades to a
\'You\'re doing great 🐥\' message. \'I want to talk to someone\' chip
reveals a small card below: \'Talking about your feelings is one of the
bravest things you can do. Reach out to someone you trust --- a friend,
a family member, a counselor --- anyone who makes you feel safe. You can
also reach out to us at duckyeqapp@gmail.com --- we read every
message.\' All text is warm, brief, and non-clinical. The modal can be
closed at any point with the X --- no completion required.

**Patterns Practiced**

Small non-intrusive wellness button (bottom-right, tiny label, does not
demand attention), WellnessModal with full-screen warm overlay,
breathing animation (Animated.timing circle scale 0.6→1.0 over 4s for
inhale, reverse for exhale with Text label swap), animated circle using
Animated.loop with sequence, two quick-action chips (Deep Breathing + I
want to talk), dismissable at any point

**Learning Takeaway**

*Why this button is small and quiet rather than prominent --- the goal
is that users who need it find it, and users who don\'t need it barely
notice it. A large button that says \'Feeling Anxious?\' on every garden
visit would feel alarming and would subtly alter how the entire
Gratitude Garden feels. A small icon with soft text in a corner is
discoverable without being intrusive. The people who need it will find
it. The people who don\'t will walk past it without a second thought.
Quiet availability is the right design for a feature like this.*

**Day 35** (Sun) **Garden visualization + Pick Me Up mode**

**Build**

Below the input, build the garden. Category-based element styles: People
= pink circle with 🌸 + text below, Moments = gold circle + ☀️, Things =
green + 🌿, Self = white/gold + ⭐, Other = blue + 💧. Elements arranged
in a flowing grid with slight random margin offsets (4--12px per entry,
calculated via useMemo from entry.id for stability between renders) for
an organic, non-database feel. Today\'s entries have a glowing teal
border. FlatList with ListHeaderComponent for today\'s pinned section.
Tap any element → expand detail card (full text, category emoji,
relative date: \'you wrote this 12 days ago\'). Load from GET
/api/gratitude on mount. Pick Me Up: floating action button at bottom
(\'Pick Me Up 🐥\'). Tap → full-screen modal overlay (richer warm
gradient). Duck in presenting pose. \'Your duck found this in your
garden.\' Large centered text of a random past entry (GET
/api/gratitude/random). \'You wrote this X days ago.\' Swipe right or
Next for another (crossfade via Animated.timing opacity). X button
closes. Edge case: fewer than 3 entries → \'Keep adding to your garden
--- your duck needs more to find for you.\'

**Patterns Practiced**

Random stable margin offsets via useMemo (same offset every render for
same entry.id), FlatList ListHeaderComponent for pinned today section,
relative date calculation (\'12 days ago\' from Date.now() -
entry.createdAt), crossfade between Pick Me Up entries, minimum entry
guard

**Learning Takeaway**

*Why \'you wrote this 12 days ago\' is more powerful than the date ---
\'March 14th\' is a timestamp. \'12 days ago\' is a story. It places the
past entry in relation to the present moment. Relative time creates
temporal connection between who you were when you wrote it and who you
are now reading it. This is the entire emotional point of Pick Me Up ---
not to browse old data, but to reconnect with past moments of
appreciation.*

**Day 36** (Mon) **Shop screen: item grid + duck preview + purchase +
equip**

**Build**

Build ShopScreen. Coin balance prominently at top (from GET
/api/coins/balance). Live duck preview: user\'s active character with
currently equipped items rendered. Tapping an unowned item in the grid
updates the duck preview to a hypothetical equipped state --- this is
preview state, separate from real equipped state. Four category tabs:
Hats, Colors, Glows, Accessories. Items from GET /api/shop/items. Each
item card: thumbnail, name, coin price (or \'Owned\' in green, or
\'Equipped\' with star). Tapping purchasable item: update duck preview
locally + show purchase modal \'Equip \[item\] for \[X\] coins? Buy and
Cancel\'. Confirming: POST /api/shop/purchase → animate coin balance
down → POST /api/shop/equip → update real equipped state in
InventoryContext → duck preview becomes real equipped state.
Insufficient coins: \'Not enough coins! Complete lessons and add to your
garden to earn more.\' Equip of owned item: free --- POST
/api/shop/equip directly. InventoryContext propagates equipped changes
to Home Profile Bar, board game path duck, and Profile screen duck.

**Patterns Practiced**

Preview state vs real equipped state (two separate local states ---
preview for browsing, real for cross-screen display), purchase +
auto-equip as sequential calls in one flow, InventoryContext
propagation, coin animation on purchase

**Learning Takeaway**

*Why the shop preview is a separate local state from real equipped state
--- when the user taps an unowned item to preview it, the duck changes
in the Shop. But they haven\'t bought it. If you updated the real
equipped state, the duck on the Home screen and Friends Panel would show
items the user hasn\'t purchased. Preview state is local to the Shop.
Real equipped state lives in InventoryContext and propagates everywhere.
Two states, one duck component, one InventoryContext.*

**Day 37** (Tue) **Full integration test: all screens including social**

**Build**

Delete app from Expo Go, reinstall clean. Full flow: Onboarding → select
Lola, enter KnownAs \'Duckster\' (8 chars), create account → PascalCase
username revealed (e.g. FluffyPaddler5590). Home → check-in modal
appears → select Happy + Excited → tap Friends chip → Done. Home →
Friends Panel empty state (no friends yet) → no Quack banner. Learn →
Test Your EQ → 5 correct → score 50, 2 stars. Pillar 1 → board game path
→ duck on Node 1 → tap node → preview modal → Start Lesson → full CDER
flow → Engage 7/10 → score 210, 3 stars → Reflect → Reward (210, 3
stars, coins animated) → back → duck on Node 2, Node 1 shows 3 stars.
Gratitude → add 3 entries → garden grows → +10 coins on first → Pick Me
Up shows one entry \'you wrote this 0 days ago\'. Shop → browse Hats →
preview hat on Lola → buy 30 coins → equip → go to Home → Lola in
Profile Bar wears hat. Profile → edit KnownAs to \'DuckyDucks\' → save →
KnownAs updates everywhere including Friends Panel. Friends flow →
search test user B → send request → accept on device B → check in on B
as Sad → Friends Panel shows B with 😢 → send Quack (Hug) to B → Home on
B shows Hug banner. Coin check: 50 + 60 (3-star lesson) + 10 (first
gratitude) - 30 (hat) = 90. Verify in Azure Portal Query Editor.

**Patterns Practiced**

Multi-account social integration testing, clean install flow, coin
economy oracle (calculate expected 90 before checking), KnownAs update
propagation across all screens, cross-screen equipped item consistency,
social flows end-to-end

**Learning Takeaway**

*Why you calculate the expected coin balance before running the
integration test --- 50 (starter) + 60 (3-star lesson first
completion) + 10 (first gratitude today) - 30 (hat) = 90. If the app
shows 90, every part of the coin economy is correct. If it shows
anything else, you know exactly which transaction is wrong. This is the
test oracle pattern --- knowing the expected answer before checking
makes discrepancies immediately meaningful.*

**Day 38** (Wed) **Bug fixes + polish + empty states**

**Build**

Fix every bug found on Day 37. Common issues to check: Friends Panel not
updating after a friend checks in (useFocusEffect missing), Quack banner
not clearing after marking seen (local state not updated after PATCH),
duck accessories not rendering on board game node avatar, check-in modal
reappearing same day (AsyncStorage gate not set after submission), Pick
Me Up crashing with fewer than 3 entries, KnownAs not updating in
Profile Bar after edit without app restart, coin balance not syncing
between Shop and Home after purchase. Add ActivityIndicator loading
states on all data-fetching screens. Add empty states everywhere:
Friends screen with no friends (\'Search for a friend by their
\@username 🐥\'), Add Friends with no results (\'No one found ---
double-check the username\'), Pending tab with no requests (\'No pending
requests --- check back later\'). Review all screen transitions. Push
\'pre-polish\' tag.

**Patterns Practiced**

Bug triage and prioritization, loading/empty/error state pattern (3
states for every async view), useFocusEffect vs useEffect (data that can
change between tab visits needs useFocusEffect), ActivityIndicator
placement, empty state copy that guides action

**Learning Takeaway**

*Why empty states matter as much as full states --- a new user who opens
the Friends screen sees zero friends. A blank screen with no guidance
leaves them wondering if the app is broken. \'Search for a friend by
their \@username\' tells them exactly what to do next. Empty states are
the app\'s first impression for every feature. They are not edge cases
--- they are the literal starting point of every user\'s relationship
with each feature.*

**Day 39** (Thu) **Week 5 review + final pre-polish pass**

**Build**

Structured review of every screen. Home: Profile Bar updates correctly
after every action (lesson, gratitude, shop, KnownAs edit)? Quack banner
dismisses cleanly? Friends Panel scrolls smoothly and ghost cards render
without null crashes? Learn: board game path auto-scrolls to current
node on every open? Stars update immediately on return post-lesson? Test
Your EQ timer color-shifts correctly green→orange→red? Gratitude: Pick
Me Up works with exactly 3 entries? Relative date says \'today\' for
same-day entries? Shop: duck preview updates instantly on item tap?
Equip badge appears correctly across all category tabs? Friends: pending
badge count updates after accepting a request? Quack rate limit shows
friendly message not raw \'429\'? Fix all issues found. Push
\'phase-5-complete\' tag.

**Patterns Practiced**

Structured screen-by-screen review checklist (systematic coverage vs
free exploration), null-safety audit (every place phrase or emotionIds
could be null handled gracefully), rate limit UX (never expose raw HTTP
status codes to users), relative date edge cases (today/yesterday/X days
ago)

**Learning Takeaway**

*Why you review phase-by-phase with a checklist rather than just using
the app freely --- free exploration finds random bugs but misses
systematic ones. A checklist ensures you visit every state of every
feature: empty state, loaded state, error state, edge case. Free testing
is biased toward the happy path. Checklists enforce systematic coverage
of every screen\'s full state space.*

**Day 40** (Fri) **Monitoring + analytics + crash reporting setup**

**Build**

Set up Sentry for React Native: npx \@sentry/wizard@latest -i
reactNative. Configure in App.tsx. Sentry captures crashes, unhandled
promise rejections, and performance traces. Add custom Sentry events for
critical social flows: if POST /api/quacks fails with non-429 status,
log \'QuackSendFailure\' with status + userId. If POST /api/checkin
fails, log \'CheckInSubmitFailure\'. Set up Azure Application Insights
for the backend API: NuGet Microsoft.ApplicationInsights.AspNetCore +
AddApplicationInsights() in Program.cs. Gives backend request traces,
exception logging, slow endpoint detection. Set up a Postman scheduled
monitor (free tier): run the full Postman collection every 12 hours
against the production API with email notification on failure. This is
your lightweight production health check.

**Patterns Practiced**

Sentry React Native integration, Azure Application Insights for ASP.NET
Core, Postman scheduled monitors as API health checks, custom Sentry
events for business-critical social flows

**Learning Takeaway**

*Why monitoring matters from Day 1 of launch --- users do not file bug
reports. They uninstall. If the social endpoints start returning 500
errors, you will not know until you see 1-star reviews. Sentry and
Application Insights catch errors proactively. The Postman monitor
catches full-flow failures before users hit them. Every day without
monitoring is a day where user-impacting bugs go undetected.*

**PHASE 6 Polish + App Store Submission**

**Goal:** Content QA across all 5 pillars. duckyeq.com landing page and
privacy policy live. App icon, splash, and 6 screenshots ready. EAS
production build succeeds. App Store Connect listing submitted for
review.

**Milestone:** *DuckyEQ is submitted to the App Store. If approved, live
within 24--48 hours of submission.*

**Day 41** (Sat) **Content QA --- play through lessons across all 5
pillars**

**Build**

Content quality day --- not feature testing. Play 3--4 lessons per
pillar (15--20 total). For each: does the Quack of the Day joke land? Do
Connect questions feel warm and personal? Do Define flashcards build
clearly? Does the Engage game match the concept? Are Reflect questions
thoughtful? Is the closing line quote-worthy? Are duck expressions
emotionally coherent per stage (thoughtful during Define, excited on
correct Engage answers, sympathetic during Reflect)? Log all issues in
notes. Fix content by updating ContentJson in Azure SQL via Portal Query
Editor: UPDATE lessons SET ContentJson=\'\...\' WHERE Id=N. No code
deployment needed. Also QA 50 EQ test questions: are explanations clear
and concise? Fix any ambiguous questions.

**Patterns Practiced**

Content QA mindset (different from feature testing --- read every word
as a first-time user), SQL UPDATE for content fixes via Azure Portal
Query Editor, expression arc coherence review, EQ test question quality
audit

**Learning Takeaway**

*Why content QA requires its own dedicated day --- feature testing asks
\'does this button work?\' Content QA asks \'is this good?\' Different
cognitive modes. When testing features you skim text. When QA-ing
content you read every word as a user would. Feature testing finds
broken buttons. Content QA finds a closing line that is generic instead
of memorable, or a quiz answer that is technically correct but
confusing. 15--20 lessons across all pillars catches systemic issues.*

**Day 42** (Sun) **duckyeq.com landing page + privacy policy + support
page**

**Build**

Build duckyeq.com as a simple HTML/CSS single-page site deployed to
Cloudflare Pages or Vercel (both free). Hero: Quack and Lola side by
side (happy PNGs), app name, tagline \'Build Your EQ. Grow Your
Gratitude. Connect with Friends. 🦆\', App Store badge placeholder.
Features: 5 cards (Home / Learn / Gratitude / Friends / Shop). Build
duckyeq.com/privacy --- REQUIRED by Apple. Must describe: data collected
(email, username, KnownAs, emotion check-ins with optional phrases,
gratitude entries, lesson progress, EQ test scores, friend connections,
Quack reactions --- check-ins + phrases fall under Health & Fitness
category), how used (app functionality only, never sold), how users
delete data (support@duckyeq.com), data retention. Use termly.io as
starting template. Build duckyeq.com/support. Deploy and verify all URLs
load over HTTPS.

**Patterns Practiced**

Static site deployment (Cloudflare Pages), App Store privacy policy
requirements, Health & Fitness data category declaration (emotion
check-ins fall here), HTTPS verification, social data disclosure (friend
connections + Quack reactions must be declared)

**Learning Takeaway**

*Why the privacy policy must explicitly mention emotion check-ins and
friend connections --- when you declare App Privacy in App Store
Connect, you select Health & Fitness for emotion check-ins (moods are
health data) and Contacts for friend connections. Apple\'s reviewers
verify the privacy policy URL you provide actually describes the
declared data types. If the policy does not mention emotion data or
friend connections, your submission can be flagged. Be specific.
Specific privacy policies build trust and pass Apple review.*

**Day 43** (Mon) **App icon + splash screen + 6 App Store screenshots**

**Build**

Finalize app icon: duck face (Quack, happy, front view) on 1024×1024
warm gold-to-orange gradient. No transparency, no rounded corners (Apple
adds rounding). Set in app.json under expo.icon. Splash screen: similar
gradient, duck centered, \'DuckyEQ\' text. Set under expo.splash. Take 6
App Store screenshots using iPhone 15 Pro Max simulator in Xcode
(1290×2796 --- required). Screenshots: (1) Home screen --- check-in
modal with emotion grid --- \'How are you really feeling? 🦆\'. (2) Home
screen --- Friends Panel with friend emotions visible --- \'See how your
friends are feeling today\'. (3) Learn screen --- board game path with
duck on current node --- \'Level up your emotional intelligence\'. (4)
CDER lesson in progress --- \'Learn the way your brain actually works\'.
(5) Gratitude Garden --- \'Grow your gratitude, one moment at a time\'.
(6) Friends screen --- friends list --- \'Build EQ together 🐥\'. Add
caption overlays in Figma or Canva.

**Patterns Practiced**

App icon spec (1024×1024, no alpha, no rounding), Xcode simulator
screenshot workflow (Device \> Screenshot in Simulator menu), ASO
screenshot strategy (first 3 shown in search --- screenshot 2 shows the
Friends social dimension), Figma overlay caption composition

**Learning Takeaway**

*Why screenshot 2 shows the Friends Panel specifically --- the Friends &
Social system is a primary differentiator of DuckyEQ versus solo EQ
learning apps. Showing friends\' emotions in the second screenshot
communicates immediately that this app has a social dimension. Users
comparing DuckyEQ to a solo mood tracker will see the social proof in
screenshot 2 and understand: this app lets you feel less alone in your
EQ journey. That is the message that drives installs.*

**Day 44** (Tue) **Apple Developer enrollment + EAS build + final
regression test**

**Build**

If not enrolled: developer.apple.com/programs/enroll. Individual
enrollment, \$99/year. Verification takes a few hours to 48 hours ---
start immediately. While waiting: final regression test. Delete app,
reinstall fresh via Expo Go. Complete full flow including social:
onboarding → check-in modal → Home (no friends, no quacks) → Test Your
EQ (4 correct, score 40, 2 stars) → Pillar 1 Lesson 1 (3 stars) →
Gratitude (2 entries, Pick Me Up) → Shop (buy and equip accessory,
verify in Home Profile Bar) → Friends (search test user, send request,
accept on other device, see emotion in Friends Panel, send Quack, verify
banner on other device). Fix any remaining bugs. Configure eas.json
(production profile, distribution: store). Run eas credentials to link
Apple Developer account (auto-creates iOS Distribution Certificate and
Provisioning Profile). Run eas build \--platform ios \--profile
production. Budget 3--4 build attempts --- first always has some
configuration issue.

**Patterns Practiced**

Apple Developer enrollment timing (48hr buffer), final regression test
as complete user journey (not feature checklist), code freeze discipline
(no new features after this day), EAS build configuration, eas
credentials for certificate and provisioning profile

**Learning Takeaway**

*Why code freeze starts on Day 44 --- every code change after the
regression test is an untested change. The regression test is your
baseline. Any change after it invalidates the baseline. Code freeze
means: fix only bugs found in today\'s regression. No new features. No
refactoring. No \'one more small thing.\' Ship what you have. Phase 2 is
for everything else.*

**Day 45** (Wed) **App Store Connect listing + submit for review +
launch prep**

**Build**

Go to appstoreconnect.apple.com. New App: Name \'DuckyEQ --- Build Your
EQ\', Primary Language English, Bundle ID com.duckyeq.app, SKU
duckyeq-v1. App Information: Primary Category = Education, Secondary =
Health & Fitness, Privacy Policy URL = duckyeq.com/privacy. Pricing:
Free. Age Rating: 12+ (social features). App Privacy declarations:
Email, Health & Fitness (emotion check-ins + phrases), Contacts (friend
connections), Other User Content (gratitude entries). Upload all 6
screenshots for 6.7\" display. Keywords: \'EQ,emotional
intelligence,SEL,gratitude,mood,feelings,empathy,friends,self-awareness,mindfulness,social
emotional\'. Description highlights the Friends system as a core
differentiator. Review Notes: \'DuckyEQ is an EQ learning app with daily
emotion check-ins, friends who can see how each other are feeling, Quack
reactions (positive gestures between friends), board game lesson
progression with CDER model, 60-second Test Your EQ, gratitude garden
with Pick Me Up, and duck character customization. Demo account:
test@duckyeq.com / DuckyTest2026! --- pre-populated with completed
lessons, friends with today\'s check-ins, an unseen Quack on the Home
screen, gratitude entries, and a shop item equipped.\' Click Submit for
Review. Launch prep: prepare social media content for go-live day.

**Patterns Practiced**

App Store Connect navigation, Health & Fitness + Contacts privacy
nutrition labels, Review Notes with demo account (pre-seeded friends +
check-ins + unseen Quack so reviewers see social features working
immediately), ASO keyword strategy (11 high-value terms)

**Learning Takeaway**

*Why the demo account must have pre-seeded friends with check-ins AND an
unseen Quack --- Apple reviewers test dozens of apps per day. They will
not create two test accounts, add each other as friends, and coordinate
check-ins to see the Friends Panel. Your demo account needs a friend who
has already checked in today and a Quack that appears as a banner
immediately on the Home screen. Show the reviewer your best version of
DuckyEQ from second one. Pre-seeded social data is the difference
between a reviewer who sees the app at its best and one who sees an
empty Friends Panel.*

**Phase Summary**

  ------------- ---------- --------------------------------------------------
  **Phase**     **Days**   **Milestone / Outcome**

  1 --- Backend 1--7       Duck mascot commissioned. Enterprise backend with
  Foundation               ALL domain models including Friendship, Quack,
                           DailyCheckIn. Interfaces, services, behaviors, and
                           full DI registered. EF Core migration applied.

  2 --- API +   8--17      All 40+ endpoints working in Postman including
  Content +                social (CheckIn, Friends, Quacks). 250 lessons
  Azure                    seeded. API live on Azure. Two-user social Postman
                           flow passes.

  3 --- RN +    18--25     Expo app on phone. Onboarding with Known As +
  Home Screen              PascalCase reveal. Daily check-in modal. Home with
                           Quack banner + Friends Panel. Profile with Known
                           As editor.

  4 --- Learn + 26--33     Friends screen (3 tabs). Board game path with duck
  Friends                  on node. CDER lessons, 4 mini-games. Test Your EQ
  Screen                   60-second timer. Social integration verified.

  5 ---         34--40     Gratitude Garden, Pick Me Up, Shop. Full
  Gratitude +              integration test including social. Coin economy
  Shop +                   verified. Monitoring configured.
  Integration              

  6 --- App     41--45     Content QA. duckyeq.com live. Privacy policy with
  Store                    social data disclosures. 6 screenshots. EAS build
  Submission               succeeds. App Store Connect submitted.
  ------------- ---------- --------------------------------------------------

**Phase 2 Preview --- Not in MVP Scope**

These features are designed and ready to build after the MVP App Store
launch. Do not build any of these during the 45-day plan.

  ---------------- -----------------------------------------------
  **Feature**      **Description**

  Arena            Async EQ battles. AI-simulated opponents
                   (IAIOpponentService) for solo play. Pillar
                   leaderboard.

  Ponds            Real-time duck social presence via Azure
                   SignalR. Anonymous avatars in a shared pond.
                   Mood-driven expressions.

  Full Profile     Mood history chart (30 days), lesson stats per
  Screen           pillar, EQ test history, streak history.

  Push             Streak reminders, Quack received, weekly shop
  Notifications    refresh. Via Expo Notifications.

  Admin Content    Add/edit lessons, EQ questions, shop items
  API              without deploying code.

  B2B School       District accounts, anonymized progress views,
  Licensing        RevenueCat subscriptions.
  ---------------- -----------------------------------------------

**🦆 Build the EQ. Grow the Garden. Connect with Friends. Ship the Duck.
🦆**

duckyeq.com · DuckyEQ 45-Day MVP Build Plan (Friends & Social Edition) ·
April 2026
