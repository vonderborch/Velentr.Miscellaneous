# Velentr.Miscellaneous.Threading
Miscellaneous helpers for threading

# Current Systems/Helpers
Class | Description | Min Supported Version
----- | ----------- | ---------------------
Guard | A thread-safe boolean check. | 1.1.5

# Guard Usage
Creating a new instance: `var guard = new Guard();`

Property Name | Description | Min Supported Version | Example Usage
----------------- | ----------- | --------------------- | -------------
Check | Returns the current state (**TRUE** if True, **FALSE** if False) without setting the Guard to True if it is currently False. | 1.1.5 | `if (guard.Check) { ... }`
CheckSet | Returns the current state and sets the state to **TRUE** if it is currently set to False. | 1.1.5 | `if (guard.CheckSet) { ... }`

Method Name | Description | Min Supported Version | Example Usage
----------------- | ----------- | --------------------- | -------------
MarkChecked | Sets the Guard to **TRUE** status. | 1.1.5 | `guard.MarkChecked();`
Reset | Sets the Guard to **FALSE** status. | 1.1.5 | `guard.Reset();`
