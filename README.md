# Simple Password Generator
*This program was made with the intention of being an internal tool for Gerber Childrenswear, LLC and their I.T. team.*

### Usage
Launching the program by itself will give you a 16 character randomized string. Launching it from a terminal [Windows Terminal, Powershell, Command Prompt, Linux Terminal via WINE] allows you to specify how long you want that string to be, using the `--length` parameter.<br>
Shorter parameters:<br>
<code>--length</code> -> <code>-l</code><br>
<code>--useWords</code> -> <code>-w</code><br>

### Sytax
```ps
.\PasswordGen.exe --length <number> --useWords <true|false>
```

```batch
start PasswordGen.exe --length <number> --useWords <true|false>
```