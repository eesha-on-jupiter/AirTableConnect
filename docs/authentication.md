# Authentication

AirTableConnect authenticates with an Airtable **personal access token** (PAT), sent as an `Authorization: Bearer <token>` header on every request. A PAT carries two things you choose when you create it: the **scopes** (what kinds of calls it may make) and the **access** list (which bases it may touch). Airtable's legacy API keys were retired in February 2024 and are not supported.

## 1. Create a personal access token

1. Go to [airtable.com/create/tokens](https://airtable.com/create/tokens) and click **Create token**.
2. Give it a name (e.g. `Grasshopper`).
3. Add the scopes from step 2 and the bases from step 3.
4. Click **Create token** and copy it straight away; Airtable only shows it once. It starts with `pat`.

## 2. Scopes this plugin needs

| Scope | Used by |
|---|---|
| `schema.bases:read` | [List Bases](components/Base/GetBases.md), [Get Base Schema](components/Base/GetBaseSchema.md) |
| `schema.bases:write` | [Create Table Layout](components/Table/CreateTable.md), [Create Field](components/Field/CreateField.md), [Update Field](components/Field/UpdateField.md) |
| `data.records:read` | [List Records](components/Record/ListRecords.md) |
| `data.records:write` | [Create Records](components/Record/CreateRecords.md), [Update or Sync Records](components/Record/UpdateRecords.md), [Upload Picture](components/Attachment/UploadAttachment.md) |

- [Connect to Airtable](components/Auth/AirTableConnectAuth.md) needs no particular scope; add `user.email:read` if you also want the user's email in its `Response`.
- A read-only canvas only needs the two `:read` scopes.

## 3. Give the token access to bases

- Under **Access**, add each base (or a whole workspace) the plugin should work with.
- A token with no bases is still valid: Connect to Airtable returns `OK`, but List Bases comes back empty and every base call fails with `HTTP 403` or `HTTP 404`.
- Bases added to the token later are picked up without creating a new token.

## 4. Wire the token in

- Put the token in one text Panel and wire it into the `Token` input of every HTTP component.
- Leading and trailing whitespace is trimmed; an empty token returns `Token is empty.` in `Status` without calling Airtable.
- Treat a canvas holding a token like a password file: clear the Panel before sharing the `.gh`.

## 5. Verify with Connect to Airtable

- Wire the token into **Auth > Connect to Airtable**.
- `Status` reads `OK` and `Response` holds `{"id":"usr..."}` when the token is valid.
- Otherwise `Status` holds the HTTP code and Airtable's error body, e.g. `HTTP 401 Unauthorized`.
- Then run **Base > List Bases**: if the base you expect is missing, revisit step 3.

## Rate limits

- Airtable allows 5 requests per second per base and returns `HTTP 429` (with a 30 second lockout) above that.
- List Records, Create Records and Update or Sync Records wait 350 ms between pages or 10-record chunks to stay under the limit.

## API version

- Every call uses Airtable's `v0` paths on `https://api.airtable.com`; Upload Picture uses `https://content.airtable.com`. The version is fixed in the plugin and not configurable from the canvas.
