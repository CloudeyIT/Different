[![Cloudey](https://raw.githubusercontent.com/CloudeyIT/Different/master/docs/logo-dark%400.5x.png#gh-light-mode-only)](https://cloudey.net)

# 🔀 Different  

_Robust, easy-to-use diff and patch library for comparing and synchronising text with zero dependencies_

---
[![GitHub](https://img.shields.io/github/license/CloudeyIT/Different)](https://github.com/CloudeyIT/Different/blob/master/LICENSE)
[![Nuget](https://img.shields.io/nuget/v/Cloudey.Different)](https://www.nuget.org/packages/Cloudey.Different/)
[![Nuget](https://img.shields.io/nuget/dt/Cloudey.Different)](https://www.nuget.org/packages/Cloudey.Different/)


## What is this

**Different** provides extension methods for creating and applying diffs and patches for plain text.   
It uses the same robust algorithms and logic as commonly seen used with git repositories, wrapped in a readable, easy-to-use library.  

## How to use

### 🔀 Diff
***Get the differences between two texts***

```c#
string from = "Hello!";
string to = "Hallo?";

List<Diff> diffs = from.Diff(to);
```
*Result*
```c#
{
    {Operation.EQUAL, "H"},
    {Operation.DELETE, "e"},
    {Operation.INSERT, "a"},
    {Operation.EQUAL, "llo"},
    {Operation.DELETE, "!"},
    {Operation.INSERT, "?"}
}
```

Details: [diff-match-patch#diff_main](https://github.com/google/diff-match-patch/wiki/API#diff_maintext1-text2--diffs)

### ✨ Semantic diff
**Get the differences between two texts, optimised for human-readability**  
Two unrelated texts can contain many coincidental matches. This can make the diff hard to understand for humans, despite being the optimal result. A semantic diff rewrites the diff to be more legible and easier to understand.

```c#
string from = "Computer";
string to = "Mouse";

List<Diff> diffs = from.SemanticDiff(to);
```
*Result*
```c#
{
    {Operation.DELETE, "Computer"},
    {Operation.INSERT, "Mouse"},
}
```
Details: [diff-match-patch#diff_cleanupSemantic](https://github.com/google/diff-match-patch/wiki/API#diff_cleanupsemanticdiffs--null)

### ⏱ Efficient diff

**Get the differences between two texts, optimised for machine processing**  
Applying a large number of small diffs can take drastically longer to process in some scenarios than a small number of larger diffs. An efficient diff rewrites the diff to eliminate and combine tiny diffs into larger ones based on an edit cost heuristic. The result is often similar to a semantic diff, but not always.

```c#
string from = "Computer";
string to = "Mouse";

List<Diff> diffs = from.EfficientDiff(to);
```
*Result*
```c#
{
    {Operation.DELETE, "Computer"},
    {Operation.INSERT, "Mouse"},
}
```
Details: [diff-match-patch#diff_cleanupEfficiency](https://github.com/google/diff-match-patch/wiki/API#diff_cleanupefficiencydiffs--null)

### 📐 Levenshtein distance
**Measure the Levenshtein distance of a diff in terms of changed characters**  
The minimum value is 0, meaning the strings are equal. The maximum value is the length of the longer string.

```c#
string from = "Hello!";
string to = "Hallo?";

List<Diff> diffs = from.Diff(to);

diffs.Levenshtein();
```
*Result*
```c#
2 // e -> a, ! -> ?
```
Details: [diff-match-patch#diff_levenshtein](https://github.com/google/diff-match-patch/wiki/API#diff_levenshteindiffs--int)

### 🔡 Visualise diffs as HTML
**Get an HTML representation of a diff**  
This can be used to visualise diffs in a human-readable way.

```c#
string from = "Hello!";
string to = "Hallo?";

List<Diff> diffs = from.Diff(to);

diffs.Html();
```
*Result*
```html
<span>H</span>
<del style="background:#ffe6e6;">e</del> <!-- Red -->
<ins style="background:#e6ffe6;">a</ins> <!-- Green -->
<span>llo</span>
<del style="background:#ffe6e6;">!</del> <!-- Red -->
<ins style="background:#e6ffe6;">?</ins> <!-- Green -->
```
Details: [diff-match-patch#diff_prettyHtml](https://github.com/google/diff-match-patch/wiki/API#diff_prettyhtmldiffs--html)

### 🩹 Patch
**Get a list of patches given two texts, a text and a diff, or only a diff**  
Patches are a list of operations to transform one text to another. They provide a way to "apply" diffs.

```c#
string from = "Hello!\nWho's there?";
string to = "Hallo?\nWho's there?";

// Given two strings
List<Patch> patches = from.Patch(to);

// Given a string and diffs
List<Diff> diffs = from.Diff(to);
List<Patch> patches = from.Patch(diffs);

// Given diffs
List<Diff> diffs = from.Diff(to);
List<Patch> patches = diffs.Patch();
```
*Result* (serialised to JSON)
```json
[
  {
    "diffs": [
      {
        "operation": 2,
        "text": "H"
      },
      {
        "operation": 0,
        "text": "ello!"
      },
      {
        "operation": 1,
        "text": "allo?"
      },
      {
        "operation": 2,
        "text": "\nWho"
      }
    ],
    "start1": 0,
    "start2": 0,
    "length1": 10,
    "length2": 10
  }
]
```
Details: [diff-match-patch#patch_make](https://github.com/google/diff-match-patch/wiki/API#patch_maketext1-text2--patches)

### 🧩 Apply patch
**Apply a list of patches to a string**  
If the patches cannot be applied, returns `null`

```c#
string from = "Hello!\nWho's there?";
string to = "Hallo?\nWho's there?";

var patch = from.Patch(to);

// Using extension method on string
string? result = from.ApplyPatches(patch);

// Using extension method on List<Patch>
string? result = patch.Apply(from);
```
*Result*
```c#
"Hallo?\nWho's there?"
```
Details: [diff-match-patch#patch_apply](https://github.com/google/diff-match-patch/wiki/API#patch_applypatches-text1--text2-results)

### 📃 Patch to text
**Get the text representation of a list of patch operations**  
The format is very similar to the standard GNU diff/patch format. This text can be stored and later converted back to patch objects (see "Text to patch" below).

```c#
string from = "Hello!\nWho's there?";
string to = "Hallo?\nWho's there?";

List<Patch> patches = from.Patch(to);

patches.Text();
```
*Result*
```
@@ -1,10 +1,10 @@
 H
-ello!
+allo?
 %0aWho
```
Details: [diff-match-patch#patch_toText](https://github.com/google/diff-match-patch/wiki/API#patch_totextpatches--text)

### 📜 Text to patch
**Transform a text representation of patches to a list of patch operations**  
Parses and returns a list of patch objects from their textual representation (see "Patch to text" above).

```c#
var patchString = @"@@ -1,10 +1,10 @@
 H
-ello!
+allo?
 %0aWho
";

// Throws ArgumentException on failure
List<Patch> patches = patchString.ParsePatches();

// Safe, returns true or false depending on success
bool success = patchString.TryParsePatches(out List<Patch>? patches);
```
*Result* (serialised to JSON)
```json
[
  {
    "diffs": [
      {
        "operation": 2,
        "text": "H"
      },
      {
        "operation": 0,
        "text": "ello!"
      },
      {
        "operation": 1,
        "text": "allo?"
      },
      {
        "operation": 2,
        "text": "\nWho"
      }
    ],
    "start1": 0,
    "start2": 0,
    "length1": 10,
    "length2": 10
  }
]
```
Details: [diff-match-patch#patch_fromText](https://github.com/google/diff-match-patch/wiki/API#patch_fromtexttext--patches)

## License

Licensed under Apache 2.0.  
**Copyright © 2021 Cloudey IT Ltd**  
Cloudey® is a registered trademark of Cloudey IT Ltd. Use of the trademark is NOT GRANTED under the license of this repository or software package.

#### For diff-match-patch library contained within repository and distributed code:

Copyright 2018 The diff-match-patch Authors.  
https://github.com/google/diff-match-patch  

**Modifications were made to the diff-match-patch library (licensed under Apache 2.0) by the authors of this package (Cloudey)**
