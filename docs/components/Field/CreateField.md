# Create Field

Creates a field on an existing table from Field JSON.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | "" | Airtable personal access token. |
| Base ID | Text | Yes | "" | Base containing the table. |
| Table ID | Text | Yes | "" | Table to add the field to (`tbl...`). |
| Field JSON | Text | Yes | "" | Assembled field JSON. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Field ID | Text | ID of the created field (`fld...`). |
| Status | Text | `OK` if the call succeeded, otherwise the error. |
| Response | Text | Raw JSON response from Airtable. |

## API endpoint

`POST /v0/meta/bases/{baseId}/tables/{tableId}/fields`

## Notes

- Button-triggered: only fires when **Create Field** is clicked; outputs clear on the next recompute.
- Each click creates another field; Airtable rejects a name already used in the table.
- Needs the `schema.bases:write` scope.
- `Table ID` goes into the URL path as-is: use the `tbl...` ID from [Get Base Schema](../Base/GetBaseSchema.md) or [Create Table Layout](../Table/CreateTable.md).

## Related

- [Build Field JSON](FieldJson.md): builds the Field JSON input.
- [Update Field](UpdateField.md): change the field later, using `Field ID`.
