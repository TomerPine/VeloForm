# Data findings: OSM Israel
## Manual checks (roads I know)
| OSM id | OSM tags | Reality | Match? |
|---|---|---|---|
| 391380656 | highway=track, no surface/tracktype | Hard-packed dirt, good for gravel | Partial: type correct, surface missing |
| 561744216 | trunk, lanes=2, oneway=yes, maxspeed=90, shoulder=both | Intercity road, 2 lanes each way, divider, shoulders | Full match |
| 744369074 | trunk, lanes=1, oneway=yes, maxspeed=60, shoulder=both, surface=asphalt | Shoulder exists but very narrow, guardrail right after it. I'd avoid it | Tag correct but insufficient: no width info |



## Conclusions
- Road classification (`highway`) matches reality in all checked samples.
- `shoulder` exists on some main roads and was accurate where present,
  but it only says *whether* a shoulder exists, not how wide it is.
  A narrow shoulder (e.g. Route 66 segment) looks the same as a wide one (Route 65).
- Tracks can be missing `surface` and `tracktype` entirely
  (e.g. 391380656)

## Decisions
- Road safety will take multiple things into consideration, not a single tag.
  (`shoulder` + `lanes` + `maxspeed` + road class.)
- Tracks without `surface` are treated as "unpaved, unknown quality":
  allowed for gravel/MTB, excluded for road bikes.
- Show the user a data-confidence indicator for segments with missing tags. (?)
- Idea: personal avoid list, so the user can mark segments to avoid
  (covers cases the data can't capture, like narrow shoulders).

# Tags that will be used

### Identification
| Field | Use |
|---|---|
| Way `id` | Unique key for each road. Used for debugging, data checks, and the personal avoid list |
| Node `id` | Builds the graph: shared nodes between ways are intersections |
| `name`, `ref` | Display only (e.g. "Route 65") |

### Routing (for MVP)
| Tag | Values | Use |
|---|---|---|
| `highway` | trunk, primary, ..., track, path, cycleway | Which roads are allowed per ride type |
| `oneway` | yes / no / -1 | Direction of travel allowed |
| `access`, `bicycle` | no, private, yes, designated | Exclude roads where bikes are not allowed |
| `surface` | asphalt, compacted, dirt, gravel, sand... | Paved vs unpaved; ride type filter |

## `highway` values reference

| Value | Meaning | Road bike | Gravel | MTB |
|---|---|---|---|---|
| `motorway` | Freeway: grade-separated, interchanges, no junctions. Bikes not allowed | ✗ | ✗ | ✗ |
| `trunk` | Main intercity road, often divided, not a freeway | ✓ (if shoulder) | ✗ | ✗ |
| `primary` | Major road connecting cities | ✓ | ✗ | ✗ |
| `secondary` | Road connecting towns | ✓ | ~ | ✗ |
| `tertiary` | Local road connecting small towns/villages | ✓ | ✓ | ~ |
| `*_link` | Ramp/connector of the class above (e.g. `trunk_link`) | same as parent | same as parent | ✗ |
| `unclassified` | Minor public road, lowest rank of the road network (often rural/agricultural) | ✓ | ✓ | ✓ |
| `residential` | Street in a residential area | ✓ | ✓ | ✓ |
| `living_street` | Residential street where pedestrians have priority | ✓ | ✓ | ✓ |
| `service` | Access road: parking lot, driveway, gas station | ~ | ✓ | ✓ |
| `track` | Agricultural/forestry dirt road (quality in `tracktype`) | ✗ | ✓ | ✓ |
| `path` | Generic narrow path (multi-use, often singletrack) | ✗ | ~ | ✓ |
| `cycleway` | Dedicated bike path | ✓ (if paved) | ✓ | ✓ |
| `footway` | Footpath/sidewalk; bikes only if `bicycle=yes` | ✗ | ~ | ~ |
| `pedestrian` | Pedestrian street/plaza | ✗ | ✗ | ✗ |
| `bridleway` | Horse path | ✗ | ~ | ~ |
| `steps` | Stairs | ✗ | ✗ | ✗ |
| `construction` | Road under construction | ✗ | ✗ | ✗ |
| `proposed` | Planned road, does not exist yet | ✗ | ✗ | ✗ |

✓ = allowed/preferred · ~ = allowed with a penalty, depends on other tags · ✗ = excluded

Notes:
- `access=*` and `bicycle=*` override this table (e.g. `bicycle=no` excludes, `bicycle=yes` on a `footway` allows).
- Road bikes also need a paved `surface`; for gravel/MTB, `surface` and `tracktype` adjust the cost.
- The ✓/~/✗ marks are an initial assumption, to be tuned in stage 3.

### Preferences 
| Tag | Use |
|---|---|
| `tracktype` | Quality of dirt roads (grade1 = good, grade5 = bad) |
| `shoulder` | Part of the road safety score |
| `lanes` | Part of the road safety score |
| `maxspeed` | Part of the road safety score |
| `cycleway` | Prefer roads/paths with bike infrastructure |

### Elevation handling
| Tag | Use |
|---|---|
| `bridge`, `tunnel` | Don't sample DEM along these segments (DEM shows the ground below/above, not the road). Interpolate linearly between the segment's endpoint elevations instead |

# Data numbers


### Main roads (trunk–tertiary): 73,733 ways.
  - `shoulder`: 314 (~0.4%). Too sparse to rely on.
  - `lanes`: 27,302 (~37%). Usable; missing usually means 1 lane per direction.


### Tracks/paths/cycleways: 108,744 ways.
  - `surface`: 15,260 (~14%). Most unpaved ways have no surface info, so surface must be inferred (rules first, ML model later).