# Record

Read, create, update and upsert table rows, and convert between Grasshopper trees and Record JSON.

| Component | Description |
|---|---|
| [List Records](ListRecords.md) | Lists records from a table, auto-paginating, with optional formula and view filters. |
| [Build Records From Table](BuildRecordsFromGrid.md) | Assembles field names and a values tree (one branch per record) into a Record JSON list. No network call. |
| [Get Field From Records](GetRecordField.md) | Extracts one field's value from each record in a Record JSON list. No network call. |
| [Create Records](CreateRecords.md) | Creates records from a Record JSON list, chunked 10 per call. Button-triggered. |
| [Update or Sync Records](UpdateRecords.md) | Updates records by ID, or upserts them by key fields, chunked 10 per call. Button-triggered. |
