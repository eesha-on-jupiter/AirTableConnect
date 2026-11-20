# Create Table Layout

Creates a new table in a base from Table JSON.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | "" | Airtable personal access token. |
| Base ID | Text | Yes | "" | Base to create the table in (`app...`). |
| Table JSON | Text | Yes | "" | Assembled table JSON. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Table ID | Text | ID of the created table (`tbl...`). |
| Status | Text | `OK` if the call succeeded, otherwise the error. |
| Response | Text | Raw JSON response from Airtable. |

## API endpoint

`POST /v0/meta/bases/{baseId}/tables`

## Notes

- Button-triggered: only fires when **Create Table** is clicked; outputs clear on the next recompute.
- Each click creates another table; Airtable rejects a name already used in the base.
- Needs the `schema.bases:write` scope.
- `Response` includes the new table's `primaryFieldId`, fields and default view.
- Airtable's API cannot create views, so the table starts with its default grid view.

## Related

- [Build Table JSON](TableJson.md): builds the Table JSON input.
- [Create Field](../Field/CreateField.md): add more columns to the table afterwards.
- [Create Records](../Record/CreateRecords.md): fill it, using `Table ID` as `Table`.
