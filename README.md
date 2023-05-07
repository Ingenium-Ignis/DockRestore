# DockRestore

DockRestore is a command-line utility for managing and restoring Docker container backups. It provides an intuitive interface to navigate your file system and restore Docker container backups from archive files.

## Features

- Navigate file system directories and files
- Restore Docker container backups from archive files
- Execute pre and post-restoration commands on other containers in the same compose project

## Requirements

- Docker

## Installation

1. Pull the image from Docker Hub

   ```bash
   docker pull ingeniumignis/dockrestore
   ```

## Usage

With DockRestore, you can extract a compressed file (the backup) into a target folder (the restore target):

```
docker run -it -v path/to/archive:/archive -v path/to/restore-target:/restore-target ingeniumignis/dockrestore
```

Commands:
- `[UP ARROW]` / `[DOWN ARROW]`: Navigation
- `[ENTER]`: Select archive for restoration or enter folder 
- `[ESC]`: Close DockRestore

### Directories

There are two directories of interest:

1. Archive directory
2. Restore target directory

#### Archive Directory

This is the directory which is first shown when the application is launched. It defaults to `/archive`, but it can be changed using the environment variable `Backup__Archive`.

When running DockRestore you should use Docker Volumes to mount the desired directory to `/archive` or the configured archive directory.

#### Restore Target Directory

This is the destination directory to which the archive will get extracted. It defaults to `/restore-target`, but it can be changed using the environment variable `Backup__RestoreTarget`.

When running DockRestore you should use Docker Volumes to mount the desired directory to `restore-target` or the configured destination directory.

### Docker Compose

DockRestore is best used in combination with Docker Compose.

Using this `docker-compose.yml`:

```yml
version: "3.9"
services:
  dockrestore:
    image: ingeniumignis/dockrestore
    volumes:
      - /var/run/docker.sock:/var/run/docker.sock:ro
      - path/to/archive:/archive
      - path/to/restore:/restore-target
    stdin_open: true
    tty: true
    environment:
      - Backup__Archive=/archive
      - Backup__RestoreTarget=/restore-target
```

You could launch DockRestore like this:

```
docker-compose run dockrestore
```

#### Pre-Extract and Post-Extract Commands

Using Docker Compose and by mounting the docker socket to DockRestore you are also able to execute both pre- and post-extract commands on other containers in the same compose project. Example:

```yml
version: "3.9"
services:
  background:
    image: alpine
    command: /bin/sh -c "mkdir -p /archive && echo 'This is a test file.' > /archive/test.txt && tar -czvf /archive/test.tar.gz -C /archive test.txt && tail -f /dev/null"
    volumes:
      - archive:/archive
    labels:
      - com.ingeniumignis.dockrestore.pre_extract=/bin/sh -c "mkdir pre"
      - com.ingeniumignis.dockrestore.post_extract=/bin/sh -c "mkdir post"
  dockrestore:
    image: ingeniumignis/dockrestore
    volumes:
      - /var/run/docker.sock:/var/run/docker.sock:ro
      - archive:/archive
      - restore:/restore-target
    stdin_open: true
    tty: true
volumes:
  archive:
  restore:
```
1. Start the background service detached:

   ```
   docker-compose up -d background
   ```

   It will create a zip file in the archive directory.

2. Check the contents of the root folder of the background service:

   ```
   docker-compose exec background /bin/sh
   ```
   
   ```
   ls
   ```
   
   There are no post or pre folders.
   
   ```
   exit
   ```

3. Run dockrestore and select the `test.tar.gz` archive:
   
   ```
   docker-compose run dockrestore
   ```

4. Check the contents of the root folder again. Both directories were created as specified in the labels of `docker-compose.yml`.

5. Stop background service:

   ```
   docker-compose down
   ```

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE.txt) file for details.
