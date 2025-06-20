# Changelog.Action

This is a GitHub Action update the versions and links in you changelog file.
As outlined in the [Keep a Changelog](https://keepachangelog.com/en/1.0.0/) standard.

## Usage

Create a `CHANGELOG.md` file in the root of your repository, and add the following content:

```markdown
# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
```

As you add features, bug fixes, or other changes to your project, you can use this action to update the changelog.

### Updating the changelog on release

Then you can use this action in your workflow to read this file:

```yaml
    - name: Update Changelog
      id: changelog
      uses: baynezy/ChangeLogger.Action@0.1.0.0
      with:
        tag: '1.0.0.0'
```

### Using the release notes output

The action provides a `release-notes` output that contains the content from the `[Unreleased]` section of the changelog. This can be useful for creating GitHub releases or other automation:

```yaml
    - name: Update Changelog
      id: changelog
      uses: baynezy/ChangeLogger.Action@0.1.0.0
      with:
        tag: '1.0.0.0'
    
    - name: Create GitHub Release
      uses: actions/create-release@v1
      with:
        tag_name: '1.0.0.0'
        release_name: 'Release 1.0.0.0'
        body: ${{ steps.changelog.outputs.release-notes }}
        draft: false
        prerelease: false
      env:
        GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
```

The `release-notes` output will contain all the content from the `[Unreleased]` section before it's transformed, preserving the original markdown formatting.

## License

[Apache 2.0](LICENSE.txt)

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull
requests to us.