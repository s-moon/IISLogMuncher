# IISLogMuncher
A small Windows console utility to report useful information about IIS logs.

## Introduction

Have you ever had to analyse IIS log files for various statistics? Want to know
how many hits your website has had, or whether a particular range of IPs are
accessing certain resources on your site? This is the tool for you!

## Example File

## Usage
```
iislogmuncher [options] filelist
-c: count number of records in the file
-h: show help text
-i: ignore blank lines
-s <number>: number of lines to skip at the start of the file
-t <number>: how many records to show in summaries
```
## Example Output

## Technologies Used
* C#
* [FileHelpers](https://github.com/MarcosMeli/FileHelpers)
* [NLog](http://nlog-project.org/)
* [NUnit](http://www.nunit.org/)

Written by Stephen Moon, 2015 (stephen@logicalmoon.com)
