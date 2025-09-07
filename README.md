# Refs

Unity containers for values and events with change/invoke notifications.

## Features

- Value references (`Ref<T>`, `SharedRef<T>`) with change notifications
- Event references (`EventRef`, `SharedEventRef`) with invoke notifications

## Requirements

- Unity 2022.2+

## Install

1. In Unity Editor click **Window** → **Package Manager**.
2. Click **+** (top left) → **Install package from git URL…**.
3. Enter this repository URL with `.git` suffix and click **Add**:
	```
	https://github.com/darksailstudio/refs.git
	```

## API

### References

Plain C# classes wrapping values (`Ref<T>`) or events (`EventRef`).

#### `Ref<T>`

- `Ref(T initialValue)` constructor.
- `Value { get; set; }` (thread-safe).
- `Changed` event (raised on `Value` change).

#### `EventRef`

- `Invoke()` method.
- `Invoked` event (raised on `Invoke`).

#### Example

```cs
var playerHP = new Ref<int>(10);
var playerDied = new EventRef();

playerHP.Changed += playerHP => {
	if (playerHP <= 0) playerDied.Invoke();
};
playerDied.Invoked += () =>	Debug.Log("Game over");
playerHP.Value -= 10; // Triggers game over
```

### Interfaces

References implement the same interfaces.

#### `IReadOnlyRef<T>` / `IReadOnlyEventRef`

Immutable access, preserves encapsulation.

```cs
private Ref<int> playerHP = new Ref<int>(10);
public IReadOnlyRef<int> PlayerHP => playerHP;
```

```cs
void Subscribe(IReadOnlyEventRef playerDied)
{
	playerDied.Invoked += () => Debug.Log("Game over");
}

var playerDied = new EventRef();

Subscribe(playerDied); // ok
```

#### `IRef<T>` / `IEventRef`

Mutable access, source-agnostic.

```cs
void Heal(IRef<int> playerHP) => playerHP.Value += 10;

var playerHP = new Ref<int>(0);

Heal(playerHP); // ok
```

```cs
void SubscribeAndPublish(IEventRef playerDied)
{
	playerDied.Invoked += () => Debug.Log("Game over");
	playerDied.Invoke();
}

var playerDied = new EventRef();

SubscribeAndPublish(playerDied); // ok
```

## License

[MIT](LICENSE.md)
