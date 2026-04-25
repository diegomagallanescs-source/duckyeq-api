**DuckyEQ**

**Lesson Plan Reference**

*CDER model · 12 mini-games*

*Scoring 0--300 · 1 to 3 stars · Daily lesson limit · Infinite retries*

*Share-worthy UI · Duck companion system · 10 expressions*

Reference document --- for solo founder development

**duckyeq.com**

# How to Use This Document

This is the full reference for the lessons inside DuckyEQ. It\'s written
in simple language on purpose --- the same words that would work for a
10-year-old, a high schooler, or a 40-year-old. No jargon. Nothing
fancy. Just direct.

Each lesson has a single unique main learning --- the Core Message. When
a lesson starts, the duck highlights the Core Message first, then the
Objective, then tells a funny joke to loosen the mood before the Connect
stage begins. The lesson structure is always the same: Core Message →
Objective → Quack of the Day → Connect → Define → Engage → Reflect →
Reward.

Everything in this doc is the spec. If you\'re implementing a lesson,
the card for that lesson has every piece you need: five connect
questions, the concept, six to eight flashcards, the game and its
scenario, five reflect questions, the reward tier, the duck\'s
expression arc, and the share-card concept.

## Writing Style

Simple punchy sentences. Analogies over definitions. Real examples from
the duck\'s own life. No abstract philosophy. If a lesson can\'t be
explained to a 10-year-old in two sentences, it\'s probably too
complicated for a 40-year-old too. Simple = universal.

## Timing Target per Lesson

Each lesson runs between 10 and 20 minutes. Rough breakdown: Core
Message + Joke = 30 seconds. Connect = 3--4 minutes. Define = 3--4
minutes. Engage = 4--5 minutes. Reflect = 3--4 minutes. Reward = 60--90
seconds. Users who want to go deep can spend more time on free-response
text inputs. Users in a hurry can type short answers and move through.

# Contents

How to Use This Document

Pillar Overview --- Self-Awareness

Lesson Flow (What Happens on Screen)

The CDER Teaching Model

Duck Expression System (10 Expressions)

Scoring System --- 0 to 300, 1 to 3 Stars

How to Implement Scoring (Backend + Frontend)

UI Sizing Rules & Share-Worthy Design

Game Library (12 Games) and Architecture

Game Rotation (No Consecutive Repeats)

Architecture Map --- How Lessons Plug into the Enterprise Pattern

# Pillars Overview

**Pillar 1: Self-Awareness**

**Pillar Overview** Self-Awareness is the ground floor. It\'s the skill
of noticing what\'s going on inside you --- your feelings, your body,
your patterns, your beliefs --- instead of pretending nothing\'s
happening. Every other pillar needs it. You can\'t manage feelings you
haven\'t noticed. You can\'t understand other people if you don\'t
understand yourself. You can\'t decide well when you don\'t know which
version of you is deciding.

**Pillar Identity in the App** Color: Reflective blue (#4A7FB8),
Environment: Still lake, mirrors, soft ripples, Icon motif: Mirror,
ripple, Mood: Calm, curious, introspective, Capstone reward (Level 50):
Mirror Crown --- permanent cosmetic, cannot be bought

**Pillar 2: Self-Management**

**Pillar Overview** Self-Management is what you do with what you notice.
Awareness without management is just watching yourself make the same
mistakes in real time. This is the skill of pausing before you react,
regulating the feelings that would otherwise run the show, and choosing
your response instead of just having one. It is not about suppressing
emotions --- it is about not letting them drive the car.

**Pillar Identity in the App** Color: Ember orange (#C45C00)
Environment: Forge, fire contained in lantern, warm controlled heat Icon
motif: Flame in a lantern, breath Mood: Grounded, steady, determined
Capstone reward (Level 50): Ember Crown --- permanent cosmetic, cannot
be bought

**Pillar 3: Social Awareness**

**Pillar Overview** Social Awareness is the skill of genuinely noticing
other people --- not just what they say, but what they feel, what they
carry, and what they need. It is the difference between being in a room
and actually being present in it. Most of us are good at recognizing
obvious emotions in people we love. This pillar pushes further: empathy
for strangers, for people unlike you, for the quiet person nobody
checked on, for the human being behind the situation.

**Pillar Identity in the App** Color: Forest green (#2E7D32)
Environment: Open meadow, soft wind, ducks in community, someone always
slightly apart Icon motif: Eye with a leaf, open hand Mood: Warm,
attentive, expansive Capstone reward (Level 50): Empath Crown ---
permanent cosmetic, cannot be bought

**Pillar 4: Relationship Skills**

**Pillar Overview** Relationship Skills are where everything becomes
real. You can understand yourself, manage your emotions, and feel for
others --- but at some point you have to actually show up. This pillar
is about the practical work of being in relationships: listening like
you mean it, saying sorry and meaning it, keeping your word, fighting
without wrecking things, and being the kind of person people can count
on. The research is clear --- nothing in life matters more than the
quality of your close relationships. These are the skills that build
them.

**Pillar Identity in the App** Color: Warm gold (#C77700) Environment:
Cozy dock, lily pads close together, ducks side by side Icon motif: Two
wings overlapping, bridge Mood: Warm, loyal, grounded Capstone reward
(Level 50): Gold Heart --- permanent cosmetic, cannot be bought

**Pillar 5: Responsible Decision-Making**

**Pillar Overview** Responsible Decision-Making is the capstone --- the
skill that puts all four others into action. Knowing yourself, managing
your state, understanding others, and showing up in relationships all
feed into this: the moment you have to choose. Every day is full of
forks. This pillar gives you a framework --- stop, think, act with
intention --- and teaches you to apply it under pressure, in dilemmas,
across time, and with other people in the frame. It is the skill that
turns everything you have learned into the person you are becoming.

**Pillar Identity in the App** Color: Deep violet (#6A1B9A) Environment:
Crossroads at dusk, lantern light, three paths forward Icon motif:
Stoplight, compass Mood: Deliberate, courageous, clear Capstone reward
(Level 50): EQ Champion --- permanent cosmetic and profile frame, cannot
be bought, earned only by completing all five pillars

# Lesson Flow (What Happens on Screen)

Every lesson follows the same seven-step flow. The duck\'s size and
expression change throughout to match the emotional tone. The whole
thing is designed so content stays the star and the duck supports
without distracting.

### 1. Lesson Opens --- Core Message & Objective (0:00--0:15)

**Screen:** Big duck, centered. Core Message floats in big text. Duck
says it out loud (speech bubble).

**Purpose:** Before anything else, the user sees the one main idea of
this lesson. The duck \'highlights\' it so it sticks. Then the Objective
appears as a smaller second line.

**Duck size:** LARGE (about 60% of screen width). Duck in \'wise\' pose
or \'excited\'.

### 2. Quack of the Day (0:15--0:45)

**Screen:** Duck setup with curious expression on the left side. User
taps screen to reveal punchline --- duck switches to happy/playful.

**Purpose:** Funny moment to break the ice. Resets the user into a warm,
playful mood before the lesson begins.

**Duck size:** MEDIUM (about 40% of screen width). Setup side +
punchline side.

### 3. CONNECT (1:00--4:00)

**Screen:** Duck goes BIG here --- about 70% of screen width. Feels like
the duck is right there with you. Text input fields appear as chat-style
bubbles.

**Purpose:** Five free-response questions. Genuine, warm, a little
personal. Typed answers stay on device only. Never sent to server. User
can type short or long --- whatever they want.

**Duck size:** LARGE / HERO. This is the bonding moment with the duck.

### 4. DEFINE (4:00--7:00)

**Screen:** Duck shrinks to small corner avatar. Flashcards fill the
screen. User taps each card to advance.

**Purpose:** 6--8 flashcards explaining the concept with simple
analogies and metaphors. Each card is a single, easy-to-read sentence or
short example.

**Duck size:** SMALL (about 20% of screen, corner). Content is the star.

### 5. ENGAGE (7:00--12:00)

**Screen:** Full game takeover. Duck only appears in small corner to
react to user actions (celebrates correct answers, encourages after
wrong ones).

**Purpose:** One of 12 mini-games. User earns points (0--300) based on
correct answers.

**Duck size:** TINY or HIDDEN. Game gets the space.

### 6. REFLECT (12:00--16:00)

**Screen:** Duck returns to medium size, seated beside text input area.
Five questions, one at a time.

**Purpose:** Five questions. The FIRST question is always \'What did you
learn from this lesson?\' --- this is non-negotiable across every lesson
in every pillar. The other four are topic-specific.

**Duck size:** MEDIUM (about 40% of screen width). Supportive posture.

### 7. REWARD (16:00--17:30)

**Screen:** Score card animates in: big number (0--300) with stars
(1--3) underneath. Duck does celebration pose. Coin shower effect.

**Purpose:** Dopamine hit. Score + stars + coins awarded. Share card
generated. User can tap to share.

**Duck size:** LARGE, celebratory pose. This is the victory moment.

# The CDER Teaching Model

Every lesson follows four learning stages after the intro and joke:
Connect, Define, Engage, Reflect. The rhythm is deliberate. Connect is
slow and personal. Define is medium and informative. Engage is fast and
fun. Reflect is slow and quiet. This pacing keeps a 15-minute session
from feeling monotone.

### Connect (3--4 min)

**Purpose:** Bond the user to the duck. Get them relaxed and talking
before the lesson begins. Five free-response questions. Some
topic-related, some not. At least one is light and off-topic --- the
goal is human connection, not quizzing.

**Mechanics:** Five questions, each with a text input box. User can type
as much or as little as they want. Answers are never sent to the server
--- they exist only in local state. This is the private, safe space.
Duck is LARGE on screen to feel close.

**Pacing tip:** Duck reacts briefly after each submission (\'Love
that.\' / \'Same, honestly.\'). The duck follow-ups add a few seconds of
bonding without delaying much.

### Define (3--4 min)

**Purpose:** Teach the main concept using simple analogies, metaphors,
and examples. The duck explains it the way the duck does it ---
personal, concrete, friendly. No jargon.

**Mechanics:** 6--8 tappable flashcards. Each card is a single short
sentence or a quick example. User taps to advance. Duck expression
changes per card to match tone.

**Pacing tip:** Include one flashcard with a mini brain-fact (\'Did you
know your brain notices feelings 5× faster than thoughts?\') for the
\'huh\' moment.

### Engage (4--5 min)

**Purpose:** Turn the concept into a game. Score from 0 to 300 based on
correct answers. Stars awarded at the end.

**Mechanics:** One of 12 games from the library. Game is parameterized
to this lesson\'s topic. Duck appears only in corner to cheer or
encourage. Different game from the previous lesson --- no consecutive
repeats.

**Pacing tip:** Best-of-3 or multi-round format stretches engagement and
gives more chances to score high.

### Reflect (3--4 min)

**Purpose:** Lock in the learning. Five free-response questions. The
first is ALWAYS \'What did you learn from this lesson?\' --- this is
standard across every lesson in every pillar. Other four are
topic-specific.

**Mechanics:** One question at a time. User types. After all five, a
\'Save to my journal?\' toggle appears (defaults ON for levels 11+, OFF
for levels 1--10 where users are still new).

**Pacing tip:** Duck closes with a short line before the reward. The
closing line is the kind of quote users would want to screenshot.

## Why Connect Answers Are Never Stored

During Connect, users are asked slightly personal questions. Their
answers live only in local component state on their phone. They are
never sent to any server. This is a privacy promise and should be made
visible in the UI --- a small \'only on your phone\' line beneath the
input. This lets users type honestly without performing for a database.

## Reflect Answers Can Be Saved

During Reflect, the user sees an optional \'Save to journal\' toggle
after answering. Default: OFF for Tier 1 (users are still warming up),
ON for Tier 2 and later. Saved reflections appear in their private
journal on the Profile screen. These power future pattern-insight
features.

# Duck Expression System --- 10 Expressions

The Fiverr art commission is for ten distinct duck expressions. No more,
no less. These cover every emotional beat the lessons need. Each
expression is delivered as a PNG at 1x/2x/3x for both Quack (boy) and
Lola (girl). The frontend picks the right asset based on the user\'s
chosen character.

  ---------------------------------------------------------------------------
  **Expression**    **When to use**                  **Art direction note**
  ----------------- -------------------------------- ------------------------
  **happy**         Default state, greetings,        *Main \'smiling duck\'
                    celebrations, punchlines landing used for good moments.*

  **curious**       Asking a question, setting up a  *Head tilted, eyes wide
                    joke, starting a flashcard       with interest.*

  **thoughtful**    Reflection prompts, heavy        *Hand (wing) on chin
                    concepts, asking the user to     pose.*
                    think                            

  **excited**       Correct answers in games, reward *Arms raised, big smile,
                    moments, level-ups               energy pose.*

  **calm**          Breathing exercises, meditation  *Sitting lotus or
                    moments, low-key learning        floating on water.*

  **playful**       Jokes, fun flashcards, casual    *Mischievous grin,
                    moments                          winking.*

  **proud**         User finishes something hard,    *Chest out, small smile,
                    milestone moments, capstone      badge/medal.*

  **sympathetic**   Hard emotional topics, user      *Soft eyes, leaning in,
                    shares something tough, support  one wing outstretched.*
                    moments                          

  **encouraging**   User struggling, retry prompts,  *Thumbs up, big
                    bounce-back moments              reassuring smile.*

  **surprised**     Brain-fact reveals, twist        *Eyes wide, beak open
                    moments, plot surprises          --- not scared, just
                                                     amazed.*
  ---------------------------------------------------------------------------

## How Expressions Drive a Lesson

Each lesson card in this document has a Duck Expression Arc showing one
expression per CDER stage. The lesson engine also exposes a DuckContext
to game components so they can override the expression during gameplay
--- excited for correct answers, encouraging on retries, surprised on a
brain-fact reveal. Jokes are tagged with two expressions too: curious
for the setup, something upbeat (happy, playful, proud) for the
punchline.

# Scoring System --- 0 to 300, 1 to 3 Stars

Every lesson earns the user a score of 0 to 300 points and a star rating
of 1 to 3 stars. Score comes from how many correct answers the user got
during the Engage game. Users can retry lessons as many times as they
want --- but their score only updates if they beat their best score.

## Score and Star Ranges

**Score range:** 0 to 300

**Score formula:** score = round( (correctAnswers / totalQuestions) ×
300 )

#### Star thresholds

- 0 -- 100 points → 1 star → \"Good effort\"

- 101 -- 200 points → 2 stars → \"Nice work\"

- 201 -- 300 points → 3 stars → \"Quack-tastic!\"

## Retry Rules

- Users can redo any completed lesson infinitely.

- Score only updates if the new score is higher than the stored best
  score.

- Star count follows the latest best score.

- Retrying a lesson you already completed does NOT count toward the
  daily lesson limit.

- Retrying gives a small coin bonus (+5) if the score improves --- but
  no big rewards for repeats.

- The coin reward for first completion still scales with the number of
  stars earned.

## Abandon Rules (User Backs Out)

- If the user navigates away from the lesson (closes app, taps another
  tab, goes to phone home screen) before completing the Reflect stage,
  the lesson session is dropped.

- A session token (created on lesson start) expires after 30 minutes of
  inactivity.

- No partial progress is saved. User must restart the lesson from
  Connect.

- This keeps lessons focused --- users either finish in one sitting or
  start fresh.

## Daily Lesson Limit

- Users can only start ONE new lesson per pillar per 24 hours.

- A \'new\' lesson means one the user has never fully completed before.

- Retries of completed lessons do NOT count toward the daily limit.

- Cooldown resets on a rolling 24-hour basis from the first completion
  timestamp.

- UI shows \'Next new lesson unlocks in 14h 22m\' style countdown on
  locked pillars.

- Retry button stays available on any completed lesson at all times.

# How to Implement Scoring (Backend + Frontend)

The full technical spec for how score, stars, retries, abandonment, and
daily limits actually work in code. This maps directly onto the
Controller → Behavior → Service → Repository pattern already set up for
DuckyEQ.

SCORING IMPLEMENTATION (Controller → Behavior → Service → Repository)

DATABASE (Entity Framework Core)

────────────────────────────────

Add the UserLessonProgress entity:

public class UserLessonProgress

{

public int Id { get; set; }

public string UserId { get; set; }

public int LessonId { get; set; }

public int BestScore { get; set; } // 0-300, best ever

public int BestStars { get; set; } // 1-3, matches best score

public int TotalAttempts { get; set; }

public DateTime? FirstCompletedAt { get; set; } // null until first pass

public DateTime LastAttemptedAt { get; set; }

public Lesson Lesson { get; set; }

public ApplicationUser User { get; set; }

}

Unique index on (UserId, LessonId) to enforce one progress row per user
per lesson.

API ENDPOINTS

──────────────

POST /api/lessons/{id}/start

Body: (empty)

Returns: { sessionToken: string, expiresAt: ISO8601 }

Creates an in-memory session entry (IMemoryCache, keyed by token, 30-min
TTL)

that stores userId + lessonId + startedAt.

POST /api/lessons/{id}/complete

Body: { sessionToken: string, correctAnswers: int, totalQuestions: int }

Returns: { score: int, stars: int, isNewBest: bool, coinsAwarded: int,
isFirstCompletion: bool }

BEHAVIOR LAYER

──────────────

public async Task\<LessonCompleteResult\> CompleteAsync(string userId,
int lessonId, CompleteDto dto)

{

// Step 1: Validate session token

var session = \_sessionService.Get(dto.SessionToken);

if (session == null \|\| session.UserId != userId \|\| session.LessonId
!= lessonId)

throw new UnauthorizedException(\"Session invalid or expired. Restart
the lesson.\");

// Step 2: Calculate score and stars

int score = \_scoringService.CalculateScore(dto.CorrectAnswers,
dto.TotalQuestions);

int stars = \_scoringService.GetStars(score);

// Step 3: Get existing progress

var existing = await \_progressRepo.GetByUserAndLessonAsync(userId,
lessonId);

bool isFirstCompletion = existing?.FirstCompletedAt == null;

bool isNewBest = existing == null \|\| score \> existing.BestScore;

// Step 4: Update progress (best score, stars, attempt count)

await \_progressRepo.UpsertAsync(userId, lessonId, score, stars,
isFirstCompletion, isNewBest);

// Step 5: Award coins (only on first completion OR score improvement on
retry)

int coins = 0;

if (isFirstCompletion)

{

coins = \_coinService.BaseCoinsForStars(stars); // 1⭐=20, 2⭐=40,
3⭐=60

await \_pillarProgressService.UnlockNextLessonAsync(userId, lessonId);

}

else if (isNewBest)

{

coins = 5; // small improvement bonus

}

if (coins \> 0) await \_coinService.AwardAsync(userId, coins);

// Step 6: Clean up the session token

\_sessionService.Remove(dto.SessionToken);

return new LessonCompleteResult { Score = score, Stars = stars,
IsNewBest = isNewBest, CoinsAwarded = coins, IsFirstCompletion =
isFirstCompletion };

}

SCORING SERVICE

────────────────

public int CalculateScore(int correct, int total)

{

if (total \<= 0) return 0;

correct = Math.Clamp(correct, 0, total);

return (int)Math.Round((double)correct / total \* 300);

}

public int GetStars(int score)

{

if (score \>= 201) return 3;

if (score \>= 101) return 2;

return 1;

}

DAILY COOLDOWN SERVICE

───────────────────────

public async Task\<DailyCooldownStatus\> GetPillarStatusAsync(string
userId, int pillarId)

{

var cutoff = DateTime.UtcNow.AddHours(-24);

var recentNew = await \_progressRepo.Query()

.Where(p =\> p.UserId == userId)

.Where(p =\> p.Lesson.PillarId == pillarId)

.Where(p =\> p.FirstCompletedAt != null && p.FirstCompletedAt \>=
cutoff)

.AnyAsync();

if (!recentNew) return DailyCooldownStatus.Available;

var nextAvailable = (await \_progressRepo.Query()

.Where(p =\> p.UserId == userId && p.Lesson.PillarId == pillarId &&
p.FirstCompletedAt \>= cutoff)

.OrderBy(p =\> p.FirstCompletedAt)

.FirstAsync()).FirstCompletedAt.Value.AddHours(24);

return new DailyCooldownStatus { Locked = true, NextAvailableAt =
nextAvailable };

}

Note: Retries of already-completed lessons are NEVER blocked by this
cooldown --- only new-lesson starts.

SESSION TOKEN SERVICE (in-memory, simple)

──────────────────────────────────────────

IMemoryCache entry keyed by GUID token string.

Value: { UserId, LessonId, StartedAt }.

TTL: 30 minutes (handles \'user backed out\' case automatically --- no
DB cleanup job needed).

FRONTEND (React Native / Expo)

───────────────────────────────

useLesson(lessonId) hook:

\- On mount: POST /start → stores sessionToken in state.

\- Tracks gameState = { correctAnswers, totalQuestions } during Engage.

\- On Reflect complete: POST /complete with session + game state →
receives score/stars/coins.

\- On unmount (navigation away, app backgrounded for \>30 min): do
nothing --- session expires naturally on the backend.

useEffect cleanup on abandon:

\- Optional: fire a POST /api/lessons/{id}/abandon to clear the session
immediately (non-critical; the TTL handles it anyway).

Score animation:

\- On reward screen, animate the score counter from 0 to finalScore over
\~1.2 seconds.

\- Stars fill in one at a time with a satisfying \'click\' sound per
star.

\- If isNewBest: show \'NEW BEST!\' banner.

# UI Sizing Rules & Share-Worthy Design

Two interlocking design goals: the duck supports the content without
competing with it, and every lesson reward becomes something users want
to screenshot and share.

## Duck Sizing Rules

- LARGE (\~70% screen width): Used in Connect (bonding moment) and
  Reward (celebration).

- MEDIUM (\~40% screen width): Used in Quack of the Day setup/punchline
  and Reflect (supportive posture).

- SMALL (\~20% screen width, corner): Used in Define (content-first).
  Duck reacts but doesn\'t distract.

- TINY or HIDDEN: Used in Engage (game is the star). Duck pops in at
  corner to celebrate correct answers.

- Every screen has generous white space. Content breathes. Avoid visual
  clutter.

## What Triggers a Share-Worthy Moment

- After every lesson reward --- user sees their score, stars, and a
  beautiful share card with the Core Message and the duck in celebration
  pose. Tap-to-share.

- Streak milestones (3 days, 7 days, 14, 30, 100) --- auto-generated
  share card with streak flame count.

- Capstone moments (Level 50 of any pillar) --- 10-second cinematic that
  is screen-recordable. Users will share to TikTok.

- Perfect runs (3 stars on first attempt) --- \'FLAWLESS\' badge
  animation.

- Monthly EQ recap card --- generated once per month with the user\'s
  top insights and stats.

- Quack of the Day --- even outside lessons, users can tap the duck on
  the home screen to get a random joke card and share it.

## Share Card Design

**Dimensions:** Primary: 1080×1920 (IG Story/TikTok vertical).
Secondary: 1080×1080 (IG square).

**Composition:** Pillar-themed background gradient (reflective blue for
Self-Awareness). Duck in celebration pose centered or left-aligned. Core
Message in large friendly font. Star rating. Level number and pillar
name. Small \'duckyeq.com\' watermark bottom-right.

**Generation:** Backend endpoint POST /api/share/lesson-card returns a
signed CDN URL pointing to a pre-rendered PNG. Alternatively, generate
client-side with react-native-view-shot on an off-screen React component
--- saves backend image processing.

**Variants:** Three automatic color variants per pillar so users can
pick the version they like best before sharing.

**Personalization:** User\'s duck name shown subtly (e.g., \'Quack & Leo
⭐⭐⭐ Level 5\'). Duck wears any equipped shop cosmetics.

## Extra Magic to Make Sharing Happen

- Auto-copy a pre-written share caption to clipboard when the user taps
  Share (e.g., \'Just earned 3 stars on Level 5 of DuckyEQ. Who\'s in?
  duckyeq.com #DuckyEQ\')

- Subtle referral code embedded in share link --- gives both users bonus
  coins if a friend joins.

- Screenshots of lesson screens have a watermark too --- can\'t avoid
  the duckyeq.com branding even on organic shots.

- Shared mood check-ins include the duck\'s expression matching the mood
  --- makes people curious what the app is.

- \'Quote of the Day\' lock-screen image --- a pillar-themed quote with
  the duck, exportable as phone wallpaper.

# Game Library (12 Games) and Architecture

The twelve-game library is shared across all five pillars. The key
architectural insight: each game is a generic engine powered by
configuration JSON. The same Emotion Match Racer code runs for
Self-Awareness Level 1 (matching feelings to faces) as for Social
Awareness Level 5 (matching body language to emotion) --- only the pairs
array changes.

This means adding a new lesson never requires new game code. Only a new
config blob. This is how the codebase stays small while content scales.

### 1. Emotion Match Racer

**Concept:** A fast-paced matching game. Emotion words on one side,
duck-face cards on the other. User taps pairs to match. Every correct
match pushes the duck\'s lily pad forward.

**Mechanics & Scoring:** 2-column match layout. 60-second round. Combo
multiplier on streaks. Score formula: correctMatches × (300 /
totalPairs).

**React Native / Expo Implementation:** React Native Reanimated for
lily-pad movement. FlatList for card deck. Gesture-handler for taps.
State: { currentPairs, score, streak, lilyPosition }. Sound via expo-av.

**Domain Model (config shape):** Game definition stored as JSON: {
pairs: \[{ left, right }\], roundSeconds, comboMultiplier }. Same engine
powers every pillar\'s lesson --- only the pairs array changes.

**Database Impact:** No game-specific tables. Lesson.EngageConfigJson
column holds the game config. GameSession entity tracks in-progress
rounds if needed.

**Used for:** Matching feelings to faces, words to concepts, triggers to
feelings, etc.

### 2. Duck Pond Drag-Drop

**Concept:** Several ponds on screen, each labeled. Items float down
from the top. User drags each into the correct pond before it hits the
bottom.

**Mechanics & Scoring:** Progressive drop speed. Mis-sorted items bounce
back with a splash. Score formula: correctPlacements × (300 /
totalItems).

**React Native / Expo Implementation:** react-native-gesture-handler
PanGestureHandler with Reanimated. Collision detection via
measureInWindow on drop zones. Items: { id, text, correctPondId }.

**Domain Model (config shape):** Config: { ponds: \[{ id, label }\],
items: \[{ text, correctPondId }\], dropSpeed, maxMistakes }.

**Database Impact:** Same generic engine table. Config swaps per lesson.

**Used for:** Sorting values into categories, identifying triggers,
classifying behaviors, etc.

### 3. Quack Quest (Branching Story)

**Concept:** A short illustrated choose-your-own-adventure. User reads a
scene, picks 1 of 3 choices, scene advances. Score based on which path
they took.

**Mechanics & Scoring:** 3--5 decision points per story. Each choice
tagged as \'best\', \'okay\', or \'suboptimal\'. Score = sum of choice
ratings × (300 / maxPossible).

**React Native / Expo Implementation:** JSON-tree data structure. Render
engine traverses the tree. react-native-svg for path visualization in
post-story debrief.

**Domain Model (config shape):** Config: { startNodeId, nodes: { id,
text, choices: \[{ text, nextNodeId, rating }\] } }. Tree stored as
JSON.

**Database Impact:** Story JSON stored in Lesson.EngageConfigJson.
StoryChoice entity could log user choices for analytics, but optional.

**Used for:** Decision-making stories, relationship scenarios, ethical
dilemmas, pattern-recognition narratives.

### 4. Feather Falls (Sorter)

**Concept:** Feathers fall from the top of the screen. Each feather
carries a word or phrase. User taps left or right to steer it into the
correct column.

**Mechanics & Scoring:** 2--3 columns. Feathers fall at increasing
speed. Wrong placements = -1 life. 3 lives total. Score = correctSorts ×
(300 / totalFeathers).

**React Native / Expo Implementation:** Grid-based state. setInterval at
60 FPS for game loop. Active feather.y += speed per tick. Edge taps move
left/right.

**Domain Model (config shape):** Config: { columns: \[{ label }\],
feathers: \[{ text, correctColumnId }\], baseSpeed, acceleration }.

**Database Impact:** Same generic engine.

**Used for:** Binary or trinary sorting: feelings vs. thoughts, healthy
vs. unhealthy coping, mine vs. inherited shoulds.

### 5. Breath Bubble Blower

**Concept:** A bubble grows and shrinks in rhythm. The user breathes
along --- inhale as it grows, exhale as it shrinks. Completing X cycles
unlocks a zen-duck scene.

**Mechanics & Scoring:** 3--5 breath cycles per round. Scoring is
participation-based: tapping to confirm each cycle = full points. Score
= cyclesCompleted × (300 / totalCycles).

**React Native / Expo Implementation:** Reanimated withSequence +
withTiming to animate circle scale over the breath pattern. Optional
background audio via expo-av.

**Domain Model (config shape):** Config: { pattern: \[inhaleSeconds,
holdSeconds, exhaleSeconds, holdSeconds\], totalCycles }.

**Database Impact:** Generic engine.

**Used for:** Breathing exercises, grounding practice, observer-self
meditations, body-scan training.

### 6. Pond Balance (Tilt)

**Concept:** Duck stands on a lily pad. Tilt the phone to keep balanced.
Items float toward the duck --- tilt toward the ones that fit, away from
the ones that don\'t.

**Mechanics & Scoring:** Accelerometer controls pad position. 30-second
rounds. Progressive difficulty. Score = correctCollections × (300 /
totalTargets).

**React Native / Expo Implementation:** expo-sensors DeviceMotion for
accelerometer x-axis. Reanimated withSpring for smooth pad movement.

**Domain Model (config shape):** Config: { targets: \[{ text, isCorrect
}\], spawnRate, roundSeconds }.

**Database Impact:** Generic engine.

**Used for:** Focus-under-pressure lessons, values-in-stress lessons,
centering practice.

### 7. Empathy Eyes

**Concept:** A close-up of a pair of illustrated eyes appears. User
picks the emotion from 4 choices. Advanced rounds use subtle
micro-expressions.

**Mechanics & Scoring:** 10 images per round. 5-second decision timer
per image. Score = correctChoices × (300 / totalImages).

**React Native / Expo Implementation:** Pre-rendered illustrations as
image assets. Standard multiple-choice UI. After-answer overlay shows
the full face.

**Domain Model (config shape):** Config: { images: \[{ asset,
correctAnswer, options, explanation }\], timerSeconds }.

**Database Impact:** Image assets stored in Azure Blob Storage or
bundled with app. Config references asset filenames.

**Used for:** Face-reading, micro-expression recognition, emotion
identification.

### 8. Duck Detective (Scene Scrub)

**Concept:** A static illustrated scene (classroom, cafe, family
dinner). User taps hotspots to collect clues about what\'s happening.
Then answers 3--4 questions.

**Mechanics & Scoring:** 4--6 hotspots per scene. Collected clues appear
in a notebook UI. Final questions score the user. Score =
correctFinalAnswers × (300 / numQuestions).

**React Native / Expo Implementation:** react-native-svg scene with
invisible Pressable regions at hotspot coords. State: cluesFound array.
Notebook as Modal.

**Domain Model (config shape):** Config: { sceneAsset, hotspots: \[{ x,
y, width, height, clueText }\], questions: \[{ text, options,
correctIndex }\] }.

**Database Impact:** Scene assets in Blob Storage. Config references
filenames.

**Used for:** Observation practice, context-reading, synthesis lessons,
\'what\'s going on?\' scenarios.

### 9. Conflict Courtroom

**Concept:** Two characters are arguing. User picks what the first
character says next. The other character\'s emotion meter reacts ---
escalate and they blow up, de-escalate and they open up.

**Mechanics & Scoring:** Emotion meter 0--100. Rounds end when meter = 0
(resolved) or = 100 (blown up). Score = 300 if resolved in ≤N turns;
lower if took longer or blew up.

**React Native / Expo Implementation:** Quack Quest tree engine + added
emotionMeter state. Each choice has meterDelta. Meter visualized with
Reanimated color interpolation.

**Domain Model (config shape):** Config: { startMeter, maxTurns, tree: {
nodes, choices with meterDelta } }.

**Database Impact:** Extends Quack Quest config.

**Used for:** Relationship repair, hard conversations, apology practice
(mostly Pillar 4 but occasional Pillar 1 use).

### 10. Quack Chat Simulator

**Concept:** A fake texting interface. A friend\'s messages come in one
at a time. User picks from 3 possible replies. Friend reacts
authentically.

**Mechanics & Scoring:** Typing-indicator delays for realism.
Chat-bubble animations. Score = goodRepliesChosen × (300 / totalTurns).

**React Native / Expo Implementation:** FlatList inverted for chat.
Messages: { id, sender, text }. Choices at bottom. Typing indicator =
animated-dot component during setTimeout delay.

**Domain Model (config shape):** Config: { conversation: \[{ senderLine,
replies: \[{ text, quality: \'best\'\|\'okay\'\|\'bad\', nextSenderLine
}\] }\] }.

**Database Impact:** Generic engine.

**Used for:** Digital-tone practice, supportive replies, hard
conversations, reframing identity.

### 11. Emotion Mixer Lab

**Concept:** Science-lab aesthetic. Drag primary-emotion beakers into a
mixing flask. Correct combinations reveal named complex emotions.

**Mechanics & Scoring:** 12 beakers unlock over time. 20+ combinations
to discover. Score = combosFound × (300 / totalTargetCombos).

**React Native / Expo Implementation:** Drag-drop beakers (same pattern
as Pond Drag-Drop). Combo lookup in a static JS object. Glow animation
on discovery.

**Domain Model (config shape):** Config: { availableBeakers,
targetCombos: \[{ ingredients, result, fact }\] }. Persist discovered
combos.

**Database Impact:** UserDiscoveredCombos entity tracks which combos
each user has found --- so they persist across attempts.

**Used for:** Emotional-vocabulary lessons, mixed-feelings lessons,
nuanced-emotion discovery.

### 12. Feather Frenzy (Tap Catcher)

**Concept:** Feathers fall from the top carrying words or behaviors.
User taps only the ones that fit the lesson\'s theme. Wrong taps lose
points.

**Mechanics & Scoring:** Arcade scoring with combo multipliers.
45-second round. Score based on correct taps minus wrong taps, scaled to
300.

**React Native / Expo Implementation:** Reanimated withTiming loops to
animate feather translateY. Pool of active feathers managed with useRef.
Tap with hitSlop for feel.

**Domain Model (config shape):** Config: { feathers: \[{ text, isCorrect
}\], spawnRate, roundSeconds }.

**Database Impact:** Generic engine.

**Used for:** Catch-the-pauses practice, healthy-coping identification,
values-spotting, positive-self-talk selection.

# Game Rotation (No Consecutive Repeats)

GAME ROTATION RULE

No two consecutive lessons use the same minigame. This keeps the user
interested and

makes each lesson feel different from the one before. The rule is
enforced at content-

authoring time (a spreadsheet of lesson-to-game assignments) rather than
in code.

Because all 12 games share one architectural pattern (config JSON → game
component), the

backend never has to care which game is which. Each lesson simply has a
gameType string

and a gameConfig JSON blob. The frontend picks the correct game
component based on

gameType and feeds it the config.

Example game rotation for Pillar 1:

L1 EMR, L2 EE, L3 FF, L4 DPDD, L5 QS, L6 EMR, L7 BBB, L8 FF, L9 QQ, L10
DD,

L11 QS, L12 DPDD, L13 FF, L14 DPDD, L15 QS, L16 FF, L17 QQ, L18 EMR, L19
DD, L20 DPDD,

L21 QQ, L22 FF, L23 DPDD, L24 QS, L25 BBB, L26 PB, L27 QS, L28 FF, L29
DPDD, L30 DD,

L31 EML, L32 FF, L33 QQ, L34 DPDD, L35 QS, L36 FF, L37 QS, L38 BBB, L39
FF, L40 DD,

L41 BBB, L42 QQ, L43 BBB, L44 EML, L45 EMR, L46 QQ, L47 DPDD, L48 QS,
L49 BBB, L50 DD.

Abbreviations: EMR = Emotion Match Racer, EE = Empathy Eyes, FF =
Feather Falls,

DPDD = Duck Pond Drag-Drop, QS = Quack Chat Simulator, BBB = Breath
Bubble Blower,

QQ = Quack Quest, DD = Duck Detective, PB = Pond Balance, EML = Emotion
Mixer Lab.

# Architecture Map --- How Lessons Plug into the Enterprise Pattern

ARCHITECTURE MAP --- HOW LESSONS PLUG INTO THE ENTERPRISE PATTERN

This section maps every piece of a lesson to the Controller → Behavior →
Service →

Repository layers already established for the DuckyEQ backend. The goal:
build the

lesson engine once, then swap content per lesson without touching code.

DOMAIN ENTITIES (Entity Framework Core)

────────────────────────────────────────

Pillar Id, Name, ColorHex, DisplayOrder

Lesson Id, PillarId, Level, Title, CoreMessage, Objective,

JokeSetup, JokePunchline, JokeSetupExpr, JokePunchlineExpr,

DefineConcept, DefineFlashcardsJson, EngageGameType,

EngageConfigJson, RewardTier, DuckArcJson, ShareCardConfigJson,

CreatedAt, UpdatedAt

LessonConnectQuestion Id, LessonId, QuestionText, DisplayOrder

LessonReflectQuestion Id, LessonId, QuestionText, DisplayOrder

UserLessonProgress Id, UserId, LessonId, BestScore, BestStars,
TotalAttempts,

FirstCompletedAt, LastAttemptedAt

UserJournalEntry Id, UserId, LessonId, QuestionText, AnswerText,
CreatedAt

(only created if user opts in to save reflection)

Note: Connect answers are NEVER stored. There is no LessonConnectAnswer
entity on

purpose. This is the privacy promise.

CONTROLLER LAYER

─────────────────

LessonsController

GET /api/lessons?pillarId={id} returns lessons for pillar
(locked/available/completed state)

GET /api/lessons/{id} returns full lesson content

POST /api/lessons/{id}/start creates session token, returns token +
expiry

POST /api/lessons/{id}/complete validates session, awards score+coins,
returns result

POST /api/lessons/{id}/reflect saves reflection answers (only if user
toggled save)

GET /api/pillars/{id}/cooldown-status returns available/locked +
countdown

BEHAVIOR LAYER (orchestration)

───────────────────────────────

LessonsBehavior

StartLessonAsync checks daily cooldown, creates session, returns lesson
content

CompleteLessonAsync validates session, calculates score/stars, updates
progress,

awards coins, unlocks next lesson on first completion

SaveReflectionAsync creates UserJournalEntry rows (only saved answers)

SERVICE LAYER (business rules)

───────────────────────────────

ScoringService CalculateScore, GetStars, BaseCoinsForStars

PillarProgressService GetCurrentLevel, UnlockNextLesson,
GetCompletedLessonsList

DailyCooldownService CanStartNewLesson, GetNextAvailableAt

SessionTokenService Create (with 30-min TTL), Get, Remove

CoinService AwardCoins, GetBalance

JournalService SaveReflectionAnswers, GetUserJournal

REPOSITORY LAYER

─────────────────

LessonRepository GetByPillarAsync, GetByIdAsync (with navigation props)

UserLessonProgressRepository GetByUserAndLessonAsync, UpsertAsync,
GetBestScoresAsync

JournalRepository InsertAsync, GetByUserAsync (paged)

FRONTEND COMPONENT MAP (React Native / Expo)

─────────────────────────────────────────────

screens/LessonScreen.tsx top-level routing component; holds sessionToken
state

├─ stages/CoreMessageStage.tsx shows core message + objective with BIG
duck

├─ stages/JokeStage.tsx setup with curious duck → tap → punchline with
happy duck

├─ stages/ConnectStage.tsx 5 questions, each with text input; answers
local only

├─ stages/DefineStage.tsx flashcard carousel, tap to advance, small duck
in corner

├─ stages/EngageStage.tsx renders one of 12 game components based on
gameType

│ ├─ games/EmotionMatchRacer.tsx

│ ├─ games/DuckPondDragDrop.tsx

│ ├─ games/QuackQuest.tsx

│ └─ \... (one per game)

├─ stages/ReflectStage.tsx 5 questions, save-to-journal toggle

└─ stages/RewardStage.tsx score animation, star fill, coin shower, share
card

components/DuckAvatar.tsx renders Quack or Lola at a given size +
expression

components/ShareCard.tsx off-screen component captured via
react-native-view-shot

hooks/useLesson.ts handles session lifecycle (start, complete, abandon)

hooks/usePillarProgress.ts current level, completed list, cooldown
countdown

CONTENT AUTHORING WORKFLOW

────────────────────────────

For MVP: lessons are seeded via an EF Core migration or via a POST
/api/admin/seed-lesson

endpoint (protected by JWT + admin role claim). The seeding payload is a
JSON version of the

lesson card from this document. One JSON file per lesson in a seed/
folder checked into

source control.

Post-MVP: build a simple admin panel that writes to the same endpoint.
Non-developers can

edit lesson content without touching code.

TESTING STRATEGY

─────────────────

Unit tests: ScoringService.CalculateScore (including edge cases: 0
total, more correct than

total, negative input), GetStars (boundary tests: 100, 101, 200, 201).
DailyCooldownService

(time-travel tests using a fake IClock).

Integration tests: Full lesson completion flow via
WebApplicationFactory. Verify session

token rejection when expired, cooldown enforcement, coin awards on first
completion vs retry.

Frontend: detox or maestro end-to-end tests for a full lesson
run-through on at least one

game of each complexity tier.

*---*
