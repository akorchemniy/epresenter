# ePresenter
ePresenter is a popular church application in the bilingual Slavic community. It was originally written by Alex Korchemniy and Peter Garbuz in 2004 using C# 1.0. It is now made available as free software under the GPLv3 license.

## Documentation
http://vendisoft.biz/epresenter/documentation/bible_searching.html

## Development pre-requisites 
 - Firebird
     - Firebird embedded is in 3rdparty.
     - Firebird .net client is required (see 3rdparty for MSI known to be compatible)
     - Database clients
        - See https://www.firebirdsql.org/en/third-party-tools/
        - https://dbeaver.jkiss.org/ - free universal db client
            - Java based. For connection string use "jdbc:firebirdsql:embedded:{file}"

## Developer notes
 - To use Entity Framework: 
     - http://stackoverflow.com/questions/1412086/how-to-register-firebird-data-provider-for-entity-framework
     - http://www.firebirdsql.org/firebirdtutorial/firebird-data-access-designer-ddex-installation.html
 - UML diagrams in the code can be rended with PlantUML or planttext.com

## Known issues
 - Release mode build fails to find fbembed.dll in local directory.
     - Work around: add ePresenter bin to path
 - Installer code is missing and needs to be replaced with WIX
