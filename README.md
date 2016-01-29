# IISLogMuncher
A small Windows console utility to report useful information about IIS logs.

## Introduction

Have you ever had to analyse IIS log files for various statistics? Want to know
how many hits your website has had, or whether a particular range of IPs are
accessing certain resources on your site? This is the tool for you!

## Example File
Of course, a real one would be soooooo much larger!
```
#Software: Microsoft Internet Information Services 7.0
#Version: 1.0
#Date: 2015-09-03 00:00:00
#Fields: date time cs-method cs-uri-stem cs-uri-query s-port cs-username c-ip cs(User-Agent) sc-status sc-substatus sc-win32-status sc-bytes time-taken
2015-09-03 00:00:00 GET /magic.aspx - 443 - 123.123.123.123 - 200 0 0 3797 31
```

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
