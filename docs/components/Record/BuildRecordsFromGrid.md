# Build Records From Table

Assembles field names and a per-record values tree into a Record JSON list.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Field Names | Text (list) | Yes | - | Field names, in the order values are supplied per record. |
| Values | Text (tree) | Yes | - | Per-record field values, one branch per record. |
| Record IDs | Text (list) | No | - | Existing record IDs, positional; omit for new records. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Record JSON | Text (list) | Assembled record JSON, one entry per record. |

## Output

Builds one `{"fields":{"<Field Name>":"<value>", ...},"id":"<Record ID>"}` per branch of Values. No network call; safe to recompute.

## Notes

- One record per branch: a single branch gives one record, a full tree gives a table.
- Values pair with Field Names by index within each branch; if the counts differ, the shorter length is used and a warning names the record.
- Empty values are skipped, so the field is left untouched on update.
- Every value is sent as text: set `Typecast` to `True` on [Create Records](CreateRecords.md) / [Update or Sync Records](UpdateRecords.md) so Airtable converts it for number, checkbox, date and select fields.
- A value that starts with `[` or `{` is embedded as JSON, which is how [Attachments JSON](../Attachment/AttachmentFromUrl.md) and multiple-select arrays (e.g. `["A","B"]`) go in.
- Record IDs pair with branches by index; `id` is added only where an ID is present. Records without one are new rows for Create Records, or upsert candidates for Update or Sync Records.

## Related

- [Create Records](CreateRecords.md), [Update or Sync Records](UpdateRecords.md): send the Record JSON list.
- [List Records](ListRecords.md): source of `Record IDs` for write-back.
- [Attachment From URL](../Attachment/AttachmentFromUrl.md): an attachment value.
