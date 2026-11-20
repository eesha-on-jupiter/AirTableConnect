# Build Select Choices

Assembles choice labels and colors into Options JSON for a single or multiple select field.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Choice Labels | Text (list) | Yes | - | Select choice labels. |
| Colors | Text (list) | No | - | Airtable color name per choice, positional. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Options JSON | Text | Assembled select-field options JSON. |

## Output

Builds `{"choices":[{"name":"<label>","color":"<color>"}, ...]}`. No network call; safe to recompute.

## Notes

- Colors pair with labels by index; a label with no color (list too short, or an empty item) gets no `color` key and Airtable picks one.
- Colors must be one of Airtable's 40 fixed names (e.g. `blueLight2`, `redBright`); use the [Airtable Color](../Presets/AirtableColorPreset.md) preset.
- Choice labels should be unique within the field.

## Related

- [Airtable Color](../Presets/AirtableColorPreset.md): values for `Colors`.
- [Build Field JSON](FieldJson.md): wire `Options JSON` in, with `singleSelect` or `multipleSelects` as the type.
