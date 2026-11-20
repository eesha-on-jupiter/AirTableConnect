# Connect to Airtable

Verifies a personal access token against Airtable and returns the connected user's ID.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | "" | Airtable personal access token. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Status | Text | `OK` if the token is valid, otherwise the error. |
| Response | Text | Raw JSON response from Airtable, e.g. `{"id":"usr..."}`. Empty on failure. |

## API endpoint

`GET /v0/meta/whoami`

## Notes

- Runs on every solve; no button.
- An empty token returns `Token is empty.` without calling Airtable.
- On an HTTP error, `Status` is `HTTP <code> <reason>` followed by Airtable's error body.
- A valid token with no bases granted still returns `OK`; check base access with [List Bases](../Base/GetBases.md).
- `Response` also includes the user's `email` when the token has the `user.email:read` scope.

## Related

- [Authentication](../../authentication.md): creating the token and choosing scopes.
- [List Bases](../Base/GetBases.md): the natural next step once `Status` is `OK`.
