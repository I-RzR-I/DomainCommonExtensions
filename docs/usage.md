# USING

I think using these methods is no need for any explanations. Here you may find a lot of extensions that help in projects development.

The source code is open, so you can check what are available and what you can find useful and can use in your project.

---

## Async lazy load `AsyncLazy<T>` and `AsyncExpiringLazy<T>`

## **`AsyncLazy<T>`**

A thread-safe asynchronous lazy initializer. Factory runs exactly once; faults and cancellations auto-reset so the next caller retries.

### Constructor

| Signature | Description | Throws |
|---|---|---|
| `AsyncLazy(Func<CancellationToken, Task<T>> factory)` | Registers the factory delegate. The factory is called on first demand, and again after any fault or cancellation. The `CancellationToken` it receives is cancelled by `Reset()` if called while the factory is running. | `ArgumentNullException` — `factory` is `null` |

### Properties

| Property | Type | Description |
|---|---|---|
| `IsValueCreated` | `bool` | `true` only when the factory completed successfully and the result has not been cleared by `Reset()`. `false` while in-progress, faulted, cancelled, or after reset. |
| `IsInitializationStarted` | `bool` | `true` once `GetValueAsync()` has been called at least once and the state has not been cleared. Stays `true` while the factory is still running or has faulted, until `Reset()` is called. |

### Methods

| Signature | Returns | Description | Notes |
|---|---|---|---|
| `GetValueAsync(CancellationToken ct = default)` | `Task<T>` | Returns the cached task on subsequent calls. Starts the factory on the first call. All concurrent callers share the same in-progress task. | If the factory faults or is cancelled, state is auto-cleared so the next call retries from scratch. The token only affects the current in-flight factory; other concurrent waiters are not independently cancelled. |
| `TryGetValue(out T value)` | `bool` | Synchronously returns the cached value without triggering initialization. | Returns `false` if not yet computed, still in-progress, faulted, or reset. Zero allocations — safe to call on hot paths. |
| `Reset()` | `void` | Atomically clears state and cancels any in-progress factory invocation. After this call both `IsValueCreated` and `IsInitializationStarted` return `false`. | Safe to call concurrently or multiple times. |
| `GetAwaiter()` | `TaskAwaiter<T>` | Enables `await myLazy` shorthand syntax equivalent to `await myLazy.GetValueAsync()`. | — |

### Behavior summary

| Scenario | Outcome |
|---|---|
| First call | Factory invoked; all concurrent callers share the same `Task<T>` |
| Subsequent calls (success) | Cached `Task<T>` returned immediately — factory not called again |
| Factory faults | State auto-cleared; next `GetValueAsync()` call retries |
| Factory cancelled | State auto-cleared; next `GetValueAsync()` call retries |
| `Reset()` while in-progress | Linked `CancellationToken` is cancelled; factory receives cancellation |
| Concurrent `Reset()` + `GetValueAsync()` | Safe — both operate on a single atomically-swapped `LazyState` reference |

---

## `AsyncExpiringLazy<T>`

A thread-safe asynchronous lazy initializer with a configurable TTL. After expiry the next caller transparently triggers a fresh factory invocation.

### Constructor

| Signature | Description | Throws |
|---|---|---|
| `AsyncExpiringLazy(Func<CancellationToken, Task<T>> factory, TimeSpan ttl)` | Registers the factory and TTL window. The factory is called on first access, after expiry, after a fault/cancellation, or after `Reset()`. | `ArgumentNullException` — `factory` is `null` |
| | | `ArgumentOutOfRangeException` — `ttl` is negative |

### Properties

| Property | Type | Description |
|---|---|---|
| `IsValueCreated` | `bool` | `true` only when the factory completed successfully **and** the cached result is still within its TTL window. `false` if not computed, faulted, expired, or reset. |
| `IsInitializationStarted` | `bool` | `true` once the first `GetValueAsync()` call has been made and `Reset()` has not cleared the entry. **Important:** remains `true` even after the TTL expires, because the entry object still exists until it is replaced or cleared. |

### Methods

| Signature | Returns | Description | Notes |
|---|---|---|---|
| `GetValueAsync(CancellationToken ct = default)` | `Task<T>` | Returns the cached value if within TTL and healthy. Otherwise starts a fresh factory invocation. Concurrent callers during a single factory run share the same in-progress task (thundering-herd guard). | Faulted or cancelled entries are treated as immediately expired regardless of TTL, allowing instant retry. |
| `TryGetValue(out T value)` | `bool` | Synchronously returns the cached value without triggering a factory invocation or refreshing an expired entry. | Returns `false` if the entry is missing, expired, or faulted. Zero allocations — safe on hot paths. |
| `Reset()` | `void` | Immediately evicts the cached entry regardless of the remaining TTL. After this call both `IsValueCreated` and `IsInitializationStarted` return `false`. | Any in-progress factory run is **not** cancelled — it will complete its `TaskCompletionSource`, but the result is invisible to new callers. Safe to call concurrently or multiple times. |
| `GetAwaiter()` | `TaskAwaiter<T>` | Enables `await myExpiringLazy` shorthand syntax equivalent to `await myExpiringLazy.GetValueAsync()`. | — |

### Behavior summary

| Scenario | Outcome |
|---|---|
| First call | Factory invoked; entry stored with expiration = `UtcNow + ttl` |
| Subsequent calls within TTL (healthy) | Cached `Task<T>` returned immediately |
| TTL elapsed | Next `GetValueAsync()` triggers a fresh factory invocation |
| `ttl = TimeSpan.Zero` | Entry expires immediately after creation; factory called on every request |
| Factory faults (async) | Entry treated as expired; next call retries without waiting for TTL |
| Factory throws synchronously | No CAS is performed; factory re-invoked on every request until it succeeds |
| Concurrent callers during refresh | Only the CAS winner invokes the factory; losers await the winner's task |
| `Reset()` while in-progress | Entry cleared; next caller gets a fresh factory run (old run still completes but its result is discarded) |

---

## Side-by-side comparison

| Feature | `AsyncLazy<T>` | `AsyncExpiringLazy<T>` |
|---|---|---|
| **Expiry** | Never — cached forever until `Reset()` | After configured TTL |
| **Reset behaviour** | Cancels the in-progress factory via linked `CancellationToken` | Clears the entry; in-progress factory is **not** cancelled |
| **Fault recovery** | Auto-reset on fault/cancel; next caller retries | Auto-reset on fault/cancel; instant retry regardless of TTL |
| **Constructor params** | `factory` | `factory` + `ttl` |
| **`IsInitializationStarted` after TTL** | N/A | Still `true` until replaced or `Reset()` |
| **Use when** | Value must be computed once and reused indefinitely | Value needs periodic refresh (tokens, configs, rates, etc) |
