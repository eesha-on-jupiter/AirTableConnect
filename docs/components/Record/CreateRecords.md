# Create Records

Creates records from a Record JSON list, auto-chunking past 10 rows per call.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | "" | Airtable personal access token. |
| Base ID | Text | Yes | "" | Base containing the table. |
| Table | Text | Yes | "" | Table name or ID. |
| Record JSON | Text (list) | Yes | - | Record JSON list to create. |
| Typecast | Boolean | No | False | Let Airtable loosely convert mismatched value types. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Record IDs | Text (list) | IDs of the created records, in input order. |
| Status | Text | `OK` if the call succeeded, otherwise the error. |
| Response | Text | Combined JSON of every chunk, `{"records":[...]}`. |

## API endpoint

`POST /v0/{baseId}/{tableIdOrName}`

## Notes

- Button-triggered: only fires when **Create Records** is clicked; outputs clear on the next recompute.
- Each click creates the full set again; use [Update or Sync Records](UpdateRecords.md) with Upsert to rerun without duplicates.
- Sends 10 records per request (Airtable's limit), waiting 350 ms between requests.
- If a chunk fails, sending stops and `Status` shows the error; chunks already sent stay created.
- With Typecast on, Airtable converts text to numbers, dates and checkboxes, and adds missing select choices.
- Needs the `data.records:write` scope.

## Related

- [Build Records From Table](BuildRecordsFromGrid.md): builds the Record JSON list.
- [Upload Picture](../Attachment/UploadAttachment.md): attach a file to one of the new `Record IDs`.
