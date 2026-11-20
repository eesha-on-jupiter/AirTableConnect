# List Records

Lists records from a table, optionally filtered, auto-paginating past 100 rows.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | "" | Airtable personal access token. |
| Base ID | Text | Yes | "" | Base containing the table. |
| Table | Text | Yes | "" | Table name or ID. |
| Filter Formula | Text | No | "" | Optional Airtable formula filter, e.g. `{Level} = 2`. |
| View | Text | No | "" | Optional view name or ID. |
| Max Records | Integer | No | 0 | Maximum records to return; 0 for no limit. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Record IDs | Text (list) | IDs of the returned records (`rec...`). |
| Record JSON | Text (list) | Full JSON per returned record, `{"id":...,"createdTime":...,"fields":{...}}`. |
| Status | Text | `OK` if the call succeeded, otherwise the error. |
| Response | Text | Combined JSON of every page, `{"records":[...]}`. |

## API endpoint

`GET /v0/{baseId}/{tableIdOrName}`

## Notes

- Runs on every solve; read-only. Needs the `data.records:read` scope.
- Requests pages of 100 and follows Airtable's `offset` cursor until the table runs out or Max Records is reached, waiting 350 ms between pages.
- Filter Formula is sent as `filterByFormula`; any record where the formula is not 0, false or empty is returned.
- With a View, records come back in that view's order and filtering.
- Empty fields are left out of a record's JSON by Airtable.
- A large table means many calls on every recompute; set Max Records or a filter while wiring.

## Related

- [Get Field From Records](GetRecordField.md): pull one column out of `Record JSON`.
- [Build Records From Table](BuildRecordsFromGrid.md): wire `Record IDs` in to write results back.
- [Update or Sync Records](UpdateRecords.md): send those results.
