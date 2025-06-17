# SemVer Action

This is a GitHub Action to allow you to use a JSON file to manage your project's Semantic Version Number.

## Usage

To use this action, you need to create a JSON file in your repository with the following structure:

```json
{
    "major": 0,
    "minor": 1,
    "patch": 0,
    "build": 0
}
```
### Reading the version number

Then you can use this action in your workflow to read this file.:

```yaml
    - name: Read Version
      uses: Afterlife-Guide/SemVer.Action@0.1.0
      with:
        path: 'version.json'
```

It will create the following outputs:

- `major`: The major version number.
- `minor`: The minor version number.
- `patch`: The patch version number.
- `build`: The build version number.
- `version`: The full version number.

### Writing the version number

You can also use this action to write the version number back to the file.:

```yaml
    - name: Write Version
      uses: Afterlife-Guide/SemVer.Action@0.1.0
      with:
        path: 'version.json'
        major: 0
        minor: 1
        patch: 1
        build: 0
```

## License

[Apache 2.0](LICENSE.txt)

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull
requests to us.