# Refs

Unity containers for values and events with change/invoke notifications, as plain C# objects or asset-backed shared instances. Events can be invoked from the inspector.

## Features

- Value references (`Ref<T>`, `SharedRef<T>`) with change notifications
- Event references (`EventRef`, `SharedEventRef`) with invoke notifications
- Shared references as `ScriptableObject` assets
- Custom property drawers for transparent editing and inspector invoke buttons

## Requirements

- Unity 2022.2+
- [Resettables](https://github.com/darksailstudio/resettables)

## Install

1. In Unity Editor click **Window** → **Package Manager**.
2. Click **+** (top left) → **Install package from git URL…**.
3. Enter this repository URL with `.git` suffix and click **Add**:
	```
	https://github.com/darksailstudio/refs.git
	```

## API

### Unique references

Plain C# classes wrapping values (`Ref<T>`) or events (`EventRef`), with custom property drawers for transparent inspector editing and event wiring.

#### `Ref<T>`

- `Ref(T initialValue)` constructor.
- `Value { get; set; }` (thread-safe).
- `Changed` event (raised on `Value` change).
- Inspector drawer shows only `Value`, raises `Changed` on edit.

#### `EventRef`

- `Invoke()` method.
- `Invoked` event (raised on `Invoke`).
- Inspector drawer shows an `Invoke` button.

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

### Shared references

Asset-backed references (`SharedRef<T>`, `SharedEventRef`) for sharing state and events across objects and scenes, implemented as `ScriptableObject` assets, runtime-only even in the editor (state reset via the [Resettables package](https://github.com/darksailstudio/resettables)). Inspired by [Game Architecture with Scriptable Objects](https://www.youtube.com/watch?v=raQ3iHhE_Kk) talk by Ryan Hipple at Unite Austin 2017.

Shared references are created via **Assets** → **Create** → **Shared References** → **\[Type\]**.

#### `SharedRef<T>`

Abstract base for asset-backed values. Same API as [`Ref<T>`](#reft) (except the constructor), but wrapped in a `ScriptableObject` asset.

Requires derived types for values due to Unity serialization constraints (assets can't be generic types). Implementations provided for common C# and Unity primitives:

- `int`, `bool`, `string`, `float`
- `Vector2`, `Vector3`, `Quaternion`, `Color`, `GameObject`

Extend to support custom types:

```cs
using DarkSail.Refs;
using UnityEngine;

[CreateAssetMenu(menuName = "My Project/My Custom Shared Ref")]
class MyCustomSharedRef : SharedRef<MySerializableType> {}
```

#### `SharedEventRef`

Same API as [`EventRef`](#eventref), but wrapped in a `ScriptableObject` asset.

#### Examples

```cs
using DarkSail.Refs;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] SharedIntRef playerHP;
	[SerializeField] SharedEventRef playerDied;

	public void TakeDamage(int damage)
	{
		playerHP.Value -= damage;
		if (playerHP.Value <= 0) playerDied.Invoke();
	}
}
```

```cs
using DarkSail.Refs;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
	[SerializeField] SharedIntRef playerHP;

	void OnEnable()
	{
		playerHP.Changed += OnPlayerHPChanged;
		OnPlayerHPChanged(playerHP.Value);
	}

	void OnDisable()
	{
		playerHP.Changed -= OnPlayerHPChanged;
	}

	void OnPlayerHPChanged(int hp)
	{
		// Render player hit points value
	}
}
```

```cs
using DarkSail.Refs;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
	[SerializeField] SharedEventRef playerDied;

	void OnEnable()
	{
		playerDied.Invoked += OnPlayerDied;
	}

	void OnDisable()
	{
		playerDied.Invoked -= OnPlayerDied;
	}

	void OnPlayerDied()
	{
		// Render game over screen
	}
}
```

### Interfaces

Unique and shared references implement the same interfaces.

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

var unique = new EventRef();
var shared = ScriptableObject.CreateInstance<SharedEventRef>();

Subscribe(unique); // ok
Subscribe(shared); // ok
```

#### `IRef<T>` / `IEventRef`

Mutable access, source-agnostic.

```cs
void Heal(IRef<int> playerHP) => playerHP.Value += 10;

var unique = new Ref<int>(0);
var shared = ScriptableObject.CreateInstance<SharedIntRef>();

Heal(unique); // ok
Heal(shared); // ok
```

```cs
void SubscribeAndPublish(IEventRef playerDied)
{
	playerDied.Invoked += () => Debug.Log("Game over");
	playerDied.Invoke();
}

var unique = new EventRef();
var shared = ScriptableObject.CreateInstance<SharedEventRef>();

SubscribeAndPublish(unique); // ok
SubscribeAndPublish(shared); // ok
```

## License

[MIT](LICENSE.md)
