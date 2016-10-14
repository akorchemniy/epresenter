# ePresenter
ePresenter is a popular church application in the bilingual Slavic community. It was originally written by Alex Korchemniy in 2004 using C# 1.0. It is now made available as free software under the GPLv3 license.

## Documentation
http://vendisoft.biz/epresenter/documentation/bible_searching.html

## Development pre-requisites 
- To use Entity Framework: 
     - http://stackoverflow.com/questions/1412086/how-to-register-firebird-data-provider-for-entity-framework
 - Firebird
     - Firebird embedded is in 3rdparty.
     - Firebird .net client is required
     - Firebird Maestro can be used to edit the database
 - You must have DDEX installed. Google
     - http://www.firebirdsql.org/firebirdtutorial/firebird-data-access-designer-ddex-installation.html

## Known issues
 - Release mode build fails to find fbembed.dll in local directory.
     - Work around: add ePresenter bin to path
 - Installer code is missing and needs to be replaced with WIX
