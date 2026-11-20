# Airtable Color

A value-list preset of Airtable's 40 fixed select-choice colors: 10 hues in four shades each.

## Inputs

None.

## Values

Display name and value are the same string.

| Shade | Values |
|---|---|
| Light 2 | `blueLight2`, `cyanLight2`, `tealLight2`, `greenLight2`, `yellowLight2`, `orangeLight2`, `redLight2`, `pinkLight2`, `purpleLight2`, `grayLight2` |
| Light 1 | `blueLight1`, `cyanLight1`, `tealLight1`, `greenLight1`, `yellowLight1`, `orangeLight1`, `redLight1`, `pinkLight1`, `purpleLight1`, `grayLight1` |
| Bright | `blueBright`, `cyanBright`, `tealBright`, `greenBright`, `yellowBright`, `orangeBright`, `redBright`, `pinkBright`, `purpleBright`, `grayBright` |
| Dark 1 | `blueDark1`, `cyanDark1`, `tealDark1`, `greenDark1`, `yellowDark1`, `orangeDark1`, `redDark1`, `pinkDark1`, `purpleDark1`, `grayDark1` |

## Feeds into

- [Build Select Choices](../Field/SelectFieldOptions.md) `Colors`, one item per choice label.

## Notes

- A Grasshopper value list: the selected item's value is its output; switch display mode from the right-click menu.
- These are Airtable's only accepted choice colors; any other string is rejected by the API.
- For several choices, use one value list per choice and merge them in label order.

## Related

- [Build Select Choices](../Field/SelectFieldOptions.md)
- [Field Type](FieldTypePreset.md)
