# Congratulations for your purchase!

This file is an offline documentation for you to reach out to, if you feel like you making any changes or adapt the code in another script.

If are not seeing this document formatted go to the website
https://stackedit.io/app
`Copy & Paste` the content of this file there.

# Behavior

The goal of the `Breakable` Script is to swap a whole object with a fractured one.

## Serialized Fields

There are the following Serialized Fields in this script:

|SerializedFields | Description                          | Data Type   |
|-----------------|--------------------------------------|-------------|
|`wholeObject`    | The object you want to break         | `Transform` |
|`fracturedObject`| The fractured object that will replace the current `wholeObject` | `Transform` |
| `isCyclic`      | The flag that will control whether the `wholeObject` will be respawned in the same place after a certain amount of time            | `bool` |


## Methods

### TriggerBreak()
#### Description
This is the main method where the breaking coroutines will be started.

### DestroyOnce()
#### Description
This method will set the variables to start the destruction process only once.

### CycleDestruction()
#### Description
This method will set the variables to start the cyclic destruction process.

### BreakObject()
#### Description
This is the method where the `wholeObject` will be hidden and the `fracturedObject` will be instantiated.
While also starting the CleanUp Coroutine.

### CleanUpCoroutine()
#### Description
This method will start a timer of `timeToCleanUp` seconds which will call the `CleanUp()` method.

### CleanUp()
#### Description
This method will Destroy the `fracturedObjectInstance`.

### ResetObject()
#### Description
This method will be used in cyclic destruction in order to show the `wholeObject` and reset the cyclic variables to restart the breaking process.

## Need Any Help Or Want to Give Suggestions?
If you have any problems or want to give suggestions for updates on this package
or future packages please send us an e-mail at nexus.arcade.studios@gmail.com

For `Suggestion of pack update` please write in the **subject** something along the lines of:

### Subject
    Suggestion of pack update `{Name of the pack you want to give suggestions to}`

or if you want to give suggestions for future packs

    Suggestion for Future Packs


For `bug fixes or problems` please write in the **subject** something along the lines of:

### Subject
    Bug found at `{Name of the pack you want to report a bug}`

or if you want help with anything related to the pack

    Help Needed `{Name of the pack you need help with}`

All of this so we can better address what's and issue and what's not, so you guys can have
a **better and faster** support.
 
## Thank you guys for supporting my work!

### Good Luck with your Gamedev Journey!

#### Nexus Arcade Studios

