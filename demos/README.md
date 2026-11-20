# AirTableConnect - Demos

> **Status:** specified, not yet built. `.gh` is a binary archive, so these can't be written from a coding session: build each one by hand in Grasshopper and save it here as `NN-name.gh` (e.g. `01-connect.gh`). The wiring below is checked against the current component ports.

Load the built assembly first: `bin/Debug/net7.0-windows/AirTableConnect.gha` (Rhino 8) or `bin/Debug/net48/AirTableConnect.gha` (Rhino 7). Every demo needs an Airtable personal access token on a text Panel wired to each HTTP component's **Token** input, with the right scopes and base access, see [docs/authentication.md](../docs/authentication.md). Write components fire only when their button is clicked.

---

## 01 - Connect
**Goal:** confirm the token reaches the Airtable API.

- Token Panel → **Auth > Connect to Airtable** `Token`.
- Read `Status` (`OK`) and `Response` (`{"id":"usr..."}`); any other `Status` is the HTTP error from Airtable.

## 02 - Discover the schema
**Goal:** find a base and its tables without leaving Grasshopper.

- Token → **Base > List Bases** `Token`; read `Base Names` and `Base IDs`.
- Pick one ID with List Item → **Base > Get Base Schema** `Base ID`; Token → `Token`.
- Read `Table Names` and `Table IDs`; the full field and view detail is in `Response`.

## 03 - Create a table layout
**Goal:** scaffold a new table with typed columns, including a color-coded select field.

- **Presets > Field Type** → **Field > Build Field JSON** `Field Type` for each column; a Panel → `Field Name`. Keep a text column first: it becomes the primary field.
- For a select column: a Panel of labels → **Field > Build Select Choices** `Choice Labels`; one **Presets > Airtable Color** per label, merged in order → `Colors`. Its `Options JSON` → that column's Build Field JSON `Options JSON`, with `singleSelect` as `Field Type`.
- Merge every `Field JSON` → **Table > Build Table JSON** `Field JSON`; a Panel → `Table Name`.
- `Table JSON` → **Table > Create Table Layout** `Table JSON`; Token → `Token`; a base ID → `Base ID`. Click **Create Table**; read `Table ID`.
- To add a column to an existing table later, send one `Field JSON` to **Field > Create Field** with `Base ID` and `Table ID` instead.

## 04 - Push parametric data
**Goal:** turn a tree computed in Grasshopper into Airtable rows in one click.

- A Panel of column names → **Record > Build Records From Table** `Field Names`.
- A tree with one branch per row (e.g. room name, area, level) → `Values`. Leave `Record IDs` empty.
- `Record JSON` → **Record > Create Records** `Record JSON`; Token → `Token`; `Base ID`; table name or ID → `Table`.
- Set `Typecast` to `True` so text values land in number, checkbox and select fields. Click **Create Records**; read `Record IDs`.

## 05 - Drive generation from Airtable
**Goal:** pull records into Grasshopper and vary geometry per record.

- Token → **Record > List Records** `Token`; `Base ID`; `Table`. Optionally a formula → `Filter Formula` (e.g. `{Level} = 2`), a view → `View`, a cap → `Max Records`.
- `Record JSON` → **Record > Get Field From Records** `Record JSON`; a field name → `Field Name`. One Get Field From Records per column.
- Each `Field Value` list lines up with `Record IDs`: convert with Number/Text params and drive geometry per item (e.g. width and depth into a Rectangle).

## 06 - Write back and sync
**Goal:** write computed results back to their records, then rerun without creating duplicates.

- Start from demo 05: List Records `Record IDs` → **Record > Build Records From Table** `Record IDs`.
- Result column name → `Field Names`; computed values grafted one per branch → `Values`.
- `Record JSON` → **Record > Update or Sync Records** `Record JSON`; Token, `Base ID`, `Table`. Click **Update Records**.
- Sync variant: skip `Record IDs`, include a unique key column (e.g. `Panel ID`) in `Field Names` and `Values`, set `Upsert` to `True` and wire `Panel ID` → `Fields To Merge On`. Each click updates matching rows and inserts new ones.

## 07 - Attach pictures
**Goal:** attach images to records from a URL, a file or a live bitmap.

- From a URL: **Attachment > Attachment From URL** `URL` (and optional `Filename`) → its `Attachments JSON` into a branch of **Record > Build Records From Table** `Values`, under an attachment field name in `Field Names` → **Record > Create Records**.
- From a file or bitmap: file path → **Attachment > Attachment From File Or Bitmap** `File Path` (or a bitmap → `Bitmap`); set `Content Type` (e.g. `image/jpeg`) for a file.
- `Attachment Upload JSON` → **Attachment > Upload Picture** `Attachment Upload JSON`; Token, `Base ID`, an existing record ID → `Record ID`, the attachment field name → `Field`. Click **Upload**.
