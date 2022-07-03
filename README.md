# sticky

Windows sticky notes.

## ⌨ Keyboard shortcuts

These keyboard shortcuts are essentially the same as are in the Microsoft Sticky Notes application.

A _plus (+)_ in a shortcut means to press multiple keys at the same time. A _comma sign (,)_ in a shortuct means to press multiple keys in order.

### Typing and editing shortcuts

    Create a new sticky note.                                                 CTRL+N
    Close the current sticky note.                                            CTRL+W
    Delete the current sticky note.                                           CTRL+D
    Move the focus to the list of all notes.                                  CTRL+H
    Forward cycle between open notes and the note list.                       CTRL+TAB
    Backward cycle between open notes and the note list.                      CTRL+SHIFT+TAB
    Select all text on the sticky note.                                       CTRL+A
    Copy the selected text to the clipboard.                                  CTRL+C
    Cut the selected text to the clipboard.                                   CTRL+X
    Paste the text of the clipboard.                                          CTRL+V
    Undo the last action.                                                     CTRL+Z
    Redo the last action.                                                     CTRL+Y
    Move one word to the left.                                                CTRL+LEFT ARROW
    Move one word to the right.                                               CTRL+RIGHT ARROW
    Move to the beginning of the line.                                        HOME
    Move to the end of the line.                                              END
    Move to the beginning of the sticky note.                                 CTRL+HOME
    Move to the end of the sticky note.                                       CTRL+END
    Delete the next word.                                                     CTRL+DELETE
    Delete the previous word.                                                 CTRL+BACKSPACE
    Search in any sticky note when in the note list.                          CTRL+F
    Close sticky notes.                                                       ALT+F4

### Formatting shortcuts

    Apply or remove bold formatting from the selected text.                   CTRL+B
    Apply or remove italic formatting from the selected text.                 CTRL+I
    Apply or remove underline from the selected text.                         CTRL+U
    Apply or remove bulleted list formatting from the selected paragraph.     CTRL+SHIFT+L
    Apply or remove strikethrough from the selected text.                     CTRL+T
    Decrease front size of the selected text.                                 CTRL+[
    Increase font size of the selected text.                                  CTRL+]
    Remove all formatting from the selected text.                             CTRL+SPACE

## 🗎 Notes

If your build randomly fails while VSCode is open add the following to your VSCode settings:

```json
{
  "files.watcherExclude": {
    "**/build-temp/**": true
  }
}
```

For more information, see [Build issue with WPF](https://github.com/dotnet/wpf/issues/4299).
