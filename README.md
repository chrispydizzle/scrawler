# scrawler
A **s**imple web**crawler** written in C#

## Solution Contents Overview
* Scrawler.Crawler - A .NET class library containing the *meat* of the project, including the web crawler and output modules.
* Scrawler - A Windows console application that utilizes the Scrawler.Crawler library to crawl a given domain passed via command line (ie: http://dogeplanet.com) and generate a simple structured sitemap.
* Scrawler.Testing - An MS Test Project containing unit tests that validate the functionality within Scrawler.Crawler.

## Build
* This project requires the .NET Framework (v4.6.2).
* It's recommended that this project be built and tested on Windows using Visual Studio 2017 Community Edition (available for free, here: https://visualstudio.microsoft.com/downloads/).

## Test
A suite of unit tests has been written using the MS Test Framework which is installed alongside Visual Studio and can be run from the Visual Studio IDE. The tests are contained in the Scrawler.Testing project.

## Run
The console application takes a single command line argument, the domain/url to be crawled, and creates an xml file describing each page found within the domain.<br/>
eg: `scrawler dogeplanet.com` <br />
Would crawl the website dogeplanet.com, creating a file `dogeplanet.com.crawlResult.xml` at the same path of the executable. <br />
If you would like to specify an output path, the application accepts a second parameter for this <br />
eg: `scrawler dogeplanet.com C:\Temp\` <br />
Would output the crawl result to C:\Temp on your local system.

## Notes
This is still a work in progress.

### Requirements
Please write a simple web crawler in a language of your choice - we'd suggest using tools / languages that you're already familiar with.

The crawler should be limited to one domain. Given a starting URL – say http://dogeplanet.com - it should visit all pages within the domain, but not follow links to external sites such as Google, Twitter, etc.
 
The output should be a simple structured site map (this does not need to be a traditional XML sitemap - just some sort of output to reflect what your crawler has discovered) showing links to other pages under the same domain, links to external URLs, and links to static content such as images for each respective page.
 
Please provide a README.md file that documents:
How to build, test, and run your solution, including any required installations
Any trade-offs that you've made, and why
Anything further that you would like to achieve with more time
Automated tests (e.g. unit, functional, integration, system) are especially important to us, as well as approaches such as TDD & BDD. We’re not looking for 100% coverage, but please make sure that you include what you believe to be an appropriate approach to testing.
 
Please make your code available in a public git repository of your choice (GitHub, Bitbucket, etc).