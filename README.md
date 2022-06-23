# sticky

Windows sticky notes.

## ⌨ Keyboard shortcuts

    Toggle bold:          CTRL+B
    Toggle italic:        CTRL+I
    Toggle underline:     CTRL+U
    Toggle strikethrough: CTRL+D
    Decrease font size:   CTRL+[
    Increase font size:   CTRL+]
    Reset formatting:     CTRL+SPACE

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
