# Velentr.Miscellaneous.Sqlite
Miscellaneous helpers for dealing with Sqlite Databases

# Usage Note
The Files/helpers in this directory require the following nuget to be installed to function: https://www.nuget.org/packages/Microsoft.Data.Sqlite/6.0.5

# Current Systems/Helpers
File | Type | Description | Min Supported Version | Example Usage
---- | ---- | ----------- | --------------------- | -------------
IModelParser | interface | An interface for a model parser. This is used when using the generic ExecuteQuery<T> method, where T must implement this interface to parse properly. | 1.1.0 | N/A (interface)
Database | class | Various methods, etc. for running queries against a SQLite database. | 1.1.0 | `var db = new Database(inMemory: true);`
