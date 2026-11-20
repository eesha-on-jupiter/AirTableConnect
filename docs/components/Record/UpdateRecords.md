# Update or Sync Records

Updates or upserts records from a Record JSON list, auto-chunking past 10 rows per call.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | "" | Airtable personal access token. |
| Base ID | Text | Yes | "" | Base containing the table. |
| Table | Text | Yes | "" | Table name or ID. |
| Record JSON | Text (list) | Yes | - | Record JSON list to update. |
| Upsert | Boolean | No | False | Insert unmatched rows instead of erroring. |
| Fields To Merge On | Text (list) | When Upsert is True | - | Field names used to match existing records. |
| Typecast | Boolean | No | False | Let Airtable loosely convert mismatched value types. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Record IDs | Text (list) | IDs of the updated (and, if upserting, created) records. |
| Status | Text | `OK` if the call succeeded, otherwise the error. |
| Response | Text | Combined JSON of every chunk, `{"records":[...],"createdRecords":[...],"updatedRecords":[...]}`. |

## API endpoint

`PATCH /v0/{baseId}/{tableIdOrName}`

## Notes

- Button-triggered: only fires when **Update Records** is clicked; outputs clear on the next recompute.
- PATCH semantics: only the fields in each record change; other fields are left as they are.
- Without Upsert, every record needs an `id` (wire `Record IDs` into [Build Records From Table](BuildRecordsFromGrid.md)).
- With Upsert, records are matched on the Fields To Merge On values; matches are updated, the rest are created. An empty Fields To Merge On returns an error without calling Airtable.
- `createdRecords` / `updatedRecords` in Response list which IDs were inserted and which were updated.
- Sends 10 records per request, waiting 350 ms between requests; a failing chunk stops the run, earlier chunks stay written.
- Needs the `data.records:write` scope.

## Related

- [Build Records From Table](BuildRecordsFromGrid.md): builds the Record JSON list.
- [List Records](ListRecords.md): source of the record IDs to write back to.
- [Create Records](CreateRecords.md): plain inserts.
