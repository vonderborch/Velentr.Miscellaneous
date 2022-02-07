# Velentr.Miscellaneous.Sqlite
Miscellaneous helpers for dealing with Sqlite Databases

# Current Systems/Helpers
File | Type | Description | Min Supported Version | Example Usage
---- | ---- | ----------- | --------------------- | -------------
IModelParser | interface | An interface for a model parser. This is used when using the generic ExecuteQuery<T> method, where T must implement this interface to parse properly. | 1.1.0 | N/A (interface)
Database | class | Various methods, etc. for running queries against a SQLite database. | 1.1.0 | `var db = new Database(inMemory: true);`
