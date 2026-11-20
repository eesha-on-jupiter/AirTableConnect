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
