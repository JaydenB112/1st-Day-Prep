# Git & PR Workflow (typical team conventions)

```bash
git checkout main && git pull
git checkout -b bugfix/OF-102-order-total-quantity     # feature/…, bugfix/…, hotfix/…, chore/…
# …code, test…
dotnet build && dotnet test
git add -p                                             # review each hunk before staging
git commit -m "OF-102: Include quantity in order total calculation"
git push -u origin bugfix/OF-102-order-total-quantity
# open a PR → fill in the template → request review → address comments → squash & merge
```

**Commit messages:** imperative mood, ticket ID first. `OF-102: Fix total` ✅ · `fixed stuff` ❌

**Keeping your branch fresh:** `git fetch && git rebase origin/main` (or `merge`, whichever the team prefers. Ask!)

**Never:** commit secrets / connection strings with passwords, force-push to `main`, merge your own PR without approval.

## Responding to review comments
- Reply to every comment: "Done", "Good catch, fixed in abc123", or a reasoned disagreement.
- Don't take it personally. Reviews are about the code, and everyone gets them.
- Resolve threads only when the reviewer is satisfied (team conventions vary).
