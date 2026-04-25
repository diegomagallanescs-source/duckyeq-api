**DuckyEQ**

**Friends & Social System --- MVP Implementation Guide**

Phase 1 MVP · April 2026

# Overview

The friends system is a core MVP differentiator. It gives DuckyEQ a
social heartbeat that learning-only EQ apps lack. The feature has three
pillars: daily emotion check-in, friend connections, and quack
reactions. Together they create a lightweight emotional presence layer
between users without requiring heavy real-time infrastructure in Phase
1.

# 1. User Identity --- Username & Known As

On profile creation, every user is assigned a permanent, unique username
(e.g. sunny_duck_42). A separate \'Known As\' field holds the name they
actually go by. Both fields are surfaced in the friends UI so people can
identify each other easily.

+---------------------------------------------------------------------------------+
| **Username Rules**                                                              |
+==================================+==============================================+
| **Format**                       | Pacal case AdjectiveNounXXXX (4 random       |
|                                  | numbers)                                     |
+----------------------------------+----------------------------------------------+
| **Uniqueness**                   | Real-time availability check against DB      |
|                                  | before assignment --- 409 if taken           |
+----------------------------------+----------------------------------------------+
| **Mutability**                   | Username is permanent and cannot be changed  |
|                                  | after creation                               |
+----------------------------------+----------------------------------------------+
| **Source**                       | You can attach a username pool doc to        |
|                                  | project context for pre-generated options    |
+----------------------------------+----------------------------------------------+
| **Known As**                     | Free-text display name, 2--30 chars ---      |
|                                  | first name or nickname. Editable any time    |
+----------------------------------+----------------------------------------------+
| **DB Column**                    | users: username (unique index), known_as,    |
|                                  | both NOT NULL                                |
+----------------------------------+----------------------------------------------+

# 2. Daily Emotion Check-In

The first time a user opens the app each calendar day, they are shown a
full-screen emotion check-in. After completing it (or skipping), they
are not shown it again until the next day. The check-in is gated
client-side by last_checkin_date stored locally, confirmed server-side
on submit.

## Step 1 --- Emotion Selection

- Six core emotions shown as duck expression photo cards with label
  underneath

- Emotions: 😊 Happy • 😢 Sad • 😠 Angry • 😰 Anxious • 😐 Meh • 🤩
  Excited

- Multi-select --- user can pick more than one

- Selected cards highlight with a teal border glow --- visually
  satisfying tap

## Step 2 --- Optional Short Phrase

- After selecting emotions, a follow-up prompt appears: \"Want to share
  why?\"

- Quick-tap reason chips (pre-written, scrollable row):

<!-- -->

- School / Work • Family • Friends • Health • Weather • Just because

- \"Don\'t really want to talk about it\" chip --- always shown last

<!-- -->

- Free-text field available below chips, max 120 characters

- If no chip or text entered, phrase is stored as null --- handled
  gracefully throughout the UI

- Continue button always visible --- this step is never blocking

+---------------------------------------------------------------------------------+
| **Check-in Data Model**                                                         |
+==================================+==============================================+
| **Table**                        | daily_checkins                               |
+----------------------------------+----------------------------------------------+
| **Columns**                      | id, user_id, checkin_date (DATE),            |
|                                  | emotion_ids (JSON array), phrase (nvarchar   |
|                                  | 120, nullable), created_at                   |
+----------------------------------+----------------------------------------------+
| **Unique constraint**            | (user_id, checkin_date) --- enforces one     |
|                                  | check-in per day server-side                 |
+----------------------------------+----------------------------------------------+
| **Gate logic**                   | GET /me/checkin/today → 200 with data if     |
|                                  | done, 204 if not yet                         |
+----------------------------------+----------------------------------------------+
| **Timezone**                     | checkin_date stored in user\'s local date    |
|                                  | (passed from client on submit)               |
+----------------------------------+----------------------------------------------+

# 3. Friends System

Users connect by searching for a friend\'s username. Friendship is
bidirectional --- both users must agree. The friends tab shows their
friend list, pending requests, and friend details.

+---------------------------------------------------------------------------------+
| **Friendships Data Model**                                                      |
+==================================+==============================================+
| **Table**                        | friendships                                  |
+----------------------------------+----------------------------------------------+
| **Columns**                      | id, requester_id, addressee_id, status       |
|                                  | (pending / accepted / declined), created_at, |
|                                  | updated_at                                   |
+----------------------------------+----------------------------------------------+
| **Unique constraint**            | (requester_id, addressee_id) --- no          |
|                                  | duplicate requests                           |
+----------------------------------+----------------------------------------------+
| **Bidirectional query**          | WHERE (requester_id = \@userId OR            |
|                                  | addressee_id = \@userId) AND status =        |
|                                  | \'accepted\'                                 |
+----------------------------------+----------------------------------------------+

## API Endpoints

  ----------------------------------------------------------------------------
  **Endpoint**                     **Method**       **Purpose**
  -------------------------------- ---------------- --------------------------
  /users/search?username=x         **GET**          Find user by username to
                                                    add friend

  /friends/request                 **POST**         Send a friend request

  /friends/requests/pending        **GET**          Get incoming pending
                                                    requests

  /friends/requests/{id}/accept    **PUT**          Accept a friend request

  /friends/requests/{id}/decline   **PUT**          Decline a friend request

  /friends                         **GET**          Get accepted friends list
                                                    with today\'s check-in

  /friends/{id}                    **GET**          Get a friend\'s profile,
                                                    level & achievements
  ----------------------------------------------------------------------------

# 4. Quack Reactions

A quack is a short, positive gesture a user can send to a friend --- a
little emotional high-five. It\'s intentionally lightweight and warm.
Users see incoming quacks on the home screen the next time they open the
app.

- Available quack types: 🤗 Hug · 😄 Smile · ✋ High Five · 💛 Thinking
  of You · 🎉 Cheer

- Sending: from a friend\'s card in the Friends tab, tap \"Send a
  Quack\" → pick gesture → confirm

- Rate limit: one quack per sender-recipient pair per day (enforced
  server-side)

- Quacks are not persistent chat --- they are single directional
  gestures, stored briefly

+---------------------------------------------------------------------------------+
| **Quacks Data Model**                                                           |
+==================================+==============================================+
| **Table**                        | quacks                                       |
+----------------------------------+----------------------------------------------+
| **Columns**                      | id, sender_id, recipient_id, quack_type      |
|                                  | (enum), sent_at, seen_at (nullable)          |
+----------------------------------+----------------------------------------------+
| **Seen logic**                   | PATCH /quacks/{id}/seen marks seen_at;       |
|                                  | unseen quacks shown on home load             |
+----------------------------------+----------------------------------------------+
| **Rate limit**                   | Unique constraint or check: one quack per    |
|                                  | (sender_id, recipient_id, DATE(sent_at))     |
+----------------------------------+----------------------------------------------+
| **Display window**               | Show quacks received in last 48 hours that   |
|                                  | have not been seen yet                       |
+----------------------------------+----------------------------------------------+

# 5. Home Screen Panels

## Incoming Quacks Banner

- Shown at top of Home screen when there are unseen quacks

- Animated duck icon with a gentle bounce to draw the eye

- \"🎉 Lola sent you a High Five!\" --- tapping marks as seen and opens
  Friends tab

- If multiple quacks, show stacked count: \"You have 3 quacks waiting
  🐥\"

- Banner disappears once all quacks are marked seen

## How Your Friends Are Feeling Today

- A horizontally scrollable row of friend emotion cards below the banner

- Each card: duck avatar thumbnail + Known As name + emotion emoji(s) +
  phrase snippet (if not null)

- If phrase is null, show emotion label(s) only --- no awkward empty
  space

- Cards for friends who haven\'t checked in yet show a soft ghost card:
  \"Hasn\'t checked in yet\"

- \"See All Friends\" button opens the full Friends tab

- If user has no friends yet: friendly empty state --- \"Add friends to
  see how they\'re feeling 🐥\"

# 6. Friends Tab (Dedicated Screen)

Accessed via the friends icon next to the profile button in the header.
Three tab sections:

## Friends List Tab

- Each friend row: avatar, Known As, \@username, emotion emoji(s) from
  today\'s check-in

- Tap a friend row → Friend Detail view showing level, achievements,
  today\'s feeling, and Send a Quack CTA

- Empty state: search prompt to add first friend

## Add Friends Tab

- Search bar for username lookup --- debounced, min 3 chars

- Result card shows Known As + \@username + \"Send Request\" button

- If already friends: shows \"Already Friends\" badge

- If pending request already sent: shows \"Request Pending\"

## Pending Requests Tab

- Incoming requests shown with Known As, \@username, Accept / Decline
  buttons

- Badge count on tab label when there are pending requests

- On accept: friend immediately appears in Friends List, both sides see
  update on next load

# 7. Architecture & Implementation Notes

+-------------------------------+---------------------------------+
| **Backend**                   | **Frontend**                    |
|                               |                                 |
| - New controllers:            | - Home screen fetches           |
|   FriendsController,          |   /me/checkin/today on launch   |
|   QuacksController,           |   --- shows modal if 204        |
|   CheckInsController          |                                 |
|                               | - Emotion cards: FlatList of    |
| - Standard Controller →       |   image + label, multi-select   |
|   Behavior → Service →        |   via local state               |
|   Repository pattern          |                                 |
|                               | - Quick-tap chips: horizontal   |
| - IFriendshipService,         |   ScrollView row, stateful      |
|   IQuackService,              |   selection                     |
|   ICheckInService interfaces  |                                 |
|                               | - Friends panel: horizontal     |
| - Username uniqueness: unique |   FlatList, skeleton loaders    |
|   index on users.username +   |   while fetching                |
|   409 on conflict             |                                 |
|                               | - Quack banner: conditional     |
| - All endpoints require JWT   |   render, animated with         |
|   auth --- no unauthenticated |   Reanimated fade-in            |
|   social data                 |                                 |
|                               | **Phase 2 Upgrades**            |
| **Rate Limiting &             |                                 |
| Validation**                  | - Real-time quack delivery via  |
|                               |   SignalR (already in Phase 2   |
| - AspNetCoreRateLimit on      |   plan)                         |
|   quack send endpoint         |                                 |
|                               | - Push notification on quack    |
| - FluentValidation for        |   received                      |
|   check-in payload            |                                 |
|   (emotion_ids required,      | - Friend activity feed          |
|   phrase length)              |   (level-ups, lesson            |
|                               |   completions)                  |
| - One-per-day constraints     |                                 |
|   enforced at DB and service  |                                 |
|   layer                       |                                 |
+===============================+=================================+

# 8. MVP Scope Boundaries

  -----------------------------------------------------------------
  **✅ In MVP**                    **⏳ Phase 2**
  -------------------------------- --------------------------------
  Daily emotion check-in modal     Push notifications for quacks

  Multi-emotion + phrase with      Real-time quack delivery
  chips                            (SignalR)

  Username + Known As identity     Friend activity feed / level-up
                                   events

  Send / accept / decline friend   Group friends / friend
  requests                         categories

  Friends feeling panel on Home    Messaging between friends

  Quack reactions (5 types)        Friend leaderboards

  Quack banner on home screen      Blocking / reporting users

  Friend detail view (level +      
  achievements)                    
  -----------------------------------------------------------------

*This is one of the app\'s biggest differentiators. The combination of
daily emotional presence + warm peer reactions creates the kind of
social habit loop that makes users return daily --- even on days they
skip a lesson.*
