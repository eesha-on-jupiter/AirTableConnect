# Attachment

Two separate ways to put a file on a record:

- **From a URL:** Attachments JSON is embedded as a field value when a record is created or updated.
- **From a file or bitmap:** the bytes are base64-encoded and uploaded to an existing record.

| Component | Description |
|---|---|
| [Attachment From URL](AttachmentFromUrl.md) | Builds embeddable Attachments JSON from a public URL. No network call. |
| [Attachment From File Or Bitmap](AttachmentFromFileOrBitmap.md) | Base64-encodes a file or bitmap into Attachment Upload JSON. No network call. |
| [Upload Picture](UploadAttachment.md) | Uploads Attachment Upload JSON to an existing record's attachment field. Button-triggered. |
