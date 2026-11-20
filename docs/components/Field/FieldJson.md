# Build Field JSON

Assembles a field name, type, description and options into Field JSON.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Field Name | Text | Yes | "" | Name of the field. |
| Field Type | Text | No | "singleLineText" | Airtable field type, e.g. from the [Field Type](../Presets/FieldTypePreset.md) preset. |
| Description | Text | No | "" | Optional field description. |
| Options JSON | Text | No | "" | Optional per-type options JSON (e.g. select choices). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Field JSON | Text | Assembled field JSON. |

## Output

Builds `{"name":"<Field Name>","type":"<Field Type>","description":"<Description>","options":<Options JSON>}`. No network call; safe to recompute.

## Notes

- `description` and `options` are left out when empty.
- Options JSON must be a JSON object or the component errors.
- Some types need options to be accepted by Airtable: `singleSelect` / `multipleSelects` need `choices` (from [Build Select Choices](SelectFieldOptions.md)); `number` and `percent` need a `precision`, `currency` a `precision` and `symbol`; `date` needs a `dateFormat`; `rating` needs `max`, `icon` and `color`; `checkbox` needs `icon` and `color`. Write those options JSON by hand in a Panel.
- On [Update Field](UpdateField.md), Airtable only accepts name, description and options changes, not a type change.

## Related

- [Field Type](../Presets/FieldTypePreset.md): values for `Field Type`.
- [Build Select Choices](SelectFieldOptions.md): builds `Options JSON` for select fields.
- [Build Table JSON](../Table/TableJson.md), [Create Field](CreateField.md), [Update Field](UpdateField.md): consume Field JSON.
