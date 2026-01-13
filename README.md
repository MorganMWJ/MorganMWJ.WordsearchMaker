# MorganMWJ.WordsearchMaker
App for making wordsearch puzzles. React UI, Web API, and Nuget Package.

## Publish the nuget package
cd MorganMWJ.WordsearchMaker\MorganMWJ.WordsearchMaker\bin\Release
dotnet nuget push .\MorganMWJ.WordsearchMaker.1.x.0.nupkg --api-key <key-here> --source https://api.nuget.org/v3/index.json

## Publish the Web API (Azure Function Http Trigger)
cd MorganMWJ.WordsearchMaker\MorganMWJ.WordsearchMaker.Api
az login
func azure functionapp publish wordsearch-generator-api

## Publish the react app 
cd wordsearch-react-app
npm run deploy


## Notes
- Later maybe add GitHub workflow for deployment of these
