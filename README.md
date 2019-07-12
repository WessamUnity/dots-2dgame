# DOTS Training Project
## 2D Game

### Goal
Create a 2D top-down game where you control the hero using mouse input. The hero can rotate around 360 degrees. The hero is trying to destroy enemies flying randomly across the screen by shooting projectiles towards them. When a projectile collides with an enemy, both the enemy and the projectile are destroyed.

This repo contains one possible implementation for the proposed 2D game. Feel free to use it as a reference.


### Plan
Starting with DOTS can sometimes be daunting. Hence, the following is one possible plan of how to approach the project. In no way is it the best or the only plan, just one of many possible.

1. Start with spawning a level entity from GameObject prefab. A good starting point where you do not need to worry about functionality yet, just spawning the entity.
2. Spawn one entity representing the player in a random position within the level, there should not be any functionality yet as well.
3. Next, move on to spawning a bot entity in a random position within the level. Then spawn many bots. Later, make the bots move in random directions around the level, make them bounce back when they reach the level bounds.
4. From there, spawn projectiles and make them move forwards according to their rotation and pre-set speed. Use the player location and rotation to spawn the projectiles. Add projectile collision with bots.
5. Capture input and use it to spawn projectiles on demand.

### Additional challenges
- Pool projectiles
- Respawn bots when dying
- Add scoring and UI

### Resources
- You can use the Materials / Prefabs as a starting point to make your life easier
- Refer to the DOTS basic and advanced samples for The Official Way™ to do things in DOTS
