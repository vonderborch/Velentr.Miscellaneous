# Velentr.Miscellaneous.StringHelpers
Miscellaneous helpers for dealing with strings

# Current Systems/Helpers
Class | Description | Min Supported Version
----- | ----------- | ---------------------
StringSimilarity | Methods related to comparing the similarity of two strings. | 1.0.0
StringSplitters | Methods related to splitting a string. | 1.0.3
TableOutputHelpers | Methods related to outputting a string as a ASCII table. | 1.1.0

# StringSimilarity Methods
Method Name | Description | Min Supported Version | Example Usage
----------- | ----------- | --------------------- | -------------
ComputeLevenshteinDistance | Computes the similarity between two strings. Lower numbers = more similar. Low performance with big strings. | 1.0.0 | `var similarity = StringSimilarity.GetDamerauLevenshteinDistance("string 1", "string 2");`
ComputeLevenshteinDistance | Computes the similarity between two strings. Lower numbers = more similar. Better performance with big strings. | 1.0.0 | `var similarity = StringSimilarity.GetDamerauLevenshteinDistance("string 1", "string 2");`

# StringSplitters Methods
Method Name | Description | Min Supported Version | Example Usage
----------- | ----------- | --------------------- | -------------
SplitStringByNewLines | Splits a string into parts based on new lines. | 1.0.3 | `var stringParts = StringSplitters.SplitStringByNewLines("AAABBBCCCDD");`
SplitStringByChunkSize | Splits a string into parts based on the size of a chunk. | 1.0.3 | `var stringParts = StringSplitters.SplitStringByChunkSize("AAABBBCCCDD", 3);`

# TableOutputHelpers Methods
Method Name | Description | Min Supported Version | Example Usage
----------- | ----------- | --------------------- | -------------
ConvertToTable | Converts a list of list of either objects or strings into a formatted ASCII table. | 1.1.0 | `var table = TableOutputHelpers.ConvertToTable(new List<string>() {"col1", "col2"}, new List<List<string>() {{"row1col1", "row1col2"}, {"row2col1", "row2col2"}});`
