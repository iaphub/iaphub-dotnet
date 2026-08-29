# Update IAPHUB Android SDK to 5.2.1

## Status

- Backlog status: Ready for implementation planning
- Spec status: Not required unless compatibility work changes public .NET behavior
- Implementation sync: Implemented and verified; ready for delivery review

## Problem

The .NET Android binding currently consumes `com.iaphub:iaphub-android-sdk`
`5.1.1`. The latest released Android SDK is `5.2.1`. Since `5.1.1`, the
native SDK moved from Google Play Billing `billing-ktx` 7.1.1 to `billing`
8.0.0 and added billing-service reconnection and product-query timeout
protection. A version-only bump may therefore leave the .NET binding with an
incompatible explicit Billing dependency.

## User Value

.NET Android consumers receive the current IAPHUB fixes and Billing Library
support without needing to change their existing IAPHUB integration or move to
a newer .NET SDK as part of the same update.

## Actors and Experience Boundary

- Primary actors are .NET Android application developers who consume this
  package and maintain an IAPHUB integration.
- Affected actors are Android customers who initialize the purchase catalog,
  view products, or recover from a temporary Google Play Billing disconnect.
- The current examples and public .NET API already provide those journeys; the
  upgrade should preserve their entry points, UI, and observable contract.
- No new interaction, navigation, copy, consent, or purchase decision is
  intended. Native `5.2.1` reconnection and timeout behavior should improve
  recovery without transferring new work to the application or customer.

## Proposed Outcome

Bind and package IAPHUB Android SDK `5.2.1`, align its Android dependencies and
binding compatibility, and prove that every Android example variant builds and
runs against the updated candidate.

## Included Scope

- Update the bound Maven artifact from `5.1.1` to `5.2.1`.
- Reconcile explicit Xamarin/.NET Android package references with the `5.2.1`
  Maven dependency model, especially the move from `billing-ktx` 7.1.1 to
  `billing` 8.0.0.
- Adjust Android binding transforms or implementation code only where the new
  native SDK requires compatibility changes.
- Restore, compile, and pack the affected Android library targets.
- Build, install, launch, and smoke-test these Android examples against the
  updated candidate:
  - MAUI, using its project reference.
  - Avalonia, using its project reference.
  - Avalonia NuGet, using a locally packed candidate rather than the currently
    published `Iaphub 1.1.2` package.
- Exercise IAPHUB initialization and the example's product-loading path far
  enough to detect native binding, dependency, billing connection, and method
  compatibility failures.
- Record the commands, device/emulator context, and relevant runtime evidence.

## Out of Scope

- Changing installed or repository-selected .NET SDK or workload versions.
- Changing target frameworks such as `net9.0` or `net10.0`.
- Updating the MAUI, Avalonia, or IAPHUB iOS SDK versions.
- Unrelated dependency modernization or example-app refactoring.
- Publishing a NuGet package, changing the public package version, or making a
  release.
- Completing a real-money or production Google Play purchase.

## Durable Specs

- None currently. This is dependency maintenance and should preserve existing
  public .NET behavior.
- If the native upgrade requires a public API or behavior change, define and
  approve that durable behavior before implementing it.

## Implementation Plans

- [Implementation plan](../implementation/2026-08-29-update-iaphub-android-sdk-5-2-1/index.md)

## Likely Ownership Areas

- `src/Iaphub/Android/IaphubSdkVersion.props`
- `src/Iaphub/Android/Transforms/Metadata.xml`, only if generated bindings
  require updated transforms
- `src/Iaphub/Android/IaphubImplementation.cs`, only if native API compatibility
  requires it
- Android example projects and temporary local-package test configuration

## Platform and Dependency Impact

- Android-only native binding and transitive dependency impact is expected.
- Google Play Billing changes from the 7.1.1 KTX artifact to the 8.0.0 base
  artifact are the primary compatibility risk.
- No data, persistence, service API, or iOS impact is expected.

## Dependencies and Sequencing

- Use the published `com.iaphub:iaphub-android-sdk:5.2.1` artifact and its
  Maven metadata as the dependency source of truth.
- Validate binding/package restore before runtime-testing examples.
- The NuGet example must resolve a local candidate containing this change;
  testing it against published `1.1.2` does not satisfy this task.
- Runtime billing connectivity may require a Google Play-enabled emulator or a
  suitable Android device and test account. Basic launch evidence alone is
  insufficient if initialization or product loading fails before reaching the
  updated SDK.

## Testability Notes

- Compare the resolved native and .NET Android dependency graph before and
  after the upgrade to catch duplicate or conflicting Billing artifacts.
- Build the Android library targets and create a local candidate NuGet package.
- Smoke-test each Android example through startup and IAPHUB product loading.
- Inspect Android logs for binding or runtime failures such as
  `ClassNotFoundException`, `NoSuchMethodError`, native callback/signature
  errors, BillingClient failures, and unhandled exceptions.
- Preserve existing unrelated warnings as observations; do not expand this
  task solely to clean them up.

## Acceptance Signals

- The Android binding resolves IAPHUB Android SDK `5.2.1` and a dependency set
  compatible with its Maven metadata, with no stale `billing-ktx` 7.1.1
  dependency left in the candidate package graph.
- Repository target frameworks and .NET SDK/workload versions remain unchanged.
- All affected Android library targets restore, build, and pack successfully.
- The MAUI, project-reference Avalonia, and local-candidate NuGet Avalonia
  Android examples each build, install, launch, and remain running on a test
  device or emulator.
- Each example reaches its normal initial UI and exercises IAPHUB
  initialization/product loading without a native binding, dependency, or
  method-compatibility crash.
- Verification evidence identifies the exact candidate package/version,
  Android environment, commands, and runtime result for each example.

## Open Questions

- None identified for backlog capture. The authoritative readiness check may
  still identify implementation or test-environment prerequisites.

## References

- [IAPHUB Android SDK v5.2.1 release](https://github.com/iaphub/iaphub-android-sdk/releases/tag/v5.2.1)
- [Changes from v5.1.1 through v5.2.1](https://github.com/iaphub/iaphub-android-sdk/compare/v5.1.1...v5.2.1)
- [Maven Central artifact metadata](https://repo.maven.apache.org/maven2/com/iaphub/iaphub-android-sdk/maven-metadata.xml)
- Prior repository update: commit `5c6b0db` (`Android SDK updated to 5.1.1`)
