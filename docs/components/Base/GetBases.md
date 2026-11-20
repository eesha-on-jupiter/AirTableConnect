# List Bases

Lists the Airtable bases the connected token can access.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | "" | Airtable personal access token. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Base Names | Text (list) | Accessible base names. |
| Base IDs | Text (list) | Accessible base IDs (`app...`), in the same order. |
| Status | Text | `OK` if the call succeeded, otherwise the error. |
| Response | Text | Raw JSON response from Airtable. |

## API endpoint

`GET /v0/meta/bases`

## Notes

- Runs on every solve; read-only.
- Only bases added to the token's access list appear; an empty list with `Status` `OK` means none were granted.
- Needs the `schema.bases:read` scope.
- Reads one page of results only; the `offset` cursor is not followed.
- `Response` also carries each base's `permissionLevel`.

## Related

- [Get Base Schema](GetBaseSchema.md): wire one of the `Base IDs` in to list its tables.
- [Connect to Airtable](../Auth/AirTableConnectAuth.md): check the token first.
