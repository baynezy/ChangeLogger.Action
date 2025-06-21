# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.1.1.13] - 2025-06-21

### Fixed

- Optional parameters bug (#47)

## [1.1.0.4] - 2025-06-20

### Added

- Release notes as output in the GitHub Action (#38)
- Environment instructions for Copilot (#39)
- Task in Cake to build and restore the project, and updating the copilot instructions to use it. (#42)

### Changed

- Made log-path and repo-path parameters optional with default values (#36)
  - log-path defaults to './CHANGELOG.md'
  - repo-path defaults to '/'
  - Renamed Option longname from 'path' to 'repo-path' for consistency

## [1.0.0.3] - 2025-06-20

### Changed

- Update README instructions to be accurate with the latest version of the action (#28)
- Dogfood the action in the repository itself (#29)

## [0.0.6.10] - 2025-06-20

## [0.0.5.9] - 2025-06-20

## [0.0.4.8] - 2025-06-20

## [0.0.3.7] - 2025-06-20

## [0.0.2.3] - 2025-06-20

### Added

- Initial project skeleton (#1)
- Initial Action Code

[unreleased]: https://github.com/baynezy/ChangeLogger.Action/compare/1.1.1.13...HEAD
[1.1.1.13]: https://github.com/baynezy/ChangeLogger.Action/compare/1.1.0.4...1.1.1.13
[1.1.0.4]: https://github.com/baynezy/ChangeLogger.Action/compare/1.0.0.3...1.1.0.4
[1.0.0.3]: https://github.com/baynezy/ChangeLogger.Action/compare/0.0.6.10...1.0.0.3
[0.0.6.10]: https://github.com/baynezy/ChangeLogger.Action/compare/0.0.5.9...0.0.6.10
[0.0.5.9]: https://github.com/baynezy/ChangeLogger.Action/compare/0.0.4.8...0.0.5.9
[0.0.4.8]: https://github.com/baynezy/ChangeLogger.Action/compare/0.0.3.7...0.0.4.8
[0.0.3.7]: https://github.com/baynezy/ChangeLogger.Action/compare/0.0.2.3...0.0.3.7
[0.0.2.3]: https://github.com/baynezy/ChangeLogger.Action/compare/2102047e7201e71c227baec5b3503a6f5ce57837...0.0.2.3