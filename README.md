# CSLox

CSharp implementation of [The Lox Interpreter](https://craftinginterpreters.com/the-lox-language.html)

## Quick Start

```console
$ ./scripts/run.sh
```

## Ahead of time (AoT) Compilation

```console
$ ./scripts/publish.sh
$ ./cslox/CSLox
```

## Tests

All test files are copied from [CraftingInterpreters Repository](https://github.com/munificent/craftinginterpreters/tree/master?tab=License-1-ov-file) and sperate them to `passing` for tests that should pass & `failing` for tests that should fail

### Run tests

```console
$ ./scripts/test-passing.sh
$ ./scripts/test-failing.sh
```