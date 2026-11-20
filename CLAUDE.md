# AirTableConnect

Grasshopper plugin for the Airtable REST API. Built via PleaseREST.

## Skills, agents, MCPs, commands

- **Skills:** pr-session, pr-spec, pr-todo, pr-conventions, pr-file-structure, pr-client,
  pr-component, pr-button, pr-builders, pr-preset, pr-utilities, pr-assembly, pr-unit-tests,
  pr-bulk-data, pr-icons, pr-docs, pr-gh-package, pr-ci-template, pr-vs-project, pr-build,
  pr-rhino-test, pr-yak-package, pr-dogfood, pr-permissions
- **Agents:** please-rest:pr-aec-advisor, please-rest:pr-api-scout, please-rest:pr-spec-critic
- **MCPs:** pr-api-extractor, pr-api-tester, pr-icon-generator
- **Commands:** /rest-new-plugin, /rest-resume-plugin, /rest-status, /rest-extract-api,
  /rest-test-api, /rest-gen-spec, /rest-gen-todo, /rest-build-component,
  /rest-build-all-components, /rest-package, /rest-add-feature

## Current build state

- **Phase:** 11 of 15 — Icons (complete)
- **Last completed step:** `icons.json` authored at the plugin root and all 20 icons rendered via
  `pr-icon-generator` and wired into `Properties/Resources.resx` — 19 component icons (24x24) plus
  the ribbon/category logo (16x16). Placeholder resources from Phases 8-10 are fully replaced.
  See "Icon system" below. Rebuild after: 0 errors, 24 warnings (all the expected `CA1416`
  net7.0-Mac set — 21 from `ButtonComponent.cs` plus 3 from `AttachmentFromFileOrBitmap.cs`'s
  `Bitmap`/`Image.Save`/`ImageFormat.Png` calls; see dogfood F4).
- **Next step:** Proceed to Phase 12 — Tests (`pr-unit-tests` for `BuilderTests.cs` +
  `BulkAssemblyTests.cs`, then `pr-rhino-test` to build the 8 demo `.gh` files and live-test every
  component in Rhino ahead of Gate 6).
- **Last active:** 2026-09-03
- **Gates passed:** 1 (Approve API), 2 (Approve structure), 3 (Select features), 4 (Resolve
  failed calls), 5 (Test auth in Rhino)
- **Gates pending:** 6 (Test in Rhino), 7 (Cross-platform test)

## Icon system

`icons.json` (plugin root) drives every icon. `plugin_prefix` is **`ATC`** — confirmed at Phase 11.
Two colours and four shapes; there is no background-watermark layer (see below for why).

| Layer | Encodes | Values |
|---|---|---|
| Colour | does it change your Airtable data? | **red `#F82B60` → `#B0113F` = writes** · **blue `#2D7FF9` → `#1B4FA8` = everything else** |
| Shape | kind of operation | `hexagon` = auth · `rounded-square` = live API call · `cut-square` = local JSON builder (no HTTP) · `bubble` = preset value list |
| FG glyph | the action | magnify, plus, pencil, sync, upload, … — always white `#FFFFFF` |

Colour is a **risk** signal, not a domain one: the six red components are exactly the ones that
mutate Airtable (`CreateTable`, `CreateField`, `UpdateField`, `CreateRecords`, `UpdateRecords`,
`UploadAttachment` — all button components). Everything else is blue and safe to recompute. Shape
carries the orthogonal live-call-vs-local-builder distinction, so the two never collide.

| Colour | Shape | Components |
|---|---|---|
| Red | rounded-square | CreateTable, CreateField, UpdateField, CreateRecords, UpdateRecords, UploadAttachment |
| Blue | rounded-square | GetBases, GetBaseSchema, ListRecords |
| Blue | cut-square | TableJson, FieldJson, SelectFieldOptions, BuildRecordsFromGrid, GetRecordField, AttachmentFromUrl, AttachmentFromFileOrBitmap |
| Blue | hexagon | AirTableConnectAuth |
| Blue | bubble | AirtableColorPreset, FieldTypePreset |

The ribbon logo (`ATC_AirTableConnectLogo`, 16x16) is a white `mdi:table` on the red gradient — it
is the plugin's identity mark, not a component, so the "red = writes" rule does not apply to it.

**No `background.icon` watermark — deliberately.** An earlier revision put a white resource
silhouette (`mdi:database`, `mdi:table`, `mdi:table-row`, `ph:brackets-curly-bold`) behind each
glyph at 0.35-0.40 opacity. Because `background.icon` defaults to filling the whole canvas and
those glyphs are near-solid silhouettes, the wash desaturated the *entire* tile — body pixels went
`#C31A49` → `#DB7692` — while the rounded corners, the only area the silhouette could not reach,
kept the true gradient. The result read as an unintended dark border around a bleached icon. Do
not reintroduce a full-bleed `background.icon`; if a watermark is ever wanted, give it an explicit
small `align.fit` so it reads as a motif rather than a wash.

An earlier three-colour scheme (red/blue/yellow by resource domain) was replaced at the user's
request. Yellow is gone entirely; the dark `#181D1F` foreground it required is gone with it, so
every glyph is now white.

`Icons/_contact-sheet.png` is a 6x-scaled review montage of all 20, not a shipped asset — safe to
delete before packaging.

## Platform context

- **Base URL:** `https://api.airtable.com` (note: `uploadAttachment` uses a different host,
  `https://content.airtable.com` — see Known limitations / builders)
- **Auth method:** Bearer token (Personal Access Token), `Authorization: Bearer {token}` header
- **API version:** v0
- **Resource hierarchy:** Base (`appXXXX`) → Table (`tblXXXX`) → Field (`fldXXXX`) / Record
  (`recXXXX`). Meta endpoints (`/v0/meta/...`) manage bases/tables/fields; data endpoints
  (`/v0/{baseId}/{tableIdOrName}`) manage records.

## Plugin structure

Tab: **AirTableConnect** · 19 components across **7 canvas subcategories** (Auth, Base, Table,
Field, Record, Attachment, Presets). Each JSON-builder component sits in the same subcategory as
the live call it feeds — the icon shape distinguishes them (`cut-square` = local builder,
`rounded-square` = live API call). A 12-subcategory layout that split every `X` / `X JSON` pair
was collapsed at Phase 11. `Utilities` holds the three no-canvas helper classes and is not a
ribbon panel.

| Subcategory | Component | Type | Role | Inputs → Outputs |
|---|---|---|---|---|
| Auth | AirTableConnectAuthComponent | component | write | Token → Status, Response |
| Base | GetBasesComponent | component | write | Token → Base Names, Base IDs, Status, Response |
| Base | GetBaseSchemaComponent | component | write | Token, Base ID → Table Names, Table IDs, Status, Response |
| Table | CreateTableComponent | button | write | Token, Base ID, Table JSON → Table ID, Status, Response |
| Table | TableJsonComponent | component | aggregator | Table Name, Description, Field JSON (list) → Table JSON |
| Field | CreateFieldComponent | button | write | Token, Base ID, Table ID, Field JSON → Field ID, Status, Response |
| Field | UpdateFieldComponent | button | write | Token, Base ID, Table ID, Field ID, Field JSON → Status, Response |
| Field | FieldJsonComponent | component | unit | Field Name, Field Type, Description, Options JSON → Field JSON |
| Field | SelectFieldOptionsComponent | component | aggregator | Choice Labels (list), Colors (list) → Options JSON |
| Record | ListRecordsComponent | component | write | Token, Base ID, Table, Filter Formula, View, Max Records → Record IDs, Record JSON, Status, Response |
| Record | CreateRecordsComponent | button | write | Token, Base ID, Table, Record JSON (list), Typecast → Record IDs, Status, Response |
| Record | UpdateRecordsComponent | button | write | Token, Base ID, Table, Record JSON (list), Upsert, Fields To Merge On, Typecast → Record IDs, Status, Response |
| Record | BuildRecordsFromGridComponent | component | aggregator | Field Names (list), Values (tree), Record IDs (list) → Record JSON (list) |
| Record | GetRecordFieldComponent | component | unit | Record JSON (list), Field Name → Field Value (list) |
| Attachment | UploadAttachmentComponent | button | write | Token, Base ID, Record ID, Field, Attachment Upload JSON → Status, Response |
| Attachment | AttachmentFromUrlComponent | component | unit | URL, Filename → Attachments JSON |
| Attachment | AttachmentFromFileOrBitmapComponent | component | unit | File Path, Bitmap, Filename, Content Type → Attachment Upload JSON |
| Presets | AirtableColorPresetComponent | preset | preset | — → Color (40 fixed values) |
| Presets | FieldTypePresetComponent | preset | preset | — → Field Type (14 curated values) |

Utility classes (no canvas presence): `AirTableConnectClient.cs`, `PluginUtilities.cs`,
`ButtonComponent.cs`.

## JSON builders

| Builder | Covers | Used by |
|---|---|---|
| TableBuilders.cs | Table | TableJsonComponent, CreateTableComponent |
| FieldBuilders.cs | Field | FieldJsonComponent, SelectFieldOptionsComponent, CreateFieldComponent, UpdateFieldComponent |
| RecordBuilders.cs | Record | BuildRecordsFromGridComponent, GetRecordFieldComponent, CreateRecordsComponent, UpdateRecordsComponent |
| AttachmentBuilders.cs | Attachment | AttachmentFromUrlComponent, AttachmentFromFileOrBitmapComponent, UploadAttachmentComponent |

## Key design decisions

- **AEC features:** Auth Manager (the whoami Auth component, no extra work) and Batch Processor
  (CreateRecords/UpdateRecords auto-chunk into groups of 10 per Airtable's cap; ListRecords
  auto-loops the `offset` pagination cursor instead of truncating at one page).
- **BuildRecordsFromGridComponent handles both single and bulk records** with one component (a
  single branch in the Values tree, or a full tabular tree) — no separate unit/aggregator pair,
  per the Gate 2 simplification pass.
- **Attachments JSON vs. Attachment Upload JSON are deliberately separate contracts** — the
  former is plain metadata embedded into a record at create/update time (via a URL); the latter
  is a base64 binary upload against an *existing* record on a different host
  (`content.airtable.com`), not interchangeable.
- **Upsert** (Update or Sync Records → `performUpsert.fieldsToMergeOn`) backs the
  "Sync Generative Model to Airtable by Key" workflow — keeps a table in lock-step with a live
  parametric model without manual record-ID tracking.

## Known limitations

- **Skipped for v1:** `get_record` (no workflow needs single-record fetch by ID — `list_records`
  with `filterByFormula`/`view` covers targeted reads), `replace_records` (destructive full
  replace, PATCH `update_records` covers every stated case), `delete_records` (destructive,
  no workflow needs deletion), RecordComment resource (unused), Webhook resource (Grasshopper
  has no persistent server to receive callbacks).
- **API extraction was manual, not scraped:** Airtable's docs site
  (`airtable.com/developers/web/api/*`) is a JS-rendered SPA; `pr-api-extractor`'s non-headless
  scraper returned `MISSING` for all 10 endpoints. `call-library.json` was filled in by hand from
  Airtable's stable, well-documented public REST contract instead. Logged as dogfood finding F1
  (TOOL, open) in `dogfood/AirTableConnect-run-01.md`.
- **All 10 endpoints are now live-validated** in `spec/call-library-validated.json`, chained
  through a real "Test Base" (`appNNi5g3qy6CBtLA`, table `Table 1` / `tblRIhMkvHjb3N71Z`) the user
  granted the PAT access to mid-Phase-7 (initially the PAT had zero accessible bases — see dogfood
  F2/F3). The trail: `list_bases` → real base → `get_base_schema` → real table/field IDs →
  `create_records`/`update_records`/`create_field`/`update_field`/`create_table`/
  `upload_attachment` all exercised against real objects in that base (a validation record, a
  test field, a second table, and a 1x1 PNG attachment were created there as a side effect —
  harmless to clean up manually if desired, since it's a scratch test base).
- **Curated field types only:** `FieldTypePreset` covers 14 writable field types; formula,
  rollup, lookup, and linkedRecord are excluded (complex options schemas, no stated workflow
  needs them).
- **Plugin icon prefix `ATC` is confirmed** (was provisional through Phase 10). Phase 11 kept it, so
  `icons.json`, `Properties/Resources.resx`, `Properties/Resources.Designer.cs` and
  `AirTableConnectInfo.cs` all agree — no rename outstanding. Any future change to it must move the
  resource keys and both references together.
- **`ButtonComponent.cs` always triggers 21 `CA1416` platform-compat warnings on the net7.0 (Mac)
  build** — GDI+ rendering APIs the analyzer flags as Windows-only, even though they work in
  practice under Rhino 8 Mac's Mono-based WinForms compat layer (which is exactly why
  `pr-vs-project`'s `.csproj` references `System.Drawing.Common` + a WinForms reference assembly
  for non-Windows targets). Expected and harmless; not fixed since `ButtonComponent.cs` must stay
  verbatim per `pr-button`. Logged as dogfood F4.

## Todo list

See `spec/todo.md` for the full, current build checklist (embedded below for quick reference —
if this drifts from the file on disk, the file on disk is authoritative).

```markdown
# AirTableConnect — Build TODO

Generated from spec/plugin-spec.json (19 components, 12 tabs, AEC features: Auth Manager, Batch Processor).

---

## PHASE 1 — AUTH
- [x] AirTableConnectClient.cs        [CODE] auth headers, base HTTP setup (https://api.airtable.com), NewRequest() helper; note content.airtable.com override for uploadAttachment
- [x] AirTableConnectAuth.cs          [CODE] Token input -> Status + Response output (calls GET /v0/meta/whoami)
- [x] AirTableConnectInfo.cs          [CODE] GH_AssemblyInfo + GH_AssemblyPriority
- [x] ButtonComponent.cs              [CODE] base class — copy verbatim, namespace only
- [x] PluginUtilities.cs              [CODE] TabName + subcategory constants (Auth, Base, Table, Field, Record, Attachment, Presets — 7 merged groups as of Phase 11; JSON builders share their parent's subcategory)
- [x] Auth icon                       [ASSET] generate_icon -> add_to_resources (deferred to Phase 11 — Icons; placeholder bitmap in place so the project builds)
- [x] Verify: AirTableConnectAuthComponent authenticates against a real Personal Access Token and returns a valid whoami response (GATE 5, confirmed live in Rhino)

## PHASE 2 — SIMPLE UNITS
- [x] GetBases.cs                     [CODE] Token -> Base Names, Base IDs, Status, Response (GET /v0/meta/bases)
- [x] GetBases icon                   [ASSET] generate_icon -> add_to_resources (deferred to Phase 11)
- [x] GetBaseSchema.cs                [CODE] Token, Base ID -> Table Names, Table IDs, Status, Response (GET /v0/meta/bases/{baseId}/tables)
- [x] GetBaseSchema icon              [ASSET] generate_icon -> add_to_resources (deferred to Phase 11)
- [x] FieldBuilders.cs                [CODE] BuildFieldJson(name, type, description, optionsJson), BuildSelectOptionsJson(choiceLabels, colors)
- [x] FieldJson.cs                    [CODE] Field Name, Field Type, Description, Options JSON -> Field JSON output
- [x] FieldJson icon                  [ASSET] generate_icon -> add_to_resources (deferred to Phase 11)
- [x] CreateField.cs                  [CODE] Token, Base ID, Table ID, Field JSON -> Field ID, Status, Response (POST create_field)
- [x] CreateField icon                [ASSET] generate_icon -> add_to_resources (deferred to Phase 11)
- [x] UpdateField.cs                  [CODE] Token, Base ID, Table ID, Field ID, Field JSON -> Status, Response (PATCH update_field)
- [x] UpdateField icon                [ASSET] generate_icon -> add_to_resources (deferred to Phase 11)
- [x] AttachmentBuilders.cs           [CODE] BuildAttachmentsJson(url, filename), BuildAttachmentUploadJson(base64, contentType, filename)
- [x] AttachmentFromUrl.cs            [CODE] URL, Filename -> Attachments JSON output
- [x] AttachmentFromUrl icon          [ASSET] generate_icon -> add_to_resources (deferred to Phase 11)
- [x] AttachmentFromFileOrBitmap.cs   [CODE] File Path, Bitmap, Filename, Content Type -> Attachment Upload JSON (base64-encode file bytes or in-memory bitmap; Bitmap wins if both given)
- [x] AttachmentFromFileOrBitmap icon [ASSET] generate_icon -> add_to_resources (deferred to Phase 11)
- [x] UploadAttachment.cs             [CODE] Token, Base ID, Record ID, Field, Attachment Upload JSON -> Status, Response (POST content.airtable.com uploadAttachment)
- [x] UploadAttachment icon           [ASSET] generate_icon -> add_to_resources (deferred to Phase 11)
- [x] ListRecords.cs                  [CODE] Token, Base ID, Table, Filter Formula, View, Max Records -> Record IDs, Record JSON, Status, Response (GET list_records; offset pagination loop built into AirTableConnectClient.ListRecordsAsync)
- [x] ListRecords icon                [ASSET] generate_icon -> add_to_resources (deferred to Phase 11)
- [x] GetRecordField.cs               [CODE] Record JSON (list), Field Name -> Field Value (list, positional) — no HTTP call, pure JSON extractor
- [x] GetRecordField icon             [ASSET] generate_icon -> add_to_resources (deferred to Phase 11)
- [ ] Verify: each unit builds valid JSON, and CreateField/UpdateField/UploadAttachment/ListRecords round-trip successfully against a live base (deferred to Phase 12 — Tests / GATE 6)

## PHASE 3 — AGGREGATOR
- [x] SelectFieldOptions.cs           [CODE] Choice Labels (list), Colors (list, positional pairing) -> Options JSON; feeds FieldJson's Options JSON input
- [x] SelectFieldOptions icon         [ASSET] generate_icon -> add_to_resources (deferred to Phase 11)
- [x] TableBuilders.cs                [CODE] BuildTableJson(name, description, fieldJsonList)
- [x] TableJson.cs                    [CODE] Table Name, Description, Field JSON (list) -> Table JSON output
- [x] TableJson icon                  [ASSET] generate_icon -> add_to_resources (deferred to Phase 11)
- [x] CreateTable.cs                  [CODE] Token, Base ID, Table JSON -> Table ID, Status, Response (POST create_table)
- [x] CreateTable icon                [ASSET] generate_icon -> add_to_resources (deferred to Phase 11)
- [x] RecordBuilders.cs               [CODE] BuildRecordsFromGrid(fieldNames, valuesTree, recordIds) -> list of Record JSON strings (positional At<T> lookup) — implemented as RecordBuilders.AssembleRecords per pr-bulk-data
- [x] BuildRecordsFromGrid.cs         [CODE] Field Names (list), Values (tree), Record IDs (list, optional) -> Record JSON (list) — handles one record or many
- [x] BuildRecordsFromGrid icon       [ASSET] generate_icon -> add_to_resources (deferred to Phase 11)
- [x] CreateRecords.cs                [CODE] Token, Base ID, Table, Record JSON (list), Typecast -> Record IDs, Status, Response (POST create_records; 10/call chunking built into AirTableConnectClient.CreateRecordsAsync)
- [x] CreateRecords icon              [ASSET] generate_icon -> add_to_resources (deferred to Phase 11)
- [x] UpdateRecords.cs                [CODE] Token, Base ID, Table, Record JSON (list), Upsert, Fields To Merge On, Typecast -> Record IDs, Status, Response (PATCH update_records; 10/call chunking built into AirTableConnectClient.UpdateRecordsAsync)
- [x] UpdateRecords icon              [ASSET] generate_icon -> add_to_resources (deferred to Phase 11)
- [ ] Verify: a full table schema pushes via CreateTable, and a multi-row tabular tree pushes via BuildRecordsFromGrid -> CreateRecords, visible on Airtable (deferred to Phase 12 — Tests / GATE 6)

## PHASE 4 — LEVEL 2 AGGREGATOR
SKIPPED — Airtable has no higher-order structure beyond Table (Field-list) and Record (list); both are covered as single-level aggregators in Phase 3.

## PHASE 5 — CUSTOM / AEC FEATURES
- [x] Batch Processor: CreateRecords.cs / UpdateRecords.cs [CODE] auto-chunk incoming Record JSON list into groups of 10 (Airtable's per-call cap), issue sequential requests, concatenate Record IDs/Status/Response across chunks — implemented inside AirTableConnectClient.CreateRecordsAsync/UpdateRecordsAsync
- [x] Batch Processor: ListRecords.cs [CODE] auto-loop the `offset` pagination token internally until exhausted or Max Records reached, instead of returning only the first page — implemented inside AirTableConnectClient.ListRecordsAsync
- [ ] Verify: a 50-record push via CreateRecords succeeds in 5 chunked calls with no manual splitting; a table with >100 records returns fully via ListRecords (deferred to Phase 12 — Tests / GATE 6)

## PHASE 6 — PRESETS
- [x] AirtableColorPreset.cs          [CODE] GH_ValueList of Airtable's 40 fixed select-choice color names
- [x] AirtableColorPreset icon        [ASSET] generate_icon -> add_to_resources (deferred to Phase 11)
- [x] FieldTypePreset.cs              [CODE] GH_ValueList of the 14 curated writable field types
- [x] FieldTypePreset icon            [ASSET] generate_icon -> add_to_resources (deferred to Phase 11)
- [ ] Verify: presets wire cleanly into SelectFieldOptions / FieldJson without manual string entry (deferred to Phase 12 — Tests / GATE 6)

## PHASE 7 — TESTS
- [ ] BuilderTests.cs                 [TEST] FieldBuilders, TableBuilders, RecordBuilders, AttachmentBuilders — pure logic, input validation, JSON shape
- [ ] BulkAssemblyTests.cs            [TEST] BuildRecordsFromGrid positional joining, chunking logic (Batch Processor)
- [ ] dotnet build                    [CODE] all green (net48 + net7.0-windows / net7.0)
- [ ] dotnet test                     [CODE] all green
- [ ] demos/connection.gh             [MCP] Auth component, live whoami check
- [ ] demos/discover-schema.gh        [MCP] List Bases -> Get Base Schema
- [ ] demos/push-parametric-data.gh   [MCP] Build Records From Table -> Create Records (tabular benchmark)
- [ ] demos/drive-generation.gh       [MCP] List Records -> Get Field From Records -> geometry
- [ ] demos/write-back-and-upsert.gh  [MCP] List Records -> compute -> Build Records From Table -> Update or Sync Records (Upsert)
- [ ] demos/attach-picture.gh         [MCP] Attachment From URL / From File Or Bitmap -> Build Records From Table -> Create Records / Upload Picture
- [ ] demos/color-coded-fields.gh     [MCP] Airtable Color preset -> Build Select Choices -> Build Field JSON -> Create Field
- [ ] demos/create-table-layout.gh    [MCP] Field Type preset -> Build Field JSON -> Build Table JSON -> Create Table
- [ ] Live MCP smoke test             [MCP] load .gha, solve each demo, auto-fix on failure, flag human only if no fix
- [ ] Verify: all demos pass and demos/ is populated (or degrade gracefully to manual)

## PHASE 8 — DOCS
- [ ] docs/index.md                   [CODE] plugin overview + install
- [ ] docs/quickstart.md              [CODE]
- [ ] docs/authentication.md          [CODE] Personal Access Token setup, scopes needed
- [ ] docs/components/[sub]/[name].md [CODE] one per component (19 total)
- [ ] docs/examples/[demo].md         [CODE] one per demo workflow (8 total)
- [ ] docs/changelog.md               [CODE] starts at 1.0.0

## PHASE 9 — GITHUB
- [ ] README.md                       [CODE] may link to docs/
- [ ] CLAUDE.md                       [CODE]
- [ ] LICENSE                         [CODE]
- [ ] .gitignore                      [CODE]

## PHASE 10 — PACKAGE
- [ ] dotnet build -c Release x3      [CODE] Rhino 7 Win, Rhino 8 Win, Rhino 8 Mac
- [ ] yak build x3                    [CODE]
- [ ] .github/workflows/publish.yml   [CODE]
- [ ] Verify: .yak files in /dist/

[HUMAN GATE 6] Rhino manual test — demos pre-built & pre-checked; open demos/ and confirm live in GH
[HUMAN GATE 7] Cross-platform test — install .yak on Rh7 Win / Rh8 Win / Rh8 Mac
```
