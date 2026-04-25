**🦆 DuckyEQ --- Project Corrections & Overrides**

*This document overrides any conflicting information in other project
context files. Always defer to the values recorded here.*

**1. Duck Character Names**

The two duck companions are named Ducky and Daisy --- not Quack and
Lola. Any reference to Quack or Lola in other documents is incorrect and
superseded by this file.

  ----------------- --------------------- --------------------- -----------------
  **Field**         **Incorrect (Old)**   **Correct (Use        **Notes**
                                          This)**               

  **Boy duck name** Quack                 Ducky                 Always use Ducky

  **Girl duck       Lola                  Daisy                 Always use Daisy
  name**                                                        

  **Enum value**    DuckCharacter.Quack   DuckCharacter.Ducky   Update enum
                                                                accordingly
  ----------------- --------------------- --------------------- -----------------

**2. How to Apply These Overrides**

When building any feature that references duck character names ---
onboarding screens, duck selector UI, database seeds, DTOs, enum values,
asset folder names, or copy --- always use Ducky and Daisy.
Search-replace any occurrence of Quack/Lola in the codebase if found.

**Asset folder naming convention:**

/src/assets/characters/ducky/ ← boy duck

/src/assets/characters/daisy/ ← girl duck

*Last updated: April 2026 · duckyeq.com*
