# Build Table JSON

Assembles a table name, description and Field JSON list into Table JSON.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Table Name | Text | Yes | "" | Name of the table. |
| Description | Text | No | "" | Optional table description. |
| Field JSON | Text (list) | Yes | - | Field JSON list; the first field becomes the primary field. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Table JSON | Text | Assembled table JSON. |

## Output

Builds `{"name":"<Table Name>","fields":[<Field JSON>, ...],"description":"<Description>"}`. No network call; safe to recompute.

## Notes

- `description` is left out when Description is empty.
- Empty Field JSON items are skipped; each remaining item must be valid JSON or the component errors.
- Airtable requires at least one field, and the first one becomes the primary field, so keep a text-like field first.

## Related

- [Build Field JSON](../Field/FieldJson.md): produces each Field JSON item.
- [Create Table Layout](CreateTable.md): sends the Table JSON.
