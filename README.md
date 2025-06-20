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
      uses: baynezy/ChangeLogger.Action@0.1.0.0
      with:
        tag: '1.0.0.0'
        repo-path: '/'
        log-path: './CHANGELOG.md'
```

## License

[Apache 2.0](LICENSE.txt)

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull
requests to us.