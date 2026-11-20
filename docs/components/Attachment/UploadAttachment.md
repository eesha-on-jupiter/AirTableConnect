# Upload Picture

Uploads a base64-encoded attachment to an existing record.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | "" | Airtable personal access token. |
| Base ID | Text | Yes | "" | Base containing the record. |
| Record ID | Text | Yes | "" | Record to attach the file to (`rec...`). |
| Field | Text | Yes | "" | Attachment field name or ID. |
| Attachment Upload JSON | Text | Yes | "" | Assembled attachment upload JSON. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Status | Text | `OK` if the call succeeded, otherwise the error. |
| Response | Text | Raw JSON response from Airtable: the record ID and the field's attachments. |

## API endpoint

`POST https://content.airtable.com/v0/{baseId}/{recordId}/{attachmentFieldIdOrName}/uploadAttachment`

## Notes

- Button-triggered: only fires when **Upload** is clicked; outputs clear on the next recompute.
- Uses Airtable's content host, not `api.airtable.com`.
- The record must already exist; create it first with [Create Records](../Record/CreateRecords.md).
- Each click adds the file to the field's existing attachments.
- Field must be an attachment-type field. Files are limited to 5 MB.
- Needs the `data.records:write` scope.

## Related

- [Attachment From File Or Bitmap](AttachmentFromFileOrBitmap.md): builds the Attachment Upload JSON.
- [Create Records](../Record/CreateRecords.md): source of a `Record ID`.
- [Attachment From URL](AttachmentFromUrl.md): attach a hosted file at create/update time instead.
