<!-- pr:begin header -->
<img src="ATC_Plugin_Icon.png" alt="AirTableConnect" width="96" align="left" style="margin-right:16px">

# AirTableConnect
> Grasshopper plugin for the Airtable REST API, by Eesha Jain
>
> Built with the [PleaseREST](https://github.com/eesha-on-jupiter/PleaseREST) Claude Code plugin, an AI pipeline that turns a REST API reference into a Grasshopper plugin.

Read and write Airtable bases from a Grasshopper canvas: discover bases and tables, scaffold new tables and fields, push a data tree as rows, pull records back to drive geometry, and write results back by record ID or by a key field.

<br clear="left">

## What you can do with it

Keep an Airtable base in step with a parametric model, and drive the model from the base:

- **Schedules and takeoffs as rows**: a Grasshopper data tree (one branch per record) becomes Airtable rows in one click, with any number of rows sent in batches for you.
- **Geometry driven by a table**: list a table's records, pull out one field per column, and vary geometry per record.
- **Write-back and sync**: update records by ID, or upsert by a key field so a rerun updates existing rows and inserts new ones without duplicates.
- **Tables and fields from a definition**: build a schema on the canvas, including color-coded select fields, and create the table or add a field to an existing one.
- **Pictures on records**: attach an image from a URL, a file on disk or a live bitmap.

Component chains for each of these are in [demos/README.md](demos/README.md).

**Under the hood**

- The six components that change Airtable data are buttons; nothing is written on canvas recompute. Read components call Airtable on every solve, and JSON builders make no network call at all.
- Create Records and Update or Sync Records split any list into Airtable's 10-records-per-call limit. List Records follows the pagination cursor until the table (or Max Records) is exhausted.
- Calls are paced 350 ms apart to stay under Airtable's 5 requests per second per base.
<!-- pr:end header -->

<!-- pr:begin install -->
## Installation

**Food4Rhino:** (pending first release)
**Yak:** (pending first release)

**From source:** `dotnet build AirTableConnect.csproj`, then drop the `.gha` for your Rhino version into Grasshopper's `Libraries` folder (Rhino 7: `bin/Debug/net48`; Rhino 8 Windows: `bin/Debug/net7.0-windows`) and restart Rhino, or point Grasshopper's developer-folder setting at the build folder. Close Rhino before rebuilding; it locks the `.gha`.

**Rhino 8 note:** the Rhino 8 build targets .NET 7 and loads only when Rhino runs on the .NET Core runtime (its default). If the AirTableConnect tab is missing, run `SetDotNetRuntime` in Rhino, choose **.NET Core**, and restart.
<!-- pr:end install -->

<!-- pr:begin quickstart -->
## Quick start

1. Create an Airtable **personal access token** at [airtable.com/create/tokens](https://airtable.com/create/tokens), with the scopes you need and access to at least one base. See [docs/authentication.md](docs/authentication.md).
2. Drop **Connect to Airtable** (Auth tab) and wire the token into `Token`; `Status` reads `OK`.
3. **List Bases** → **Get Base Schema** (Base tab) to find the `Base ID` and `Table ID` you want.
4. Build rows: a list of column names into **Build Records From Table** `Field Names`, and a tree with one branch per row into `Values`.
5. Wire its `Record JSON` into **Create Records** with the token, `Base ID` and `Table`, set `Typecast` to `True`, and click **Create Records**.

Reference for every component: [docs/index.md](docs/index.md).
<!-- pr:end quickstart -->

<!-- pr:begin components -->
## Components

| Tab | Component | Inputs | Outputs | Description |
|---|---|---|---|---|
| Auth | Connect to Airtable | Token | Status, Response | Verifies a Personal Access Token against Airtable and returns the connected user's ID. |
| Base | List Bases | Token | Base Names, Base IDs, Status, Response | Lists the Airtable bases the connected token can access. |
| Base | Get Base Schema | Token, Base ID | Table Names, Table IDs, Status, Response | Lists the tables defined in an Airtable base. |
| Table | Build Table JSON | Table Name, Description, Field JSON | Table JSON | Assembles a table name, description, and Field JSON list into Table JSON. |
| Table | Create Table Layout | Token, Base ID, Table JSON | Table ID, Status, Response | Creates a new table from Table JSON. |
| Field | Build Field JSON | Field Name, Field Type, Description, Options JSON | Field JSON | Assembles a field name, type, description, and options into Field JSON. |
| Field | Build Select Choices | Choice Labels, Colors | Options JSON | Assembles choice labels and colors into Options JSON for a select field. |
| Field | Create Field | Token, Base ID, Table ID, Field JSON | Field ID, Status, Response | Creates a field on an existing table from Field JSON. |
| Field | Update Field | Token, Base ID, Table ID, Field ID, Field JSON | Status, Response | Updates an existing field's name, description, or options from Field JSON. |
| Record | List Records | Token, Base ID, Table, Filter Formula, View, Max Records | Record IDs, Record JSON, Status, Response | Lists records from a table, optionally filtered, auto-paginating past 100 rows. |
| Record | Get Field From Records | Record JSON, Field Name | Field Value | Extracts one field's value from each record in a Record JSON list. |
| Record | Build Records From Table | Field Names, Values, Record IDs | Record JSON | Assembles field names and a per-record values tree into a Record JSON list. |
| Record | Create Records | Token, Base ID, Table, Record JSON, Typecast | Record IDs, Status, Response | Creates records from a Record JSON list, auto-chunking past 10 rows per call. |
| Record | Update or Sync Records | Token, Base ID, Table, Record JSON, Upsert, Fields To Merge On, Typecast | Record IDs, Status, Response | Updates or upserts records from a Record JSON list, auto-chunking past 10 rows per call. |
| Attachment | Attachment From URL | URL, Filename | Attachments JSON | Assembles a URL and filename into embeddable Attachments JSON. |
| Attachment | Attachment From File Or Bitmap | File Path, Bitmap, Filename, Content Type | Attachment Upload JSON | Base64-encodes a file path or bitmap into Attachment Upload JSON for an existing record. |
| Attachment | Upload Picture | Token, Base ID, Record ID, Field, Attachment Upload JSON | Status, Response | Uploads a base64-encoded attachment to an existing record. |
| Presets | Airtable Color | None | Value list | Airtable's 40 fixed select-choice colors, for Build Select Choices. |
| Presets | Field Type | None | Value list | 14 writable field types, for Build Field JSON. |
<!-- pr:end components -->

<!-- pr:begin requirements -->
## Requirements

- Rhino 7 or Rhino 8 on Windows. Mac is not yet supported.
- An Airtable account with access to the base you want to work with
- An Airtable **personal access token** with access to that base and these scopes:
  - `schema.bases:read`: List Bases, Get Base Schema
  - `schema.bases:write`: Create Table Layout, Create Field, Update Field
  - `data.records:read`: List Records
  - `data.records:write`: Create Records, Update or Sync Records, Upload Picture

A read-only canvas needs only the two `:read` scopes.

## Known limitations

- Deleting records, full replace, single-record fetch, record comments and webhooks are not included.
- Field types are limited to 14 writable types. Formula, rollup, lookup and linked-record fields cannot be created from the plugin.
- A batch that fails partway stops at the failing chunk; chunks already sent stay written.

## Documentation

- [docs/index.md](docs/index.md): overview and every component by tab
- [docs/authentication.md](docs/authentication.md): tokens, scopes and base access
- [docs/workflows.md](docs/workflows.md): what you can build
- [demos/README.md](demos/README.md): seven wirings, step by step
- [docs/changelog.md](docs/changelog.md)

## Development

```
dotnet build AirTableConnect.csproj      # net48, net7.0-windows, net7.0
```

This plugin was generated with the **PleaseREST** Claude Code plugin; `CLAUDE.md` holds the build state and design notes. The build is at pipeline Phase 11 of 15: the components and icons are done, and the unit tests, demo `.gh` files and packages are still to come.
<!-- pr:end requirements -->

<!-- pr:begin license -->
## License

AirTableConnect is released under the MIT License. Copyright (c) 2026 Eesha Jain. See [LICENSE](LICENSE).
<!-- pr:end license -->
