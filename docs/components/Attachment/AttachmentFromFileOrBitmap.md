# Attachment From File Or Bitmap

Base64-encodes a file path or bitmap into Attachment Upload JSON for an existing record.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| File Path | Text | One of File Path / Bitmap | "" | Path to the file on disk. |
| Bitmap | Generic | One of File Path / Bitmap | - | In-memory bitmap; wins over File Path if both are given. |
| Filename | Text | No | "" | Optional filename. |
| Content Type | Text | No | "" | Optional MIME type. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Attachment Upload JSON | Text | Assembled attachment upload JSON. |

## Output

Builds `{"contentType":"<Content Type>","file":"<base64 bytes>","filename":"<Filename>"}`. No network call; safe to recompute.

## Notes

- Bitmap input expects a `System.Drawing.Bitmap`; it is encoded as PNG, with defaults `image/png` and `image.png`.
- A file is read as-is; defaults are `application/octet-stream` and the file's own name. Set Content Type (e.g. `image/jpeg`) so Airtable previews it as an image.
- With neither input set, the component shows an error; a missing file also errors.
- Airtable's upload endpoint accepts files up to 5 MB.

## Related

- [Upload Picture](UploadAttachment.md): sends the JSON to a record.
- [Attachment From URL](AttachmentFromUrl.md): for files already hosted online.
