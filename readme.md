# Poll Demo

This is a sample project designed to show how you can use Serverless SingalR, Azure functions and React together.

This was created as part of the festive tech calender 2022. You can see a video talking over how it works on [You Tube](https://youtu.be/lR3MqxV2IJQ).

## Limitations 
This is a sample and is not production ready code. It should be noted:

- Vote end point is not protected in any way
- Not tracking if you voted before (just hit refresh)
- No batch sending, or rate limiting so you will get a message for every vote

## Technologies
* .net 6 v4 Azure Functions
* UI based on Vite [React TypeScript template](https://www.digitalocean.com/community/tutorials/how-to-set-up-a-react-project-with-vite)
 

## Running the Project

### Azure Setup
Create an Azure storage account, and two tables. You will need to add records to the tables with any questions.

#### Questions Table:

|PartitionKey |	RowKey |	Question	| Option |
|--------------|--------|----------------|--------|
|3kTMd |	1 |	How many days to keep your tree up?	| 1 Day|
|3kTMd |	2 |		|5 Days|
|3kTMd |	3 |		|14 Days|


#### Votes Table: 

|PartitionKey |	RowKey |	OptionId|
|---------|--------|----------------|
3kTMd |	0691199e-5a34-4305-9f86-6d51f2f22c2a	|1|

You will need to give your account the permission  `Storage Table Data Contributor`. This is needed even if you are the account owner.

You will also need to create a SignalR resource in azure setting the details outlined in the video.

### API
The first step is to rename `local.settings.example.json` to `local.settings.json`. The connection to the table storage and the SignalR will need to be added to the file. You should then be able to run the function via command line or in visual studio.

## User Interface
Run `yarn install` from the app folder and then run `yarn Dev` to start the UI.