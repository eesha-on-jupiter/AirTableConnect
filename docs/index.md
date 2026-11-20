# AirTableConnect

AirTableConnect is a Grasshopper plugin for Rhino 7/8 (Windows) that wraps the [Airtable Web API](https://airtable.com/developers/web/api/introduction).
It reads and writes Airtable bases straight from a Grasshopper canvas: discover bases and tables, scaffold new tables and fields, push a data tree as rows, pull records back to drive geometry, write computed results back by record ID or by a key field, and attach images to records.

- **Platform:** [Airtable Web API](https://airtable.com/developers/web/api/introduction) v0
- **Auth:** Airtable personal access token required, see [Authentication](authentication.md)
- **Version:** 1.0.0
- **Rhino support:** Rhino 7 (.NET Framework 4.8), Rhino 8 Windows (.NET 7), Mac not yet supported
- **License:** _(added once licensed)_
- **GitHub:** _(added once the repo is published)_
- **Food4Rhino:** _(added once listed on food4rhino.com)_
- **Released:** _(added on first public release)_

_(space for a demo video or GIF)_

## Features

- **Schema discovery on the canvas:** [List Bases](components/Base/GetBases.md) and [Get Base Schema](components/Base/GetBaseSchema.md) return base and table names and IDs, so nothing has to be copied out of the Airtable web UI.
- **Tables and fields from Grasshopper:** [Build Field JSON](components/Field/FieldJson.md) and [Build Table JSON](components/Table/TableJson.md) assemble a schema locally; [Create Table Layout](components/Table/CreateTable.md), [Create Field](components/Field/CreateField.md) and [Update Field](components/Field/UpdateField.md) push it.
- **Color-coded select fields:** [Build Select Choices](components/Field/SelectFieldOptions.md) pairs choice labels with Airtable's 40 fixed color names from the [Airtable Color](components/Presets/AirtableColorPreset.md) preset.
- **Tree-to-rows in one push:** [Build Records From Table](components/Record/BuildRecordsFromGrid.md) turns a field-name list and a values tree (one branch per record) into Record JSON; [Create Records](components/Record/CreateRecords.md) sends any number of rows, auto-chunked into Airtable's 10-records-per-call limit.
- **Full-table reads:** [List Records](components/Record/ListRecords.md) follows Airtable's pagination cursor until the table (or Max Records) is exhausted, with optional formula and view filters; [Get Field From Records](components/Record/GetRecordField.md) pulls one column out as a list.
- **Write-back and upsert:** [Update or Sync Records](components/Record/UpdateRecords.md) patches records by ID, or upserts them by one or more key fields so a rerun updates existing rows and inserts new ones without duplicates.
- **Attachments from a URL, a file or a bitmap:** [Attachment From URL](components/Attachment/AttachmentFromUrl.md) embeds a hosted file into a record at create/update time; [Attachment From File Or Bitmap](components/Attachment/AttachmentFromFileOrBitmap.md) plus [Upload Picture](components/Attachment/UploadAttachment.md) uploads local bytes to an existing record.
- **Presets:** value lists for the 14 supported field types and the 40 select-choice colors, see [Presets tab](components/Presets/index.md).

## Install

Drop the `.gha` for your Rhino version into your Grasshopper `Libraries` folder and restart Rhino (a Package Manager listing is not published yet).

## Components by tab

| Tab | Type | Components | What it's for |
|---|---|---|---|
| [Auth](components/Auth/index.md) | HTTP | 1 | Check a personal access token against Airtable |
| [Base](components/Base/index.md) | HTTP | 2 | List accessible bases and the tables in a base |
| [Table](components/Table/index.md) | Mixed | 2 | Build Table JSON and create a new table from it |
| [Field](components/Field/index.md) | Mixed | 4 | Build field and select-choice JSON, create or update a field |
| [Record](components/Record/index.md) | Mixed | 5 | List, create, update and upsert records, build and read Record JSON |
| [Attachment](components/Attachment/index.md) | Mixed | 3 | Build attachment JSON from a URL, file or bitmap and upload it |
| [Presets](components/Presets/index.md) | Preset | 2 | Value lists for field types and select-choice colors |

19 components total.

## How writes are triggered

- The six components that change data in Airtable only fire on a click of the button drawn on the component, never on canvas recompute: Create Table Layout, Create Field, Update Field, Create Records, Update or Sync Records, Upload Picture.
- Their outputs are filled only on the solve right after the click; any later recompute clears them until the next click.
- Read components (Connect to Airtable, List Bases, Get Base Schema, List Records) call Airtable on every solve.
- JSON builders (Build Table JSON, Build Field JSON, Build Select Choices, Build Records From Table, Get Field From Records, both Attachment builders) and the Presets make no network call at all.

## Where to go next

- Need a token? See [Authentication](authentication.md).
- What you can build: [Workflows](workflows.md).
- Release history: [Changelog](changelog.md).
