# 🦆 DuckyEQ

**The social network built around emotions.**

DuckyEQ teaches social-emotional learning (SEL) through five pillar-based skill trees, a customizable duck companion, and a social layer where users check in on how their friends are feeling. Think Duolingo meets emotional intelligence.

\---

## What It Does

* **Learn** — Progress through 250+ lessons across 5 CASEL-aligned EQ pillars: Self-Awareness, Self-Management, Social Awareness, Relationship Skills, and Responsible Decision-Making. Lessons use scenario challenges, quick quizzes, breathing exercises, reflection prompts, and teach-back formats.
* **Duck Companion** — Choose Ducky (boy) or Daisy (girl), level them up, and customize with accessories from the weekly rotating Shop.
* **Friends \& Social** — Daily emotion check-ins, friend connections, and Quack reactions create a lightweight social habit loop.
* **Board Game Progression** — Duolingo-style node path with your duck walking through each pillar world as you advance.

\---

## Tech Stack

|Layer|Technology|
|-|-|
|Frontend|React Native + TypeScript, Expo, EAS Build|
|Backend|ASP.NET Core Web API (C#)|
|ORM|Entity Framework Core|
|Database|Azure SQL Database|
|Storage|Azure Blob Storage|
|Hosting|Azure App Service|
|Auth|JWT + BCrypt|
|Payments|RevenueCat|

Backend follows a **Controller → Behavior → Service → Repository** architecture with full dependency injection for testability and scalability.

\---

## Project Status

🚧 **MVP in active development** — targeting App Store launch in 2026.

**MVP Screens:** Home · Learn · Shop · Profile Bar  
**Phase 2 (post-launch):** Arena, Ponds (real-time), full Profile, friend system, push notifications

\---

## Domain

[duckyeq.com](https://duckyeq.com) · API: `api.duckyeq.com` · Deep link: `duckyeq://`

\---

*Built solo by Diego — because EQ is a skill, and skills can be taught.*

