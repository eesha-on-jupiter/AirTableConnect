# Changelog

## 1.0.0

Initial release.

- **Auth tab:** token check against Airtable's `/v0/meta/whoami`.
- **Base tab:** list the bases a token can access, and the tables (names and IDs) in a base.
- **Table tab:** assemble Table JSON from a name, description and Field JSON list, and create the table with a button click.
- **Field tab:** assemble Field JSON and select-field choices with colors, create a field on a table or update an existing one.
- **Record tab:** list records with auto-pagination and formula/view filters; build Record JSON from a field-name list and a values tree; create, update or upsert any number of records with automatic 10-per-call chunking; extract one field from a Record JSON list.
- **Attachment tab:** embeddable Attachments JSON from a URL, base64 Attachment Upload JSON from a file or bitmap, and upload to an existing record's attachment field.
- **Presets tab:** value lists for 14 writable field types and Airtable's 40 select-choice colors.

### Known limitations

- Deleting records, destructive full replace (`PUT`), single-record fetch, record comments and webhooks are not included.
- Field types are curated to 14 writable types: formula, rollup, lookup and linked-record fields cannot be created from the plugin.
- List Bases reads only the first page of Airtable's base list.
- A Create Records or Update or Sync Records batch that fails partway stops at the failing chunk; chunks already sent stay written.
- Airtable's API has no endpoint for creating views, so a new table gets Airtable's default grid view only.
