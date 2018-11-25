# scrawler
A **s**imple web**crawler** written in C#

## Solution Contents Overview
* Scrawler.Engine - A .NET class library containing the *meat* of the project, including the web crawler and output modules.
* Scrawler - A Windows console application that utilizes the Scrawler.Crawler library to crawl a given domain passed via command line (ie: http://cpsharp.net) and generate a simple structured sitemap.
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
If you would like to specify a threadcount, the application accepts a second parameter for this <br />
eg: `scrawler dogeplanet.com 12` <br />
Would execute the crawl across 12 threads. The default is four.

## Output
Output is in xml format- a sample of me crawling cpsharp.net can be found here: https://github.com/chrispydizzle/scrawler/cpsharp.net.crawlResult.xml
<br />The format is as follows:<br />
```xml
<CrawlResult xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" Domain="cpsharp.net" BaseUrl="http://cpsharp.net/">
    <TravesedPages>
        <Page Url="https://cpsharp.net/category/root/contact/" StatusCode="200">
          <InternalPageLinks>
            <LinkTo Url="https://cpsharp.net/category/root/aboutus/" />
            <LinkTo Url="https://cpsharp.net/category/root/tech/" />
            <LinkTo Url="https://cpsharp.net/category/root/contact/" />
            <LinkTo Url="https://cpsharp.net/category/root/services/" />
            <LinkTo Url="https://cpsharp.net/category/root/folio/" />
            ...Links to other pages within the same domain appear here
          </InternalPageLinks>
          <InternalStaticLinks>
            <LinkTo Url="https://cpsharp.net/wp-content/uploads/2015/09/xcropped-cpsharp11.png.pagespeed.ic.1QCdkhCAxl.png" />
            ...References to internal static elements (ie: images you host on this domain) appear here.
          </InternalStaticLinks>
          <ExternalPageLinks>
            <LinkTo Url="https://twitter.com/cpsharp" />
            ...References to external pages (ie: a fb page you linked to) will appear here.
          </ExternalPageLinks>
          <ExternalStaticLinks>
            <LinkTo Url="https://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js" />
            ...References to external static elements (ie: cdn javascipt) appear here.
          </ExternalStaticLinks>
        </Page>
        ...trimmed here
    </TraversedPages>
</CrawlResult>
```

## Notes
So, what would I like to see added, time permitting?
* At the moment, there's no way to specify a maximum depth, which means if you crawl a site that has links twenty levels deep, well, this guys is gonna crawl it.
* I'd have liked to improve the console input, to allow for more options and a better user experience. At the moment you just throw in a domain or a url and optionally the thread count.
* While the xml output is by all means descriptive, it could definitely be made prettier.

### Other Dependencies
I've made use of the HtmlAgilityPack (freely available, here: https://html-agility-pack.net/) to manage html parsing. <br />

### Stated Requirements
Please write a simple web crawler in a language of your choice - we'd suggest using tools / languages that you're already familiar with.

The crawler should be limited to one domain. Given a starting URL – say http://cpsharp.net - it should visit all pages within the domain, but not follow links to external sites such as Google, Twitter, etc.
 
The output should be a simple structured site map (this does not need to be a traditional XML sitemap - just some sort of output to reflect what your crawler has discovered) showing links to other pages under the same domain, links to external URLs, and links to static content such as images for each respective page.
 
Please provide a README.md file that documents:
How to build, test, and run your solution, including any required installations
Any trade-offs that you've made, and why
Anything further that you would like to achieve with more time
Automated tests (e.g. unit, functional, integration, system) are especially important to us, as well as approaches such as TDD & BDD. We’re not looking for 100% coverage, but please make sure that you include what you believe to be an appropriate approach to testing.
 
Please make your code available in a public git repository of your choice (GitHub, Bitbucket, etc).