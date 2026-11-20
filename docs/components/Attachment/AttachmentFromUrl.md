# Attachment From URL

Assembles a URL and filename into embeddable Attachments JSON.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| URL | Text | Yes | "" | Publicly reachable URL of the file. |
| Filename | Text | No | "" | Optional filename. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Attachments JSON | Text | Assembled attachments JSON. |

## Output

Builds `[{"url":"<URL>","filename":"<Filename>"}]`, a one-item array ready to be an attachment field's value. No network call; safe to recompute.

## Notes

- `filename` is left out when empty; Airtable then takes the name from the URL.
- The URL must be reachable by Airtable's servers: Airtable downloads the file and stores its own copy.
- Use it as a value in [Build Records From Table](../Record/BuildRecordsFromGrid.md): values starting with `[` are embedded as JSON, not as text.
- Setting an attachment field this way on an update replaces whatever attachments the field had.
- Not interchangeable with Attachment Upload JSON: this is metadata embedded in a record, [Upload Picture](UploadAttachment.md) takes base64 bytes.

## Related

- [Build Records From Table](../Record/BuildRecordsFromGrid.md): embed the JSON as a field value.
- [Create Records](../Record/CreateRecords.md), [Update or Sync Records](../Record/UpdateRecords.md): send it.
- [Attachment From File Or Bitmap](AttachmentFromFileOrBitmap.md): for local files and bitmaps.
