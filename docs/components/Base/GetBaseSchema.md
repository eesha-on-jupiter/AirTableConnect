# Get Base Schema

Lists the tables defined in an Airtable base.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | "" | Airtable personal access token. |
| Base ID | Text | Yes | "" | Base to inspect (`app...`). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Table Names | Text (list) | Table names in the base. |
| Table IDs | Text (list) | Table IDs (`tbl...`), in the same order. |
| Status | Text | `OK` if the call succeeded, otherwise the error. |
| Response | Text | Raw JSON response from Airtable. |

## API endpoint

`GET /v0/meta/bases/{baseId}/tables`

## Notes

- Runs on every solve; read-only.
- An empty `Base ID` returns `Base ID is empty.` without calling Airtable.
- Needs the `schema.bases:read` scope and access to the base.
- Only table names and IDs are split out; each table's fields (with field IDs) and views are in `Response`.

## Related

- [List Bases](GetBases.md): source of the `Base ID`.
- [List Records](../Record/ListRecords.md), [Create Field](../Field/CreateField.md): take a table name or ID from here.
