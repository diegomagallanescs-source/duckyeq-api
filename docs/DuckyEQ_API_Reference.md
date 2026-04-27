# DuckyEQ API Reference

**Base URL (local):** `http://localhost:5115`  
**Base URL (production):** `https://api.duckyeq.com`

All protected endpoints require a JWT Bearer token:
```
Authorization: Bearer <token>
```

---

## Table of Contents

1. [Auth](#auth)
2. [User](#user)
3. [Pillars](#pillars)
4. [Lessons](#lessons)
5. [Coins](#coins)
6. [Shop](#shop)
7. [Gratitude](#gratitude)
8. [EQ Test](#eq-test)
9. [Check-In](#check-in)
10. [Friends & User Search](#friends--user-search)
11. [Quacks](#quacks)
12. [Enums](#enums)
13. [Shared DTO Shapes](#shared-dto-shapes)
14. [Error Conventions](#error-conventions)

---

## Auth

No JWT required.

### `POST /api/auth/register`

Creates a new account. The server generates a unique PascalCase username (e.g. `SunnyDuck4832`) — the client never sends one.

**Request body**
```json
{
  "email": "diego@example.com",
  "password": "MyPass123!",
  "knownAs": "Diego",
  "duckCharacter": 0
}
```

| Field | Rules |
|---|---|
| `email` | valid email format |
| `password` | min 8 characters |
| `knownAs` | 2–10 chars, letters/numbers/spaces only |
| `duckCharacter` | `0` = Ducky (boy), `1` = Daisy (girl) |

**Response `201`**
```json
{
  "token": "<jwt>",
  "username": "SunnyDuck4832",
  "knownAs": "Diego",
  "userId": "guid"
}
```

**JWT claims**
```
nameid   → userId (Guid)
username → PascalCase username
knownAs  → display name
exp      → expiry timestamp
```

---

### `POST /api/auth/login`

**Request body**
```json
{
  "email": "diego@example.com",
  "password": "MyPass123!"
}
```

**Response `200`** — same shape as register response.

**Errors:** `401` invalid credentials.

---

### `POST /api/auth/verify-email?token={token}`

Verifies email with a token sent by Azure Communication Services.

**Response `200`** `true` if verified, `false` if token invalid/expired.

---

## User

All endpoints require JWT.

### `GET /api/user/profile`

Returns the full profile for the authenticated user.

**Response `200`**
```json
{
  "username": "SunnyDuck4832",
  "knownAs": "Diego",
  "duckCharacter": 0,
  "overallXP": 120,
  "overallLevel": 3,
  "streakDays": 5,
  "emailVerified": true,
  "equippedItems": {
    "hat": null,
    "accessory": { ...ShopItemDto },
    "glow": null,
    "color": null
  }
}
```

---

### `PUT /api/user/duck-character`

**Request body**
```json
{ "duckCharacter": 1 }
```

**Response `200`** — no body.

---

### `PUT /api/user/known-as`

Updates display name and **reissues a new JWT** so all screens reflect the change immediately. The client must replace its stored token with the one returned.

**Request body**
```json
{ "knownAs": "Ducky" }
```

**Response `200`** — same `AuthResult` shape as login (new token + username + knownAs + userId).

**Errors:** `400` validation failure (length/characters).

---

## Pillars

All endpoints require JWT. Pillar IDs are integers mapping to the `Pillar` enum:

| ID | Name |
|---|---|
| 1 | Self-Awareness |
| 2 | Self-Management |
| 3 | Social Awareness |
| 4 | Relationship Skills |
| 5 | Responsible Decision-Making |

### `GET /api/pillars/progress`

Returns progress for all 5 pillars. Creates missing rows on first call (safe to call at app startup).

**Response `200`** — array of `PillarProgressDto`
```json
[
  {
    "pillar": 1,
    "name": "Self-Awareness",
    "currentLevel": 2,
    "xp": 340,
    "isUnlocked": true
  }
]
```

---

### `GET /api/pillars/{id}/lessons`

Returns all 10 lessons for a pillar with the caller's progress baked in. This is the board game path dataset — all 10 nodes are loaded in one call.

**Response `200`** — array of `LessonWithProgressDto`
```json
[
  {
    "id": "guid",
    "pillar": 1,
    "level": 1,
    "title": "What is Self-Awareness?",
    "objective": "Identify your core emotions",
    "bestScore": 240,
    "bestStars": 3,
    "firstCompletedAt": "2026-04-20T10:00:00Z",
    "isUnlocked": true
  }
]
```

`bestScore`, `bestStars`, `firstCompletedAt` are `null` until the lesson is completed.

**Errors:** `400` invalid pillar id.

---

### `GET /api/pillars/{id}/cooldown-status`

Returns whether the pillar is locked (cooldown between lesson progression levels).

**Response `200`**
```json
{
  "isLocked": true,
  "nextAvailableAt": "2026-04-28T08:00:00Z"
}
```

`nextAvailableAt` is `null` when `isLocked` is `false`.

---

## Lessons

All endpoints require JWT.

### `GET /api/lessons/{pillarId}/{level}`

Returns the full lesson content for a specific pillar + level combination.

**Response `200`**
```json
{
  "id": "guid",
  "pillar": 1,
  "level": 3,
  "title": "Naming Your Emotions",
  "contentJson": "{ ...CDER lesson payload... }"
}
```

**Errors:** `400` invalid pillar id, `404` lesson not found.

---

### `POST /api/lessons/{id}/start`

Creates a short-lived session token (30-min TTL via IMemoryCache) that must be passed to `/complete`. Prevents submitting scores for lessons that were never actually started.

**Response `200`**
```json
{
  "sessionToken": "uuid-string",
  "expiresAt": "2026-04-27T20:00:00Z"
}
```

**Errors:** `404` lesson not found, `409` pillar cooldown is active.

---

### `POST /api/lessons/{id}/complete`

Submits a lesson result. Validates the session token, calculates score, updates progress, and awards coins.

**Request body**
```json
{
  "sessionToken": "uuid-string",
  "correctAnswers": 8,
  "totalQuestions": 10
}
```

**Scoring formula:** `round(correctAnswers / totalQuestions × 300)`  
**Star thresholds:** ≤100 → 1 star, ≤200 → 2 stars, >200 → 3 stars  
**Coin awards:** 1 star = 20 coins, 2 stars = 40, 3 stars = 60 (first completion or new best only)

**Response `200`**
```json
{
  "score": 240,
  "stars": 3,
  "isNewBest": true,
  "isFirstCompletion": false,
  "coinsAwarded": 0
}
```

**Errors:** `400` invalid/expired session token, `403` session belongs to a different user.

---

## Coins

Requires JWT.

### `GET /api/coins/balance`

**Response `200`**
```json
{ "balance": 150 }
```

---

## Shop

All endpoints require JWT. For MVP the shop is static — all active items are always available.

### `GET /api/shop/items`

Returns all active shop items. `isOwned` and `isEquipped` are always `false` on this endpoint (not user-specific). Use `/shop/inventory` for the user's owned items with equip state.

**Response `200`** — array of `ShopItemDto`
```json
[
  {
    "id": "guid",
    "name": "Rainbow Hat",
    "category": 0,
    "coinPrice": 50,
    "duckyImageUrl": "https://...",
    "daisyImageUrl": "https://...",
    "isOwned": false,
    "isEquipped": false,
    "rarity": "Common",
    "description": "A cheerful rainbow hat"
  }
]
```

---

### `POST /api/shop/purchase`

Validates the user has enough coins and doesn't already own the item, deducts coins, adds to inventory, and **auto-equips** the item (unequipping the current item in the same category first).

**Request body**
```json
{ "shopItemId": "guid" }
```

**Response `200`**
```json
{
  "success": true,
  "newBalance": 100,
  "equippedItems": { ...EquippedItems }
}
```

**Errors:** `404` item not found, `409` already owned, `402` insufficient coins.

---

### `POST /api/shop/equip`

Equips an item the user already owns. Unequips the current item in the same category.

**Request body**
```json
{ "shopItemId": "guid" }
```

**Response `200`** — `EquippedItems` (all four category slots)

**Errors:** `404` item not found or not in inventory.

---

### `GET /api/shop/inventory`

Returns all items the user owns.

**Response `200`** — array of `UserInventoryDto`
```json
[
  {
    "id": "guid",
    "shopItemId": "guid",
    "itemName": "Rainbow Hat",
    "category": 0,
    "isEquipped": true,
    "purchasedAt": "2026-04-27T10:00:00Z"
  }
]
```

---

## Gratitude

All endpoints require JWT.

### `POST /api/gratitude`

Adds a gratitude entry. Awards **10 coins** on the first entry of each calendar day only.

**Request body**
```json
{
  "text": "Grateful for sunny weather today",
  "category": 4
}
```

`category` — see `GratitudeCategory` enum below.

**Response `201`**
```json
{
  "id": "guid",
  "coinsAwarded": 10
}
```

---

### `GET /api/gratitude?page=1&pageSize=20`

Returns paginated entries for the user, newest first.

**Response `200`** — array of `GratitudeEntryDto`
```json
[
  {
    "id": "guid",
    "text": "Grateful for sunny weather today",
    "category": 4,
    "createdAt": "2026-04-27T10:00:00Z"
  }
]
```

---

### `GET /api/gratitude/random`

Returns a random entry for Pick Me Up mode.

**Response `200`** — single `GratitudeEntryDto`  
**Response `204`** — user has fewer than 3 entries (guard: not enough history yet)

---

### `GET /api/gratitude/streak`

**Response `200`**
```json
{
  "currentStreak": 5,
  "longestStreak": 12
}
```

---

## EQ Test

All endpoints require JWT. The test is time-boxed at 60 seconds — the server never validates how many questions were attempted, only `correctAnswers`.

### `GET /api/eq-test/questions`

Returns 15 random questions. `correctOption` is intentionally excluded from the response — the client never knows the right answer before submission.

**Response `200`** — array of `EQTestQuestionDto`
```json
[
  {
    "id": "guid",
    "questionText": "When a friend is upset, the best first step is...",
    "optionA": "Give advice immediately",
    "optionB": "Listen without judgment",
    "optionC": "Change the subject",
    "optionD": "Walk away"
  }
]
```

---

### `POST /api/eq-test/submit`

Scores the submission server-side by looking up correct answers. The client submits every answer it collected during the 60-second window.

**Request body**
```json
{
  "answers": [
    { "questionId": "guid", "selectedOption": 1 },
    { "questionId": "guid", "selectedOption": 0 }
  ]
}
```

`selectedOption` — `EQTestOption` enum: `0=A, 1=B, 2=C, 3=D`

**Scoring:** `score = correctAnswers × 10`  
**Star thresholds:** 0 correct = 0 stars, 1–30 = 1 star, 31–60 = 2 stars, 61+ = 3 stars

**Response `200`**
```json
{
  "score": 110,
  "stars": 3,
  "correctAnswers": 11,
  "isNewBest": true
}
```

---

### `GET /api/eq-test/best-score`

**Response `200`** — same shape as submit response (`isNewBest` is always `false` here)  
**Response `204`** — user has never taken the test

---

## Check-In

All endpoints require JWT.

### `GET /api/checkin/today`

Used by the Home screen on load to determine whether to show the check-in modal.

**Response `204`** — user has NOT checked in today → show modal  
**Response `200`** — user HAS checked in → `CheckInDto`

```json
{
  "id": "guid",
  "userId": "guid",
  "checkInDate": "2026-04-27",
  "emotionIds": ["Happy"],
  "phrase": "Feeling productive today",
  "createdAt": "2026-04-27T09:15:00Z"
}
```

`phrase` is `null` if the user chose not to share one.

---

### `POST /api/checkin`

**Request body**
```json
{
  "emotionIds": ["Happy", "Excited"],
  "phrase": "Feeling productive today"
}
```

| Field | Rules |
|---|---|
| `emotionIds` | non-empty array, each value must be a valid `Emotion` enum name (case-insensitive) |
| `phrase` | optional, max 120 chars; omit or send `null` for no phrase |

**Response `201`** — `CheckInDto`

**Errors:** `400` validation failure, `409` already checked in today.

---

## Friends & User Search

All endpoints require JWT. Friend requests use **username** (not userId) to look up the target — matches how users share their identity socially.

### `GET /api/users/search?username={prefix}`

Prefix search. Filters out the caller. Returns friendship status relative to the caller so the UI can show the right button state (Add / Pending / Friends).

**Minimum 3 characters** required.

**Response `200`** — array of `UserSearchResultDto`
```json
[
  {
    "userId": "guid",
    "username": "BouncyDabbler7718",
    "knownAs": "Tester2",
    "duckCharacter": 1,
    "equippedItems": { ...EquippedItems },
    "existingRelationship": null
  }
]
```

`existingRelationship` — `FriendshipStatus?`: `null` = no relationship, `0` = Pending, `1` = Accepted, `2` = Declined

**Errors:** `400` query fewer than 3 characters.

---

### `POST /api/friends/request`

Sends a friend request by target username. Guards against self-request, duplicate requests, and existing friendships.

**Request body**
```json
{ "targetUsername": "BouncyDabbler7718" }
```

**Response `201`** — `FriendshipDto`
```json
{
  "id": "guid",
  "requesterId": "guid",
  "addresseeId": "guid",
  "status": 0,
  "createdAt": "2026-04-27T10:00:00Z"
}
```

**Errors:** `404` user not found, `409` request or friendship already exists.

---

### `GET /api/friends/requests/pending`

Returns all incoming (not outgoing) pending requests for the authenticated user, with the requester's full duck profile.

**Response `200`** — array of `FriendRequestDto`
```json
[
  {
    "friendshipId": "guid",
    "requesterId": "guid",
    "requesterKnownAs": "Tester1",
    "requesterUsername": "JollyFeather9217",
    "requesterDuckCharacter": 0,
    "requesterEquippedItems": { ...EquippedItems },
    "createdAt": "2026-04-27T10:00:00Z"
  }
]
```

---

### `PUT /api/friends/requests/{id}/accept`

Only the **addressee** can accept. `{id}` is the `friendshipId`.

**Response `200`** — updated `FriendshipDto` with `status: 1`

**Errors:** `404` not found, `403` caller is not the addressee.

---

### `PUT /api/friends/requests/{id}/decline`

Only the **addressee** can decline.

**Response `204`**

**Errors:** `404` not found, `403` caller is not the addressee.

---

### `GET /api/friends`

Returns all accepted friends with today's check-in data merged in. Uses a single LEFT JOIN — friends who haven't checked in today have `hasCheckedInToday: false` and null emotion fields.

**Response `200`** — array of `FriendWithCheckInDto`
```json
[
  {
    "friendshipId": "guid",
    "userId": "guid",
    "knownAs": "Tester2",
    "username": "BouncyDabbler7718",
    "overallLevel": 3,
    "duckCharacter": 1,
    "equippedItems": { ...EquippedItems },
    "hasCheckedInToday": true,
    "emotionIds": ["Excited"],
    "phrase": null
  }
]
```

---

### `GET /api/friends/{id}`

`{id}` is the **friend's userId** (not the friendshipId). Returns the full detail view including XP, used for the Friend Profile screen.

**Response `200`** — `FriendDetailDto`
```json
{
  "userId": "guid",
  "knownAs": "Tester2",
  "username": "BouncyDabbler7718",
  "overallLevel": 3,
  "overallXP": 450,
  "duckCharacter": 1,
  "equippedItems": { ...EquippedItems },
  "hasCheckedInToday": true,
  "todayEmotionIds": ["Excited"],
  "todayPhrase": null,
  "friendshipId": "guid"
}
```

**Errors:** `404` no accepted friendship found between the two users.

---

## Quacks

All endpoints require JWT. A Quack is a short positive gesture sent to a friend (Hug, Smile, HighFive, ThinkingOfYou, Cheer). Rate limit: **one Quack per sender-recipient pair per calendar day**.

### `POST /api/quacks`

**Request body**
```json
{
  "recipientId": "guid",
  "quackType": 0
}
```

`quackType` — `QuackType` enum: `0=Hug, 1=Smile, 2=HighFive, 3=ThinkingOfYou, 4=Cheer`

**Response `201`** — `QuackDto`
```json
{
  "id": "guid",
  "senderId": "guid",
  "senderKnownAs": "Tester1",
  "senderUsername": "JollyFeather9217",
  "senderDuckCharacter": 0,
  "recipientId": "guid",
  "quackType": 0,
  "sentAt": "2026-04-27T10:00:00Z",
  "seenAt": null
}
```

**Errors:**  
- `403` sender and recipient are not accepted friends  
- `429` rate limit — already sent a Quack to this friend today. Includes `Retry-After: <midnight UTC in RFC1123 format>` header.

---

### `GET /api/quacks/unseen`

Returns all unseen Quacks sent to the authenticated user within the last 48 hours. Used for the Home screen banner.

**Response `200`** — array of `QuackDto` (with `seenAt: null`)

---

### `PATCH /api/quacks/{id}/seen`

Marks a specific Quack as seen. Only works for Quacks addressed to the authenticated user.

**Response `204`**

**Errors:** `404` Quack not found or already marked seen.

---

## Enums

All enums serialize as integers in JSON.

### `DuckCharacter`
| Value | Name |
|---|---|
| 0 | Ducky (boy duck) |
| 1 | Daisy (girl duck) |

### `Pillar`
| Value | Name |
|---|---|
| 1 | SelfAwareness |
| 2 | SelfManagement |
| 3 | SocialAwareness |
| 4 | RelationshipSkills |
| 5 | ResponsibleDecisionMaking |

### `Emotion`
| Value | Name |
|---|---|
| 0 | Happy |
| 1 | Sad |
| 2 | Angry |
| 3 | Anxious |
| 4 | Meh |
| 5 | Excited |

*For check-in, send emotion names as strings (e.g. `"Happy"`), not integers.*

### `QuackType`
| Value | Name |
|---|---|
| 0 | Hug |
| 1 | Smile |
| 2 | HighFive |
| 3 | ThinkingOfYou |
| 4 | Cheer |

### `ShopCategory`
| Value | Name |
|---|---|
| 0 | Hat |
| 1 | Accessory |
| 2 | Glow |
| 3 | Color |

### `GratitudeCategory`
| Value | Name |
|---|---|
| 0 | School |
| 1 | Work |
| 2 | Family |
| 3 | Friends |
| 4 | Health |
| 5 | Self |
| 6 | Other |

### `FriendshipStatus`
| Value | Name |
|---|---|
| 0 | Pending |
| 1 | Accepted |
| 2 | Declined |

### `EQTestOption`
| Value | Name |
|---|---|
| 0 | A |
| 1 | B |
| 2 | C |
| 3 | D |

---

## Shared DTO Shapes

### `EquippedItems`
Appears on `UserProfileDto`, `FriendWithCheckInDto`, `FriendDetailDto`, `UserSearchResultDto`, `FriendRequestDto`, and `PurchaseResponse`.

```json
{
  "hat": null,
  "accessory": { ...ShopItemDto },
  "glow": null,
  "color": null
}
```

Each slot is a `ShopItemDto?` — `null` means nothing equipped in that category.

### `ShopItemDto`
```json
{
  "id": "guid",
  "name": "Rainbow Hat",
  "category": 0,
  "coinPrice": 50,
  "duckyImageUrl": "https://...",
  "daisyImageUrl": "https://...",
  "isOwned": true,
  "isEquipped": true,
  "rarity": "Common",
  "description": "A cheerful rainbow hat"
}
```

---

## Error Conventions

All error responses follow this shape:
```json
{ "error": "Human-readable message." }
```

FluentValidation failures (400) follow the ASP.NET default shape:
```json
{
  "errors": {
    "FieldName": ["Validation message."]
  }
}
```

| Status | Meaning |
|---|---|
| `400` | Validation failure or bad input |
| `401` | Missing or invalid JWT |
| `402` | Insufficient QuackCoins |
| `403` | Action not permitted for this user |
| `404` | Resource not found |
| `409` | Conflict (duplicate check-in, duplicate friend request, already owned) |
| `429` | Rate limited (Quack already sent today) — check `Retry-After` header |
| `204` | No content — intentional signal (not checked in today, no best score yet, < 3 gratitude entries) |
