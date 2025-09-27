# Megabonk Minimap

A BepInEx plugin that lets you adjust the minimap:

- **Size** → how large the minimap UI appears on screen
- **Zoom** → how much of the map the minimap camera shows
  - Higher zoom value = zoomed out (see more of the map)
  - Lower zoom value = zoomed in (see less)
- **Always Show Boss** → if boss arrow should be visible before you spot the portal
  - true: the minimap creates the boss arrow immediately.
  - false: the arrow only appears after the portal is spotted (default behaviour).

---

## ⚙️ Configuration
After the first run, edit:
`BepInEx/config/svindler_MegabonkMinimap.cfg`
Options:

```ini
[Minimap]
## Scale of the minimap UI (higher = bigger minimap on screen).
Size = 1.5

## Zoom level of the minimap camera.
## Higher = zoomed out (see more of the map).
## Lower = zoomed in (see less).
Zoom = 110

## If true, the minimap creates the boss arrow immediately.
## If false, the arrow only appears after the portal is spotted (default behaviour).
AlwaysShowBossArrow = false
```
