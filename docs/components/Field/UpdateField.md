# Update Field

Updates an existing field's name, description or options from Field JSON.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | "" | Airtable personal access token. |
| Base ID | Text | Yes | "" | Base containing the table. |
| Table ID | Text | Yes | "" | Table containing the field (`tbl...`). |
| Field ID | Text | Yes | "" | Field to update (`fld...`). |
| Field JSON | Text | Yes | "" | Assembled field JSON. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Status | Text | `OK` if the call succeeded, otherwise the error. |
| Response | Text | Raw JSON response from Airtable. |

## API endpoint

`PATCH /v0/meta/bases/{baseId}/tables/{tableId}/fields/{fieldId}`

## Notes

- Button-triggered: only fires when **Update Field** is clicked; outputs clear on the next recompute.
- Needs the `schema.bases:write` scope.
- Only `name`, `description` and `options` can change; a field's type is fixed once created.
- [Build Field JSON](FieldJson.md) always writes a `type` key: keep it equal to the field's current type. If Airtable refuses it, send a hand-written body from a Panel instead, e.g. `{"name":"New name"}`.
- Updating select choices replaces the list: include the existing choices along with the new ones.
- Field IDs are in [Get Base Schema](../Base/GetBaseSchema.md)'s `Response`, or the `Field ID` output of [Create Field](CreateField.md).

## Related

- [Build Field JSON](FieldJson.md): builds the Field JSON input.
- [Create Field](CreateField.md): source of a `Field ID`.
