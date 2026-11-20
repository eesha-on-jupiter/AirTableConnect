# Field Type

A value-list preset of 14 writable Airtable field types.

## Inputs

None.

## Values

| Display Name | Value |
|---|---|
| Single line text | `singleLineText` |
| Long text | `multilineText` |
| Number | `number` |
| Checkbox | `checkbox` |
| Single select | `singleSelect` |
| Multiple select | `multipleSelects` |
| Date | `date` |
| Attachment | `attachment` |
| URL | `url` |
| Email | `email` |
| Phone number | `phoneNumber` |
| Currency | `currency` |
| Percent | `percent` |
| Rating | `rating` |

## Feeds into

- [Build Field JSON](../Field/FieldJson.md) `Field Type`.

## Notes

- A Grasshopper value list: the selected item's value is its output; switch display mode from the right-click menu.
- Formula, rollup, lookup and linked-record types are left out: they need options this plugin does not build.
- Several types need `Options JSON` on Build Field JSON before Airtable accepts them: select types need choices, `number` / `percent` need a precision, `currency` a precision and symbol, `date` needs a date format, `checkbox` / `rating` need an icon and color.

## Related

- [Build Field JSON](../Field/FieldJson.md)
- [Build Select Choices](../Field/SelectFieldOptions.md)
