# WinAppStock

This is a very simple library allowing to store your content in AppData user directory on Windows. 

It uses C# and .NET of course.

## Why is it useful?

Well, maybe it's not. I just needed to have an interface to store application data files and hide manual checks for file presence. 

## Stocks

The library makes it easy to access your own directory in AppData (identified as *stock*) and files inside it. No need to check if the directory/file exists or not. If it doesn't - it is created on the fly just when you try to reference the *stock* with a given name.

## Files

*Stocks* may contain files. When you reference a file that doesn't exist -- it is created on the fly. Becaue you may want to initialize the file with specific structure you are encouraged to pass the initializer function to the `GetChildFile()` method.

## Deriving

I believe you may need to write your own custom methods to let them make something more than returning the raw `FileStream`. If so - deriving the library main class is easy.

## Development

Since I haven't used the library anywhere yet it is a subject of possible changes to make it more useful in use.

## Credits

Copyright: Piotr Trojanowski 2015

Source code is published under GNU LGPL license.
