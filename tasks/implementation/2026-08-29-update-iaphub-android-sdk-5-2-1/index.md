# Update IAPHUB Android SDK to 5.2.1

## Status

- Status: Review
- Base branch: master
- Work branch: codex/update-iaphub-android-sdk-5-2-1
- PR target: master
- Worktree policy: required
- Worktree path: /Users/clawcorp/Documents/Workspace/projects/iaphub/iaphub-dotnet-worktrees/update-iaphub-android-sdk-5-2-1
- Checkout state: active
- Delivery policy: pull-request
- Pull request state: ready
- Repository strategy: fork-pr
- Canonical repository: iaphub/iaphub-dotnet
- Canonical remote: upstream
- Canonical remote-base ref: upstream/master
- Working repository: maxs15Codex/iaphub-dotnet
- Push remote: origin
- PR base repository: iaphub/iaphub-dotnet
- PR head repository: maxs15Codex/iaphub-dotnet
- PR head identity: maxs15Codex:codex/update-iaphub-android-sdk-5-2-1
- Task branch write: origin codex/update-iaphub-android-sdk-5-2-1 only
- Base branch write: forbidden
- Stacked PR: Not allowed

## Goal

Upgrade the .NET Android binding from IAPHUB Android SDK `5.1.1` to `5.2.1`,
align the Google Play Billing binding with the native artifact, and prove that
all three Android examples build and exercise SDK startup and product loading
without changing repository .NET SDK versions or target frameworks.

## Work Source

- Backlog or direct spec source: [update-iaphub-android-sdk-5-2-1.md](../../backlog/update-iaphub-android-sdk-5-2-1.md)
- Durable specs: Not applicable; this maintenance change preserves the public
  .NET API and existing product journey. Any discovered public behavior change
  returns to backlog/spec review before implementation continues.
- Readiness authority: Checked backlog; ready for implementation planning with
  no blocking gaps.
- Adopted task root: `tasks/backlog/update-iaphub-android-sdk-5-2-1.md`
- Task-owned paths at plan creation: the adopted backlog and this implementation
  plan.
- Artifact provenance: locally-created-task-state adopted from the main
  checkout at SHA-256
  `d40eb6306fe15211e8e9f9dd210570a9ac849c71f1e765a0755d867fe724c707`.
- Recovery evidence: commit `84aa254361fc2195e53768225f34dc9b43fa6f1e`
  on the task branch; the verified duplicate source was removed from
  `/Users/clawcorp/Documents/Workspace/projects/iaphub/iaphub-dotnet/tasks/backlog/update-iaphub-android-sdk-5-2-1.md`.
- Selected base: `upstream/master` at
  `82b4854b4f319e1f5ae7c86446ef5cb10a087cf0`.

## Included Scope

- Update `com.iaphub:iaphub-android-sdk` from `5.1.1` to `5.2.1`.
- Replace the explicit `billing-ktx` 7.1.1 .NET binding with the base
  `billing` 8.0.0 binding required by the `5.2.1` Maven POM.
- Restore and compile both `net9.0-android` and `net10.0-android` Iaphub targets.
- Create a locally identifiable, non-published candidate package and prove its
  Android dependency graph contains BillingClient 8.0.0 without BillingClient
  KTX 7.1.1.
- Build and runtime-smoke the MAUI project-reference, Avalonia
  project-reference, and Avalonia local-candidate NuGet Android examples.
- Exercise each example through `Continue as guest`, which calls SDK startup
  and product loading, and capture process, foreground-activity, UI, and crash
  log evidence.

## Out Of Scope

- Repository or installed .NET SDK, workload, or target-framework changes.
- MAUI, Avalonia, IAPHUB iOS SDK, or unrelated dependency upgrades.
- Public .NET API or example UX changes.
- A public package-version edit, NuGet publication, or release.
- A real-money or production Google Play purchase.
- Cleanup of pre-existing compiler or Android native-library warnings unrelated
  to the binding upgrade.

## Ownership Paths

- Update: `src/Iaphub/Android/IaphubSdkVersion.props` — own the IAPHUB Maven
  coordinate and explicit .NET Android dependency bindings, including the
  coherent Activity/Lifecycle/Kotlin binding set required by Release AOT.
- Update:
  `examples/Iaphub.Example.Avalonia/Iaphub.Example.Avalonia.Android/Iaphub.Example.Avalonia.Android.csproj`
  — retain the split SavedState facade during Release trimming so Activity AOT
  can resolve its forwarded saved-state type.
- Update:
  `examples/Iaphub.Example.Avalonia/Iaphub.Example.Avalonia/Iaphub.Example.Avalonia.csproj`
  — let the shared Avalonia compilation select either the existing local
  project reference or the existing `Iaphub 1.1.2` package through an explicit
  test-mode property, defaulting to the local project reference.
- Update:
  `examples/Iaphub.Example.Shared/Iaphub.Example.Shared.csproj`
  — apply the same conditional reference mode to the cross-framework services
  project, which also references `src/Iaphub/Iaphub.csproj` directly.
- Update:
  `examples/Iaphub.Example.Avalonia.Nuget/Android/Iaphub.Example.Avalonia.Nuget.Android.csproj`
  — retain the same SavedState facade for Release AOT. Candidate restore and
  build proof pass `IaphubExampleUseNuget=true` as a global MSBuild property
  because NuGet's project walk does not propagate per-reference properties;
  default builds retain the repository's existing local-reference behavior.
- Preserve unless compilation proves a native API incompatibility:
  `src/Iaphub/Android/Transforms/Metadata.xml` — binding-shape transforms.
- Preserve unless compilation proves a native API incompatibility:
  `src/Iaphub/Android/IaphubImplementation.cs` — public .NET-to-native adapter.
- Update: `tasks/backlog/update-iaphub-android-sdk-5-2-1.md` — readiness and
  implementation linkage.
- Create and maintain:
  `tasks/implementation/2026-08-29-update-iaphub-android-sdk-5-2-1/index.md` —
  execution and verification evidence while the task is active.
- No repository test source file is planned; package-graph checks, Android
  builds, and device-level runtime evidence directly exercise this dependency
  integration in the repository's existing examples.

## Implementation Approach

- Relevant context inspected: prior update commit `5c6b0db`, current
  `IaphubSdkVersion.props`, `Iaphub.csproj`, Android binding transforms and
  adapter, all Android example project references, shared startup/product-load
  flow, the upstream `5.2.1` release/POM, and Microsoft BillingClient 8.0.0
  NuGet metadata.
- Chosen approach: make the smallest dependency-only change in
  `IaphubSdkVersion.props`: set the native SDK to `5.2.1`, replace
  `Xamarin.Android.Google.BillingClient.Ktx` `7.1.1.4` with
  `Xamarin.Android.Google.BillingClient` `8.0.0`, and leave other pinned
  bindings unchanged unless the resolved graph demonstrates a direct conflict.
  Restore proved direct minimums for Kotlin `2.1.21`, Coroutines `1.10.2`, and
  Lifecycle `2.9.1`; the default Avalonia Release/AOT build then proved the
  transitive Activity `1.10.1.2` binding incompatible with that split SavedState
  shape. Use the same-native-version binding patch Activity `1.10.1.3` and its
  coherent minimums (Kotlin `2.2.0.1`, Coroutines `1.10.2.1`, Lifecycle
  `2.9.2.1`), and root the SavedState facade in both Avalonia Android apps.
  Correct the Android NuGet example's existing mixed reference graph by making
  both shared projects choose package mode only when
  `IaphubExampleUseNuget=true`; normal project-reference examples keep the
  default local project reference. Isolated restore/build verification passes
  the mode globally so restore and build are evaluated consistently;
  per-reference mode metadata is rejected because it makes an ordinary
  restore/build use mismatched project lock files.
- Key decisions and boundaries: a version-only bump is rejected because it
  would retain the wrong Billing artifact; broad dependency modernization is
  rejected because it is independently owned and unnecessary. Binding
  transforms, adapter code, public APIs, TFMs, and UI remain unchanged unless a
  compile-time native signature break proves the plan stale, in which case the
  plan gate must be rerun before expanding scope.
- Risks and proof: compile both Android TFMs to detect generated-binding breaks;
  inspect assets and the candidate package to detect duplicate/stale Billing
  artifacts and verify the aligned Activity/Lifecycle/Kotlin set; compare the
  passing upstream Release baseline with the initial updated-graph AOT failure
  and the repaired Release build; isolate the NuGet example cache/feed and compare candidate hashes
  to prevent accidental published-package reuse; inspect its restore graph to
  prove `src/Iaphub/Iaphub.csproj` is absent; exercise startup and product
  loading in every app; inspect logs for missing classes, methods, callbacks,
  BillingClient failures, unhandled exceptions, and process death.
- Readability and ownership: keep native dependency ownership in the existing
  single props file and avoid new wrapper abstractions, transforms, scripts, or
  checked-in test configuration unless changed-path evidence requires them.
- Experience evidence: primary actors are .NET Android integrators and affected
  actors are Android purchasers. The current journey is login or continue as
  guest, SDK startup, product loading, store display, and handled error/retry.
  The proposed journey is identical; native reconnection and timeout protection
  affect recovery only. Maintainer upgrades are infrequent, while customer
  catalog loading can occur in every purchase session and has financial trust
  consequences. The smoke action is reversible because it creates no purchase;
  the test retains user control by stopping before any transaction. Applicable
  principles are consistent entry/action behavior, visible loading/error
  feedback, bounded waiting, and recovery without hidden work or state loss.
  Material states are login, loading, catalog/empty result, handled error, and
  successful relaunch after interruption; navigation, consent, confirmation,
  and purchase handoffs do not change. Evidence will be `direct-current` device
  execution for all examples with medium confidence because no Play-published
  real purchase is performed. Failure responses must remain visible/handled
  and must not strand or crash the app; heuristic smoke evidence does not prove
  empirical usability or purchase completion.

## Product And System Behavior

- The public .NET API, example entry points, product-loading contract, and
  purchase UI remain unchanged.
- The bound native implementation gains upstream `5.2.1` automatic billing
  service reconnection and product-query timeout protection.
- Primary scenario: continue as guest, initialize IAPHUB, load products, and
  reach the store UI.
- Alternate/recovery scenario: unavailable catalog or Billing service produces
  the existing handled error/empty state without a binding crash or stuck
  loading state; app relaunch remains available.

## Data And Persistence Impact

- No repository data, schema, persistence, migration, or rollout-state change.
- Android binary/transitive dependency compatibility is the only migration
  concern; candidate graph inspection proves the Billing artifact transition.

## Client, API, UI, And Platform Impact

- Android-only packaging and runtime impact.
- No public .NET API, MAUI/Avalonia UI, navigation, copy, consent, or action
  priority changes.
- Both `net9.0-android` and `net10.0-android` remain supported and are verified.
- The examples share `com.iaphub.example`, so runtime tests install and inspect
  them sequentially to avoid treating a previously installed variant as the
  current candidate.

## Package Versioning

- Publishable packages affected: `Iaphub`
- Versioning decision: deferred-with-reason
- Deferral reason: the approved backlog explicitly excludes a public package
  version change, publishing, and release. Local verification uses the existing
  `1.1.2` version in an isolated feed and identifies the candidate by SHA-256.
- Versioning mechanism: none; no versioning artifact is created in this task.
- Internal dependency updates: replace
  `Xamarin.Android.Google.BillingClient.Ktx` `7.1.1.4` with
  `Xamarin.Android.Google.BillingClient` `8.0.0`.
- Local publish behavior: `dotnet pack` writes a non-published candidate to a
  temporary local feed. NuGet package-source mapping and an isolated package
  cache force only `Iaphub` to resolve from that feed for the NuGet example.

## Documentation Impact

- The backlog and this plan record scope and evidence.
- README/API documentation remains unchanged because installation and public
  behavior do not change.

## Tests And Acceptance Mapping

- Native SDK and Billing artifact coordinates match upstream `5.2.1` metadata
  -> inspect `IaphubSdkVersion.props`, restore both Android TFMs, and inspect
  resolved assets/package dependency metadata for BillingClient 8.0.0 with no
  BillingClient KTX 7.1.1.
- .NET SDKs, target frameworks, and public adapter stay unchanged -> compare
  `Iaphub.csproj`, Android transforms, and `IaphubImplementation.cs` with
  `upstream/master`; any unexpected diff fails this criterion.
- A candidate package contains the updated Android binding -> pack the Android
  and shared TFMs, inspect the `.nupkg`, and record its SHA-256.
- MAUI project-reference example works -> Release build, sequential install and
  launch, UI dump/tap of `Continue as guest`, foreground PID/activity evidence,
  post-action UI capture, and filtered crash/binding log review.
- Avalonia project-reference example works -> the same direct-current build and
  runtime evidence using its Android project.
- Avalonia NuGet example uses the candidate and works -> isolated local-feed
  restore, prove its restore graph contains the `Iaphub` package but not the
  local `src/Iaphub/Iaphub.csproj`, compare cached candidate SHA-256 with the
  packed file, build, install, launch, exercise guest/product loading, and
  inspect the same runtime evidence.
- A real purchase is manual-only and excluded because it requires Play
  publication/test-account state and can create an external transaction; no
  purchase-completion claim is made.

## Ordered Implementation Tasks

1. Update `src/Iaphub/Android/IaphubSdkVersion.props` to native SDK `5.2.1`, the
   base BillingClient `8.0.0` binding, and only dependency-binding updates
   demonstrated by restore or Release/AOT compatibility evidence; restore/build
   `net9.0-android` and `net10.0-android`, and inspect resolved assets for stale
   KTX, duplicate Billing artifacts, or incoherent Activity/Lifecycle/Kotlin
   versions.
2. Pack an Android-capable local `Iaphub 1.1.2` candidate, record its SHA-256,
   add conditional package-selection wiring to both shared projects, retain the SavedState facade in both
   Avalonia Android apps, build the MAUI and project-reference Avalonia Android
   examples, then restore and build the NuGet Avalonia example
   through a temporary mapped feed and isolated package cache with
   `IaphubExampleUseNuget=true` passed globally. Prove its restore graph excludes
   the local Iaphub project and its cached nupkg hash matches the candidate.
3. On the Android emulator/device, sequentially deploy each example, drive
   `Continue as guest`, and record direct-current process, activity, UI, and
   filtered log evidence for SDK initialization and product loading; do not
   perform a purchase.
4. Update this plan with exact verification/package/runtime evidence, verify
   the scoped diff and deferred package-version decision, finalize task
   artifacts under repository policy, and deliver the same-name branch as a
   ready fork PR to `iaphub/iaphub-dotnet:master`.

## Verification Commands

- `dotnet restore src/Iaphub/Iaphub.csproj`
- `dotnet build src/Iaphub/Iaphub.csproj -f net9.0-android -c Release`
- `dotnet build src/Iaphub/Iaphub.csproj -f net10.0-android -c Release`
- `dotnet list src/Iaphub/Iaphub.csproj package --framework net9.0-android --include-transitive`
- `dotnet list src/Iaphub/Iaphub.csproj package --framework net10.0-android --include-transitive`
- `dotnet pack src/Iaphub/Iaphub.csproj -c Release -o /tmp/iaphub-sdk521-verification/feed-final`
- `shasum -a 256 /tmp/iaphub-sdk521-verification/feed-final/Iaphub.1.1.2.nupkg`
- `dotnet build examples/Iaphub.Example.Maui/Iaphub.Example.Maui.csproj -f net10.0-android -c Release`
- `dotnet build examples/Iaphub.Example.Avalonia/Iaphub.Example.Avalonia.Android/Iaphub.Example.Avalonia.Android.csproj -f net9.0-android -c Release`
- `NUGET_PACKAGES=/tmp/iaphub-sdk521-verification/packages-final dotnet restore examples/Iaphub.Example.Avalonia.Nuget/Android/Iaphub.Example.Avalonia.Nuget.Android.csproj --configfile /tmp/iaphub-sdk521-verification/NuGet.Config -p:IaphubExampleUseNuget=true`
- `! rg -F 'src/Iaphub/Iaphub.csproj' examples/Iaphub.Example.Avalonia.Nuget/Android/obj/Iaphub.Example.Avalonia.Nuget.Android.csproj.nuget.dgspec.json`
- `rg -F 'Iaphub/1.1.2' examples/Iaphub.Example.Avalonia.Nuget/Android/obj/project.assets.json`
- `NUGET_PACKAGES=/tmp/iaphub-sdk521-verification/packages-final dotnet build examples/Iaphub.Example.Avalonia.Nuget/Android/Iaphub.Example.Avalonia.Nuget.Android.csproj -f net9.0-android -c Release --no-restore -p:IaphubExampleUseNuget=true`
- `dotnet restore examples/Iaphub.Example.Avalonia.Nuget/Android/Iaphub.Example.Avalonia.Nuget.Android.csproj && dotnet build examples/Iaphub.Example.Avalonia.Nuget/Android/Iaphub.Example.Avalonia.Nuget.Android.csproj -f net9.0-android -c Debug --no-restore`
- `dotnet build examples/Iaphub.Example.Maui/Iaphub.Example.Maui.csproj -t:Run -f net10.0-android -c Debug -p:AdbTarget=-e`
- `dotnet build examples/Iaphub.Example.Avalonia/Iaphub.Example.Avalonia.Android/Iaphub.Example.Avalonia.Android.csproj -t:Run -f net9.0-android -c Debug -p:AdbTarget=-e`
- `NUGET_PACKAGES=/tmp/iaphub-sdk521-verification/packages-final dotnet build examples/Iaphub.Example.Avalonia.Nuget/Android/Iaphub.Example.Avalonia.Nuget.Android.csproj -t:Run -f net9.0-android -c Debug --no-restore -p:IaphubExampleUseNuget=true -p:AdbTarget=-e`
- After each sequential install: `adb shell uiautomator dump`, `adb shell input tap`, `adb shell pidof com.iaphub.example`, `adb shell dumpsys activity activities`, `adb exec-out screencap -p`, and filtered `adb logcat` inspection.
- `git diff --check && git diff --stat upstream/master...HEAD && git status --short`

Passing means both Android library TFMs and all three examples build, the
NuGet example is proven to consume the local candidate, every example remains
foreground/running after SDK startup and product loading, no scoped crash or
binding/dependency error is present, the expected existing warnings are
documented, and no .NET SDK, TFM, public API, or out-of-scope package-version
change appears in the delivery diff.

## Rollout And Delivery

- No data migration, feature flag, or package publication occurs.
- Commit the verified dependency change and required task evidence on
  `codex/update-iaphub-android-sdk-5-2-1`.
- Push only that same-name branch to `origin` and open a ready cross-repository
  PR targeting `iaphub/iaphub-dotnet:master` from
  `maxs15Codex:codex/update-iaphub-android-sdk-5-2-1`.
- Human merge and a future patch-version/release task remain outside this run.

## Completion Checklist

- [x] Native SDK is `5.2.1` and the explicit Billing binding is base 8.0.0.
- [x] Both Android library TFMs restore/build and their graphs contain no stale
      BillingClient KTX 7.1.1.
- [x] The local candidate package is hashed and inspected.
- [x] MAUI and both Avalonia Android examples build successfully.
- [x] Each example is installed, launched, driven through guest/product
      loading, and has direct-current process/UI/log evidence.
- [x] The NuGet example's isolated cached nupkg hash matches the candidate.
- [x] The NuGet example restore graph contains the candidate package and no
      transitive reference to `src/Iaphub/Iaphub.csproj`.
- [x] .NET SDK, workload, TFMs, public APIs, iOS, and public package version are
      unchanged.
- [ ] Scoped verification is recorded and the ready fork PR is created.
- [ ] Independent implementation review reports no blockers or should-fix
      findings.

## Plan Readiness

- [x] Plan status is approved for implementation or Draft execution is
      explicitly authorized. The user's end-to-end request authorizes Draft
      execution after the plan check passes.
- [x] Durable specs and project policy own the complete intended behavior. No
      new product behavior is introduced; existing public behavior is preserved.
- [x] Worktree, checkout, delivery, repository-role, branch, PR-target, write
      authority, and stacked-PR fields are valid and mutually consistent.
- [x] The implementation approach, ownership paths, ordered tasks, acceptance
      mapping, and verification commands are concrete.
- [x] Package versioning is responsible for every affected publishable package;
      the version edit and release are explicitly deferred outside this task.
- [x] Active Engineering Quality and User Experience requirements are
      integrated; inactive Web Client and Visual Fidelity sections are absent.
- [x] Open questions are resolved or explicitly non-blocking.

## Open Questions

- None.

## Implementation Evidence

- Dependency result: `net9.0-android` and `net10.0-android` restore and Release
  builds pass with zero warnings and zero errors. Both resolved graphs contain
  base `Xamarin.Android.Google.BillingClient` `8.0.0`, Activity `1.10.1.3`,
  Lifecycle Common/Runtime `2.9.2.1`, Kotlin `2.2.0.1`, and Coroutines
  `1.10.2.1`; neither contains BillingClient KTX. SavedState resolves to
  `1.3.1.1` with its Android implementation.
- Compatibility finding: the initial native/Billing update exposed an Avalonia
  Release/AOT failure because Activity binding `1.10.1.2` expected the former
  SavedState shape while the new Lifecycle graph uses split SavedState
  assemblies. The unchanged upstream baseline passed. Activity binding patch
  `1.10.1.3`, its coherent dependency minimums, and explicitly retaining the
  SavedState facade in both Avalonia Android apps restore a passing default
  Release/AOT build.
- Package result: the full Release pack passes for all repository package TFMs.
  `/tmp/iaphub-sdk521-verification/feed-final/Iaphub.1.1.2.nupkg` has SHA-256
  `0d4a1a78b67008b727f09c0e8aef7ad6a4494eea77925b1a28efa22310c6aeea`.
  Its Android dependency groups contain the exact updated bindings and no
  BillingClient KTX. The public package version remains `1.1.2` and the package
  was not published, per the explicit `deferred-with-reason` decision.
- Example build result: MAUI `net10.0-android` Release passes with two existing
  warnings; project-reference Avalonia `net9.0-android` Release passes with four
  existing Android native-library page-size warnings; local-candidate NuGet
  Avalonia `net9.0-android` Release passes with those four warnings plus two
  existing nullable warnings. A clean, ordinary default NuGet-example restore
  and Debug build also pass with four existing warnings.
- Candidate isolation: restore through the mapped temporary feed and isolated
  `/tmp/iaphub-sdk521-verification/packages-final` cache contains
  `Iaphub/1.1.2`, excludes `src/Iaphub/Iaphub.csproj`, and caches a nupkg whose
  hash exactly matches the packed candidate. Candidate mode is passed globally
  so NuGet restore and MSBuild evaluate the same graph; mode changes in one
  worktree are verified from clean intermediates.
- Runtime result: direct-current smoke tests ran sequentially on
  `opencellar_pixel_8_api_36_1` (Android 16 / API 36.1, Google APIs). All three
  variants installed, launched, remained foreground with distinct current
  processes/activities, accepted `Continue as guest`, and reached the handled
  product catalog/empty state. Captures are under
  `/tmp/iaphub-sdk521-verification/*-before.png` and `*-after.png`.
- Runtime logs: BillingClient `8.0.0` attempted connection and reported the
  expected service-not-registered/result-3 condition because this emulator has
  no Play Store and the app was not Play-installed. The examples handled that
  state and displayed no products. No fatal exception, process death,
  ClassNotFound, NoClassDef, NoSuchMethod, UnsatisfiedLink, or binding failure
  was present. No real purchase claim is made.
- Engineering Quality self-review: the five production/example files match the
  plan, dependency ownership remains centralized, the candidate graph and AOT
  regression are directly proven, and no public API, TFM, .NET SDK, adapter,
  transform, iOS, or unrelated dependency change is present.
- User Experience self-review: the unchanged login, guest, loading, and handled
  empty-catalog states were exercised in all variants with direct-current,
  medium-confidence evidence. The no-Play environment verifies recovery and
  crash safety but cannot verify Play product delivery or purchase completion.
- Diff result: `git diff --check` passes; `Iaphub.csproj`, `Metadata.xml`, and
  `IaphubImplementation.cs` are byte-for-byte unchanged from `upstream/master`.
- Commits: adoption recovery commit
  `84aa254361fc2195e53768225f34dc9b43fa6f1e`; final evidence checkpoint and
  implementation delivery commits are recorded in the pull request.
- Pull-request delivery: pending final artifact checkpoint and independent
  implementation review.
- Final review: not run
