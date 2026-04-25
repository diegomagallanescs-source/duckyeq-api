**DuckyEQ**

**Single-Emotion Check-In --- Change Log & Pending Work**

*Decision recorded April 2026 · Affects: Domain · Infrastructure · API ·
Frontend*

+-----------------------------------------------------------------------+
| **🦆 The Decision**                                                   |
|                                                                       |
| Originally the daily check-in allowed multi-select emotions           |
| (selectedEmotions: string\[\]).                                       |
|                                                                       |
| This was changed to a single required emotion before any code beyond  |
| Day 4 was written.                                                    |
|                                                                       |
| Reason: forces genuine self-reflection, cleaner data for mood         |
| history, and simpler UX for all ages.                                 |
|                                                                       |
| The Emotion enum is unchanged --- the change is only in how many      |
| values are stored per check-in.                                       |
+-----------------------------------------------------------------------+

**All Layers --- What Changes**

*Track each layer\'s status here as you build through the 45-day plan.*

  -------------------- ---------------------------------------- ---------------
  **Layer**            **What Changed**                         **Status**

  **Domain**           DailyCheckIn entity: EmotionId (Emotion, **✅ Done**
                       single value) replaces EmotionIds        
                       (IList\<Emotion\>). Factory signature    
                       updated to accept Emotion emotion        
                       instead of IList\<Emotion\>.             

  **Domain**           Emotion enum: no changes --- all six     **✅ Done**
                       values (Happy, Sad, Angry, Anxious, Meh, 
                       Excited) remain.                         

  **Infrastructure**   EF migration: daily_checkins table gets  **⏳ Pending**
                       emotion_id (int, NOT NULL) instead of    
                       emotion_ids (JSON array). No migration   
                       run yet --- schema starts correct.       

  **Contracts**        CheckInRequest DTO: change EmotionIds    **⏳ Pending**
                       (List\<string\>) → EmotionId (string).   
                       Update FluentValidation rule from        
                       NotEmpty() on list → NotEmpty() on       
                       single string.                           

  **Contracts**        ICheckInService interface: update        **⏳ Pending**
                       SubmitCheckIn method signature to accept 
                       single Emotion (or string) instead of    
                       collection.                              

  **Services**         CheckInBehavior + CheckInService: update **⏳ Pending**
                       method signatures and internal logic.    
                       Remove any loop/aggregate over emotion   
                       list.                                    

  **API**              CheckInController: pass single EmotionId **⏳ Pending**
                       through to service. No structural change 
                       beyond the DTO update.                   

  **Frontend**         CheckInModal (Day 22): state changes     **⏳ Pending**
                       from selectedEmotions: string\[\] →      
                       selectedEmotion: string \| null. Tapping 
                       a card sets it (replaces prior           
                       selection). Tapping selected card        
                       deselects (→ null). Teal glow applied to 
                       single matching card only.               

  **Frontend**         Friends Feeling Panel (HomeScreen):      **⏳ Pending**
                       friend cards show single emotion emoji,  
                       not mapped array.                        

  **Frontend**         checkInService.submitCheckIn(): update   **⏳ Pending**
                       call signature to pass single string     
                       instead of array.                        
  -------------------- ---------------------------------------- ---------------

**Layer Details**

**Domain --- DailyCheckIn Entity (✅ Done)**

**Property**

+-----------------------------------------------------------------------+
| **Before → After**                                                    |
|                                                                       |
| BEFORE: public IList\<Emotion\> EmotionIds { get; private set; }      |
|                                                                       |
| AFTER: public Emotion EmotionId { get; private set; }                 |
+-----------------------------------------------------------------------+

**Factory Method**

+-----------------------------------------------------------------------+
| **Before → After**                                                    |
|                                                                       |
| BEFORE: public static DailyCheckIn Create(Guid userId, DateOnly       |
| checkinDate, IList\<Emotion\> emotionIds, string? phrase)             |
|                                                                       |
| AFTER: public static DailyCheckIn Create(Guid userId, DateOnly        |
| checkinDate, Emotion emotionId, string? phrase)                       |
+-----------------------------------------------------------------------+

**Guard Clause Pattern (keep the same approach)**

- emotionId is an enum --- no null guard needed at factory level

- phrase: null = intentionally not shared. Empty string rejected and
  converted to null (unchanged from Day 4)

**Infrastructure --- EF Migration (⏳ Pending --- Day \~8)**

When you scaffold your first migration, the daily_checkins table will
have:

  ------------------ ------------------ ----------------------------------
  **Column**         **Type**           **Notes**

  emotion_id         int NOT NULL       EF stores enum as int. 0=Happy,
                                        1=Sad, etc. (check your enum
                                        order)

  phrase             nvarchar(120) NULL Unchanged --- null means
                                        intentionally not shared

  checkin_date       date NOT NULL      Unchanged
  ------------------ ------------------ ----------------------------------

+-----------------------------------------------------------------------+
| **⚠️ Enum Ordering Note**                                             |
|                                                                       |
| EF Core stores enums as their integer value by default.               |
|                                                                       |
| Make sure your Emotion enum values are explicitly ordered and never   |
| reordered after first migration.                                      |
|                                                                       |
| Example: Happy = 0, Sad = 1, Angry = 2, Anxious = 3, Meh = 4, Excited |
| = 5                                                                   |
|                                                                       |
| If you reorder them later, existing rows will map to wrong emotions.  |
| Pin the order now.                                                    |
+-----------------------------------------------------------------------+

**Contracts --- DTO & Validation (⏳ Pending --- Day \~9)**

+-----------------------------------------------------------------------+
| **CheckInRequest DTO**                                                |
|                                                                       |
| BEFORE: public List\<string\> EmotionIds { get; set; }                |
|                                                                       |
| AFTER: public string EmotionId { get; set; }                          |
|                                                                       |
| FluentValidation rule:                                                |
|                                                                       |
| BEFORE: RuleFor(x =\> x.EmotionIds).NotEmpty();                       |
|                                                                       |
| AFTER: RuleFor(x =\> x.EmotionId).NotEmpty().Must(v =\>               |
| Enum.TryParse\<Emotion\>(v, true, out \_)).WithMessage(\'Invalid      |
| emotion value.\');                                                    |
+-----------------------------------------------------------------------+

**Frontend --- CheckInModal (⏳ Pending --- Day 22)**

This is the main user-facing change. File:
/src/features/home/CheckInModal.tsx

**State**

+-----------------------------------------------------------------------+
| **Before → After**                                                    |
|                                                                       |
| BEFORE: const \[selectedEmotions, setSelectedEmotions\] =             |
| useState\<string\[\]\>(\[\]);                                         |
|                                                                       |
| AFTER: const \[selectedEmotion, setSelectedEmotion\] =                |
| useState\<string \| null\>(null);                                     |
+-----------------------------------------------------------------------+

**Tap Handler**

+-----------------------------------------------------------------------+
| **Before → After**                                                    |
|                                                                       |
| BEFORE: toggle add/remove from selectedEmotions array                 |
|                                                                       |
| AFTER: if (emotion === selectedEmotion) setSelectedEmotion(null); //  |
| deselect                                                              |
|                                                                       |
| else setSelectedEmotion(emotion); // select new                       |
+-----------------------------------------------------------------------+

**Glow Border**

+-----------------------------------------------------------------------+
| **Before → After**                                                    |
|                                                                       |
| BEFORE: selectedEmotions.includes(emotion) → apply teal border        |
|                                                                       |
| AFTER: selectedEmotion === emotion → apply teal border                |
+-----------------------------------------------------------------------+

**Continue Button**

+-----------------------------------------------------------------------+
| **Unchanged logic, updated variable**                                 |
|                                                                       |
| BEFORE: disabled={selectedEmotions.length === 0}                      |
|                                                                       |
| AFTER: disabled={selectedEmotion === null}                            |
+-----------------------------------------------------------------------+

**Submit Call**

+-----------------------------------------------------------------------+
| **Before → After**                                                    |
|                                                                       |
| BEFORE: checkInService.submitCheckIn(selectedEmotions, phrase)        |
|                                                                       |
| AFTER: checkInService.submitCheckIn(selectedEmotion, phrase)          |
+-----------------------------------------------------------------------+

**Frontend --- Friends Feeling Panel (⏳ Pending --- Day 23)**

File: /src/features/home/HomeScreen.tsx --- Friends Feeling Panel
section.

+-----------------------------------------------------------------------+
| **Before → After**                                                    |
|                                                                       |
| BEFORE: friend.emotions.map(e =\> \<EmojiIcon key={e} emotion={e}     |
| /\>)                                                                  |
|                                                                       |
| AFTER: \<EmojiIcon emotion={friend.emotion} /\> // single value, no   |
| map                                                                   |
+-----------------------------------------------------------------------+

**Quick Reference Checklist**

*Use this when you reach each layer. Check off as you go.*

  ---- ------------------------------------------- ----------------------
       **Task**                                    **Day / File**

  ☑    DailyCheckIn entity: EmotionId (single      *Day 4 --- ✅ Done*
       Emotion)                                    

  ☐    EF migration: emotion_id int column (pin    *Day \~8 ---
       enum order!)                                Infrastructure*

  ☐    CheckInRequest DTO: EmotionId string +      *Day \~9 ---
       FluentValidation                            Contracts*

  ☐    ICheckInService + CheckInBehavior +         *Day \~9 --- Services*
       CheckInService signatures                   

  ☐    CheckInController: pass single EmotionId to *Day \~9 --- API*
       service                                     

  ☐    CheckInModal: selectedEmotion state + tap   *Day 22 --- Frontend*
       handler + glow                              

  ☐    checkInService.submitCheckIn(): single      *Day 22 --- Frontend*
       string param                                

  ☐    Friends Feeling Panel: single emotion emoji *Day 23 --- Frontend*
       per friend card                             
  ---- ------------------------------------------- ----------------------

*DuckyEQ --- Internal Development Reference · Do not ship with app*
