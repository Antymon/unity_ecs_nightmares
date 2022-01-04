# What?
Entity/Component/System demo example of a mobile shooter made in Unity3d 2017.

This is not made with Unity DOTS framework only because this project is older and
flexible ECS support at time was poor.

There is no testing here. This is bad. You should test anything that is easily testable, e.g., the data/controller part.

# Features

- Aside from mobile, project is desktop-playable as a touch simulation - mouse button and spacebar simulate touches. The original idea was that second tap is the shooting; so spacebar key immetiates second tap, not shooting per se;
that is - if key is pressed but not mouse - one touch is present

- Project was develpoed in an ECS framework called Entitas

- Most of assets come from Unity training project called Nightmares; and most of assets were modified in some small ways (replacing shaders to mobile-friendly, downscaling textures etc)

- Entity deserialization is via data bound to unity game objects (unity style), however dependency is inverted and entitas has no knowledge how deserialization iz implemented

- Entitas is an ECS framowork designed for the sake of unity-agnostic execution

- Most of the time only unity utilities not calling into editor are used within entitas part of implementation - so that they should be executable outside unity (but WITH unity dlls) after replacing few calls (Random for instance)

- Few project specific utilites have been created to support bootstraping and Unity-Entitas binding 

- Entitas does aggressive pooling of its entities - counts can be seen during the runtime in editors panels

- AI was described within little FSM, however possibly Behaviour tree would be more future-proof choice

- All specification parameters are init-time configurable (with exception of level [time constraints]) from within relevant unity gameobjects /prefabs

- Various small additions - scrambler (aka inverter) takes time parameter in frames

- Pool on game objects: for example player and enemy are created only once throughout whole excecution; gameobject.Destroy is probably never called at all for anything

# Details

## Data layer design

Entities(as components groupings, not in Entitas' sense):

	Player
	Enemy
	Bullet
	HealthSpawner
	InversionSpawner

	Joystick
	Healthbar
	RoundCounter
	PauseMenu
	PauseBtn
	HealthDecreaseOverlay
	ScoreCounter

Components:

	TouchInput
		touches
	Health
		hp
		cap
	HealthChangedListener
		listener
	Damage
		hp
	Spawner
		freqency
		cap
	Level
		spawner_cap
		round_time
		nuber_of_rounds
		current_round
		player_won
	Agent
		id
		effects
		name
	Effect
		effect
	Aggressor
		target
	Projectile
		cooldown_time
	Collision
		Entity self
		Entity other
		
## AI design
Bunch of parameterizable behaviors. Selection hard-coded. Nothing to do with learnable AI.
```
aggressiveness = f(intended attack time, inttended resting time)
sneakiness = f(chance of using hiding points when approaching)
awarness = f(scanned radius, density of scan)
shooting efficiency = f(expected missing angle, aiming time) c {machine, sniper, assault}
```

### Enemy objectives:
1. survive
2. kill

### Facts
Data pieces driving rules.
```
enemy.hp
self.hp
enemy.aggressive
self.aggressive
enemy.position
```

### Rules
Main loop
```
if(self.hp high)
	if(self very aggressive)
		attack
	if(enemy not aggressive)
		attack
	if(enemy.hp low)
		attack
	//enemy aggressive and hp high and self not aggressive
	wait
if(enemy.hp substantially lower)
	attack
if(extremely aggressive)
	attack
if(self.hp low) 
	if(safe)
		wait
	if(hp spawner)
		if(hiding point)
			run whichever closer
		else
			run for spawner
	if(hidding point)
		run for hidding point
		
	run away dodging bullets
	
```
Attack
```
	if (in range)
		aim
		shoot
	else
		approach range
```
Run:
```
	head direction as a sine function
```

Randomization:
- shooting error
- aiming time

## Bindings generation:

	Input:PLAYER Player
	Match:(.*) (.*)
	Replace:    public static readonly EntityPrefabNameBinding $1_BINDING = new EntityPrefabNameBinding\(Type.$1, "$2"\);
	Output:    public static readonly EntityPrefabNameBinding PLAYER_BINDING = new EntityPrefabNameBinding(Type.PLAYER, "Player");

