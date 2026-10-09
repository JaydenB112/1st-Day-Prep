# First Week Playbook

## Before day one
- [ ] Install: **.NET SDK** (check the version their `global.json` targets), **Visual Studio 2022** or **Rider** (most .NET shops) / VS Code + C# Dev Kit, **Git**, **Docker Desktop**, **SSMS** or **Azure Data Studio**, Postman/Insomnia (or `.http` files)
- [ ] Practise in this repo: build, run, debug (set a breakpoint in `OrdersController`!), run tests, branch, commit, PR
- [ ] Be able to explain the request flow in `docs/ARCHITECTURE.md` out loud

## Day 1: what usually happens
- Laptop setup, accounts (Azure DevOps or GitHub, Jira, Slack/Teams, VPN, SSO)
- **Repo access + getting it to build locally** (often the hardest part of week one. That's normal!)
- Meet your manager, team, and maybe an onboarding buddy
- First ticket is often tiny: a README fix, a log message, a "good first issue" bug

## Questions worth asking (write the answers down)
**Codebase**
- What's the solution/repo structure? Monorepo or many services? Which one should I learn first?
- How do I run it locally? Is there a seed DB, Docker Compose, or shared dev environment?
- Which .NET version are we on? Any legacy .NET Framework code?
- EF Core, Dapper, stored procs, or a mix?
- MediatR/CQRS? AutoMapper? What's the logging stack?

**Process**
- Branch naming and commit message conventions? Squash or merge?
- How many approvals does a PR need? Who should I ask to review?
- How are tickets sized and assigned? When is sprint planning?
- What does "done" mean here (tests? docs? deployed to QA?)
- How do deployments work and how often? Can I watch one?

**People**
- Who do I ask when I'm stuck, and how long should I struggle before asking? (A good rule is 30–60 min, then ask with what you've tried.)
- Who owns which services/domains?
- Is there an on-call rotation, and when would I join it?

## Getting unstuck: how to ask a good question
> "I'm working on **OF-103**. `GET /api/orders/1` returns empty items. I've confirmed the rows exist in the DB
> and that `/customers/1/orders` returns them. I suspect `OrderRepository.GetByIdAsync` but I'm not sure why the
> two queries differ. Could you point me in the right direction?"

Context → what you tried → your hypothesis → specific ask.

## Standup template (≤ 60 seconds)
- **Yesterday:** Finished OF-102, PR is up.
- **Today:** Starting OF-103, reading up on EF eager loading.
- **Blockers:** Need access to the staging DB. Who grants that?

## Week-one mindset
- Read more than you write. Read merged PRs to learn the team's conventions.
- Keep a personal `notes.md` of commands, acronyms, and who-knows-what.
- Small PRs > big PRs.
- Nobody expects you to know their codebase. They do expect you to ask questions and write things down.
