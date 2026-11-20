# Get Field From Records

Extracts one field's value from each record in a Record JSON list.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Record JSON | Text (list) | Yes | - | Record JSON list. |
| Field Name | Text | Yes | "" | Field to extract. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Field Value | Text (list) | Extracted field value, positional per record. |

## Output

Reads `fields.<Field Name>` from each record and returns it as text, one item per input record. No network call; safe to recompute.

## Notes

- Output stays aligned with the input: a record without the field (Airtable omits empty fields) or an empty item gives `""`.
- Text values come out as plain strings; numbers as their text and checkboxes as `True` / `False`; arrays and objects (attachments, multiple selects, linked records) as compact JSON text.
- Field Name must match the field's name exactly, including case.
- Each Record JSON item must be valid JSON or the component errors.

## Related

- [List Records](ListRecords.md): source of `Record JSON`; its `Record IDs` line up with `Field Value`.
